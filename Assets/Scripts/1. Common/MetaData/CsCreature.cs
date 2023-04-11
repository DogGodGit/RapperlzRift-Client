using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreature
{
	int m_nCreatureId;
	CsCreatureCharacter m_csCreatureCharacter;
	CsCreatureGrade m_csCreatureGrade;
	int m_nMinQuality;
	int m_nMaxQuality;

	List<CsCreatureBaseAttrValue> m_listCsCreatureBaseAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CreatureId
	{
		get { return m_nCreatureId; }
	}

	public CsCreatureCharacter CreatureCharacter
	{
		get { return m_csCreatureCharacter; }
	}

	public CsCreatureGrade CreatureGrade
	{
		get { return m_csCreatureGrade; }
	}

	public int MinQuality
	{
		get { return m_nMinQuality; }
	}

	public int MaxQuality
	{
		get { return m_nMaxQuality; }
	}

	public List<CsCreatureBaseAttrValue> CreatureBaseAttrValueList
	{
		get { return m_listCsCreatureBaseAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreature(WPDCreature creature)
	{
		m_nCreatureId = creature.creatureId;
		m_csCreatureCharacter = CsGameData.Instance.GetCreatureCharacter(creature.creatureCharacterId);
		m_csCreatureGrade = CsGameData.Instance.GetCreatureGrade(creature.grade);
		m_nMinQuality = creature.minQuality;
		m_nMaxQuality = creature.maxQuality;

		m_listCsCreatureBaseAttrValue = new List<CsCreatureBaseAttrValue>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureBaseAttrValue GetCreatureBaseAttrValue(int nAttrId)
	{
		for (int i = 0; i < m_listCsCreatureBaseAttrValue.Count; i++)
		{
			if (m_listCsCreatureBaseAttrValue[i].CreatureBaseAttr.Attr.AttrId == nAttrId)
				return m_listCsCreatureBaseAttrValue[i];
		}

		return null;
	}
}
