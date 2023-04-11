using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsArtifactRoomFloor
{
	int m_nFloor;
	string m_strName;
	int m_nRequiredHeroLevel;
	long m_lRecommendBattlePower;
	int m_nSweepDuration;
	int m_nSweepDia;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int Floor
	{
		get { return m_nFloor; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public long RecommendBattlePower
	{
		get { return m_lRecommendBattlePower; }
	}

	public int SweepDuration
	{
		get { return m_nSweepDuration; }
	}

	public int SweepDia
	{
		get { return m_nSweepDia; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactRoomFloor(WPDArtifactRoomFloor artifactRoomFloor)
	{
		m_nFloor = artifactRoomFloor.floor;
		m_strName = CsConfiguration.Instance.GetString(artifactRoomFloor.nameKey);
		m_nRequiredHeroLevel = artifactRoomFloor.requiredHeroLevel;
		m_lRecommendBattlePower = artifactRoomFloor.recommendBattlePower;
		m_nSweepDuration = artifactRoomFloor.sweepDuration;
		m_nSweepDia = artifactRoomFloor.sweepDia;
		m_csItemReward = CsGameData.Instance.GetItemReward(artifactRoomFloor.itemRewardId);
	}
}
