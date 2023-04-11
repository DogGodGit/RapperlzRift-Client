using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryTransformationObject
{
	int m_nWaveNo;
	int m_nTransformationObjectId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	string m_strObjectPrefabName;
	float m_flObjectInteractionDuration;
	float m_flObjectInteractionMaxRange;
	float m_flObjectScale;
	int m_nObjectHeight;
	float m_flObjectRadius;
	string m_strObjectInteractionText;
	int m_nObjectLifetime;
	CsMonsterInfo m_csMonsterTransformation;
	int m_nTransformationLifetime;

	//---------------------------------------------------------------------------------------------------
	public int WaveNo
	{
		get { return m_nWaveNo; }
	}

	public int TransformationObjectId
	{
		get { return m_nTransformationObjectId; }
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

	public CsMonsterInfo TransformationMonster
	{
		get { return m_csMonsterTransformation; }
	}

	public int TransformationLifetime
	{
		get { return m_nTransformationLifetime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryTransformationObject(WPDWarMemoryTransformationObject warMemoryTransformationObject)
	{
		m_nWaveNo = warMemoryTransformationObject.waveNo;
		m_nTransformationObjectId = warMemoryTransformationObject.transformationObjectId;
		m_flXPosition = warMemoryTransformationObject.xPosition;
		m_flYPosition = warMemoryTransformationObject.yPosition;
		m_flZPosition = warMemoryTransformationObject.zPosition;
		m_flRadius = warMemoryTransformationObject.radius;
		m_strObjectPrefabName = warMemoryTransformationObject.objectPrefabName;
		m_flObjectInteractionDuration = warMemoryTransformationObject.objectInteractionDuration;
		m_flObjectInteractionMaxRange = warMemoryTransformationObject.objectInteractionMaxRange;
		m_flObjectScale = warMemoryTransformationObject.objectScale;
		m_nObjectHeight = warMemoryTransformationObject.objectHeight;
		m_flObjectRadius = warMemoryTransformationObject.objectRadius;
		m_strObjectInteractionText = CsConfiguration.Instance.GetString(warMemoryTransformationObject.objectInteractionTextKey);
		m_nObjectLifetime = warMemoryTransformationObject.objectLifetime;
		m_csMonsterTransformation = CsGameData.Instance.GetMonsterInfo(warMemoryTransformationObject.transformationMonsterId);
		m_nTransformationLifetime = warMemoryTransformationObject.transformationLifetime;
	}
}
