using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarReward
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
	public CsFearAltarReward(WPDFearAltarReward fearAltarReward)
	{
		m_nLevel = fearAltarReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(fearAltarReward.expRewardId);
	}
}
