using System.Collections;
using UnityEngine;

public class CsFishingArea : CsBaseArea
{
	enum EnFishingType { Continent, Guild }

	int m_nFishingBoxNo = 0;
	int m_nFishingSpotId = 0;
	EnFishingType m_enFishingType = EnFishingType.Continent;

	public int FishingBoxNo { get { return m_nFishingBoxNo; } }

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		m_nFishingBoxNo = int.Parse(transform.name);
		StartCoroutine(CheckLocation());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CheckLocation()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);

		if (CsIngameData.Instance.IngameManagement.IsContinent())
		{
			m_enFishingType = EnFishingType.Continent;
			m_nFishingSpotId = CsGameData.Instance.FishingQuest.GetFishingQuestSpot(1).SpotId;

			if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam != CsGameData.Instance.MyHeroInfo.Nation.NationId)
			{
				Destroy(gameObject);
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
				CsFishingQuestManager.Instance.FishingZones.Add(this);
			}
		}
		else if (CsGuildManager.Instance.GuildTerritory.LocationId == CsGameData.Instance.MyHeroInfo.LocationId)
		{
			m_enFishingType = EnFishingType.Guild;
			m_nFishingSpotId = CsGameData.Instance.FishingQuest.GetFishingQuestGuildTerritorySpot(1).SpotId;
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			CsFishingQuestManager.Instance.FishingZones.Add(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		if (CsFishingQuestManager.Instance.FinshingBoxNo == m_nFishingBoxNo)
		{
			CsFishingQuestManager.Instance.FishingZone(false, m_nFishingSpotId, m_nFishingBoxNo);
		}

		CsFishingQuestManager.Instance.FishingZones.Remove(this);
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		if (CsIngameData.Instance.ActiveScene)
		{
			if (CsFishingQuestManager.Instance.FinshingBoxNo == 0)
			{
				CsFishingQuestManager.Instance.FishingZone(true, m_nFishingSpotId, m_nFishingBoxNo);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StayAction()
	{
		if (CsIngameData.Instance.ActiveScene)
		{
			if (CsFishingQuestManager.Instance.FinshingBoxNo != m_nFishingBoxNo)
			{
				CsFishingQuestManager.Instance.FishingZone(true, m_nFishingSpotId, m_nFishingBoxNo);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		if (CsIngameData.Instance.ActiveScene)
		{
			if (CsFishingQuestManager.Instance.FinshingBoxNo == m_nFishingBoxNo)
			{
				CsFishingQuestManager.Instance.FishingZone(false, m_nFishingSpotId, m_nFishingBoxNo);
			}
		}
	}
}
