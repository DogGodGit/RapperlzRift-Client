using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-24)
//---------------------------------------------------------------------------------------------------

public class CsTeamBattlefieldAvailableReward
{
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTeamBattlefieldAvailableReward(WPDTeamBattlefieldAvailableReward teamBattlefieldAvailableReward)
	{
	}
}
