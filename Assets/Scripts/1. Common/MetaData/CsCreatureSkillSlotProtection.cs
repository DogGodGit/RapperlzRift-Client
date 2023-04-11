using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureSkillSlotProtection
{
	int m_nProtectionCount;
	int m_nRequiredSkillCount;
	int m_nRequiredItemCount;

	//---------------------------------------------------------------------------------------------------
	public int ProtectionCount
	{
		get { return m_nProtectionCount; }
	}

	public int RequiredSkillCount
	{
		get { return m_nRequiredSkillCount; }
	}

	public int RequiredItemCount
	{
		get { return m_nRequiredItemCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillSlotProtection(WPDCreatureSkillSlotProtection creatureSkillSlotProtection)
	{
		m_nProtectionCount = creatureSkillSlotProtection.protectionCount;
		m_nRequiredSkillCount = creatureSkillSlotProtection.requiredSkillCount;
		m_nRequiredItemCount = creatureSkillSlotProtection.requiredItemCount;
	}
}
