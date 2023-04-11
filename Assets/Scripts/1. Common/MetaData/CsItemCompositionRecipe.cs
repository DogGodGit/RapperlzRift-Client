using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-15)
//---------------------------------------------------------------------------------------------------

public class CsItemCompositionRecipe
{
	CsItem m_csItem;
	int m_nMaterialItemId;
	int m_nMaterialItemCount;
	int m_nGold;

	//---------------------------------------------------------------------------------------------------
	public CsItem Item
	{
		get { return m_csItem; }
	}

	public int MaterialItemId
	{
		get { return m_nMaterialItemId; }
	}

	public int MaterialItemCount
	{
		get { return m_nMaterialItemCount; }
	}

	public int Gold
	{
		get { return m_nGold; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemCompositionRecipe(WPDItemCompositionRecipe itemCompositionRecipe)
	{
		m_csItem = CsGameData.Instance.GetItem(itemCompositionRecipe.itemId);
		m_nMaterialItemId = itemCompositionRecipe.materialItemId;
		m_nMaterialItemCount = itemCompositionRecipe.materialItemCount;
		m_nGold = itemCompositionRecipe.gold;
	}

}
