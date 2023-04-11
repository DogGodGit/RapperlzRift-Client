using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-04)
//---------------------------------------------------------------------------------------------------

public class CsHeroMainGearOptionAttr
{
	int m_nIndex;
	CsMainGearOptionAttrGrade m_csMainGearOptionAttrGrade;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public CsMainGearOptionAttrGrade MainGearOptionAttrGrade
	{
		get { return m_csMainGearOptionAttrGrade; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public int Value
	{
		get { return m_csAttrValueInfo.Value; }
	}

	public long AttrValueId
	{
		get { return m_csAttrValueInfo.AttrValueId; }
	}

	public int BattlePowerValue
	{
		get { return m_csAttr.BattlePowerFactor * m_csAttrValueInfo.Value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGearOptionAttr(PDHeroMainGearOptionAttr heroMainGearOptionAttr)
	{
		m_nIndex = heroMainGearOptionAttr.index;
		m_csMainGearOptionAttrGrade = CsGameData.Instance.GetMainGearOptionAttrGrade(heroMainGearOptionAttr.grade);
		m_csAttr = CsGameData.Instance.GetAttr(heroMainGearOptionAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(heroMainGearOptionAttr.attrValueId);
	}

}
