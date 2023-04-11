using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-21)
//---------------------------------------------------------------------------------------------------

public class CsHeroOrdealQuest
{
	CsOrdealQuest m_csOrdealQuest;
	bool m_bCompleted;
	List<CsHeroOrdealQuestSlot> m_listCsHeroOrdealQuestSlot;

	//---------------------------------------------------------------------------------------------------
	public CsOrdealQuest OrdealQuest
	{
		get { return m_csOrdealQuest; }
	}

	public bool Completed
	{
		get { return m_bCompleted; }
		set { m_bCompleted = value; }
	}

	public List<CsHeroOrdealQuestSlot> HeroOrdealQuestSlotList
	{
		get { return m_listCsHeroOrdealQuestSlot; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroOrdealQuest(PDHeroOrdealQuest heroOrdealQuest)
	{
		m_csOrdealQuest = CsGameData.Instance.GetOrdealQuest(heroOrdealQuest.questNo);
		m_bCompleted = heroOrdealQuest.completed;

		m_listCsHeroOrdealQuestSlot = new List<CsHeroOrdealQuestSlot>();

		for (int i = 0; i < heroOrdealQuest.slots.Length; i++)
		{
			m_listCsHeroOrdealQuestSlot.Add(new CsHeroOrdealQuestSlot(m_csOrdealQuest, heroOrdealQuest.slots[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroOrdealQuestSlot GetHeroOrdealQuestSlot(int nIndex)
	{
		for (int i = 0; i < m_listCsHeroOrdealQuestSlot.Count; i++)
		{
			if (m_listCsHeroOrdealQuestSlot[i].Index == nIndex)
				return m_listCsHeroOrdealQuestSlot[i];
		}

		return null;
	}
}
