using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearLevel
{
	int m_nSubGearId;                       // 보조장비ID
	int m_nLevel;                           // 레벨
	CsSubGearGrade m_csSubGearGrade;        // 보조장비등급	
	int m_nNextLevelUpRequiredGold;         // 다음레벨업필요골드
	CsItem m_csItemNextGradeUp1;            // 다음승급필요아이템1
	int m_nNextGradeUpItem1Count;           // 다음승급필요아이템1수량
	CsItem m_csItemNextGradeUp2;            // 다음승급필요아이템2
	int m_nNextGradeUpItem2Count;           // 다음승급필요아이템2수량

	List<CsSubGearLevelQuality> m_listCsSubGearLevelQuality;    // 보조장비 레벨 품질 목록

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsSubGearGrade SubGearGrade
	{
		get { return m_csSubGearGrade; }
	}

	public int NextLevelUpRequiredGold
	{
		get { return m_nNextLevelUpRequiredGold; }
	}

	public CsItem NextGradeUpItem1
	{
		get { return m_csItemNextGradeUp1; }
	}

	public int NextGradeUpItem1Count
	{
		get { return m_nNextGradeUpItem1Count; }
	}

	public CsItem NextGradeUpItem2
	{
		get { return m_csItemNextGradeUp2; }
	}

	public int NextGradeUpItem2Count
	{
		get { return m_nNextGradeUpItem2Count; }
	}

	public List<CsSubGearLevelQuality> SubGearLevelQualityList
	{
		get { return m_listCsSubGearLevelQuality; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearLevel(WPDSubGearLevel subGearLevel)
	{
		m_nSubGearId = subGearLevel.subGearId;
		m_nLevel = subGearLevel.level;
		m_csSubGearGrade = CsGameData.Instance.GetSubGearGrade(subGearLevel.grade);
		m_nNextLevelUpRequiredGold = subGearLevel.nextLevelUpRequiredGold;
		m_csItemNextGradeUp1 = CsGameData.Instance.GetItem(subGearLevel.nextGradeUpItem1Id);
		m_nNextGradeUpItem1Count = subGearLevel.nextGradeUpItem1Count;
		m_csItemNextGradeUp2 = CsGameData.Instance.GetItem(subGearLevel.nextGradeUpItem2Id);
		m_nNextGradeUpItem2Count = subGearLevel.nextGradeUpItem2Count;

		m_listCsSubGearLevelQuality = new List<CsSubGearLevelQuality>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearLevelQuality GetSubGearLevelQuality(int nQuality)
	{
		for (int i = 0; i < m_listCsSubGearLevelQuality.Count; i++)
		{
			if (m_listCsSubGearLevelQuality[i].Quality == nQuality)
				return m_listCsSubGearLevelQuality[i];
		}

		return null;
	}
}
