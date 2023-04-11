using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using System;
using UnityEngine.SceneManagement;
using SimpleDebugLog;

//---------------------------------------------------------------------------------------------------
// 작성 : 강훈 (2018-04-02)
//---------------------------------------------------------------------------------------------------
public enum EnNationWarPlayerState
{
	None = 0,
	Offense,
	Defense
}

public class CsNationWarManager
{
	bool m_bWaitResponse = false;

	List<CsNationWarDeclaration> m_listNationWarDeclaration;
	CsNationWarDeclaration m_csMyNationWarDeclaration;
	int m_nWeeklyNationWarDeclarationCount;

	bool m_bNationWarJoined;
	int m_nNationWarKillCount = 0;
	int m_nNationWarAssistCount = 0;
	int m_nNationWarDeadCount = 0;
	int m_nNationWarImmediateRevivalCount;
	
	DateTime m_dtNationWarTransmissionCountDate;
	int m_nDailyNationWarFreeTransmissionCount;
	int m_nDailyNationWarPaidTransmissionCount;
	
	DateTime m_dtNationCallCountDate;
	int m_nDailyNationWarCallCount;
	float m_flNationWarCallRemainingCoolTime;

	DateTime m_dtNationWarConvergingAttackCountDate;
	int m_nDailyNationWarConvergingAttackCount;
	float m_flNationWarConvergingAttackRemainingCoolTime;

	int m_nNationWarConvergingAttackTargetArrangeId;

	bool m_bAppearNpc = false;

	List<PDSimpleNationWarMonsterInstance> m_listSimpleNationWarMonster;

	//---------------------------------------------------------------------------------------------------
	public static CsNationWarManager Instance
    {
		get { return CsSingleton<CsNationWarManager>.GetInstance(); }
    }
    
	//---------------------------------------------------------------------------------------------------

	public event Delegate<int, int, Vector3> EventMoveToNationWarMonster;

	// Command
	public event Delegate EventMyNationWarDeclaration;
	public event Delegate <PDNationWarHistory[]> EventNationWarHistory;
	public event Delegate <int> EventNationWarJoin;
	public event Delegate <PDContinentEntranceInfo> EventContinentEnterForNationWarJoin;
	public event Delegate <PDSimpleNationWarMonsterInstance[]> EventNationWarInfo;
	public event Delegate <int> EventNationWarTransmission;
	public event Delegate <PDContinentEntranceInfo> EventContinentEnterForNationWarTransmission;
	public event Delegate <int> EventNationWarNpcTransmission;
	public event Delegate <PDContinentEntranceInfo> EventContinentEnterForNationWarNpcTransmission;
	public event Delegate <int> EventNationWarRevive;
	public event Delegate <PDContinentEntranceInfo> EventContinentEnterForNationWarRevive;
	public event Delegate EventMyNationWarCall;
	public event Delegate <int> EventNationWarCallTransmission;
	public event Delegate <PDContinentEntranceInfo> EventContinentEnterForNationWarCallTransmission;
	public event Delegate EventMyNationWarConvergingAttack;
	public event Delegate <int, long, PDNationWarRanking[], PDNationWarRanking[]> EventNationWarResult;
	public event Delegate EventNationTransmission;

	// Event
	public event Delegate <CsNationWarDeclaration> EventNationWarDeclaration;
	public event Delegate <Guid> EventNationWarStart;
	public event Delegate <Guid, int> EventNationWarFinished;
	public event Delegate <PDItemBooty[], long, int, int, PDItemBooty, PDItemBooty, bool> EventNationWarWin;
	public event Delegate <PDItemBooty[], long, int, int, PDItemBooty, PDItemBooty, bool> EventNationWarLose;
	public event Delegate <int> EventNationWarMonsterBattleModeStart;
	public event Delegate <int> EventNationWarMonsterBattleModeEnd;
	public event Delegate <int> EventNationWarMonsterDead;
	public event Delegate <int> EventNationWarMonsterEmergency;
	public event Delegate <int, int> EventNationWarMonsterSpawn;
	public event Delegate <int> EventNationWarConvergingAttack;
	public event Delegate EventNationWarConvergingAttackFinished;
	public event Delegate <Guid,string,int, DateTime> EventNationWarCall;
	public event Delegate EventNationWarKillCountUpdated;
	public event Delegate EventNationWarAssistCountUpdated;
	public event Delegate EventNationWarDeadCountUpdated;
	public event Delegate EventNationWarImmediateRevivalCountUpdated;
	public event Delegate <Guid, string, int, int> EventNationWarMultiKill;
	public event Delegate <Guid, string, int, Guid, string, int, int> EventNationWarNoblesseKill;

	public event Delegate EventNationWarNpcDialog;

