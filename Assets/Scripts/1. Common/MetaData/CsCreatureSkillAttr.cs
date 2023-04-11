using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureSkillAttr
{
	int m_nSkillId;
	CsCreatureSkillGrade m_csCreatureSkillGrade;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public CsCreatureSkillGrade CreatureSkillGrade
	{
		get { return m_csCreatureSkillGrade; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillAttr(WPDCreatureSkillAttr creatureSkillAttr)
	{
		m_nSkillId = creatureSkillAttr.skillId;
		m_csCreatureSkillGrade = CsGameData.Instance.GetCreatureSkillGrade(creatureSkillAttr.skillGrade);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(creatureSkillAttr.attrValueId);
	}
}
