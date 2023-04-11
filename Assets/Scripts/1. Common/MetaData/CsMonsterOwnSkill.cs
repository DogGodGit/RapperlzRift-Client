using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterOwnSkill
{
	int m_nMonsterId;
	int m_nSkillId;

	//---------------------------------------------------------------------------------------------------
	public int MonsterId
	{
		get { return m_nMonsterId; }
	}

	public int SkillId
	{
		get { return m_nSkillId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterOwnSkill(WPDMonsterOwnSkill monsterOwnSkill)
	{
		m_nMonsterId = monsterOwnSkill.monsterId;
		m_nSkillId = monsterOwnSkill.skillId;
	}
}
