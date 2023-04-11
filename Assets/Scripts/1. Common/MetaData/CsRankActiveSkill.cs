using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsRankActiveSkill : IComparable
{
	int m_nSkillId;
	int m_nRequiredRankNo;
	string m_strName;
	string m_strDescription;
	string m_strImageName;
	int m_nType;
	float m_flCoolTime;
	float m_flCastRange;
	CsAbnormalState m_csAbnormalState;
	int m_nSortNo;

	List<CsRankActiveSkillLevel> m_listCsRankActiveSkillLevel;

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

	public int Type
	{
		get { return m_nType; }
	}

	public float CoolTime
	{
		get { return m_flCoolTime; }
	}

	public float CastRange
	{
		get { return m_flCastRange; }
	}

	public CsAbnormalState AbnormalState
	{
		get { return m_csAbnormalState; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsRankActiveSkillLevel> RankActiveSkillLevelList
	{
		get { return m_listCsRankActiveSkillLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankActiveSkill(WPDRankActiveSkill rankActiveSkill)
	{
		m_nSkillId = rankActiveSkill.skillId;
		m_nRequiredRankNo = rankActiveSkill.requiredRankNo;
		m_strName = CsConfiguration.Instance.GetString(rankActiveSkill.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(rankActiveSkill.descriptionKey);
		m_strImageName = rankActiveSkill.imageName;
		m_nType = rankActiveSkill.type;
		m_flCoolTime = rankActiveSkill.coolTime;
		m_flCastRange = rankActiveSkill.castRange;
		m_csAbnormalState = CsGameData.Instance.GetAbnormalState(rankActiveSkill.abnormalStateId);
		m_nSortNo = rankActiveSkill.sortNo;

		m_listCsRankActiveSkillLevel = new List<CsRankActiveSkillLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankActiveSkillLevel GetRankActiveSkillLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsRankActiveSkillLevel.Count; i++)
		{
			if (m_listCsRankActiveSkillLevel[i].Level == nLevel)
				return m_listCsRankActiveSkillLevel[i];
		}

		return null;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsRankActiveSkill)obj).SortNo);
	}
	#endregion Interface(IComparable) implement

}
