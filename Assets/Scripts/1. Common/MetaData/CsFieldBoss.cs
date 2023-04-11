using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsFieldBoss : IComparable
{
	int m_nFieldBossId;
	string m_strName;
	string m_strImageName;
	CsMonsterArrange m_csMonsterArrange;
	CsContinent m_csContinent;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	CsItemReward m_csItemReward;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int FieldBossId
	{
		get { return m_nFieldBossId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public CsMonsterArrange MonsterArrange
	{
		get { return m_csMonsterArrange; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFieldBoss(WPDFieldBoss fieldBoss)
	{
		m_nFieldBossId = fieldBoss.fieldBossId;
		m_strName = CsConfiguration.Instance.GetString(fieldBoss.nameKey);
		m_strImageName = fieldBoss.imageName;
		m_csMonsterArrange = CsGameData.Instance.GetMonsterArrange(fieldBoss.monsterArrangeId);
		m_csContinent = CsGameData.Instance.GetContinent(fieldBoss.continentId);
		m_flXPosition = fieldBoss.xPosition;
		m_flYPosition = fieldBoss.yPosition;
		m_flZPosition = fieldBoss.zPosition;
		m_flYRotation = fieldBoss.yRotation;
		m_csItemReward = CsGameData.Instance.GetItemReward(fieldBoss.itemRewardId);
		m_nSortNo = fieldBoss.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsFieldBoss)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
