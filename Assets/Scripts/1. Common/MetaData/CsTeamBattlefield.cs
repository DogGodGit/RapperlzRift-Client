using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-24)
//---------------------------------------------------------------------------------------------------

public class CsTeamBattlefield
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	long m_lRecommendBattlePower;
	string m_strSceneName;
	int m_nRequiredConditionType;
	int m_nRequiredConditionValue;
	int m_nEnterMaxMemberCount;
	int m_nOpenTime;
	int m_nEnterWaitingTime;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	int m_nKillPoint;
	int m_nTeamObjectivePoint;
	int m_nSafeRevivalWaitingTime;
	int m_nRevivalInvincibleTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsTeamBattlefieldAvailableReward> m_listCsTeamBattlefieldAvailableReward;
	List<CsTeamBattlefieldOpenDayOfWeek> m_listCsTeamBattlefieldOpenDayOfWeek;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public long RecommendBattlePower
	{
		get { return m_lRecommendBattlePower; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredConditionValue
	{
		get { return m_nRequiredConditionValue; }
	}

	public int EnterMaxMemberCount
	{
		get { return m_nEnterMaxMemberCount; }
	}

	public int OpenTime
	{
		get { return m_nOpenTime; }
	}

	public int EnterWaitingTime
	{
		get { return m_nEnterWaitingTime; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
	}

	public int KillPoint
	{
		get { return m_nKillPoint; }
	}

	public int TeamObjectivePoint
	{
		get { return m_nTeamObjectivePoint; }
	}

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public int RevivalInvincibleTime
	{
		get { return m_nRevivalInvincibleTime; }
	}

	public CsLocation Location
	{
		get { return m_csLocation; }
	}

	public float X
	{
		get { return m_flX; }
	}

	public float Z
	{
		get { return m_flZ; }
	}

	public float XSize
	{
		get { return m_flXSize; }
	}

	public float ZSize
	{
		get { return m_flZSize; }
	}

	public List<CsTeamBattlefieldAvailableReward> TeamBattlefieldAvailableRewardList
	{
		get { return m_listCsTeamBattlefieldAvailableReward; }
	}

	public List<CsTeamBattlefieldOpenDayOfWeek> TeamBattlefieldOpenDayOfWeekList
	{
		get { return m_listCsTeamBattlefieldOpenDayOfWeek; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTeamBattlefield(WPDTeamBattlefield teamBattlefield)
	{
		
	}
}
