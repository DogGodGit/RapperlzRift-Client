using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-20)
//---------------------------------------------------------------------------------------------------

// 오픈 이벤트 관리(메뉴, 스킬, 토스트, 튜토리얼)
public class CsOpenToastManager
{
	int m_nLevelCheck = 0;
	bool m_bProgress = false;

	List<CsMenuContent> m_listOpenedContent = new List<CsMenuContent>();	// 개방된 메뉴컨텐츠
	List<CsMenuContent> m_listTutorialContent = new List<CsMenuContent>();	// 튜토리얼이 있는 메뉴컨텐츠
	List<int> m_listOpenedSkill = new List<int>();							// 개방된 스킬
	List<EnTutorialType> m_listTypeTutorial = new List<EnTutorialType>();

	// 오픈 애니메이션
	public event Delegate EventCheckCreatureCardAnimation;					// 크리처 카드 오픈 애니메이션 체크
	public event Delegate<int> EventToastOpenSkill;                         // 스킬 오픈 토스트
	public event Delegate<CsMenuContent> EventOpenContentAnimation;			// 메뉴 개방
	public event Delegate<EnTutorialType> EventOpenTypeTutorialAnimation;	// 이벤트로 받는 튜토리얼
	public event Delegate<CsMenuContent> EventOpenTutorialAnimation;		// 메뉴컨텐츠 개방 시 튜토리얼
	public event Delegate StartTutorial;

	//---------------------------------------------------------------------------------------------------
	public static CsOpenToastManager Instance
	{
		get { return CsSingleton<CsOpenToastManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpenToastManager()
	{
		CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
		CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
		CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;

		CsGameEventUIToUI.Instance.EventTutorialEnd += OnEventTutorialEnd;
		
		CsGameEventToUI.Instance.EventStartTutorial += OnEventStartTutorial;
		CsGameEventUIToUI.Instance.EventReferenceTutorial += OnEventReferenceTutorial;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		
	}

	//---------------------------------------------------------------------------------------------------
	void CheckOpenToast()
	{
		if (m_bProgress)
		{
			return;
		}

		if (m_listOpenedSkill.Count > 0)
		{
			m_bProgress = true;

			if (EventToastOpenSkill != null)
			{
				EventToastOpenSkill(m_listOpenedSkill[0]);
			}
			
			return;
		}

		if (m_listOpenedContent.Count > 0)
		{
			m_bProgress = true;

			if (EventOpenContentAnimation != null)
			{
				EventOpenContentAnimation(m_listOpenedContent[0]);
			}
			
			return;
		}

		if (m_listTypeTutorial.Count > 0)
		{
			m_bProgress = true;

			if (EventOpenTypeTutorialAnimation != null)
			{
				EventOpenTypeTutorialAnimation(m_listTypeTutorial[0]);
				m_listTypeTutorial.RemoveAt(0);
			}

			return;
		}


		if (m_listTutorialContent.Count > 0)
		{
			m_bProgress = true;

			if (EventOpenTutorialAnimation != null)
			{
				EventOpenTutorialAnimation(m_listTutorialContent[0]);
				m_listTutorialContent.RemoveAt(0);
			}
			
			return;
		}

		// 다음 메인 퀘스트 이벤트
		CsGameEventUIToUI.Instance.OnEventContinueNextQuest();
	}

	//---------------------------------------------------------------------------------------------------
	void SetNextToast()
	{
		if (m_bProgress)
		{
			return;
		}

		m_bProgress = true;

		if (EventCheckCreatureCardAnimation != null)
		{
			EventCheckCreatureCardAnimation();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
	{
		List<CsMenuContent> listOpenedContent;

		if (m_nLevelCheck < CsGameData.Instance.MyHeroInfo.Level)
		{
			m_nLevelCheck = CsGameData.Instance.MyHeroInfo.Level;

			listOpenedContent = CsGameData.Instance.MenuContentList.FindAll(menuContent => menuContent.IsDisplay &&
																							(csMainQuest.MainQuestNo == menuContent.RequiredMainQuestNo ||
																							menuContent.RequiredHeroLevel == CsGameData.Instance.MyHeroInfo.Level) &&
																							menuContent.RequiredMainQuestNo > 0);

		}
		else
		{
			listOpenedContent = CsGameData.Instance.MenuContentList.FindAll(menuContent => menuContent.IsDisplay &&
																							csMainQuest.MainQuestNo == menuContent.RequiredMainQuestNo &&
																							menuContent.RequiredMainQuestNo > 0);
		}

		if (listOpenedContent != null)
		{
			// 메뉴개방
			m_listOpenedContent.AddRange(listOpenedContent);

			// 튜토리얼
			m_listTutorialContent.AddRange(listOpenedContent.FindAll(menuContent => menuContent.MenuContentTutorialStepList.Count > 0));
		}

		// 스킬 오픈
		for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
		{
			if (CsGameData.Instance.MyHeroInfo.HeroSkillList[i].JobSkillMaster.OpenRequiredMainQuestNo == csMainQuest.MainQuestNo)
			{
				m_listOpenedSkill.Add(CsGameData.Instance.MyHeroInfo.HeroSkillList[i].JobSkill.SkillId);
			}
		}

		SetNextToast();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		SetNextToast();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroLevelUp()
	{
		if (m_nLevelCheck < CsGameData.Instance.MyHeroInfo.Level)
		{
			m_nLevelCheck = CsGameData.Instance.MyHeroInfo.Level;

			List<CsMenuContent> listOpenedContent = CsGameData.Instance.MenuContentList.FindAll(menuContent => menuContent.IsDisplay && menuContent.RequiredHeroLevel == CsGameData.Instance.MyHeroInfo.Level);

			// 메뉴개방
			m_listOpenedContent.AddRange(listOpenedContent);

			// 튜토리얼
			m_listTutorialContent.AddRange(listOpenedContent.FindAll(menuContent => menuContent.MenuContentTutorialStepList.Count > 0));

			SetNextToast();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CreatureCardAnimationFinished()
	{
		m_bProgress = false;

		CheckOpenToast();
	}

	//---------------------------------------------------------------------------------------------------
	public void OpenSkillAnimationFinished()
	{
		if (m_listOpenedSkill.Count > 0)
		{
			m_listOpenedSkill.RemoveAt(0);
		}

		m_bProgress = false;

		CheckOpenToast();
	}

	//---------------------------------------------------------------------------------------------------
	public void OpenContentAnimationFinished()
	{
		if (m_listOpenedContent.Count > 0)
		{
			m_listOpenedContent.RemoveAt(0);
		}

		m_bProgress = false;

		CheckOpenToast();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTutorialEnd()
	{
		m_bProgress = false;

		CheckOpenToast();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStartTutorial()
	{
		m_listTypeTutorial.Add(EnTutorialType.FirstStart);

		SetNextToast();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventReferenceTutorial(EnTutorialType enTutorialType)
	{
		m_listTypeTutorial.Add(enTutorialType);

		SetNextToast();
	}

	//---------------------------------------------------------------------------------------------------
}
