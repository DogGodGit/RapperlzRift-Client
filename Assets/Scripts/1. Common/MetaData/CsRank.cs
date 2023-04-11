using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsRank
{
	int m_nRankNo;
	string m_strName;
	string m_strColorCode;
	int m_nRequiredExploitPoint;
	CsGoldReward m_csGoldReward;

	List<CsRankAttr> m_listCsRankAttr;
	List<CsRankReward> m_listCsRankReward;

	//---------------------------------------------------------------------------------------------------
	public int RankNo
	{
		get { return m_nRankNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public int RequiredExploitPoint
	{
		get { return m_nRequiredExploitPoint; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public List<CsRankAttr> RankAttrList
	{
		get { return m_listCsRankAttr; }
	}
	
	public List<CsRankReward> RankRewardList
	{
		get { return m_listCsRankReward; }
	}

	public int BattlePower
	{
		get
		{
			int nBattlePower = 0;

			for (int i = 0; i < m_listCsRankAttr.Count; i++)
			{
				nBattlePower += m_listCsRankAttr[i].BattlePowerValue;
			}

			return nBattlePower;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsRank(WPDRank rank)
	{
		m_nRankNo = rank.rankNo;
		m_strName = CsConfiguration.Instance.GetString(rank.nameKey);
		m_strColorCode = rank.colorCode;
		m_nRequiredExploitPoint = rank.requiredExploitPoint;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(rank.goldRewardId);

		m_listCsRankAttr = new List<CsRankAttr>();
		m_listCsRankReward = new List<CsRankReward>();
	}
}
