using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsTitle : IComparable
{
	int m_nTitleId;
	string m_strName;
	CsTitleType m_csTitleType;
	CsTitleGrade m_csTitleGrade;
	string m_strAcquisitionText;
	string m_strBackgroundImageName;
	int m_nLifetime;
	int m_nActivationRequiredAccomplishmentLevel;
	int m_nSortNo;

	List<CsTitleActiveAttr> m_listCsTitleActiveAttr;
	List<CsTitlePassiveAttr> m_listCsTitlePassiveAttr;

	//---------------------------------------------------------------------------------------------------
	public int TitleId
	{
		get { return m_nTitleId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsTitleType TitleType
	{
		get { return m_csTitleType; }
	}

	public CsTitleGrade TitleGrade
	{
		get { return m_csTitleGrade; }
	}

	public string AcquisitionText
	{
		get { return m_strAcquisitionText; }
	}

	public string BackgroundImageName
	{
		get { return m_strBackgroundImageName; }
	}

	public int Lifetime
	{
		get { return m_nLifetime; }
	}

	public List<CsTitleActiveAttr> TitleActiveAttrList
	{
		get { return m_listCsTitleActiveAttr; }
	}

	public List<CsTitlePassiveAttr> TitlePassiveAttrList
	{
		get { return m_listCsTitlePassiveAttr; }
	}

	public int ActivationRequiredAccomplishmentLevel
	{
		get { return m_nActivationRequiredAccomplishmentLevel; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitle(WPDTitle title)
	{
		m_nTitleId = title.titleId;
		m_strName = CsConfiguration.Instance.GetString(title.nameKey);
		m_csTitleType = CsGameData.Instance.GetTitleType(title.type);
		m_csTitleGrade = CsGameData.Instance.GetTitleGrade(title.grade);
		m_strAcquisitionText = CsConfiguration.Instance.GetString(title.acquisitionTextKey);
		m_strBackgroundImageName = title.backgroundImageName;
		m_nLifetime = title.lifetime;


		m_listCsTitleActiveAttr = new List<CsTitleActiveAttr>();
		m_listCsTitlePassiveAttr = new List<CsTitlePassiveAttr>();
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsTitle)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
