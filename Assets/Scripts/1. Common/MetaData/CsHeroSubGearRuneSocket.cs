using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsHeroSubGearRuneSocket
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
	}

	public int BattlePowerValue
	{
		get { return m_csAttr.BattlePowerFactor * CsGameData.Instance.GetAttrValueInfo(m_csItem.LongValue1).Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearRuneSocket(PDHeroSubGearRuneSocket heroSubGearRuneSocket)
	{
		m_nIndex = heroSubGearRuneSocket.index;
		m_csItem = CsGameData.Instance.GetItem(heroSubGearRuneSocket.itemId);
		m_csAttr = CsGameData.Instance.GetAttr(m_csItem.Value1);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearRuneSocket(int nIndex, int nItemId)
	{
		m_nIndex = nIndex;
		m_csItem = CsGameData.Instance.GetItem(nItemId);
		m_csAttr = CsGameData.Instance.GetAttr(m_csItem.Value1);
	}
}
