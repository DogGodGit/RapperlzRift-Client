using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimStepWave
{
	int m_nStepNo;
	int m_nWaveNo;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nTargetType;
	int m_nTargetArrangeKey;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public int TargetType
	{
		get { return m_nTargetType; }
	}

	public int TargetArrangeKey
	{
		get { return m_nTargetArrangeKey; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStepWave(WPDRuinsReclaimStepWave ruinsReclaimStepWave)
	{
		m_nStepNo = ruinsReclaimStepWave.stepNo;
		m_nWaveNo = ruinsReclaimStepWave.waveNo;
		m_strTargetTitle = CsConfiguration.Instance.GetString(ruinsReclaimStepWave.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(ruinsReclaimStepWave.targetContentKey);
		m_nTargetType = ruinsReclaimStepWave.targetType;
		m_nTargetArrangeKey = ruinsReclaimStepWave.targetArrangeKey;
	}
}
