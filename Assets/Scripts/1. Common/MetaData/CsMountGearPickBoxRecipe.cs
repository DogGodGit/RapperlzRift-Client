using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearPickBoxRecipe
{
	CsItem m_csItem;
	int m_nRequiredHeroLevel;
	int m_nGold;
	bool m_bOwned;
	CsItem m_csMaterialItem1Id;
	int m_nMaterialItem1Count;
	CsItem m_csMaterialItem2Id;
	int m_nMaterialItem2Count;
	CsItem m_csMaterialItem3Id;
	int m_nMaterialItem3Count;
	CsItem m_csMaterialItem4Id;
	int m_nMaterialItem4Count;

	//---------------------------------------------------------------------------------------------------
	public CsItem Item
	{
		get { return m_csItem; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int Gold
	{
		get { return m_nGold; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	public CsItem MaterialItem1
	{
		get { return m_csMaterialItem1Id; }
	}

	public int MaterialItem1Count
	{
		get { return m_nMaterialItem1Count; }
	}

	public CsItem MaterialItem2
	{
		get { return m_csMaterialItem2Id; }
	}

	public int MaterialItem2Count
	{
		get { return m_nMaterialItem2Count; }
	}

	public CsItem MaterialItem3
	{
		get { return m_csMaterialItem3Id; }
	}

	public int MaterialItem3Count
	{
		get { return m_nMaterialItem3Count; }
	}

	public CsItem MaterialItem4
	{
		get { return m_csMaterialItem4Id; }
	}

	public int MaterialItem4Count
	{
		get { return m_nMaterialItem4Count; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearPickBoxRecipe(WPDMountGearPickBoxRecipe mountGearPickBoxRecipe)
	{
		m_csItem = CsGameData.Instance.GetItem(mountGearPickBoxRecipe.itemId);
		m_nRequiredHeroLevel = mountGearPickBoxRecipe.requiredHeroLevel;
		m_nGold = mountGearPickBoxRecipe.gold;
		m_bOwned = mountGearPickBoxRecipe.owned;
		m_csMaterialItem1Id = CsGameData.Instance.GetItem(mountGearPickBoxRecipe.materialItem1Id);
		m_nMaterialItem1Count = mountGearPickBoxRecipe.materialItem1Count;
		m_csMaterialItem2Id = CsGameData.Instance.GetItem(mountGearPickBoxRecipe.materialItem2Id);
		m_nMaterialItem2Count = mountGearPickBoxRecipe.materialItem2Count;
		m_csMaterialItem3Id = CsGameData.Instance.GetItem(mountGearPickBoxRecipe.materialItem3Id);
		m_nMaterialItem3Count = mountGearPickBoxRecipe.materialItem3Count;
		m_csMaterialItem4Id = CsGameData.Instance.GetItem(mountGearPickBoxRecipe.materialItem4Id);
		m_nMaterialItem4Count = mountGearPickBoxRecipe.materialItem4Count;
	}
}
