using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarExpReward
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
	public CsNationWarExpReward(WPDNationWarExpReward nationWarExpReward)
	{
		m_nLevel = nationWarExpReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(nationWarExpReward.expRewardId);
	}
}
