using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsSupplySupportQuestCart : IComparable
{
	int m_nCartId;
	CsItemReward m_csItemRewardDestruction;
	int m_nSortNo;

	List<CsSupplySupportQuestReward> m_listCsSupplySupportQuestReward;

	//---------------------------------------------------------------------------------------------------
	public int CartId
	{
		get { return m_nCartId; }
	}

	public CsItemReward DestructionItemReward
	{
		get { return m_csItemRewardDestruction; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsSupplySupportQuestReward> SupplySupportQuestRewardList
	{
		get { return m_listCsSupplySupportQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestCart(WPDSupplySupportQuestCart supplySupportQuestCart)
	{
		m_nCartId = supplySupportQuestCart.cartId;
		m_csItemRewardDestruction = CsGameData.Instance.GetItemReward(supplySupportQuestCart.destructionItemRewardId);
		m_nSortNo = supplySupportQuestCart.sortNo;

		m_listCsSupplySupportQuestReward = new List<CsSupplySupportQuestReward>();
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsSupplySupportQuestCart)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