	//---------------------------------------------------------------------------------------------------
	public List<CsNationWarDeclaration> NationWarDeclarationList
    {
		get { return m_listNationWarDeclaration; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsNationWarDeclaration MyNationWarDeclaration
	{
		get { return m_csMyNationWarDeclaration; }
    }
		
	//---------------------------------------------------------------------------------------------------
	public int WeeklyNationWarDeclarationCount
    {
		get { return m_nWeeklyNationWarDeclarationCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public bool NationWarJoined
    {
		get { return m_bNationWarJoined; }
    }

	//---------------------------------------------------------------------------------------------------
	public int NationWarKillCount
    {
		get { return m_nNationWarKillCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public int NationWarAssistCount
    {
		get { return m_nNationWarAssistCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public int NationWarDeadCount
    {
		get { return m_nNationWarDeadCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public int NationWarImmediateRevivalCount
    {
		get { return m_nNationWarImmediateRevivalCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public DateTime NationWarTransmissionCountDate
	{
		get { return m_dtNationWarTransmissionCountDate; }
		set { m_dtNationWarTransmissionCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public int DailyNationWarFreeTransmissionCount
    {
		get { return m_nDailyNationWarFreeTransmissionCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public int DailyNationWarPaidTransmissionCount
    {
		get { return m_nDailyNationWarPaidTransmissionCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public DateTime NationWarCallCountDate
	{
		get { return m_dtNationCallCountDate; }
		set { m_dtNationCallCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public int DailyNationWarCallCount
    {
		get { return m_nDailyNationWarCallCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public float NationWarCallRemainingCoolTime
    {
        get { return Mathf.Max(m_flNationWarCallRemainingCoolTime - Time.time, 0); }
    }

	//---------------------------------------------------------------------------------------------------
	public DateTime NationWarConvergingAttackCountDate
	{
		get { return m_dtNationWarConvergingAttackCountDate; }
		set { m_dtNationWarConvergingAttackCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public int DailyNationWarConvergingAttackCount
    {
		get { return m_nDailyNationWarConvergingAttackCount; }
    }

	//---------------------------------------------------------------------------------------------------
	public float NationWarConvergingAttackRemainingCoolTime
    {
        get { return Mathf.Max(m_flNationWarConvergingAttackRemainingCoolTime - Time.time, 0); }
    }

	//---------------------------------------------------------------------------------------------------
	public int NationWarConvergingAttackTargetArrangeId
    {
		get { return m_nNationWarConvergingAttackTargetArrangeId; }
        set { m_nNationWarConvergingAttackTargetArrangeId = value; }
    }

	//---------------------------------------------------------------------------------------------------
	public bool AppearNpc
	{
		get { return m_bAppearNpc; }
		set { m_bAppearNpc = value; }
	}

    //---------------------------------------------------------------------------------------------------
    public List<PDSimpleNationWarMonsterInstance> ListSimpleNationWarMonsterInstance
    {
        get { return m_listSimpleNationWarMonster; }
    }

	//---------------------------------------------------------------------------------------------------
	public void Init(PDNationWarDeclaration[] nationWarDeclarations, int nWeeklyNationWarDeclarationCount, bool bNationWarJoined, int nNationWarKillCount, int nNationWarAssistCount, int nNationWarDeadCount,
	                    int nNationWarImmediateRevivalCount, int nDailyNationWarFreeTransmissionCount, int nDailyNationWarPaidTransmissionCount, int nDailyNationWarCallCount, float flNationWarCallRemainingCoolTime,
	                    int nDailyNationWarConvergingAttackCount, float flNationWarConvergingAttackRemainingCoolTime, int nNationWarConvergingAttackTargetArrangeId, PDSimpleNationWarMonsterInstance[] simpleNationWarMonster)
	{
		UnInit();
		dd.d("CsNationWarManager.Init");
		dd.d(CsGameData.Instance.MyHeroInfo.Nation, CsGameData.Instance.MyHeroInfo.Level);

		//금일 국가전선포 목록
		m_listNationWarDeclaration = new List<CsNationWarDeclaration>();
		for (int i = 0; i < nationWarDeclarations.Length; i++)
		{
			CsNationWarDeclaration cs = new CsNationWarDeclaration(nationWarDeclarations[i]);
			m_listNationWarDeclaration.Add(cs); 
			dd.d("CsNationWarDeclaration ", i, cs.NationId, cs.TargetNationId);
		}

		//simpleNationWarMonster list
        m_listSimpleNationWarMonster = new List<PDSimpleNationWarMonsterInstance>(simpleNationWarMonster);

		m_nWeeklyNationWarDeclarationCount = nWeeklyNationWarDeclarationCount;           //금주 국가전선포횟수

		m_bNationWarJoined = bNationWarJoined;                                           //국가전 참여여부
		m_nNationWarKillCount = nNationWarKillCount;                                     //국가전 킬 횟수
		m_nNationWarAssistCount = nNationWarAssistCount;                                 //국가전 도움 횟수
		m_nNationWarDeadCount = nNationWarDeadCount;                                     //국가전 사망 횟수
		m_nNationWarImmediateRevivalCount = nNationWarImmediateRevivalCount;             //국가전 즉시 부활 횟수

		m_nDailyNationWarFreeTransmissionCount = nDailyNationWarFreeTransmissionCount;   //금일 일일국가전무료전송횟수
		m_nDailyNationWarPaidTransmissionCount = nDailyNationWarPaidTransmissionCount;   //금일 일일국가전유료전송횟수

		m_nDailyNationWarCallCount = nDailyNationWarCallCount;											//금일 일일국가전소집횟수
		m_flNationWarCallRemainingCoolTime = flNationWarCallRemainingCoolTime + Time.time;				//국가전소집남은쿨타임

		m_nDailyNationWarConvergingAttackCount = nDailyNationWarConvergingAttackCount;                   //금일 일일국가전집중공격횟수
		m_flNationWarConvergingAttackRemainingCoolTime = flNationWarConvergingAttackRemainingCoolTime + Time.time;   //국가전집중공격남은쿨타임

		m_nNationWarConvergingAttackTargetArrangeId = nNationWarConvergingAttackTargetArrangeId;         //국가전집중공격대상몬스터배치ID
		
		if (GetMyHeroNationWarDeclaration() != null)
		{
            m_csMyNationWarDeclaration = GetMyHeroNationWarDeclaration();

            if (m_csMyNationWarDeclaration.Status == EnNationWarDeclaration.Current)
			{
				// NPC 등장 유무 확인
				if (m_listSimpleNationWarMonster.Find(a => a.monsterArrangeId == 7) == null)
				{
					m_bAppearNpc = true;
				}
			}
		}

		// Command
		CsRplzSession.Instance.EventResNationWarDeclaration += OnEventResNationWarDeclaration;
		CsRplzSession.Instance.EventResNationWarHistory += OnEventResNationWarHistory;
		CsRplzSession.Instance.EventResNationWarJoin += OnEventResNationWarJoin;
		CsRplzSession.Instance.EventResContinentEnterForNationWarJoin += OnEventResContinentEnterForNationWarJoin;
		CsRplzSession.Instance.EventResNationWarInfo += OnEventResNationWarInfo;
		CsRplzSession.Instance.EventResNationWarTransmission += OnEventResNationWarTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarTransmission += OnEventResContinentEnterForNationWarTransmission;
		CsRplzSession.Instance.EventResNationWarNpcTransmission += OnEventResNationWarNpcTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarNpcTransmission += OnEventResContinentEnterForNationWarNpcTransmission;
		CsRplzSession.Instance.EventResNationWarRevive += OnEventResNationWarRevive;
		CsRplzSession.Instance.EventResContinentEnterForNationWarRevive += OnEventResContinentEnterForNationWarRevive;
		CsRplzSession.Instance.EventResNationWarCall += OnEventResNationWarCall;
		CsRplzSession.Instance.EventResNationWarCallTransmission += OnEventResNationWarCallTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarCallTransmission += OnEventResContinentEnterForNationWarCallTransmission;
		CsRplzSession.Instance.EventResNationWarConvergingAttack += OnEventResNationWarConvergingAttack;
		CsRplzSession.Instance.EventResNationWarResult += OnEventResNationWarResult;

		// Event
		CsRplzSession.Instance.EventEvtNationWarDeclaration += OnEventEvtNationWarDeclaration;
		CsRplzSession.Instance.EventEvtNationWarStart += OnEventEvtNationWarStart;
		CsRplzSession.Instance.EventEvtNationWarFinished += OnEventEvtNationWarFinished;
		CsRplzSession.Instance.EventEvtNationWarWin += OnEventEvtNationWarWin;
		CsRplzSession.Instance.EventEvtNationWarLose += OnEventEvtNationWarLose;
		CsRplzSession.Instance.EventEvtNationWarMonsterBattleModeStart += OnEventEvtNationWarMonsterBattleModeStart;
		CsRplzSession.Instance.EventEvtNationWarMonsterBattleModeEnd += OnEventEvtNationWarMonsterBattleModeEnd;
		CsRplzSession.Instance.EventEvtNationWarMonsterDead += OnEventEvtNationWarMonsterDead;
		CsRplzSession.Instance.EventEvtNationWarMonsterEmergency += OnEventEvtNationWarMonsterEmergency;
		CsRplzSession.Instance.EventEvtNationWarMonsterSpawn += OnEventEvtNationWarMonsterSpawn;
		CsRplzSession.Instance.EventEvtNationWarConvergingAttack += OnEventEvtNationWarConvergingAttack;
		CsRplzSession.Instance.EventEvtNationWarConvergingAttackFinished += OnEventEvtNationWarConvergingAttackFinished;
		CsRplzSession.Instance.EventEvtNationWarCall += OnEventEvtNationWarCall;
		CsRplzSession.Instance.EventEvtNationWarKillCountUpdated += OnEventEvtNationWarKillCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarAssistCountUpdated += OnEventEvtNationWarAssistCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarDeadCountUpdated += OnEventEvtNationWarDeadCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarImmediateRevivalCountUpdated += OnEventEvtNationWarImmediateRevivalCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarMultiKill += OnEventEvtNationWarMultiKill;
		CsRplzSession.Instance.EventEvtNationWarNoblesseKill += OnEventEvtNationWarNoblesseKill;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResNationWarDeclaration -= OnEventResNationWarDeclaration;
		CsRplzSession.Instance.EventResNationWarHistory -= OnEventResNationWarHistory;
		CsRplzSession.Instance.EventResNationWarJoin -= OnEventResNationWarJoin;
		CsRplzSession.Instance.EventResContinentEnterForNationWarJoin -= OnEventResContinentEnterForNationWarJoin;
		CsRplzSession.Instance.EventResNationWarInfo -= OnEventResNationWarInfo;
		CsRplzSession.Instance.EventResNationWarTransmission -= OnEventResNationWarTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarTransmission -= OnEventResContinentEnterForNationWarTransmission;
		CsRplzSession.Instance.EventResNationWarNpcTransmission -= OnEventResNationWarNpcTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarNpcTransmission -= OnEventResContinentEnterForNationWarNpcTransmission;
		CsRplzSession.Instance.EventResNationWarRevive -= OnEventResNationWarRevive;
		CsRplzSession.Instance.EventResContinentEnterForNationWarRevive -= OnEventResContinentEnterForNationWarRevive;
		CsRplzSession.Instance.EventResNationWarCall -= OnEventResNationWarCall;
		CsRplzSession.Instance.EventResNationWarCallTransmission -= OnEventResNationWarCallTransmission;
		CsRplzSession.Instance.EventResContinentEnterForNationWarCallTransmission -= OnEventResContinentEnterForNationWarCallTransmission;
		CsRplzSession.Instance.EventResNationWarConvergingAttack -= OnEventResNationWarConvergingAttack;
		CsRplzSession.Instance.EventResNationWarResult -= OnEventResNationWarResult;

		// Event
		CsRplzSession.Instance.EventEvtNationWarDeclaration -= OnEventEvtNationWarDeclaration;
		CsRplzSession.Instance.EventEvtNationWarStart -= OnEventEvtNationWarStart;
		CsRplzSession.Instance.EventEvtNationWarFinished -= OnEventEvtNationWarFinished;
		CsRplzSession.Instance.EventEvtNationWarWin -= OnEventEvtNationWarWin;
		CsRplzSession.Instance.EventEvtNationWarLose -= OnEventEvtNationWarLose;
		CsRplzSession.Instance.EventEvtNationWarMonsterBattleModeStart -= OnEventEvtNationWarMonsterBattleModeStart;
		CsRplzSession.Instance.EventEvtNationWarMonsterBattleModeEnd -= OnEventEvtNationWarMonsterBattleModeEnd;
		CsRplzSession.Instance.EventEvtNationWarMonsterDead -= OnEventEvtNationWarMonsterDead;
		CsRplzSession.Instance.EventEvtNationWarMonsterEmergency -= OnEventEvtNationWarMonsterEmergency;
		CsRplzSession.Instance.EventEvtNationWarMonsterSpawn -= OnEventEvtNationWarMonsterSpawn;
		CsRplzSession.Instance.EventEvtNationWarConvergingAttack -= OnEventEvtNationWarConvergingAttack;
		CsRplzSession.Instance.EventEvtNationWarConvergingAttackFinished -= OnEventEvtNationWarConvergingAttackFinished;
		CsRplzSession.Instance.EventEvtNationWarCall -= OnEventEvtNationWarCall;
		CsRplzSession.Instance.EventEvtNationWarKillCountUpdated -= OnEventEvtNationWarKillCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarAssistCountUpdated -= OnEventEvtNationWarAssistCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarDeadCountUpdated -= OnEventEvtNationWarDeadCountUpdated;
		CsRplzSession.Instance.EventEvtNationWarImmediateRevivalCountUpdated -= OnEventEvtNationWarImmediateRevivalCountUpdated;
        CsRplzSession.Instance.EventEvtNationWarMultiKill -= OnEventEvtNationWarMultiKill;
        CsRplzSession.Instance.EventEvtNationWarNoblesseKill -= OnEventEvtNationWarNoblesseKill;

		m_bWaitResponse = false;
		m_listNationWarDeclaration = null;
		m_csMyNationWarDeclaration = null;
		m_bNationWarJoined = false;
		m_bAppearNpc = false;
		m_listSimpleNationWarMonster = null;
	}

    //---------------------------------------------------------------------------------------------------
    public CsNationWarDeclaration GetMyHeroNationWarDeclaration()
    {
        int nMyHeroNationId = CsGameData.Instance.MyHeroInfo.Nation.NationId;
        CsNationWarDeclaration csNationWarDeclaration = m_listNationWarDeclaration.Find(a => a.NationId == nMyHeroNationId || a.TargetNationId == nMyHeroNationId);
        return csNationWarDeclaration;
    }

	//---------------------------------------------------------------------------------------------------
	public CsNationWarDeclaration GetNationWarDeclaration(int nNationId)
	{
		CsNationWarDeclaration csNationWarDeclaration = m_listNationWarDeclaration.Find(a => a.NationId == nNationId || a.TargetNationId == nNationId);
		return csNationWarDeclaration;
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetWeeklyNationWarDeclarationCount()
    {
        m_nWeeklyNationWarDeclarationCount = CsGameData.Instance.NationWar.WeeklyDeclarationMaxCount;
    }

	//---------------------------------------------------------------------------------------------------
	public void MoveToNationWarMonster(int nTargetArrangeId) // 특정 몬스터에게 이동
	{
		CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nTargetArrangeId);
		
        if (EventMoveToNationWarMonster != null && csNationWarMonsterArrange != null)
		{
			EventMoveToNationWarMonster(m_csMyNationWarDeclaration.TargetNationId, csNationWarMonsterArrange.Continent.ContinentId, csNationWarMonsterArrange.Position);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateSimpleNationWarMonsterList(int nArrangeId)
    {
        PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = m_listSimpleNationWarMonster.Find(a => a.monsterArrangeId == nArrangeId);

        if (simpleNationWarMonsterInstance == null)
        {
            return;
        }
        else
        {
            simpleNationWarMonsterInstance.isBattleMode = false;

            switch (nArrangeId)
            {
                case 2:
                    if (simpleNationWarMonsterInstance.nationId == m_csMyNationWarDeclaration.NationId)
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.TargetNationId;
                    }
                    else
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.NationId;
                    }

                    break;
                case 3:
                    if (simpleNationWarMonsterInstance.nationId == m_csMyNationWarDeclaration.NationId)
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.TargetNationId;
                    }
                    else
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.NationId;
                    }

                    break;
                case 4:
                    m_listSimpleNationWarMonster.Remove(simpleNationWarMonsterInstance);

                    break;
                case 5:
                    m_listSimpleNationWarMonster.Remove(simpleNationWarMonsterInstance);

                    break;
                case 6:
                    m_listSimpleNationWarMonster.Remove(simpleNationWarMonsterInstance);

                    break;
                case 7:
                    if (simpleNationWarMonsterInstance.nationId == m_csMyNationWarDeclaration.NationId)
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.TargetNationId;
                    }
                    else
                    {
                        simpleNationWarMonsterInstance.nationId = m_csMyNationWarDeclaration.NationId;
                    }

                    break;
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	public void NationTransmissionReadyOK()
	{
		if (m_bWaitResponse) return;

		if (EventNationTransmission != null)
		{
			dd.d("NationTransmissionReadyOK");
			EventNationTransmission();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsEnemyNation(int nNationId)
	{
		// 국가가 다르면서 동맹이 아닐때.적대 국가로 처리.
		int nMyHeroNationId = CsGameData.Instance.MyHeroInfo.Nation.NationId;
		return (nMyHeroNationId != nNationId && CsNationAllianceManager.Instance.GetNationAllianceId(nMyHeroNationId) != nNationId);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsNaionWar()
	{
		return m_csMyNationWarDeclaration != null && m_csMyNationWarDeclaration.Status == EnNationWarDeclaration.Current;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsNationWarNpc(int nNpcId)
	{
		if (m_csMyNationWarDeclaration != null && m_csMyNationWarDeclaration.Status == EnNationWarDeclaration.Current)
		{
			return CsGameData.Instance.NationWar.GetNationWarNpc(nNpcId) == null ? false : true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 NPC 다이얼로그
	public void NationWarNpcDialog()
	{
		Debug.Log("#### NationWarNpcDialog #####");
		if (m_bAppearNpc)
		{
			if (EventNationWarNpcDialog != null)
			{
				EventNationWarNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------

	#region Command

	//---------------------------------------------------------------------------------------------------
	//국가전 선포
	public void SendNationWarDeclaration(int nTargetNationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarDeclarationCommandBody cmdBody = new NationWarDeclarationCommandBody();
			cmdBody.targetNationId = nTargetNationId;

			CsRplzSession.Instance.Send(ClientCommandName.NationWarDeclaration, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarDeclaration(int nReturnCode, NationWarDeclarationResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarDeclaration()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.NationFund = responseBody.nationFund;
			CsNationWarDeclaration csNationWarDeclaration = new CsNationWarDeclaration(responseBody.declaration);
			m_listNationWarDeclaration.Add(csNationWarDeclaration);
			m_nWeeklyNationWarDeclarationCount = responseBody.weeklyNationWarDeclarationCount;

			if (EventMyNationWarDeclaration != null)
			{
				EventMyNationWarDeclaration();
			}
		}
		else if (nReturnCode == 101)
		{
            //권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
            //이미 자신의 국가가 국가전을 선포 또는 선포당한 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
            //이미 대상국가가 국가전을 선포 또는 선포당한 상태입니다..
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
            //국가전 가능 요일이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
            //현재 국가전 기능을 사용할 수 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
            //국가전 선포를 할 수없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
            //국가자금이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00107"));
		}
		else if (nReturnCode == 108)
		{
            //금주 국가전 선포횟수가 최대 입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00108"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 히스토리
	public void SendNationWarHistory()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarHistoryCommandBody cmdBody = new NationWarHistoryCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarHistory, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarHistory(int nReturnCode, NationWarHistoryResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarHistory()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			if (EventNationWarHistory != null)
			{
				EventNationWarHistory(responseBody.histories);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 참여
	public void SendNationWarJoin()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarJoinCommandBody cmdBody = new NationWarJoinCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarJoin, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarJoin(int nReturnCode, NationWarJoinResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarJoin()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationWarJoin;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.targetNationId;
            
			if (EventNationWarJoin != null)
			{
				EventNationWarJoin(responseBody.targetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            //현재 국가전중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
            //영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
            //영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
            //영웅의 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00304"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 참여에 의한 대륙입장
	public void SendContinentEnterForNationWarJoin()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationWarJoinCommandBody cmdBody = new ContinentEnterForNationWarJoinCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationWarJoin, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationWarJoin(int nReturnCode, ContinentEnterForNationWarJoinResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResContinentEnterForNationWarJoin()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = responseBody.entranceInfo.continentId;
			if (EventContinentEnterForNationWarJoin != null)
			{
				EventContinentEnterForNationWarJoin(responseBody.entranceInfo);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 정보
	public void SendNationWarInfo()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarInfoCommandBody cmdBody = new NationWarInfoCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarInfo, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarInfo(int nReturnCode, NationWarInfoResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarInfo()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
            m_listSimpleNationWarMonster.Clear();
            m_listSimpleNationWarMonster.AddRange(responseBody.monsterInsts);

			if (EventNationWarInfo != null)
			{
				EventNationWarInfo(responseBody.monsterInsts);
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00501"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 전송
	public void SendNationWarTransmission(int nTargetAreaMonsterArrangeId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarTransmissionCommandBody cmdBody = new NationWarTransmissionCommandBody();
			cmdBody.targetAreaMonsterArrangeId = nTargetAreaMonsterArrangeId;

			CsRplzSession.Instance.Send(ClientCommandName.NationWarTransmission, cmdBody);
		}
	}

	void OnEventResNationWarTransmission(int nReturnCode, NationWarTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationWarTransmission;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.targetNationId;
            
			if (EventNationWarTransmission != null)
			{
				EventNationWarTransmission(responseBody.targetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
            // 영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00603"));
		}
		else if (nReturnCode == 104)
		{
            // 국가전에 참여하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00604"));
		}
		else if (nReturnCode == 105)
		{
            // 대상지역을 점령중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00605"));
		}
		else if (nReturnCode == 106)
		{
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00606"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 전송에 대한 대륙입장
	public void SendContinentEnterForNationWarTransmission()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationWarTransmissionCommandBody cmdBody = new ContinentEnterForNationWarTransmissionCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationWarTransmission, cmdBody);
		}
	}

	void OnEventResContinentEnterForNationWarTransmission(int nReturnCode, ContinentEnterForNationWarTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDIa;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;
			m_nDailyNationWarFreeTransmissionCount = responseBody.nationWarFreeTransmissionCount;
			m_nDailyNationWarPaidTransmissionCount = responseBody.nationWarPaidTransmissionCount;
			m_dtNationWarTransmissionCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.LocationId = responseBody.entranceInfo.continentId;

			if (EventContinentEnterForNationWarTransmission != null)
			{
				EventContinentEnterForNationWarTransmission(responseBody.entranceInfo);
			}
		}
		else if (nReturnCode == 101)
		{
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00701"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 NPC전송
	public void SendNationWarNpcTransmission(int nNpcId, int nTrasmissionExitNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarNpcTransmissionCommandBody cmdBody = new NationWarNpcTransmissionCommandBody();
			cmdBody.npcId = nNpcId;
			cmdBody.transmissionExitNo = nTrasmissionExitNo;

			CsRplzSession.Instance.Send(ClientCommandName.NationWarNpcTransmission, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarNpcTransmission(int nReturnCode, NationWarNpcTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarNpcTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationWarNpcTransmission;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.targetNationId;

			if (EventNationWarNpcTransmission != null)
			{
				EventNationWarNpcTransmission(responseBody.targetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00802"));
		}
		else if (nReturnCode == 103)
		{
            // 영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00803"));
		}
		else if (nReturnCode == 104)
		{
            // 국가전에 참여하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00804"));
		}
		else if (nReturnCode == 105)
		{
            // 대상NPC와 상호작용할 수 없는 위치 입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00805"));
		}
		else if (nReturnCode == 106)
		{
            // 대상NPC가 활성화되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_00806"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 NPC전송에 대한 대륙 입장
	public void SendContinentEnterForNationWarNpcTransmission()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationWarNpcTransmissionCommandBody cmdBody = new ContinentEnterForNationWarNpcTransmissionCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationWarNpcTransmission, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationWarNpcTransmission(int nReturnCode, ContinentEnterForNationWarNpcTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResContinentEnterForNationWarNpcTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = responseBody.entranceInfo.continentId;
			if (EventContinentEnterForNationWarNpcTransmission != null)
			{
				EventContinentEnterForNationWarNpcTransmission(responseBody.entranceInfo);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 부활
	public void SendNationWarRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarReviveCommandBody cmdBody = new NationWarReviveCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarRevive, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarRevive(int nReturnCode, NationWarReviveResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResContinentEnterForNationWarNpcTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationWarRevive;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.revivalTargetNationId;

			if (EventNationWarRevive != null)
			{
				EventNationWarRevive(responseBody.revivalTargetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            // 영웅이 죽은 상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001001"));
		}
		else if (nReturnCode == 102)
		{
            // 국가전에 참여하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001002"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 부활에 대한 대륙입장
	public void SendContinentEnterForNationWarRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationWarReviveCommandBody cmdBody = new ContinentEnterForNationWarReviveCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationWarRevive, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationWarRevive(int nReturnCode, ContinentEnterForNationWarReviveResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResContinentEnterForNationWarRevive()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;			
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.LocationId = responseBody.entranceInfo.continentId;
			
			if (EventContinentEnterForNationWarRevive != null)
			{
				EventContinentEnterForNationWarRevive(responseBody.entranceInfo);
			} 
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 소집
	public void SendNationWarCall()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarCallCommandBody cmdBody = new NationWarCallCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarCall, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarCall(int nReturnCode, NationWarCallResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarCall()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_nDailyNationWarCallCount = responseBody.dailyNationWarCallCount;
			m_flNationWarCallRemainingCoolTime = responseBody.nationWarCallRemainingCoolTime + Time.time;
			m_dtNationCallCountDate = responseBody.date;
           
			if (EventMyNationWarCall != null)
			{
				EventMyNationWarCall();
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전 중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001201"));
		}
		else if (nReturnCode == 102)
		{
            // 국가전에 참여하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001202"));
		}
		else if (nReturnCode == 103)
		{
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001203"));
		}
		else if (nReturnCode == 104)
		{
            // 금일 국가전 소집횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001204"));
		}
		else if (nReturnCode == 105)
		{
            // 쿨타임이 만료되지 않았습니다..
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001205"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}
	//---------------------------------------------------------------------------------------------------
	// 국가전 소집전송
	public void SendNationWarCallTransmission()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarCallTransmissionCommandBody cmdBody = new NationWarCallTransmissionCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarCallTransmission, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarCallTransmission(int nReturnCode, NationWarCallTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarCallTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationWarCallTransmission;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.targetNationId;

			if (EventNationWarCallTransmission != null)
			{
				EventNationWarCallTransmission(responseBody.targetContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전 중이 아닙니다..
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001301"));
		}
		else if (nReturnCode == 102)
		{
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001302"));
		}
		else if (nReturnCode == 103)
		{
            // 영우이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001303"));
		}
		else if (nReturnCode == 104)
		{
            // 영웅의 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001304"));
		}
		else if (nReturnCode == 105)
		{
            // 국가전 소집전송 사용가능 시간이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001305"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 소집전송에 대한 대륙입장
	public void SendContinentEnterForNationWarCallTransmission()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationWarCallTransmissionCommandBody cmdBody = new ContinentEnterForNationWarCallTransmissionCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationWarCallTransmission, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationWarCallTransmission(int nReturnCode, ContinentEnterForNationWarCallTransmissionResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarCallTransmission()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = responseBody.entranceInfo.continentId;
			if (EventContinentEnterForNationWarCallTransmission != null)
			{
				EventContinentEnterForNationWarCallTransmission(responseBody.entranceInfo);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 집중공격
	public void SendNationWarConvergingAttack(int nTargetMonsterArrangeId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarConvergingAttackCommandBody cmdBody = new NationWarConvergingAttackCommandBody();
			cmdBody.targetMonsterArrangeId = nTargetMonsterArrangeId;

			CsRplzSession.Instance.Send(ClientCommandName.NationWarConvergingAttack, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarConvergingAttack(int nReturnCode, NationWarConvergingAttackResponseBody responseBody)
	{
		Debug.Log("##############  OnEventResNationWarCallTransmission()  nReturnCode = " + nReturnCode);
		
        if (nReturnCode == 0)
		{
			m_dtNationWarConvergingAttackCountDate = responseBody.date;
			m_nDailyNationWarConvergingAttackCount = responseBody.dailyNationWarConvergingAttackCount;
            m_flNationWarConvergingAttackRemainingCoolTime = responseBody.nationWarConvergingAttackReminaingCoolTime + Time.time;

			if (EventMyNationWarConvergingAttack != null)
			{
				EventMyNationWarConvergingAttack();
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 국가전 중이 아닙니다.
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001501"));
		}
		else if (nReturnCode == 102)
		{
            // 국가전에 참여하지 않았습니다.
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001502"));
		}
		else if (nReturnCode == 103)
		{
            // 권한이 없습니다.
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001503"));
		}
		else if (nReturnCode == 104)
		{
            // 대상지역은 이미 점령한 지역입니다.
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001504"));
		}
		else if (nReturnCode == 105)
		{
            // 금일 집중공격횟수가 최대입니다.
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001505"));
		}
		else if (nReturnCode == 106)
		{
            // 쿨타임이 만료되지 않았습니다. 
            m_nNationWarConvergingAttackTargetArrangeId = 0;
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001506"));
		}
		else
		{
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
            m_nNationWarConvergingAttackTargetArrangeId = 0;
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 결과
	public void SendNationWarResult()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			NationWarResultCommandBody cmdBody = new NationWarResultCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.NationWarResult, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResNationWarResult(int nReturnCode, NationWarResultResponseBody responseBody)
    {
        Debug.Log("##############  OnEventResNationWarResult()  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			//m_nNationWarRanking = responseBody.myRanking;
			m_nNationWarKillCount = responseBody.myKillCount;
			m_nNationWarAssistCount = responseBody.myAssistCount;
			m_nNationWarDeadCount = responseBody.myDeadCount;
			m_nNationWarImmediateRevivalCount = responseBody.myImmediateRevivalCount;
            
			   
			if (EventNationWarResult != null)
			{
				EventNationWarResult(responseBody.winNationId, responseBody.rewardedExp, responseBody.offenseNationRankings, responseBody.defenseNationRankings);
			}
		}
		else if (nReturnCode == 101)
		{
            // 현재 국가전 중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001601"));
		}
		else if (nReturnCode == 102)
		{
            // 국가전에 참여하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001602"));
		}
		else if (nReturnCode == 103)
		{
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001603"));
		}
		else if (nReturnCode == 104)
		{
            // 대상지역은 이미 점령한 지역입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001604"));
		}
		else if (nReturnCode == 105)
		{
            // 금일 집중공격횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001605"));
		}
		else if (nReturnCode == 106)
		{
            // 쿨타임이 만료되지 않았습니다. 
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A70_ERROR_001606"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

        m_bWaitResponse = false;
	}

#endregion Command

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------

#region Event

	//---------------------------------------------------------------------------------------------------
	// 국가전 선포 (선포자외 모든 유저)
	void OnEventEvtNationWarDeclaration(SEBNationWarDeclarationEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarDeclaration   ########");

		if (EventNationWarDeclaration != null)
		{
            CsNationWarDeclaration csNationWarDeclaration = new CsNationWarDeclaration(eventBody.nationWarDeclaration);
            m_listNationWarDeclaration.Add(csNationWarDeclaration);
            EventNationWarDeclaration(csNationWarDeclaration);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 시작
	void OnEventEvtNationWarStart(SEBNationWarStartEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarStart   ########");
		if (EventNationWarStart != null)
		{
			m_csMyNationWarDeclaration = m_listNationWarDeclaration.Find(a => a.DeclarationId == eventBody.declarationId);

			if (m_csMyNationWarDeclaration != null)
            {
				m_csMyNationWarDeclaration.Status = EnNationWarDeclaration.Current;
            }

            // SendNationWarInfo();
			EventNationWarStart(eventBody.declarationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 종료
	void OnEventEvtNationWarFinished(SEBNationWarFinishedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarFinished   ########");
		if (EventNationWarFinished != null)
		{
			if (m_csMyNationWarDeclaration == null)
			{
				m_csMyNationWarDeclaration = m_listNationWarDeclaration.Find(a => a.DeclarationId == eventBody.declarationId);
			}

			m_csMyNationWarDeclaration.Status = EnNationWarDeclaration.End;

			EventNationWarFinished(eventBody.declarationId, eventBody.winNationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 승리
	void OnEventEvtNationWarWin(SEBNationWarWinEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarWin   ########");

		//m_nObjectiveAchievementOwnDia = eventBody.objectiveAchievementOwnDia;
        m_bNationWarJoined = true;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.ExploitPoint = eventBody.exploitPoint;

		CsGameData.Instance.MyHeroInfo.OwnDia = eventBody.ownDia;
		CsGameData.Instance.MyHeroInfo.ExploitPointDate = eventBody.date;
		CsGameData.Instance.MyHeroInfo.DailyExploitPoint = eventBody.dailyExploitPoint;
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (EventNationWarWin != null)
		{
			EventNationWarWin(eventBody.joinBooties, eventBody.joinAcquiredExp, eventBody.joinAcquiredExploitPoint, eventBody.objectiveAchievementAcquiredExploitPoint, eventBody.rankingBooty, eventBody.luckyBooty, bLevelUp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 패배
	void OnEventEvtNationWarLose(SEBNationWarLoseEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarLose   ########");

        //m_nObjectiveAchievementOwnDia = eventBody.objectiveAchievementOwnDia;
        m_bNationWarJoined = true;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.ExploitPoint = eventBody.exploitPoint;

		CsGameData.Instance.MyHeroInfo.OwnDia = eventBody.ownDia;

		CsGameData.Instance.MyHeroInfo.ExploitPointDate = eventBody.date;
		CsGameData.Instance.MyHeroInfo.DailyExploitPoint = eventBody.dailyExploitPoint;
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (EventNationWarLose != null)
		{
			EventNationWarLose(eventBody.joinBooties, eventBody.joinAcquiredExp, eventBody.joinAcquiredExploitPoint, eventBody.objectiveAchievementAcquiredExploitPoint, eventBody.rankingBooty, eventBody.luckyBooty, bLevelUp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 몬스터 전투모드 시작
	void OnEventEvtNationWarMonsterBattleModeStart(SEBNationWarMonsterBattleModeStartEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMonsterBattleModeStart #######");
        
        PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = m_listSimpleNationWarMonster.Find(a => a.monsterArrangeId == eventBody.monsterArrangeId);

        if (simpleNationWarMonsterInstance != null)
        {
            simpleNationWarMonsterInstance.isBattleMode = true;
        }

		if (EventNationWarMonsterBattleModeStart != null)
		{
			EventNationWarMonsterBattleModeStart(eventBody.monsterArrangeId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 몬스터 전투모드 종료
	void OnEventEvtNationWarMonsterBattleModeEnd(SEBNationWarMonsterBattleModeEndEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMonsterBattleModeEnd #######");

        PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = m_listSimpleNationWarMonster.Find(a => a.monsterArrangeId == eventBody.monsterArrangeId);

        if (simpleNationWarMonsterInstance != null)
        {
            simpleNationWarMonsterInstance.isBattleMode = false;
        }

		if (EventNationWarMonsterBattleModeEnd != null)
		{
			EventNationWarMonsterBattleModeEnd(eventBody.monsterArrangeId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 몬스터 사망
	void OnEventEvtNationWarMonsterDead(SEBNationWarMonsterDeadEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMonsterDead #######");
        UpdateSimpleNationWarMonsterList(eventBody.monsterArrangeId);

		if (EventNationWarMonsterDead != null)
		{
			EventNationWarMonsterDead(eventBody.monsterArrangeId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 몬스터 긴급상황
	void OnEventEvtNationWarMonsterEmergency(SEBNationWarMonsterEmergencyEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMonsterEmergency #######");
		if (EventNationWarMonsterEmergency != null)
		{
			EventNationWarMonsterEmergency(eventBody.monsterArrangeId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 몬스터 스폰
	void OnEventEvtNationWarMonsterSpawn(SEBNationWarMonsterSpawnEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMonsterSpawn #######");
		if (EventNationWarMonsterSpawn != null)
		{
			EventNationWarMonsterSpawn(eventBody.monsterArrangeId, eventBody.nationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 집중공격
	void OnEventEvtNationWarConvergingAttack(SEBNationWarConvergingAttackEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarConvergingAttack   ########");
		m_dtNationWarConvergingAttackCountDate = eventBody.date;
		m_nDailyNationWarConvergingAttackCount = eventBody.dailyNationWarConvergingAttackCount;
		m_flNationWarConvergingAttackRemainingCoolTime = eventBody.nationWarConvergingAttackReminaingCoolTime + Time.time;

        m_nNationWarConvergingAttackTargetArrangeId = eventBody.targetMonsterArrangeId;

		if (EventNationWarConvergingAttack != null)
		{
			EventNationWarConvergingAttack(eventBody.targetMonsterArrangeId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 집중공격종료
	void OnEventEvtNationWarConvergingAttackFinished(SEBNationWarConvergingAttackFinishedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarConvergingAttackFinished   ########");
        m_nNationWarConvergingAttackTargetArrangeId = 0;

		if (EventNationWarConvergingAttackFinished != null)
		{
			EventNationWarConvergingAttackFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 소집
	void OnEventEvtNationWarCall(SEBNationWarCallEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarCall   ########");
		m_dtNationCallCountDate = eventBody.date;
		m_nDailyNationWarCallCount = eventBody.dailyNationWarCallCount;
		m_flNationWarCallRemainingCoolTime = eventBody.nationWarCallRemainingCoolTime + Time.time;
        
		if (EventNationWarCall != null)
		{
			EventNationWarCall(eventBody.callerId, eventBody.callerName, eventBody.callerNoblesseId, eventBody.date);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 킬횟수 갱신
	void OnEventEvtNationWarKillCountUpdated(SEBNationWarKillCountUpdatedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarKillCountUpdated   ########");

		m_nNationWarKillCount = eventBody.killCount;

		// 누적킬횟수
		CsAccomplishmentManager.Instance.AccNationWarKillCount = eventBody.accKillCount;

		if (EventNationWarKillCountUpdated != null)
		{
			EventNationWarKillCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 도움횟수 갱신
	void OnEventEvtNationWarAssistCountUpdated(SEBNationWarAssistCountUpdatedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarAssistCountUpdated   ########");

		m_nNationWarAssistCount = eventBody.assistCount;

		if (EventNationWarAssistCountUpdated != null)
		{
			EventNationWarAssistCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 사망횟수 갱신
	void OnEventEvtNationWarDeadCountUpdated(SEBNationWarDeadCountUpdatedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarDeadCountUpdated   ########");

		m_nNationWarDeadCount = eventBody.deadCount;

		if (EventNationWarDeadCountUpdated != null)
		{
			EventNationWarDeadCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가전 즉시 부활횟수 갱신
	void OnEventEvtNationWarImmediateRevivalCountUpdated(SEBNationWarImmediateRevivalCountUpdatedEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarImmediateRevivalCountUpdated   ########");
		m_nNationWarImmediateRevivalCount = eventBody.immediateRevivalCount;
		// 누적즉시부활횟수
		CsAccomplishmentManager.Instance.AccNationWarImmediateRevivalCount = eventBody.accImmediateRevivalCount;

		if (EventNationWarImmediateRevivalCountUpdated != null)
		{
			EventNationWarImmediateRevivalCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtNationWarMultiKill(SEBNationWarMultiKillEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarMultiKill   ########");

		if (EventNationWarMultiKill != null)
		{
            EventNationWarMultiKill(eventBody.heroId, eventBody.heroName, eventBody.nationId, eventBody.killCount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtNationWarNoblesseKill(SEBNationWarNoblesseKillEventBody eventBody)
	{
		Debug.Log("####### OnEventEvtNationWarNoblesseKill   ########");
		if (EventNationWarNoblesseKill != null)
		{
			EventNationWarNoblesseKill(eventBody.killerId, eventBody.killerName, eventBody.killerNationId, eventBody.deadHeroId, eventBody.deadHeroName, eventBody.deadNationId, eventBody.deadNoblesseId);
		}
	}

#endregion Event


	//---------------------------------------------------------------------------------------------------
}
