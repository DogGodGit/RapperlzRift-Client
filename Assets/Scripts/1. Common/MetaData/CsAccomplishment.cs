using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsAccomplishment : IComparable
{
	int m_nAccomplishmentId;
	CsAccomplishmentCategory m_csAccomplishmentCategory;
	/*
	1 : 레벨 달성
	2 : 전투력 달성
	3 : 보유골드 달성
	4 : 메인퀘스트 완료
	5 : 몬스터 처치 달성
	6 : 영혼을탐하는자입장횟수
	7 : 에픽미끼사용 x회
	8 : 전설미끼사용 x회
	9 : 국가전승리 횟수
	10 : 국가전적처치수
	11 : 국가전사령관처치
	12 : 국가전즉시부활(무료유료)
	13 : 획득메인장비등급
	14 : 강화된메인장비장착
	15 : 카드조합모두활성화
	*/
	int m_nType;
	string m_strName;
	string m_strObjectiveText;
	long m_lObjectiveValue;
	int m_nRewardType;  // 1:칭호, 2:아이템
	int m_nRewardTitleId;
	CsItemReward m_csItemReward;
	CsAccomplishmentPointReward m_csAccomplishmentPointReward;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int AccomplishmentId
	{
		get { return m_nAccomplishmentId; }
	}

	public CsAccomplishmentCategory AccomplishmentCategory
	{
		get { return m_csAccomplishmentCategory; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ObjectiveText
	{
		get { return m_strObjectiveText; }
	}

	public long ObjectiveValue
	{
		get { return m_lObjectiveValue; }
	}

	public int RewardType
	{
		get { return m_nRewardType; }
	}

	public int RewardTitleId
	{
		get { return m_nRewardTitleId; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	public CsAccomplishmentPointReward AccomplishmentPointReward
	{
		get { return m_csAccomplishmentPointReward; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishment(WPDAccomplishment accomplishment)
	{
		m_nAccomplishmentId = accomplishment.accomplishmentId;
		m_csAccomplishmentCategory = CsGameData.Instance.GetAccomplishmentCategory(accomplishment.categoryId);
		m_nType = accomplishment.type;
		m_strName = CsConfiguration.Instance.GetString(accomplishment.nameKey);
		m_strObjectiveText = CsConfiguration.Instance.GetString(accomplishment.objectiveTextKey);
		m_lObjectiveValue = accomplishment.objectiveValue;
		m_nRewardType = accomplishment.rewardType; 
		m_nRewardTitleId = accomplishment.rewardTitleId;
		m_csItemReward = CsGameData.Instance.GetItemReward(accomplishment.itemRewardId);
		m_nSortNo = accomplishment.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsAccomplishment)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
