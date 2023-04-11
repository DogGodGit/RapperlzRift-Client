using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipStep
{
	int m_nStepNo;
	int m_nTargetMonsterKillCount;
	string m_strTargetTitle;
	string m_strTargetContent;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int TargetMonsterKillCount
	{
		get { return m_nTargetMonsterKillCount; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipStep(WPDTradeShipStep tradeShipStep)
	{
		m_nStepNo = tradeShipStep.stepNo;
		m_nTargetMonsterKillCount = tradeShipStep.targetMonsterKillCount;
		m_strTargetTitle = CsConfiguration.Instance.GetString(tradeShipStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(tradeShipStep.targetContentKey);
	}
}
