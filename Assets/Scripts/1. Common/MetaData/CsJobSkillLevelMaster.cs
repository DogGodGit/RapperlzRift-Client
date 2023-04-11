using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsJobSkillLevelMaster
{
	int m_nSkillId;							// 스킬ID
	int m_nLevel;							// 스킬레벨	
	int m_nNextLevelUpRequiredHeroLevel;	// 다음레벨필요영웅레벨
	long m_lNextLevelUpGold;				// 다음레벨업필요골드
	CsItem m_csItemNextLevelUp;				// 다음레벨업아이템
	int m_nNextLevelUpItemCount;			// 다음레벨업아이템수량

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpRequiredHeroLevel
	{
		get { return m_nNextLevelUpRequiredHeroLevel; }
	}

	public long NextLevelUpGold
	{
		get { return m_lNextLevelUpGold; }
	}

	public CsItem NextLevelUpItem
	{
		get { return m_csItemNextLevelUp; }
	}

	public int NextLevelUpItemCount
	{
		get { return m_nNextLevelUpItemCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillLevelMaster(WPDJobSkillLevelMaster jobSkillLevelMaster)
	{
		m_nSkillId = jobSkillLevelMaster.skillId;
		m_nLevel = jobSkillLevelMaster.level;
		m_nNextLevelUpRequiredHeroLevel = jobSkillLevelMaster.nextLevelUpRequiredHeroLevel;
		m_lNextLevelUpGold = jobSkillLevelMaster.nextLevelUpGold;
		m_csItemNextLevelUp = CsGameData.Instance.GetItem(jobSkillLevelMaster.nextLevelUpItemId);
		m_nNextLevelUpItemCount = jobSkillLevelMaster.nextLevelUpItemCount;
	}
}
