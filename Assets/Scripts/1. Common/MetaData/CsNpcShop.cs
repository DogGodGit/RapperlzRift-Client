using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-02)
//---------------------------------------------------------------------------------------------------

public class CsNpcShop
{
	int m_nShopId;
	string m_strName;
	int m_nNpcId;

	List<CsNpcShopCategory> m_listCsNpcShopCategory;

	//---------------------------------------------------------------------------------------------------
	public int ShopId
	{
		get { return m_nShopId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public List<CsNpcShopCategory> NpcShopCategoryList
	{
		get { return m_listCsNpcShopCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShop(WPDNpcShop npcShop)
	{
		m_nShopId = npcShop.shopId;
		m_strName = CsConfiguration.Instance.GetString(npcShop.nameKey);
		m_nNpcId = npcShop.npcId;

		m_listCsNpcShopCategory = new List<CsNpcShopCategory>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShopCategory GetNpcShopCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsNpcShopCategory.Count; i++)
		{
			if (m_listCsNpcShopCategory[i].CategoryId == nCategoryId)
				return m_listCsNpcShopCategory[i];
		}

		return null;
	}

}
