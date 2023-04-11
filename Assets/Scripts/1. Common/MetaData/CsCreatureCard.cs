using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCard
{
	int m_nCreatureCardId;
	string m_strName;
	string m_strDescription;
	CsCreatureCardCategory m_csCreatureCardCategory;
	CsCreatureCardGrade m_csCreatureCardGrade;
	int m_nAttack;
	int m_nLife;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int CreatureCardId
	{
		get { return m_nCreatureCardId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public CsCreatureCardCategory CreatureCardCategory
	{
		get { return m_csCreatureCardCategory; }
	}

	public CsCreatureCardGrade CreatureCardGrade
	{
		get { return m_csCreatureCardGrade; }
	}

	public int Attack
	{
		get { return m_nAttack; }
	}

	public int Life
	{
		get { return m_nLife; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValue
	{
		get { return m_csAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCard(WPDCreatureCard creatureCard)
	{
		m_nCreatureCardId = creatureCard.creatureCardId;
		m_strName = CsConfiguration.Instance.GetString(creatureCard.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(creatureCard.descriptionKey);
		m_csCreatureCardCategory = CsGameData.Instance.GetCreatureCardCategory(creatureCard.categoryId);
		m_csCreatureCardGrade = CsGameData.Instance.GetCreatureCardGrade(creatureCard.grade);
		m_nAttack = creatureCard.attack;
		m_nLife = creatureCard.life;
		m_csAttr = CsGameData.Instance.GetAttr(creatureCard.attrId);
		m_csAttrValue = CsGameData.Instance.GetAttrValueInfo(creatureCard.attrValueId);
	}
}
