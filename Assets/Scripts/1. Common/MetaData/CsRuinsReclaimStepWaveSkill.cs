using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimStepWaveSkill
{
	int m_nStepNo;
	int m_nWaveNo;
	int m_nCastingInterval;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strGuideContent;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nTransformationMonsterId;
	int m_nTransformationLifetime;
	string m_strObjectPrefabName;
	float m_flObjectInteractionDuration;
	float m_flObjectInteractionMaxRange;
	float m_flObjectScale;
	int m_nObjectHeight;
	float m_flObjectRadius;
	string m_strObjectInteractionText;
	int m_nObjectLifetime;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public int CastingInterval
	{
		get { return m_nCastingInterval; }
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

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int TransformationMonsterId
	{
		get { return m_nTransformationMonsterId; }
	}

	public int TransformationLifetime
	{
		get { return m_nTransformationLifetime; }
	}

	public string ObjectPrefabName
	{
		get { return m_strObjectPrefabName; }
	}

	public float ObjectInteractionDuration
	{
		get { return m_flObjectInteractionDuration; }
	}

	public float ObjectInteractionMaxRange
	{
		get { return m_flObjectInteractionMaxRange; }
	}

	public float ObjectScale
	{
		get { return m_flObjectScale; }
	}

	public int ObjectHeight
	{
		get { return m_nObjectHeight; }
	}

	public float ObjectRadius
	{
		get { return m_flObjectRadius; }
	}

	public string ObjectInteractionText
	{
		get { return m_strObjectInteractionText; }
	}

	public int ObjectLifetime
	{
		get { return m_nObjectLifetime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStepWaveSkill(WPDRuinsReclaimStepWaveSkill ruinsReclaimStepWaveSkill)
	{
		m_nStepNo = ruinsReclaimStepWaveSkill.stepNo;
		m_nWaveNo = ruinsReclaimStepWaveSkill.waveNo;
		m_nCastingInterval = ruinsReclaimStepWaveSkill.castingInterval;
		m_strGuideImageName = ruinsReclaimStepWaveSkill.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(ruinsReclaimStepWaveSkill.guideTitleKey);
		m_strGuideContent = CsConfiguration.Instance.GetString(ruinsReclaimStepWaveSkill.guideContentKey);
		m_flXPosition = ruinsReclaimStepWaveSkill.xPosition;
		m_flYPosition = ruinsReclaimStepWaveSkill.yPosition;
		m_flZPosition = ruinsReclaimStepWaveSkill.zPosition;
		m_flRadius = ruinsReclaimStepWaveSkill.radius;
		m_nTransformationMonsterId = ruinsReclaimStepWaveSkill.transformationMonsterId;
		m_nTransformationLifetime = ruinsReclaimStepWaveSkill.transformationLifetime;
		m_strObjectPrefabName = ruinsReclaimStepWaveSkill.objectPrefabName;
		m_flObjectInteractionDuration = ruinsReclaimStepWaveSkill.objectInteractionDuration;
		m_flObjectInteractionMaxRange = ruinsReclaimStepWaveSkill.objectInteractionMaxRange;
		m_flObjectScale = ruinsReclaimStepWaveSkill.objectScale;
		m_nObjectHeight = ruinsReclaimStepWaveSkill.objectHeight;
		m_flObjectRadius = ruinsReclaimStepWaveSkill.objectRadius;
		m_strObjectInteractionText = CsConfiguration.Instance.GetString(ruinsReclaimStepWaveSkill.objectInteractionTextKey);
		m_nObjectLifetime = ruinsReclaimStepWaveSkill.objectLifetime;
	}
}
