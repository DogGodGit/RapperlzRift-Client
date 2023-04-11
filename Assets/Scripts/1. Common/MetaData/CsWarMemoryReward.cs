using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryReward
{
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryReward(WPDWarMemoryReward warMemoryReward)
	{
		m_nLevel = warMemoryReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(warMemoryReward.expRewardId);
	}
}
