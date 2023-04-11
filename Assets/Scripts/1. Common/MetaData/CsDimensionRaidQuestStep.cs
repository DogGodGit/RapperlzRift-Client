using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsDimensionRaidQuestStep
{
	int m_nStep;
	string m_strTargetTitle;
	string m_strTargetContent;
	CsNpcInfo m_csNpcInfoTarget;
	string m_strTargetInteractionText;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsNpcInfo TargetNpcInfo
	{
		get { return m_csNpcInfoTarget; }
	}

	public string TargetInteractionText
	{
		get { return m_strTargetInteractionText; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionRaidQuestStep(WPDDimensionRaidQuestStep dimensionRaidQuestStep)
	{
		m_nStep = dimensionRaidQuestStep.step;
		m_strTargetTitle = CsConfiguration.Instance.GetString(dimensionRaidQuestStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(dimensionRaidQuestStep.targetContentKey);
		m_csNpcInfoTarget = CsGameData.Instance.GetNpcInfo(dimensionRaidQuestStep.targetNpcId);
		m_strTargetInteractionText = CsConfiguration.Instance.GetString(dimensionRaidQuestStep.targetInteractionTextKey);
	}
}
