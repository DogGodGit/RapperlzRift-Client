using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsFishingQuestSpot
{
	int m_nSpotId;
	CsContinent m_csContinent;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	public int SpotId
	{
		get { return m_nSpotId; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
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
	public CsFishingQuestSpot(WPDFishingQuestSpot fishingQuestSpot)
	{
		m_nSpotId = fishingQuestSpot.spotId;
		m_csContinent = CsGameData.Instance.GetContinent(fishingQuestSpot.continentId);
		m_flXPosition = fishingQuestSpot.xPosition;
		m_flYPosition = fishingQuestSpot.yPosition;
		m_flZPosition = fishingQuestSpot.zPosition;
		m_flRadius = fishingQuestSpot.radius;
	}
}
