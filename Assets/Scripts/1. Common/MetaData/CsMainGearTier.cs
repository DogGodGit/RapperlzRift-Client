using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMainGearTier
{
	int m_nTier;                // 티어
	int m_nRequiredHeroLevel;   // 장착필요영웅레벨

	//---------------------------------------------------------------------------------------------------
	public int Tier
	{
		get { return m_nTier; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearTier(WPDMainGearTier mainGearTier)
	{
		m_nTier = mainGearTier.tier;
		m_nRequiredHeroLevel = mainGearTier.requiredHeroLevel;
	}

}
