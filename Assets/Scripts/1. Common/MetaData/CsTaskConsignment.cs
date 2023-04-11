using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsTaskConsignment
{
	int m_nConsignmentId;
	string m_strName;
	string m_strDescription;
	CsItem m_csItemRequired;
	int m_nRequiredItemCount;
	int m_nCompletionRequiredTime;
	int m_nImmediateCompletionRequiredGold;
	int m_nImmediateCompletionRequiredGoldReduceInterval;
	CsTodayTask m_csTodayTask;

	List<CsTaskConsignmentAvailableReward> m_listCsTaskConsignmentAvailableReward;
	List<CsTaskConsignmentExpReward> m_listCsTaskConsignmentExpReward;

	//---------------------------------------------------------------------------------------------------
	public int ConsignmentId
	{
		get { return m_nConsignmentId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public CsItem RequiredItem
	{
		get { return m_csItemRequired; }
	}

	public int RequiredItemCount
	{
		get { return m_nRequiredItemCount; }
	}

	public int CompletionRequiredTime
	{
		get { return m_nCompletionRequiredTime; }
	}

	public int ImmediateCompletionRequiredGold
	{
		get { return m_nImmediateCompletionRequiredGold; }
	}

	public int ImmediateCompletionRequiredGoldReduceInterval
	{
		get { return m_nImmediateCompletionRequiredGoldReduceInterval; }
	}

	public CsTodayTask TodayTask
	{
		get { return m_csTodayTask; }
	}

	public List<CsTaskConsignmentAvailableReward> TaskConsignmentAvailableRewardList
	{
		get { return m_listCsTaskConsignmentAvailableReward; }
	}

	public List<CsTaskConsignmentExpReward> TaskConsignmentExpRewardList
	{
		get { return m_listCsTaskConsignmentExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTaskConsignment(WPDTaskConsignment taskConsignment)
	{
		m_nConsignmentId = taskConsignment.consignmentId;
		m_strName = CsConfiguration.Instance.GetString(taskConsignment.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(taskConsignment.descriptionKey);
		m_csItemRequired = CsGameData.Instance.GetItem(taskConsignment.requiredItemId);
		m_nRequiredItemCount = taskConsignment.requiredItemCount;
		m_nCompletionRequiredTime = taskConsignment.completionRequiredTime;
		m_nImmediateCompletionRequiredGold = taskConsignment.immediateCompletionRequiredGold;
		m_nImmediateCompletionRequiredGoldReduceInterval = taskConsignment.immediateCompletionRequiredGoldReduceInterval;
		m_csTodayTask = CsGameData.Instance.GetTodayTask(taskConsignment.todayTaskId);

		m_listCsTaskConsignmentAvailableReward = new List<CsTaskConsignmentAvailableReward>();
		m_listCsTaskConsignmentExpReward = new List<CsTaskConsignmentExpReward>();
	}
}
