using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroMainGearRefinementAttr
{
	int m_nIndex;
	CsMainGearOptionAttrGrade m_nGrade;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public CsMainGearOptionAttrGrade Grade
	{
		get { return m_nGrade; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public int Value
	{
		get { return m_csAttrValueInfo.Value; }
	}

	public int BattlePower
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGearRefinementAttr(PDHeroMainGearRefinementAttr heroMainGearRefinementAttr)
	{
		m_nIndex = heroMainGearRefinementAttr.index;
		m_nGrade = CsGameData.Instance.GetMainGearOptionAttrGrade(heroMainGearRefinementAttr.grade);
		m_csAttr = CsGameData.Instance.GetAttr(heroMainGearRefinementAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(heroMainGearRefinementAttr.attrValueId);
	}
}
