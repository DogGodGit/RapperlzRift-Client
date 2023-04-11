using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimObjectArrange
{
	int m_nStepNo;
	int m_nArrangeNo;
	string m_strPrefabName;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	float m_flObjectInteractionDuration;
	float m_flObjectInteractionMaxRange;
	float m_flObjectScale;
	int m_nObjectHeight;
	float m_flObjectRadius;
	string m_strObjectInteractionText;
	CsGoldReward m_csGoldReward;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int ArrangeNo
	{
		get { return m_nArrangeNo; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
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

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimObjectArrange(WPDRuinsReclaimObjectArrange ruinsReclaimObjectArrange)
	{
		m_nStepNo = ruinsReclaimObjectArrange.stepNo;
		m_nArrangeNo = ruinsReclaimObjectArrange.arrangeNo;
		m_strPrefabName = ruinsReclaimObjectArrange.prefabName;
		m_flXPosition = ruinsReclaimObjectArrange.xPosition;
		m_flYPosition = ruinsReclaimObjectArrange.yPosition;
		m_flZPosition = ruinsReclaimObjectArrange.zPosition;
		m_flRadius = ruinsReclaimObjectArrange.radius;
		m_flObjectInteractionDuration = ruinsReclaimObjectArrange.objectInteractionDuration;
		m_flObjectInteractionMaxRange = ruinsReclaimObjectArrange.objectInteractionMaxRange;
		m_flObjectScale = ruinsReclaimObjectArrange.objectScale;
		m_nObjectHeight = ruinsReclaimObjectArrange.objectHeight;
		m_flObjectRadius = ruinsReclaimObjectArrange.objectRadius;
		m_strObjectInteractionText = CsConfiguration.Instance.GetString(ruinsReclaimObjectArrange.objectInteractionTextKey);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(ruinsReclaimObjectArrange.goldRewardId);
		m_csItemReward = CsGameData.Instance.GetItemReward(ruinsReclaimObjectArrange.itemRewardId);
	}
}
