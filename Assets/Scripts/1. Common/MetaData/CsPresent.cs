using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsPresent
{
	int m_nPresentId;
	string m_strName;
	string m_strDescription;
	int m_nRequiredVipLevel;
	string m_strImageName;
	int m_nDisplayCount;
	int m_nRequiredDia;
	int m_nPopularityPoint;
	int m_nContributionPoint;
	bool m_bIsMessageSend;
	string m_strMessageText;
	bool m_bIsEffectDisplay;
	string m_strEffectPrefabName;

	//---------------------------------------------------------------------------------------------------
	public int PresentId
	{
		get { return m_nPresentId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredVipLevel
	{
		get { return m_nRequiredVipLevel; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int DisplayCount
	{
		get { return m_nDisplayCount; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	public int PopularityPoint
	{
		get { return m_nPopularityPoint; }
	}

	public int ContributionPoint
	{
		get { return m_nContributionPoint; }
	}

	public bool IsMessageSend
	{
		get { return m_bIsMessageSend; }
	}

	public string MessageText
	{
		get { return m_strMessageText; }
	}

	public bool IsEffectDisplay
	{
		get { return m_bIsEffectDisplay; }
	}

	public string EffectPrefabName
	{
		get { return m_strEffectPrefabName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPresent(WPDPresent present)
	{
		m_nPresentId = present.presentId;
		m_strName = CsConfiguration.Instance.GetString(present.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(present.descriptionKey);
		m_nRequiredVipLevel = present.requiredVipLevel;
		m_strImageName = present.imageName;
		m_nDisplayCount = present.displayCount;
		m_nRequiredDia = present.requiredDia;
		m_nPopularityPoint = present.popularityPoint;
		m_nContributionPoint = present.contributionPoint;
		m_bIsMessageSend = present.isMessageSend;
		m_strMessageText = CsConfiguration.Instance.GetString(present.messageTextKey);
		m_bIsEffectDisplay = present.isEffectDisplay;
		m_strEffectPrefabName = present.effectPrefabName;
	}
}
