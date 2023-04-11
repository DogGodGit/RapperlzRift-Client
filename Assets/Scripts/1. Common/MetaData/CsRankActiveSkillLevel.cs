using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankActiveSkillLevel
{
	int m_nSkillId;
	int m_nLevel;
	string m_strEffectDescription;
	long m_lNextLevelUpRequiredGold;
	CsItem m_csItemNextLevelUpRequired;
	int m_nNextLevelUpRequiredItemCount;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public string EffectDescription
	{
		get { return m_strEffectDescription; }
	}

	public long NextLevelUpRequiredGold
	{
		get { return m_lNextLevelUpRequiredGold; }
	}

	public CsItem NextLevelUpRequiredItem
	{
		get { return m_csItemNextLevelUpRequired; }
	}

	public int NextLevelUpRequiredItemCount
	{
		get { return m_nNextLevelUpRequiredItemCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankActiveSkillLevel(WPDRankActiveSkillLevel rankActiveSkillLevel)
	{
		m_nSkillId = rankActiveSkillLevel.skillId;
		m_nLevel = rankActiveSkillLevel.level;
		m_strEffectDescription = CsConfiguration.Instance.GetString(rankActiveSkillLevel.effectDescriptionKey);
		m_lNextLevelUpRequiredGold = rankActiveSkillLevel.nextLevelUpRequiredGold;
		m_csItemNextLevelUpRequired = CsGameData.Instance.GetItem(rankActiveSkillLevel.nextLevelUpRequiredItemId);
		m_nNextLevelUpRequiredItemCount = rankActiveSkillLevel.nextLevelUpRequiredItemCount;
	}

}
