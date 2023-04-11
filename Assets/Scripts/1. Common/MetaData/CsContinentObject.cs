using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsContinentObject
{
	int m_nObjectId;								// 오브젝트ID
	string m_strName;								// 이름
	float m_flInteractionDuration;					// 상호작용시간
	float m_flInteractionMaxRange;					// 상효작용최대범위
	string m_strInteractionText;					// 상호작용텍스트	
	string m_strPrefabName;							// 프리팹이름
	float m_flScale;								// 크기
	int m_nHeight;									// 높이
	float m_flRadius;								// 반지름
	int m_nRegenTime;								// 리젠시간
	bool m_bIsPublic;								// 공용여부
	bool m_bInteractionCompletionAnimationEnabled;  // 상호작용완료애니메이션여부


	//---------------------------------------------------------------------------------------------------
	public int ObjectId
	{
		get { return m_nObjectId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public float InteractionDuration
	{
		get { return m_flInteractionDuration; }
	}

	public float InteractionMaxRange
	{
		get { return m_flInteractionMaxRange; }
	}

    public string InteractionText
    {
        get { return m_strInteractionText; }
    }

	public string PrefabName
	{
		get { return m_strPrefabName; }
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

	public int RegenTime
	{
		get { return m_nRegenTime; }
	}

	public bool IsPublic
	{
		get { return m_bIsPublic; }
	}

	public bool InteractionCompletionAnimationEnabled
	{
		get { return m_bInteractionCompletionAnimationEnabled; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentObject(WPDContinentObject continentObject)
	{
		m_nObjectId = continentObject.objectId;
		m_strName = CsConfiguration.Instance.GetString(continentObject.nameKey);
		m_flInteractionDuration = continentObject.interactionDuration;
		m_flInteractionMaxRange = continentObject.interactionMaxRange;
		m_strInteractionText = CsConfiguration.Instance.GetString(continentObject.interactionTextKey);
		m_strPrefabName = continentObject.prefabName;
		m_flScale = continentObject.scale;
		m_nHeight = continentObject.height;
		m_flRadius = continentObject.radius;
		m_nRegenTime = continentObject.regenTime;
		m_bIsPublic = continentObject.isPublic;
		m_bInteractionCompletionAnimationEnabled = continentObject.interactionCompletionAnimationEnabled;
	}
}
