using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearLevelQuality
{
	int m_nSubGearId;
	int m_nLevel;
	int m_nQuality;
	CsItem m_csItemNextQualityUp1;
	int m_nNextQualityUpItem1Count;
	CsItem m_csItemNextQualityUp2;
	int m_nNextQualityUpItem2Count;

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int Quality
	{
		get { return m_nQuality; }
	}

	public CsItem NextQualityUpItem1
	{
		get { return m_csItemNextQualityUp1; }
	}

	public int NextQualityUpItem1Count
	{
		get { return m_nNextQualityUpItem1Count; }
	}

	public CsItem NextQualityUpItem2
	{
		get { return m_csItemNextQualityUp2; }
	}

	public int NextQualityUpItem2Count
	{
		get { return m_nNextQualityUpItem2Count; }

	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearLevelQuality(WPDSubGearLevelQuality subGearLevelQuality)
	{
		m_nSubGearId = subGearLevelQuality.subGearId;
		m_nLevel = subGearLevelQuality.level;
		m_nQuality = subGearLevelQuality.quality;
		m_csItemNextQualityUp1 = CsGameData.Instance.GetItem(subGearLevelQuality.nextQualityUpItem1Id);
		m_nNextQualityUpItem1Count = subGearLevelQuality.nextQualityUpItem1Count;
		m_csItemNextQualityUp2 = CsGameData.Instance.GetItem(subGearLevelQuality.nextQualityUpItem2Id);
		m_nNextQualityUpItem2Count = subGearLevelQuality.nextQualityUpItem2Count;
	}

}
