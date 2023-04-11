using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankPassiveSkillLevel
{
	int m_nSkillId;
	int m_nLevel;
	string m_strEffectDescription;
	long m_lNextLevelUpRequiredGold;
	int m_nNextLevelUpRequiredSpiritStone;

	List<CsRankPassiveSkillAttrLevel> m_listCsRankPassiveSkillAttrLevel;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public string EffectDescription
	{
		get { return m_strEffectDescription; }
	}

	public long NextLevelUpRequiredGold
	{
		get { return m_lNextLevelUpRequiredGold; }
	}

	public int NextLevelUpRequiredSpiritStone
	{
		get { return m_nNextLevelUpRequiredSpiritStone; }
	}

	public List<CsRankPassiveSkillAttrLevel> RankPassiveSkillAttrLevelList
	{
		get { return m_listCsRankPassiveSkillAttrLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkillLevel(WPDRankPassiveSkillLevel rankPassiveSkillLevel)
	{
		m_nSkillId = rankPassiveSkillLevel.skillId;
		m_nLevel = rankPassiveSkillLevel.level;
		m_strEffectDescription = CsConfiguration.Instance.GetString(rankPassiveSkillLevel.effectDescriptionKey);
		m_lNextLevelUpRequiredGold = rankPassiveSkillLevel.nextLevelUpRequiredGold;
		m_nNextLevelUpRequiredSpiritStone = rankPassiveSkillLevel.nextLevelUpRequiredSpiritStone;

		m_listCsRankPassiveSkillAttrLevel = new List<CsRankPassiveSkillAttrLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkillAttrLevel GetRankPassiveSkillAttrLevel(int nAttrId)
	{
		for (int i = 0; i < m_listCsRankPassiveSkillAttrLevel.Count; i++)
		{
			if (m_listCsRankPassiveSkillAttrLevel[i].Attr.AttrId == nAttrId)
				return m_listCsRankPassiveSkillAttrLevel[i];
		}

		return null;
	}

}
