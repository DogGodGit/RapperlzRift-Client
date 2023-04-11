using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiography : IComparable
{
	int m_nBiographyId;
	string m_strTitle;
	string m_strName;
	string m_strDescription;
	string m_strOpenConditionText;
	string m_strTargetTitle;
	CsItem m_csItemRequired;
	int m_nSortNo;

	List<CsBiographyReward> m_listCsBiographyReward;
	List<CsBiographyQuest> m_listCsBiographyQuest;

	//---------------------------------------------------------------------------------------------------
	public int BiographyId
	{
		get { return m_nBiographyId; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string OpenConditionText
	{
		get { return m_strOpenConditionText; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public CsItem RequiredItem
	{
		get { return m_csItemRequired; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsBiographyReward> BiographyRewardList
	{
		get { return m_listCsBiographyReward; }
	}

	public List<CsBiographyQuest> BiographyQuestList
	{
		get { return m_listCsBiographyQuest; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiography(WPDBiography biography)
	{
		m_nBiographyId = biography.biographyId;
		m_strTitle = CsConfiguration.Instance.GetString(biography.titleKey);
		m_strName = CsConfiguration.Instance.GetString(biography.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(biography.descriptionKey);
		m_strOpenConditionText = CsConfiguration.Instance.GetString(biography.openConditionTextKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(biography.targetTitleKey);
		m_csItemRequired = CsGameData.Instance.GetItem(biography.requiredItemId);
		m_nSortNo = biography.sortNo;

		m_listCsBiographyReward = new List<CsBiographyReward>();
		m_listCsBiographyQuest = new List<CsBiographyQuest>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuest GetBiographyQuest(int nQuestNo)
	{
		for (int i = 0; i < m_listCsBiographyQuest.Count; i++)
		{
			if (m_listCsBiographyQuest[i].QuestNo == nQuestNo)
				return m_listCsBiographyQuest[i];
		}

		return null;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsBiography)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
