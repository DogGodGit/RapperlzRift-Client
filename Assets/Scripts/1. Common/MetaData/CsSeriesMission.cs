using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------
/*
1 : 낚시퀘스트
2 : 현상금사냥퀘스트
3 : 오시리스의방(골드던전)
4 : 수련동굴(경험치던전)
5 : 검투대회(PVP)
6 : 공포의제단
7 : 차원습격퀘스트
8 : 의문의상자퀘스트
9 : 보급지원
*/

public enum EnSeriesMissionType
{
	FishingQuest = 1,
	BountyHunterQuest = 2,
	GoldDungeon = 3,
	ExpDungeon = 4,
	PVP = 5,
    DiemnsionRaidQuest = 7, 
	MysteryBoxQuest = 8,
    SupplySupportQuest = 9, 
}

public class CsSeriesMission : IComparable
{
	int m_nMissionId;
	string m_strName;
	string m_strDescription;
	int m_nSortNo;

	List<CsSeriesMissionStep> m_listCsSeriesMissionStep;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public EnSeriesMissionType SeriesMissionType
	{
		get { return (EnSeriesMissionType)m_nMissionId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsSeriesMissionStep> CsSeriesMissionStepList
	{
		get { return m_listCsSeriesMissionStep; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSeriesMission(WPDSeriesMission seriesMission)
	{
		m_nMissionId = seriesMission.missionId;
		m_strName = CsConfiguration.Instance.GetString(seriesMission.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(seriesMission.descriptionKey);
		m_nSortNo = seriesMission.sortNo;

		m_listCsSeriesMissionStep = new List<CsSeriesMissionStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSeriesMissionStep GetSeriesMissionStep(int nStep)
	{
		for (int i = 0; i < m_listCsSeriesMissionStep.Count; i++)
		{
			if (m_listCsSeriesMissionStep[i].Step == nStep)
				return m_listCsSeriesMissionStep[i];
		}

		return null;
	}


	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsSeriesMission)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
