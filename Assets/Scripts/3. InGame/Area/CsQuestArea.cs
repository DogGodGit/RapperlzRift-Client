using ClientCommon;
using UnityEngine;

public class CsQuestArea : CsBaseArea
{
	enum EnQuestType { None, GuildMission, Scenery, Biography, TrueHero, Weekly, CreatureFarm }

	bool m_bEnter = true;
	int m_nQuestId;
	EnQuestType m_enQuestType = EnQuestType.None;

	//----------------------------------------------------------------------------------------------------
	public void Init(CsWeeklyQuestMission csWeeklyQuestMission)
	{
		SetArea(EnQuestType.Weekly, csWeeklyQuestMission.TargetPosition, csWeeklyQuestMission.TargetRadius);
	}

	//----------------------------------------------------------------------------------------------------
	public void Init(CsCreatureFarmQuestMission csCreatureFarmQuestMission)
	{
		SetArea(EnQuestType.CreatureFarm, csCreatureFarmQuestMission.TargetPosition, csCreatureFarmQuestMission.TargetRadius);
	}

	//----------------------------------------------------------------------------------------------------
	public void Init(CsBiographyQuest csBiographyQuest)
	{
		SetArea(EnQuestType.Biography, csBiographyQuest.TargetPosition, csBiographyQuest.TargetRadius);
	}

	//----------------------------------------------------------------------------------------------------
	public void Init(CsSceneryQuest csSceneryQuest)
	{
		CsIllustratedBookManager.Instance.EventSceneryQuestCompleted += OnEventSceneryQuestCompleted;
		m_nQuestId = csSceneryQuest.QuestId;
		SetArea(EnQuestType.Scenery, csSceneryQuest.Position, csSceneryQuest.Radius);
	}

	//----------------------------------------------------------------------------------------------------
	public void Init(Vector3 vtPos, float flRadius)
	{
		SetArea(EnQuestType.TrueHero, vtPos, flRadius);
	}

	//----------------------------------------------------------------------------------------------------
	public void Init(CsGuildMission csGuildMission)
	{
		SetArea(EnQuestType.GuildMission, csGuildMission.TargetPosition, csGuildMission.ContinentObjectTarget.InteractionMaxRange);
	}

	//----------------------------------------------------------------------------------------------------
	void SetArea(EnQuestType enQuestType, Vector3 vrPos, float flRadius)
	{
		Debug.Log("CsQuestArea.Init()              enQuestType = " + enQuestType);
		m_enQuestType = enQuestType;
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		transform.position = vrPos;
		SphereCollider sphereCollider = transform.GetComponent<SphereCollider>();
		sphereCollider.enabled = true;
		sphereCollider.isTrigger = true;
		sphereCollider.radius = flRadius - 0.5f;
	}

	//----------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		if (m_enQuestType == EnQuestType.Scenery)
		{
			CsIllustratedBookManager.Instance.EventSceneryQuestCompleted -= OnEventSceneryQuestCompleted;
		}
		else if (m_enQuestType == EnQuestType.TrueHero)
		{
			if (CsTrueHeroQuestManager.Instance.InteractionState == EnInteractionState.ViewButton)
			{
				CsTrueHeroQuestManager.Instance.ChangeTrueHeroInteractionState(EnInteractionState.None);
			}
		}
		else if (m_enQuestType == EnQuestType.GuildMission)
		{
			if (m_bEnter)
			{
				CsGuildManager.Instance.GuildSpiritArea(false);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		m_bEnter = true;
		switch (m_enQuestType)
		{
			case EnQuestType.Weekly:
				CsWeeklyQuestManager.Instance.WeeklyQuestRoundMoveMissionComplete();
				break;
			case EnQuestType.CreatureFarm:
				CsCreatureFarmQuestManager.Instance.MissionMoveObjectiveComplete();
				break;
			case EnQuestType.Biography:
				CsBiographyManager.Instance.BiographyQuestMoveObjectiveComplete();
				break;
			case EnQuestType.TrueHero:
				CsTrueHeroQuestManager.Instance.ChangeTrueHeroInteractionState(EnInteractionState.ViewButton);
				break;
			case EnQuestType.GuildMission:
				CsGuildManager.Instance.GuildSpiritArea(true);
				break;
			case EnQuestType.Scenery:
				CsIllustratedBookManager.Instance.SceneryQuestStart(m_nQuestId);
				break;
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void StayAction()
	{
		if (m_enQuestType == EnQuestType.Scenery)
		{
			if (CsIllustratedBookManager.Instance.SceneryQuestId != m_nQuestId)
			{
				CsIllustratedBookManager.Instance.SceneryQuestStart(m_nQuestId);
			}
		}
		else if (m_enQuestType == EnQuestType.TrueHero)
		{
			if (CsTrueHeroQuestManager.Instance.InteractionState == EnInteractionState.None)
			{
				CsTrueHeroQuestManager.Instance.ChangeTrueHeroInteractionState(EnInteractionState.ViewButton);
			}
		}
		else if (m_enQuestType == EnQuestType.GuildMission)
		{
			if (m_bEnter == false)
			{
				CsGuildManager.Instance.GuildSpiritArea(true);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		if (m_enQuestType == EnQuestType.TrueHero)
		{
			if (CsTrueHeroQuestManager.Instance.InteractionState == EnInteractionState.ViewButton)
			{
				CsTrueHeroQuestManager.Instance.ChangeTrueHeroInteractionState(EnInteractionState.None);
			}
		}
		else if (m_enQuestType == EnQuestType.GuildMission)
		{
			if (m_bEnter)
			{
				CsGuildManager.Instance.GuildSpiritArea(false);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventSceneryQuestCompleted(PDItemBooty pDItemBooty, int nQuestId)
	{
		if (m_enQuestType == EnQuestType.Scenery)
		{
			if (nQuestId == m_nQuestId)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
