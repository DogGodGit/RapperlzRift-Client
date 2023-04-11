using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsRechargeEvent
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredUnOwnDia;

	List<CsRechargeEventReward> m_listCsRechargeEventReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredUnOwnDia
	{
		get { return m_nRequiredUnOwnDia; }
	}

	public List<CsRechargeEventReward> RechargeEventRewardList
	{
		get { return m_listCsRechargeEventReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRechargeEvent(WPDRechargeEvent rechargeEvent)
	{
		m_strName = CsConfiguration.Instance.GetString(rechargeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(rechargeEvent.descriptionKey);
		m_nRequiredUnOwnDia = rechargeEvent.requiredUnOwnDia;

		m_listCsRechargeEventReward = new List<CsRechargeEventReward>();
	}
}
