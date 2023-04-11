using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-26)
//---------------------------------------------------------------------------------------------------

public class CsGuildAltarDefenseMonsterAttrFactor
{
	int m_nHeroLevel;
	float m_flMaxHpFactor;
	float m_flOffenseFactor;

	//---------------------------------------------------------------------------------------------------
	public int HeroLevel
	{
		get { return m_nHeroLevel; }
	}

	public float MaxHpFactor
	{
		get { return m_flMaxHpFactor; }
	}

	public float OffenseFactor
	{
		get { return m_flOffenseFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildAltarDefenseMonsterAttrFactor(WPDGuildAltarDefenseMonsterAttrFactor guildAltarDefenseMonsterAttrFactor)
	{
		m_nHeroLevel = guildAltarDefenseMonsterAttrFactor.heroLevel;
		m_flMaxHpFactor = guildAltarDefenseMonsterAttrFactor.maxHpFactor;
		m_flOffenseFactor = guildAltarDefenseMonsterAttrFactor.offenseFactor;
	}
}
