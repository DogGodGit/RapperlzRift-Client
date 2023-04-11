using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseSlotExtendRecipe
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
	public CsWarehouseSlotExtendRecipe(WPDWarehouseSlotExtendRecipe warehouseSlotExtendRecipe)
	{
		m_nSlotCount = warehouseSlotExtendRecipe.slotCount;
		m_nDia = warehouseSlotExtendRecipe.dia;
	}
}
