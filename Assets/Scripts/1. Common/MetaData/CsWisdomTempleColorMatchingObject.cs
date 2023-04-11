using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleColorMatchingObject
{
	int m_nObjectId;
	string m_strPrefabName;
	float m_flInteractionDuration;
	float m_flInteractionMaxRange;
	float m_flScale;
	int m_nHeight;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	public int ObjectId
	{
		get { return m_nObjectId; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float InteractionDuration
	{
		get { return m_flInteractionDuration; }
	}

	public float InteractionMaxRange
	{
		get { return m_flInteractionMaxRange; }
	}

	public float Scale
	{
		get { return m_flScale; }
	}

	public int Height
	{
		get { return m_nHeight; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleColorMatchingObject(WPDWisdomTempleColorMatchingObject wisdomTempleColorMatchingObject)
	{
		m_nObjectId = wisdomTempleColorMatchingObject.objectId;
		m_strPrefabName = wisdomTempleColorMatchingObject.prefabName;
		m_flInteractionDuration = wisdomTempleColorMatchingObject.interactionDuration;
		m_flInteractionMaxRange = wisdomTempleColorMatchingObject.interactionMaxRange;
		m_flScale = wisdomTempleColorMatchingObject.scale;
		m_nHeight = wisdomTempleColorMatchingObject.height;
		m_flRadius = wisdomTempleColorMatchingObject.radius;
	}

}
