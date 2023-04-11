using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroMountGearOptionAttr
{
	int m_nIndex;
	CsMountGearOptionAttrGrade m_csMountGearOptionAttrGrade;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public CsMountGearOptionAttrGrade MountGearOptionAttrGrade
	{
		get { return m_csMountGearOptionAttrGrade; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public int BattlePowerValue
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMountGearOptionAttr(PDHeroMountGearOptionAttr heroMountGearOptionAttr)
	{
		m_nIndex = heroMountGearOptionAttr.index;
		m_csMountGearOptionAttrGrade = CsGameData.Instance.GetMountGearOptionAttrGrade(heroMountGearOptionAttr.grade);
		m_csAttr = CsGameData.Instance.GetAttr(heroMountGearOptionAttr.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(heroMountGearOptionAttr.attrValueId);
	}

	//---------------------------------------------------------------------------------------------------
	public void Update(PDHeroMountGearOptionAttr heroMountGearOptionAttr)
	{
		if (m_nIndex == heroMountGearOptionAttr.index)
		{ 
			m_csMountGearOptionAttrGrade = CsGameData.Instance.GetMountGearOptionAttrGrade(heroMountGearOptionAttr.grade);
			m_csAttr = CsGameData.Instance.GetAttr(heroMountGearOptionAttr.attrId);
			m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(heroMountGearOptionAttr.attrValueId);
		}
	}

}
