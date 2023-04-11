using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteMonster
{
	int m_nEliteMonsterId;
	CsEliteMonsterMaster m_csEliteMonsterMaster;
	int m_nStarGrade;
	CsAttr m_csAttr;
	CsMonsterArrange m_csMonsterArrange;

	List<CsEliteMonsterKillAttrValue> m_listCsEliteMonsterKillAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int EliteMonsterId
	{
		get { return m_nEliteMonsterId; }
	}

	public CsEliteMonsterMaster EliteMonsterMaster
	{
		get { return m_csEliteMonsterMaster; }
	}

	public int StarGrade
	{
		get { return m_nStarGrade; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsMonsterArrange MonsterArrange
	{
		get { return m_csMonsterArrange; }
	}

	public List<CsEliteMonsterKillAttrValue> EliteMonsterKillAttrValueList
	{
		get { return m_listCsEliteMonsterKillAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonster(WPDEliteMonster eliteMonster)
	{
		m_nEliteMonsterId = eliteMonster.eliteMonsterId;
		m_csEliteMonsterMaster = CsGameData.Instance.GetEliteMonsterMaster(eliteMonster.eliteMonsterMasterId);
		m_nStarGrade = eliteMonster.starGrade;
		m_csAttr = CsGameData.Instance.GetAttr(eliteMonster.attrId);
		m_csMonsterArrange = CsGameData.Instance.GetMonsterArrange(eliteMonster.monsterArrangeId);

		m_listCsEliteMonsterKillAttrValue = new List<CsEliteMonsterKillAttrValue>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterKillAttrValue GetEliteMonsterKillAttrValue(int nKillCount)
	{
		for (int i = 0; i < m_listCsEliteMonsterKillAttrValue.Count; i++)
		{
			if (m_listCsEliteMonsterKillAttrValue[i].KillCount > nKillCount)
			{
				if (i == 0)
				{
					return null;
				}
				else
				{
					return m_listCsEliteMonsterKillAttrValue[i - 1];
				}
			}
		}

		return m_listCsEliteMonsterKillAttrValue[m_listCsEliteMonsterKillAttrValue.Count - 1];
	}
}
