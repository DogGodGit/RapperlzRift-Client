using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public enum EnUsingType
{
	NotAvailable = 0,
	OnlyOne = 1,
	Multiple = 2
}

public class CsItem
{
	int m_nItemId;						// 아이템ID
	string m_strName;					// 이름
	string m_strDescription;			// 설명

	CsItemType m_csItemType;			// 아이템타입

	CsItemGrade m_csItemGrade;			// 아이템등급
	int m_nLevel;						// 레벨
	int m_nRequiredMinHeroLevel;		// 사용가능최소레벨
	int m_nRequiredMaxHeroLevel;		// 사용가능최대레벨
	int m_nUsingType;                   // 인벤토리사용타입
	bool m_bUsingRecommendationEnabled; // 사용추천여부
	bool m_bSaleable;					// 판매가능여부
	int m_nSaleGold;                    // 판매골드
	bool m_bAutoUsable;                 // 자동사용여부
	int m_nValue1;						// 값1
	int m_nValue2;                      // 값2
	long m_lLongValue1;
	long m_lLongValue2;

	int m_nComp = -1;

	//---------------------------------------------------------------------------------------------------
	public int ItemId
	{
		get { return m_nItemId; }
	}

	public EnUsingType UsingType
	{
		get { return (EnUsingType)m_nUsingType; }
	}

	public bool UsingRecommendationEnabled
	{
		get { return m_bUsingRecommendationEnabled; }
	}

	public CsItemType ItemType
	{
		get { return m_csItemType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public CsItemGrade ItemGrade
	{
		get { return m_csItemGrade; }
	}

	public int Grade
	{
		get { return m_csItemGrade.Grade; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int RequiredMinHeroLevel
	{
		get { return m_nRequiredMinHeroLevel; }
	}

	public int RequiredMaxHeroLevel
	{
		get { return m_nRequiredMaxHeroLevel; }
	}

	public bool Saleable
	{
		get { return m_bSaleable; }
	}

	public int SaleGold
	{
		get { return m_nSaleGold; }
	}

	public bool AutoUsable
	{
		get { return m_bAutoUsable; }
	}

	public int Value1
	{
		get { return m_nValue1; }
	}

	public int Value2
	{
		get { return m_nValue2; }
	}

	public long LongValue1
	{
		get { return m_lLongValue1; }
	}

	public long LongValue2
	{
		get { return m_lLongValue2; }
	}

	public string Image
	{
		get { return string.Format("item_{0}", m_nItemId); }
	}

	public bool IsComposable
	{
		get
		{
			if (m_nComp == -1)
			{
				CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(m_nItemId);
				
				if (csItemCompositionRecipe == null)
				{
					m_nComp = 0;
				}
				else
				{
					m_nComp = 1;
				}
			}

			return (m_nComp == 1) ? true : false; 
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsItem(WPDItem item)
	{
		m_nItemId = item.itemId;
		m_strName = CsConfiguration.Instance.GetString(item.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(item.descriptionKey);
		m_csItemType = CsGameData.Instance.GetItemType(item.itemType);
		m_csItemGrade = CsGameData.Instance.GetItemGrade(item.grade);
		m_nLevel = item.level;
		m_nRequiredMinHeroLevel = item.requiredMinHeroLevel;
		m_nRequiredMaxHeroLevel = item.requiredMaxHeroLevel;
		m_nUsingType = item.usingType;
		m_bUsingRecommendationEnabled = item.usingRecommendationEnabled;
		m_bSaleable = item.saleable;
		m_nSaleGold = item.saleGold;
		m_bAutoUsable = item.autoUsable;
		m_nValue1 = item.value1;
		m_nValue2 = item.value2;
		m_lLongValue1 = item.longValue1;
		m_lLongValue2 = item.longValue2;
	}
}
