using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreatureSkill
{
	int m_nSlotIndex;
	CsCreatureSkill m_csCreatureSkill;
	CsCreatureSkillGrade m_csCreatureSkillGrade;

	//---------------------------------------------------------------------------------------------------
	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public CsCreatureSkill CreatureSkill
	{
		get { return m_csCreatureSkill; }
	}

	public CsCreatureSkillGrade CreatureSkillGrade
	{
		get { return m_csCreatureSkillGrade; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreatureSkill(PDHeroCreatureSkill heroCreatureSkill)
	{
		m_nSlotIndex = heroCreatureSkill.slotIndex;
		m_csCreatureSkill = CsGameData.Instance.GetCreatureSkill(heroCreatureSkill.skillId);
		m_csCreatureSkillGrade = CsGameData.Instance.GetCreatureSkillGrade(heroCreatureSkill.grade);
	}
}
