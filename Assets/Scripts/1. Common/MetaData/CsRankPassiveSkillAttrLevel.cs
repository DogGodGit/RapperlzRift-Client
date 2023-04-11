using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankPassiveSkillAttrLevel
{
	int m_nSkillId;
	int m_nLevel;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkillAttrLevel(WPDRankPassiveSkillAttrLevel rankPassiveSkillAttrLevel)
	{
		m_nSkillId = rankPassiveSkillAttrLevel.skillId;
		m_csAttr = CsGameData.Instance.GetAttr(rankPassiveSkillAttrLevel.attrId);
		m_nLevel = rankPassiveSkillAttrLevel.level;
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(rankPassiveSkillAttrLevel.attrValueId);
	}
}
