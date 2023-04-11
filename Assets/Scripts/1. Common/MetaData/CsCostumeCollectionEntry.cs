using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeCollectionEntry : IComparable
{
	int m_nCostumeCollectionId;
	CsCostume m_csCostume;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int CostumeCollectionId
	{
		get { return m_nCostumeCollectionId; }
	}

	public CsCostume Costume
	{
		get { return m_csCostume; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeCollectionEntry(WPDCostumeCollectionEntry costumeCollectionEntry)
	{
		m_nCostumeCollectionId = costumeCollectionEntry.costumeCollectionId;
		m_csCostume = CsGameData.Instance.GetCostume(costumeCollectionEntry.costumeId);
		m_nSortNo = costumeCollectionEntry.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsCostumeCollectionEntry)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
