using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureSkillSlotOpenRecipe
{
	int m_nSlotCount;
	CsItem m_csItemRequired;
	int m_nRequiredItemCount;

	//---------------------------------------------------------------------------------------------------
	public int SlotCount
	{
		get { return m_nSlotCount; }
	}

	public CsItem ItemRequired
	{
		get { return m_csItemRequired; }
	}

	public int RequiredItemCount
	{
		get { return m_nRequiredItemCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillSlotOpenRecipe(WPDCreatureSkillSlotOpenRecipe creatureSkillSlotOpenRecipe)
	{
		m_nSlotCount = creatureSkillSlotOpenRecipe.slotCount;
		m_csItemRequired = CsGameData.Instance.GetItem(creatureSkillSlotOpenRecipe.requiredItemId);
		m_nRequiredItemCount = creatureSkillSlotOpenRecipe.requiredItemCount;
	}
}
