using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsHeroSubGearSoulstoneSocket
{
	int m_nIndex;
	CsItem m_csItem;
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
		set { m_csItem = value; }
	}

	public int BattlePowerValue
	{
		get { return m_csAttr.BattlePowerFactor * CsGameData.Instance.GetAttrValueInfo(m_csItem.LongValue1).Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearSoulstoneSocket(PDHeroSubGearSoulstoneSocket heroSubGearSoulstoneSocket)
	{
		m_nIndex = heroSubGearSoulstoneSocket.index;
		m_csItem = CsGameData.Instance.GetItem(heroSubGearSoulstoneSocket.itemId);
		m_csAttr = CsGameData.Instance.GetAttr(m_csItem.Value1);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearSoulstoneSocket(int nIndex, int nItemId)
	{
		m_nIndex = nIndex;
		m_csItem = CsGameData.Instance.GetItem(nItemId);
		m_csAttr = CsGameData.Instance.GetAttr(m_csItem.Value1);
	}
}
