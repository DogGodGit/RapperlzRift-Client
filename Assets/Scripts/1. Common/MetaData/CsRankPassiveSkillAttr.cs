using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankPassiveSkillAttr
{
	int m_nSkillId;
	CsAttr m_csAttr;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkillAttr(WPDRankPassiveSkillAttr rankPassiveSkillAttr)
	{
		m_nSkillId = rankPassiveSkillAttr.skillId;
		m_csAttr = CsGameData.Instance.GetAttr(rankPassiveSkillAttr.attrId);
	}

}
