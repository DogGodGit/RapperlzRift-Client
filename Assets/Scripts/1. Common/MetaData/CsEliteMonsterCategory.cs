using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteMonsterCategory
{
	int m_nCategoryId;
	string m_strName;
	int m_nRecommendMinHeroLevel;
	int m_nRecommendMaxHeroLevel;
	CsContinent m_csContinent;

	//---------------------------------------------------------------------------------------------------
	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RecommendMinHeroLevel
	{
		get { return m_nRecommendMinHeroLevel; }
	}

	public int RecommendMaxHeroLevel
	{
		get { return m_nRecommendMaxHeroLevel; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterCategory(WPDEliteMonsterCategory eliteMonsterCategory)
	{
		m_nCategoryId = eliteMonsterCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(eliteMonsterCategory.nameKey);
		m_nRecommendMinHeroLevel = eliteMonsterCategory.recommendMinHeroLevel;
		m_nRecommendMaxHeroLevel = eliteMonsterCategory.recommendMaxHeroLevel;
		m_csContinent = CsGameData.Instance.GetContinent(eliteMonsterCategory.continentId);
	}
}
