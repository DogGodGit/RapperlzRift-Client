using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsHeroPotionAttr
{
	CsPotionAttr m_csPotionAttr;
	int m_nCount;

	//---------------------------------------------------------------------------------------------------
	public CsPotionAttr PotionAttr
	{
		get { return m_csPotionAttr; }
	}

	public int Count
	{
		get { return m_nCount; }
		set { m_nCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroPotionAttr(PDHeroPotionAttr heroPotionAttr)
	{
		m_csPotionAttr = CsGameData.Instance.GetPotionAttr(heroPotionAttr.potionAttrId);
		m_nCount = heroPotionAttr.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroPotionAttr(int nPotionAttrId, int nCount)
	{
		m_csPotionAttr = CsGameData.Instance.GetPotionAttr(nPotionAttrId);
		m_nCount = nCount;
	}
}
