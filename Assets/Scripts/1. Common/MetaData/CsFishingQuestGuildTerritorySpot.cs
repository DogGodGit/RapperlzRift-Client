using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-11)
//---------------------------------------------------------------------------------------------------

public class CsFishingQuestGuildTerritorySpot
{
	int m_nSpotId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	public int SpotId
	{
		get { return m_nSpotId; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFishingQuestGuildTerritorySpot(WPDFishingQuestGuildTerritorySpot fishingQuestGuildTerritorySpot)
	{
		m_nSpotId = fishingQuestGuildTerritorySpot.spotId;
		m_flXPosition = fishingQuestGuildTerritorySpot.xPosition;
		m_flYPosition = fishingQuestGuildTerritorySpot.yPosition;
		m_flZPosition = fishingQuestGuildTerritorySpot.zPosition;
		m_flRadius = fishingQuestGuildTerritorySpot.radius;
	}
}
