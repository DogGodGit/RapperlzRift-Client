using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsFirstChargeEvent
{
	string m_strName;
	string m_strDescription;

	List<CsFirstChargeEventReward> m_listCsFirstChargeEventReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public List<CsFirstChargeEventReward> FirstChargeEventRewardList
	{
		get { return m_listCsFirstChargeEventReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFirstChargeEvent(WPDFirstChargeEvent firstChargeEvent)
	{
		m_strName = CsConfiguration.Instance.GetString(firstChargeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(firstChargeEvent.descriptionKey);

		m_listCsFirstChargeEventReward = new List<CsFirstChargeEventReward>();
	}
}
