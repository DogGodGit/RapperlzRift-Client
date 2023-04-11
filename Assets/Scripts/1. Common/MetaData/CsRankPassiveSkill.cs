using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankPassiveSkill : IComparable
{
	int m_nSkillId;
	int m_nRequiredRankNo;
	string m_strName;
	string m_strDescription;
	string m_strImageName;
	int m_nSortNo;

	List<CsRankPassiveSkillAttr> m_listCsRankPassiveSkillAttr;
	List<CsRankPassiveSkillLevel> m_listCsRankPassiveSkillLevel;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int RequiredRankNo
	{
		get { return m_nRequiredRankNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsRankPassiveSkillAttr> RankPassiveSkillAttrList
	{
		get { return m_listCsRankPassiveSkillAttr; }
	}

	public List<CsRankPassiveSkillLevel> RankPassiveSkillLevelList
	{
		get { return m_listCsRankPassiveSkillLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkill(WPDRankPassiveSkill rankPassiveSkill)
	{
		m_nSkillId = rankPassiveSkill.skillId;
		m_nRequiredRankNo = rankPassiveSkill.requiredRankNo;
		m_strName = CsConfiguration.Instance.GetString(rankPassiveSkill.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(rankPassiveSkill.descriptionKey);
		m_strImageName = rankPassiveSkill.imageName;
		m_nSortNo = rankPassiveSkill.sortNo;

		m_listCsRankPassiveSkillAttr = new List<CsRankPassiveSkillAttr>();
		m_listCsRankPassiveSkillLevel = new List<CsRankPassiveSkillLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkillLevel GetRankPassiveSkillLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsRankPassiveSkillLevel.Count; i++)
		{
			if (m_listCsRankPassiveSkillLevel[i].Level == nLevel)
				return m_listCsRankPassiveSkillLevel[i];
		}

		return null;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsRankPassiveSkill)obj).SortNo);
	}
	#endregion Interface(IComparable) implement

}
