using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-15)
//---------------------------------------------------------------------------------------------------

public class CsInventorySlotExtendRecipe : IComparable
{
	int m_nSlotCount;
	int m_nDia;

	//---------------------------------------------------------------------------------------------------
	public int SlotCount
	{
		get { return m_nSlotCount; }
	}

	public int Dia
	{
		get { return m_nDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlotExtendRecipe(WPDInventorySlotExtendRecipe inventorySlotExtendRecipe)
	{
		m_nSlotCount = inventorySlotExtendRecipe.slotCount;
		m_nDia = inventorySlotExtendRecipe.dia;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSlotCount.CompareTo(((CsInventorySlotExtendRecipe)obj).SlotCount);
	}
	#endregion Interface(IComparable) implement

}
