using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleStep
{
	int m_nStepNo;
	int m_nType;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strGuideTitle;
	string m_strGuideContent;
	int m_nStartDelayTime;
	CsItemReward m_csItemReward;

	List<CsWisdomTempleQuizMonsterPosition> m_listCsWisdomTempleQuizMonsterPosition;
	List<CsWisdomTempleQuizPoolEntry> m_listCsWisdomTempleQuizPoolEntry;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string GuideContent
	{
		get { return m_strGuideContent; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	public List<CsWisdomTempleQuizMonsterPosition> WisdomTempleQuizMonsterPositionList
	{
		get { return m_listCsWisdomTempleQuizMonsterPosition; }
	}

	public List<CsWisdomTempleQuizPoolEntry> WisdomTempleQuizPoolEntryList
	{
		get { return m_listCsWisdomTempleQuizPoolEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleStep(WPDWisdomTempleStep wisdomTempleStep)
	{
		m_nStepNo = wisdomTempleStep.stepNo;
		m_nType = wisdomTempleStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(wisdomTempleStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(wisdomTempleStep.targetContentKey);
		m_strGuideTitle = CsConfiguration.Instance.GetString(wisdomTempleStep.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(wisdomTempleStep.guideContentKey);
		m_nStartDelayTime = wisdomTempleStep.startDelayTime;
		m_csItemReward = CsGameData.Instance.GetItemReward(wisdomTempleStep.itemRewardId);

		m_listCsWisdomTempleQuizMonsterPosition = new List<CsWisdomTempleQuizMonsterPosition>();
		m_listCsWisdomTempleQuizPoolEntry = new List<CsWisdomTempleQuizPoolEntry>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleQuizPoolEntry GetWisdomTempleQuizPoolEntry(int nQuizNo)
	{
		for (int i = 0; i < m_listCsWisdomTempleQuizPoolEntry.Count; i++)
		{
			if (m_listCsWisdomTempleQuizPoolEntry[i].QuizNo == nQuizNo)
			{
				return m_listCsWisdomTempleQuizPoolEntry[i];
			}
		}

		return null;
	}
}
