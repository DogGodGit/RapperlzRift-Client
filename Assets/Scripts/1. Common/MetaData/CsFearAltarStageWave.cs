using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarStageWave
{
	int m_nStageId;
	int m_nWaveNo;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;
	int m_nType;
	float m_flHalidomMonsterXPosition;
	float m_flHalidomMonsterYPosition;
	float m_flHalidomMonsterZPosition;
	int m_nHalidomMonsterYRotationType;
	float m_flHalidomMonsterYRotation;

	//---------------------------------------------------------------------------------------------------
	public int StageId
	{
		get { return m_nStageId; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string GuideContent
	{
		get { return m_strGuideContent; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public float HalidomMonsterXPosition
	{
		get { return m_flHalidomMonsterXPosition; }
	}

	public float HalidomMonsterYPosition
	{
		get { return m_flHalidomMonsterYPosition; }
	}

	public float HalidomMonsterZPosition
	{
		get { return m_flHalidomMonsterZPosition; }
	}

	public int HalidomMonsterYRotationType
	{
		get { return m_nHalidomMonsterYRotationType; }
	}

	public float HalidomMonsterYRotation
	{
		get { return m_flHalidomMonsterYRotation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarStageWave(WPDFearAltarStageWave fearAltarStageWave)
	{
		m_nStageId = fearAltarStageWave.stageId;
		m_nWaveNo = fearAltarStageWave.waveNo;
		m_strGuideImageName = fearAltarStageWave.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(fearAltarStageWave.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(fearAltarStageWave.guideContentKey);
		m_nType = fearAltarStageWave.type;
		m_flHalidomMonsterXPosition = fearAltarStageWave.halidomMonsterXPosition;
		m_flHalidomMonsterYPosition = fearAltarStageWave.halidomMonsterYPosition;
		m_flHalidomMonsterZPosition = fearAltarStageWave.halidomMonsterZPosition;
		m_nHalidomMonsterYRotationType = fearAltarStageWave.halidomMonsterYRotationType;
		m_flHalidomMonsterYRotation = fearAltarStageWave.halidomMonsterYRotation;
	}
}
