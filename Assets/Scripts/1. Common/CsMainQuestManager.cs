using ClientCommon;
using SimpleDebugLog;
using System;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-19)
//---------------------------------------------------------------------------------------------------

public enum EnMainQuestState { None, Accepted, Executed, Completed};

public class CsMainQuestManager
{
	//---------------------------------------------------------------------------------------------------
	public static CsMainQuestManager Instance
	{
		get { return CsSingleton<CsMainQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	bool m_bWaitResponse = false;

	CsMainQuest m_csMainQuest;		// 현재 메인퀘스트
	int m_nProgressCount = 0;		// 진행 카운트
	int m_nCartContinentId;			// 카트 생성 대륙
	long m_lCartInstanceId;			// 카트 인스턴스ID
	Vector3 m_vtCartPosition;		// 카트 위치
	float m_flCartRotationY;		// 카트 방향
	bool m_bWaitMainQuestComplete;	// 완료 대기 상태.

	bool m_bAuto = false;

	EnMainQuestState m_enMainQuestState = EnMainQuestState.None;	

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ui, ig
	public event Delegate EventNextAutoPlay;
	public event Delegate<int> EventAcceptDialog; // ui
	public event Delegate<int, long[]> EventAccepted; // ui, ig						// 메인 퀘스트 수락 후
	public event Delegate EventNationTransmission; // ui							// 국가 이동.

	public event Delegate<int> EventExecuteDataUpdated;	// ui, ig					// 메인 퀘스트 갱신
	public event Delegate<int> EventCompleteDialog; // ui
	public event Delegate<CsMainQuest, bool, long> EventCompleted; // ui			// 메인 퀘스트 완료 후


	public event Delegate<long[]> EventMainQuestMonsterTransformationCanceled;
	public event Delegate<long[]> EventMainQuestMonsterTransformationFinished;
	public event Delegate<Guid, int, int, int, long[]> EventHeroMainQuestMonsterTransformationStarted;
	public event Delegate<Guid, int, int, long[]> EventHeroMainQuestMonsterTransformationCanceled;
	public event Delegate<Guid, int, int, long[]> EventHeroMainQuestMonsterTransformationFinished;
	public event Delegate<Guid, int, Vector3> EventHeroMainQuestTransformationMonsterSkillCast;


	//---------------------------------------------------------------------------------------------------
	public CsMainQuest MainQuest
	{
		get { return m_csMainQuest; }
	}

	public CsMainQuest PrevMainQuest
	{
		get
		{
			return CsGameData.Instance.GetMainQuest(MainQuest.MainQuestNo - 1);
		}
	}

	public bool Auto
	{
		get { return m_bAuto; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
	}

	public int CartContinentId
	{
		get { return m_nCartContinentId; }
		set { m_nCartContinentId = value; }
	}

	public long CartInstanceId
	{
		get { return m_lCartInstanceId; }
		set { m_lCartInstanceId = value; }
	}

	public Vector3 CartPosition
	{
		get { return m_vtCartPosition; }
		set { m_vtCartPosition = value; }
	}

	public float CartRotationY
	{
		get { return m_flCartRotationY; }
		set { m_flCartRotationY = value; }
	}

	public bool WaitMainQuestComplete
	{
		get { return m_bWaitMainQuestComplete; }
	}

	public bool IsAccepted
	{
		get { return m_enMainQuestState >= EnMainQuestState.Accepted && m_enMainQuestState != EnMainQuestState.Completed; }
	}

	public bool IsExecuted
	{
		get { return m_enMainQuestState >= EnMainQuestState.Executed && m_enMainQuestState != EnMainQuestState.Completed; }	
	}

	public bool IsKillAccepted
	{
		get { return m_enMainQuestState == EnMainQuestState.Accepted && m_csMainQuest.MainQuestType == EnMainQuestType.Kill; }	
	}

	public EnMainQuestState MainQuestState
	{
		get { return m_enMainQuestState; }
	}

	public bool ResponseReady
	{
		get { return m_bWaitResponse; }
	}

	public string ObjectiveMessage
	{
		get
		{
			if (m_csMainQuest != null &&
				m_enMainQuestState != EnMainQuestState.Completed)
			{
				if (m_csMainQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					// 수락했을 경우.
					if (IsAccepted)
					{
						// 미션 완료
						if (IsExecuted)
						{
							// 완료NPC가 있을 경우
							if (m_csMainQuest.CompletionNpc != null)
							{
								if (m_csMainQuest.MainQuestType == EnMainQuestType.None)
								{
									return string.Format(m_csMainQuest.TargetText, m_csMainQuest.CompletionNpc.Name);
								}
								else
								{
									return string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), m_csMainQuest.CompletionNpc.Name);
								}
							}
							else
							{
								return "";
							}
						}
						else
						{
							switch (m_csMainQuest.MainQuestType) // 던전인 경우 추가 필요.
							{
								case EnMainQuestType.None:          // 목표없음.
									return string.Format(m_csMainQuest.TargetText, m_csMainQuest.CompletionNpc.Name);
								case EnMainQuestType.Move:          // 이동
									return m_csMainQuest.TargetText;
								case EnMainQuestType.Kill:          // 몬스터처치
									return string.Format(m_csMainQuest.TargetText, m_csMainQuest.TargetMonster.Name, m_nProgressCount, m_csMainQuest.TargetCount);
								case EnMainQuestType.Collect:       // 수집
									return string.Format(m_csMainQuest.TargetText, m_nProgressCount, m_csMainQuest.TargetCount);
								case EnMainQuestType.Interaction:   // 상호작용
									return string.Format(m_csMainQuest.TargetText, m_csMainQuest.TargetContinentObject.Name, m_nProgressCount, m_csMainQuest.TargetCount);
                                case EnMainQuestType.Dungeon:
                                    return m_csMainQuest.TargetText;
                                case EnMainQuestType.Cart:
                                    return string.Format(m_csMainQuest.TargetText, m_csMainQuest.CompletionNpc.Name);
                                default:
									return "";
							}
						}
					}
					else // 수락하지 않았을 경우.
					{
                        if (m_csMainQuest.StartNpc != null)
                        {
                            return string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), m_csMainQuest.StartNpc.Name);
                        }
                        else
                        {
                            switch (m_csMainQuest.MainQuestType) // 던전인 경우 추가 필요.
                            {
                                case EnMainQuestType.None:          // 목표없음.
                                    return string.Format(m_csMainQuest.TargetText, m_csMainQuest.CompletionNpc.Name);
                                case EnMainQuestType.Move:          // 이동
                                    return m_csMainQuest.TargetText;
                                case EnMainQuestType.Kill:          // 몬스터처치
                                    return string.Format(m_csMainQuest.TargetText, m_csMainQuest.TargetMonster.Name, m_nProgressCount, m_csMainQuest.TargetCount);
                                case EnMainQuestType.Collect:       // 수집
                                    return string.Format(m_csMainQuest.TargetText, m_nProgressCount, m_csMainQuest.TargetCount);
                                case EnMainQuestType.Interaction:   // 상호작용
                                    return string.Format(m_csMainQuest.TargetText, m_csMainQuest.TargetContinentObject.Name, m_nProgressCount, m_csMainQuest.TargetCount);
                                case EnMainQuestType.Dungeon:
                                    return m_csMainQuest.TargetText;
                                case EnMainQuestType.Cart:
                                    return string.Format(m_csMainQuest.TargetText, m_csMainQuest.CompletionNpc.Name);
                                default:
                                    return "";
                            }
                        }
      //                  // 클릭을 했을 경우.
      //                  if (m_bAuto)
						//{
						//	// 시작 NPC가 있으면
						//	if (m_csMainQuest.StartNpc != null)
						//	{
						//		return string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), m_csMainQuest.StartNpc.Name);
						//	}
						//	else // 시작 NPC가 없을 경우.
						//	{
						//		return CsConfiguration.Instance.GetString("A12_TXT_00001");
						//	}
						//}
						//else
						//{
						//	// 이곳을 눌러 퀘스트를 받으세요.
						//	return CsConfiguration.Instance.GetString("A12_TXT_00001");
						//}
					}
				}
				else
				{
					// 레벨 미달 메시지
					return string.Format(CsConfiguration.Instance.GetString("A12_TXT_01003"), m_csMainQuest.RequiredHeroLevel);
				}
			}
			else
			{
				// 완료 메시지 표시.
				return CsConfiguration.Instance.GetString("A12_TXT_00003");
			}
		}
	}


	//---------------------------------------------------------------------------------------------------
	public CsMainQuestManager()
	{
		// Command
		CsRplzSession.Instance.EventResMainQuestAccept += OnEventResMainQuestAccept;
		CsRplzSession.Instance.EventResMainQuestComplete += OnEventResMainQuestComplete;

		// Event
		CsRplzSession.Instance.EventEvtMainQuestUpdated += OnEventEvtMainQuestUpdated;

		CsRplzSession.Instance.EventEvtMainQuestMonsterTransformationCanceled += OnEventEvtMainQuestMonsterTransformationCanceled;
		CsRplzSession.Instance.EventEvtMainQuestMonsterTransformationFinished += OnEventEvtMainQuestMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationStarted += OnEventEvtHeroMainQuestMonsterTransformationStarted;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationCanceled += OnEventEvtHeroMainQuestMonsterTransformationCanceled;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationFinished += OnEventEvtHeroMainQuestMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtHeroMainQuestTransformationMonsterSkillCast += OnEventEvtHeroMainQuestTransformationMonsterSkillCast;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroMainQuest heroMainQuest)
	{
		// 최조 입장 시
		UnInit();

		if (heroMainQuest == null)
		{
			// 처음 메인 퀘스트로 세팅
			m_csMainQuest = CsGameData.Instance.MainQuestList[0];
			dd.d("---------------------------------------------------------------------------------------------------");
			dd.d("CsMainQuestManager.Initialize    MainQuestNo = ", m_csMainQuest.MainQuestNo);			
		}
		else
		{
			// 다음 퀘스트 수락전
			if (heroMainQuest.completed)    
			{
				MoveNextQuest(heroMainQuest.no);
			}
			else
			{
				// 현재 퀘스트 상태 세팅
				m_csMainQuest = CsGameData.Instance.GetMainQuest(heroMainQuest.no);
				m_nProgressCount = heroMainQuest.progressCount;
				m_nCartContinentId = heroMainQuest.cartContinentId;
				m_lCartInstanceId = heroMainQuest.cartInstanceId;				
				m_vtCartPosition = CsRplzSession.Translate(heroMainQuest.cartPosition);
				m_flCartRotationY = heroMainQuest.cartRotationY;

				if (m_nProgressCount < m_csMainQuest.TargetCount)
				{
					m_enMainQuestState = EnMainQuestState.Accepted;
				}
				else
				{
					m_enMainQuestState = EnMainQuestState.Executed;
				}
			}

			if (m_csMainQuest != null)
			{
				dd.d("---------------------------------------------------------------------------------------------------");
				dd.d("CsMainQuestManager.Initialize", heroMainQuest.no, heroMainQuest.completed, m_nProgressCount, m_csMainQuest.TargetCount, m_csMainQuest.MainQuestNo);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_bWaitResponse = false;
		m_csMainQuest = null;
		m_bWaitMainQuestComplete = false;
		m_bAuto = false;
		m_nProgressCount = 0;		
		m_nCartContinentId = 0;			
		m_lCartInstanceId = 0;			
		m_vtCartPosition = Vector3.zero;		
		m_flCartRotationY = 0;

		m_enMainQuestState = EnMainQuestState.None;
	}

	//---------------------------------------------------------------------------------------------------
	void DisplayQuestInfo(CsMainQuest cs)
	{
		if (cs == null) return;
		dd.d(cs.MainQuestNo, cs.StartNpc != null, cs.MainQuestType, cs.TargetCount, cs.CompletionNpc != null);
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		dd.d("CsMainQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto, m_enMainQuestState);
		if (m_bWaitResponse) return;
		if (m_csMainQuest == null) return; // 더이상 진행할 메인퀘스트가 없을때.

		if (!m_bAuto)
		{
			DisplayQuestInfo(m_csMainQuest);
			m_bAuto = true;

			if (m_enMainQuestState == EnMainQuestState.None)
			{
				if (m_csMainQuest.StartNpc == null)
				{
					SendMainQuestAccept();
				}
			}
			else if (m_enMainQuestState == EnMainQuestState.Executed)
			{
				if (m_csMainQuest.CompletionNpc == null)
				{
					SendMainQuestComplete();
				}
			}

			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void NextAutoPlay()
	{
		dd.d("CsMainQuestManager.NextAutoPlay", m_bWaitResponse, m_bAuto, m_enMainQuestState);
		if (m_bWaitResponse) return;
		if (m_enMainQuestState != EnMainQuestState.None) return;

		DisplayQuestInfo(m_csMainQuest);

		if (m_csMainQuest.StartNpc == null)
		{
			SendMainQuestAccept();
		}

		if (EventNextAutoPlay != null)
		{
			EventNextAutoPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		dd.d("CsMainQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
		if (m_bAuto == true)
		{
			m_bAuto = false;
			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void MoveNextQuest(int nQuestNo)
	{
		if (CsGameData.Instance.GetMainQuest(nQuestNo + 1) == null)
		{
			m_csMainQuest = CsGameData.Instance.GetMainQuest(nQuestNo);
			m_enMainQuestState = EnMainQuestState.Completed;
		}
		else
		{
			m_csMainQuest = CsGameData.Instance.GetMainQuest(nQuestNo + 1);
			m_enMainQuestState = EnMainQuestState.None;
		}
		
		m_nProgressCount = 0;

		if (m_csMainQuest != null)
		{
			dd.d("---------------------------------------------------------------------------------------------------");
			dd.d("CsMainQuestManager.MoveNextQuest", m_csMainQuest.MainQuestNo, m_csMainQuest.TargetCount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SaveMainQuestCart(int nCartContinentId, long lCartInstanceId, Vector3 vtCartPosition, float flCartRotationY)
	{
		dd.d("CsMainQuestManager.SaveMainQuestCart", nCartContinentId, lCartInstanceId, vtCartPosition, flCartRotationY);
		CartContinentId = nCartContinentId;
		CartInstanceId = lCartInstanceId;
		CartPosition = vtCartPosition;
		CartRotationY = flCartRotationY;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsInteractionQuest(int nObjectId)
	{
		return (m_enMainQuestState == EnMainQuestState.Accepted &&
				m_csMainQuest.MainQuestType == EnMainQuestType.Interaction && 
				m_csMainQuest.TargetContinentObject != null && 
				m_csMainQuest.TargetContinentObject.ObjectId == nObjectId);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsMainQuestNpc(int nNpcId)
	{
		Debug.Log("IsMainQuestNpc  nNpcId = "+ nNpcId);
		if (m_csMainQuest == null) return false;

		if (m_enMainQuestState == EnMainQuestState.None)
		{
			return (m_csMainQuest.StartNpc != null && m_csMainQuest.StartNpc.NpcId == nNpcId);
		}
		else if(m_enMainQuestState == EnMainQuestState.Accepted)
		{
			return (m_csMainQuest.TargetNpc != null && m_csMainQuest.TargetNpc.NpcId == nNpcId);
		}
		else if (m_enMainQuestState == EnMainQuestState.Executed)
		{
			return (m_csMainQuest.CompletionNpc != null && m_csMainQuest.CompletionNpc.NpcId == nNpcId);
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void AcceptReadyOK()
	{
		dd.d("AcceptReadyOK", m_enMainQuestState, m_bWaitResponse);
		if (m_enMainQuestState == EnMainQuestState.None && !m_bWaitResponse)
		{
			if (m_csMainQuest.StartNpc != null)
			{
				if (EventAcceptDialog != null)
				{
					dd.d("AcceptReadyOK1");
					EventAcceptDialog(m_csMainQuest.StartNpc.NpcId);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Accept()
	{
		dd.d("Accept()", IsAccepted, m_bWaitResponse);
		if (!IsAccepted && !m_bWaitResponse)
		{
			SendMainQuestAccept();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NationTransmissionReadyOK()
	{
		if (m_bWaitResponse) return;

		if (EventNationTransmission != null)
		{
			EventNationTransmission();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CompleteReadyOK()
	{
		dd.d("CompleteReadyOK", m_enMainQuestState, m_bWaitResponse);
		if (m_enMainQuestState == EnMainQuestState.Executed && !m_bWaitResponse)
		{
			if (m_csMainQuest.CompletionNpc != null)
			{
				if (EventCompleteDialog != null)
				{
					dd.d("CompleteReadyOK");
					EventCompleteDialog(m_csMainQuest.CompletionNpc.NpcId);                    
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트완료
	public void Complete()
	{
		if (m_bWaitMainQuestComplete || m_enMainQuestState == EnMainQuestState.Executed)
		{
			m_bWaitMainQuestComplete = false;
			SendMainQuestComplete();
		}
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트수락
	void SendMainQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			dd.d("CsMainQuestManager.SendMainQuestAccept()");
			MainQuestAcceptCommandBody cmdBody = new MainQuestAcceptCommandBody();
			cmdBody.mainQuestNo = m_csMainQuest.MainQuestNo;
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestAccept, cmdBody);
		}
	}
	
	void OnEventResMainQuestAccept(int nReturnCode, MainQuestAcceptResponseBody responseBody)
	{
		dd.d("OnEventResMainQuestAccept", nReturnCode);
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.CartInstance = responseBody.cartInst;

			Debug.Log("#####     OnEventResMainQuestAccept     CsGameData.Instance.MyHeroInfo.CartInstance  = " + CsGameData.Instance.MyHeroInfo.CartInstance);
			if (m_csMainQuest.TargetCount == 0)
			{
				m_enMainQuestState = EnMainQuestState.Executed;
			}
			else
			{
				m_enMainQuestState = EnMainQuestState.Accepted;
			}

            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventAccepted != null)
			{
				EventAccepted(responseBody.transformationMonsterId, responseBody.removedAbnormalStateEffects);
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 메인퀘스트를 진행중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 시작NPC와의 상호작용할 수 있는 위치가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00103"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트완료
	void SendMainQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			dd.d("CsMainQuestManager.SendMainQuestComplete()");
			MainQuestCompleteCommandBody cmdBody = new MainQuestCompleteCommandBody();
			cmdBody.mainQuestNo = m_csMainQuest.MainQuestNo;
			CsRplzSession.Instance.Send(ClientCommandName.MainQuestComplete, cmdBody);
		}
	}

	void OnEventResMainQuestComplete(int nReturnCode, MainQuestCompleteResponseBody responseBody)
	{
		dd.d("OnEventResMainQuestComplete", nReturnCode);
		m_bWaitResponse = false;
	
		if (nReturnCode == 0)
		{
			CsConfiguration.Instance.SendFirebaseLogEvent("mainQuest_complete", m_csMainQuest.MainQuestNo.ToString());

			CsMyHeroInfo csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			CsMainQuest csOldMainQuest = m_csMainQuest;
			int nOldLevel = csMyHeroInfo.Level;
            long lOndGold = csMyHeroInfo.Gold;
			int nOldRank = csMyHeroInfo.RankNo;

			if (m_csMainQuest.MainQuestType == EnMainQuestType.Cart)
			{
				CsCartManager.Instance.RemoveMyCart();
			}

			// 다음 퀘스트 조회
			MoveNextQuest(m_csMainQuest.MainQuestNo);

			csMyHeroInfo.Level = responseBody.level;
			csMyHeroInfo.Exp = responseBody.exp;
			csMyHeroInfo.MaxHp = responseBody.maxHp;
			csMyHeroInfo.Hp = responseBody.hp;
			csMyHeroInfo.Gold = responseBody.gold;
			csMyHeroInfo.RankNo = responseBody.rankNo;
			csMyHeroInfo.AddHeroMainGears(responseBody.rewardMainGears);
			csMyHeroInfo.AddHeroSubGears(responseBody.rewardSubGears);
			csMyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            csMyHeroInfo.AddHeroMounts(responseBody.rewardMounts);

			// 변경된 영웅크리쳐카드 목록
			CsCreatureCardManager.Instance.AddHeroCreatureCards(responseBody.changedCreatureCards);
			List<CsHeroCreatureCard> list = new List<CsHeroCreatureCard>();
			for (int i = 0; i < responseBody.changedCreatureCards.Length; i++)
			{
				list.Add(new CsHeroCreatureCard(responseBody.changedCreatureCards[i]));
			}
			CsGameEventUIToUI.Instance.OnEventGetHeroCreatureCard(list);

			//최대골드
			CsAccomplishmentManager.Instance.MaxGold = responseBody.maxGold;

			//최대획득메인장비등급
			CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade = responseBody.maxAcquisitionMainGearGrade;

			if (responseBody.rewardMounts.Length == 1)
            {
                CsCommandEventManager.Instance.SendMountEquip(responseBody.rewardMounts[0].mountId);
            }

			// 전투력 업데이트
			csMyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == csMyHeroInfo.Level) ? false : true;

			if (EventCompleted != null)
			{
                EventCompleted(csOldMainQuest, bLevelUp, responseBody.acquiredExp);
			}

			// 계급이 처음 상승했을 경우 계급 스킬 업데이트
			if (nOldRank + csMyHeroInfo.RankNo == 1)
			{
				csMyHeroInfo.UpdateRankSkill(csMyHeroInfo.RankNo);
			}

            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), csMyHeroInfo.Gold - lOndGold));
			NextAutoPlay();
		}
		else if (nReturnCode == 101)
		{
            // 이미 완료되었습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A12_ERROR_00201"));
            //CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
            // 목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A12_ERROR_00202"));
           // CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
            // 완료NPC와 상호작용할 수 있는 위치가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A12_ERROR_00203"));
           // CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A12_ERROR_00204"));
           // CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A12_ERROR_00204"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트갱신
	void OnEventEvtMainQuestUpdated(SEBMainQuestUpdatedEventBody eventBody)
	{
		//dd.d("OnEventEvtMainQuestUpdated1", eventBody.progressCount, m_csMainQuest.TargetCount);
		if (m_csMainQuest.MainQuestNo == eventBody.mainQuestNo)
		{
			m_nProgressCount = eventBody.progressCount;

			if (m_nProgressCount >= m_csMainQuest.TargetCount)
			{
				if (m_enMainQuestState == EnMainQuestState.Executed)
				{
					//dd.d("OnEventEvtMainQuestUpdated2", eventBody.progressCount);
					return;
				}

				m_enMainQuestState = EnMainQuestState.Executed;
			}

			if (EventExecuteDataUpdated != null)
			{
				EventExecuteDataUpdated(m_nProgressCount);
			}

			if (m_enMainQuestState == EnMainQuestState.Executed)
			{
				if (m_csMainQuest.CompletionNpc == null)
				{
					if (CsIngameData.Instance.IngameManagement.IsContinent())
					{
						SendMainQuestComplete();
					}
					else
					{
						m_bWaitMainQuestComplete = true;
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMainQuestMonsterTransformationCanceled(SEBMainQuestMonsterTransformationCanceledEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		if (EventMainQuestMonsterTransformationCanceled != null)
		{
			EventMainQuestMonsterTransformationCanceled(eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMainQuestMonsterTransformationFinished(SEBMainQuestMonsterTransformationFinishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		if (EventMainQuestMonsterTransformationFinished != null)
		{
			EventMainQuestMonsterTransformationFinished(eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationStarted(SEBHeroMainQuestMonsterTransformationStartedEventBody eventBody)
	{
		if (EventHeroMainQuestMonsterTransformationStarted != null)
		{
			EventHeroMainQuestMonsterTransformationStarted(eventBody.heroId, eventBody.transformationMonsterId, eventBody.maxHP, eventBody.hp, eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationCanceled(SEBHeroMainQuestMonsterTransformationCanceledEventBody eventBody)
	{
		if (EventHeroMainQuestMonsterTransformationCanceled != null)
		{
			EventHeroMainQuestMonsterTransformationCanceled(eventBody.heroId, eventBody.maxHP, eventBody.hp, eventBody.removedAbnormalStateEffects);
		}		
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationFinished(SEBHeroMainQuestMonsterTransformationFinishedEventBody eventBody)
	{
		if (EventHeroMainQuestMonsterTransformationFinished != null)
		{
			EventHeroMainQuestMonsterTransformationFinished(eventBody.heroId, eventBody.maxHP, eventBody.hp, eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestTransformationMonsterSkillCast(SEBHeroMainQuestTransformationMonsterSkillCastEventBody eventBody)
	{
		if (EventHeroMainQuestTransformationMonsterSkillCast != null)
		{
			EventHeroMainQuestTransformationMonsterSkillCast(eventBody.heroId, eventBody.skillId, CsRplzSession.Translate(eventBody.skillTargetPosition));
		}
	}

	#endregion Protocol.Event

}


//			 UI											Manager									Ingame
//	None				>>StartAutoPlay() 							
//																		>>EventStartAutoPlay	
//
//
//----------------------------------------------------------------------------------------------------- repeat
//																		(AcceptReadyOK())<<				
//						(EventAcceptDialog)<<																
//						>>(Accept())																
//													Accept Request
//													Accept Response
//	Accepted			EventAccepted<<									>>EventAccepted		
//													Play Mission
//						EventExecuteDataUpdated<<						>>EventExecuteDataUpdated												
//													Check Mission count
//	Executed																		
//																		(CompleteReadyOK())<<				
//						(EventCompleteDialog)<<																
//						>>(Complete())																
//													Complete Request
//													Complete Response
//	None				EventCompleted<<								>>EventCompleted		
//						EventNextAutoPlay<<								>>EventNextAutoPlay		
//----------------------------------------------------------------------------------------------------- repeat
//																		(AcceptReadyOK())<<
//
//
//						>>StopAutoPlay																
//																>>EventStopAutoPlay						
