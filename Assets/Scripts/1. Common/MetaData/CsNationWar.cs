using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWar
{
	int m_nDeclarationAvailableServerOpenDayCount;
	int m_nDeclarationStartTime;
	int m_nDeclarationEndTime;
	int m_nDeclarationRequiredNationFund;
	int m_nWeeklyDeclarationMaxCount;
	int m_nStartTime;
	int m_nEndTime;
	int m_nResultDisplayEndTime;
	int m_nJoinPopupDisplayDuration;
	CsContinent m_csContinentOffenseStart;
    Vector3 m_vtOffenseStartPosition;
	int m_nOffenseStartYRotationType;
	float m_flOffenseStartYRotation;
	float m_flOffenseStartRadius;
	CsContinent m_csContinentDefenseStart;
    Vector3 m_vtDefenseStartPosition;
	int m_nDefenseStartYRotationType;
	float m_flDefenseStartYRotation;
	float m_flDefenseStartRadius;
	int m_nFreeTransmissionCount;
	int m_nNationCallCount;
	int m_nNationCallCoolTime;
	int m_nNationCallLifetime;
	float m_flNationCallRadius;
	int m_nConvergingAttackCount;
	int m_nConvergingAttackCoolTime;
	int m_nConvergingAttackLifetime;
	CsItemReward m_csItemReward1WinNation;
	CsItemReward m_csItemReward2WinNation;
	CsItemReward m_csItemRewardWinNationAlliance;
	CsItemReward m_csItemRewardId1LoseNation;
	CsItemReward m_csItemRewardId2LoseNation;
	CsItemReward m_csItemRewardLoseNationAlliance;
	CsExploitPointReward m_csExploitPointRewardWinNation;
	CsExploitPointReward m_csExploitPointRewardLoseNation;

	List<CsNationWarNpc> m_listCsNationWarNpc;
	List<CsNationWarAvailableDayOfWeek> m_listCsNationWarAvailableDayOfWeek;
	List<CsNationWarMonsterArrange> m_listCsNationWarMonsterArrange;
	List<CsNationWarHeroObjectiveEntry> m_listCsNationWarHeroObjectiveEntry;
	List<CsNationWarPaidTransmission> m_listCsNationWarPaidTransmission;
	List<CsNationWarExpReward> m_listCsNationWarExpReward;

	//---------------------------------------------------------------------------------------------------
	public int DeclarationAvailableServerOpenDayCount
	{
		get { return m_nDeclarationAvailableServerOpenDayCount; }
	}

	public int DeclarationStartTime
	{
		get { return m_nDeclarationStartTime; }
	}

	public int DeclarationEndTime
	{
		get { return m_nDeclarationEndTime; }
	}

	public int DeclarationRequiredNationFund
	{
		get { return m_nDeclarationRequiredNationFund; }
	}

	public int WeeklyDeclarationMaxCount
	{
		get { return m_nWeeklyDeclarationMaxCount; }
	}

	public int StartTime
	{
		get { return m_nStartTime; }
	}

	public int EndTime
	{
		get { return m_nEndTime; }
	}

	public int ResultDisplayEndTime
	{
		get { return m_nResultDisplayEndTime; }
	}

	public int JoinPopupDisplayDuration
	{
		get { return m_nJoinPopupDisplayDuration; }
	}

	public CsContinent StartContinentOffense
	{
		get { return m_csContinentOffenseStart; }
	}

    public Vector3 OffenseStartPosition
    {
        get { return m_vtOffenseStartPosition; }
    }

	public int OffenseStartYRotationType
	{
		get { return m_nOffenseStartYRotationType; }
	}

	public float OffenseStartYRotation
	{
		get { return m_flOffenseStartYRotation; }
	}

	public float OffenseStartRadius
	{
		get { return m_flOffenseStartRadius; }
	}

	public CsContinent StartContinentDefense
	{
		get { return m_csContinentDefenseStart; }
	}

    public Vector3 DefenseStartPosition
    {
        get { return m_vtDefenseStartPosition; }
    }

	public int DefenseStartYRotationType
	{
		get { return m_nDefenseStartYRotationType; }
	}

	public float DefenseStartYRotation
	{
		get { return m_flDefenseStartYRotation; }
	}

	public float DefenseStartRadius
	{
		get { return m_flDefenseStartRadius; }
	}

	public int FreeTransmissionCount
	{
		get { return m_nFreeTransmissionCount; }
	}

	public int NationCallCount
	{
		get { return m_nNationCallCount; }
	}

	public int NationCallCoolTime
	{
		get { return m_nNationCallCoolTime; }
	}

	public int NationCallLifetime
	{
		get { return m_nNationCallLifetime; }
	}

	public float NationCallRadius
	{
		get { return m_flNationCallRadius; }
	}

	public int ConvergingAttackCount
	{
		get { return m_nConvergingAttackCount; }
	}

	public int ConvergingAttackCoolTime
	{
		get { return m_nConvergingAttackCoolTime; }
	}

	public int ConvergingAttackLifetime
	{
		get { return m_nConvergingAttackLifetime; }
	}

	public CsItemReward WinNationItemReward1
	{
		get { return m_csItemReward1WinNation; }
	}

	public CsItemReward WinNationItemReward2
	{
		get { return m_csItemReward2WinNation; }
	}

	public CsItemReward LoseNationItemRewardId1
	{
		get { return m_csItemRewardId1LoseNation; }
	}

	public CsItemReward LoseNationItemRewardId2
	{
		get { return m_csItemRewardId2LoseNation; }
	}

	public CsExploitPointReward WinNationExploitPointReward
	{
		get { return m_csExploitPointRewardWinNation; }
	}

	public CsExploitPointReward LoseNationExploitPointReward
	{
		get { return m_csExploitPointRewardLoseNation; }
	}

	public List<CsNationWarNpc> NationWarNpcList
	{
		get { return m_listCsNationWarNpc; }
	}

	public List<CsNationWarAvailableDayOfWeek> NationWarAvailableDayOfWeekList
	{
		get { return m_listCsNationWarAvailableDayOfWeek; }
	}

	public List<CsNationWarMonsterArrange> NationWarMonsterArrangeList
	{
		get { return m_listCsNationWarMonsterArrange; }
	}

	public List<CsNationWarHeroObjectiveEntry> NationWarHeroObjectiveEntryList
	{
		get { return m_listCsNationWarHeroObjectiveEntry; }
	}

	public List<CsNationWarPaidTransmission> NationWarPaidTransmissionList
	{
		get { return m_listCsNationWarPaidTransmission; }
	}

	public List<CsNationWarExpReward> NationWarExpRewardList
	{
		get { return m_listCsNationWarExpReward; }
	}

	public CsItemReward WinNationAllianceItemReward
	{
		get { return m_csItemRewardWinNationAlliance; }
	}

	public CsItemReward LoseNationAllianceItemReward
	{
		get { return m_csItemRewardLoseNationAlliance; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWar(WPDNationWar nationWar)
	{
		m_nDeclarationAvailableServerOpenDayCount = nationWar.declarationAvailableServerOpenDayCount;
		m_nDeclarationStartTime = nationWar.declarationStartTime;
		m_nDeclarationEndTime = nationWar.declarationEndTime;
		m_nDeclarationRequiredNationFund = nationWar.declarationRequiredNationFund;
		m_nWeeklyDeclarationMaxCount = nationWar.weeklyDeclarationMaxCount;
		m_nStartTime = nationWar.startTime;
		m_nEndTime = nationWar.endTime;
		m_nResultDisplayEndTime = nationWar.resultDisplayEndTime;
		m_nJoinPopupDisplayDuration = nationWar.joinPopupDisplayDuration;
		m_csContinentOffenseStart = CsGameData.Instance.GetContinent(nationWar.offenseStartContinentId);
        m_vtOffenseStartPosition = new Vector3(nationWar.offenseStartXPosition, nationWar.offenseStartYPosition, nationWar.offenseStartZPosition);
		m_nOffenseStartYRotationType = nationWar.offenseStartYRotationType;
		m_flOffenseStartYRotation = nationWar.offenseStartYRotation;
		m_flOffenseStartRadius = nationWar.offenseStartRadius;
		m_csContinentDefenseStart = CsGameData.Instance.GetContinent(nationWar.defenseStartContinentId);	
        m_vtDefenseStartPosition = new Vector3(nationWar.defenseStartXPosition, nationWar.defenseStartYPosition, nationWar.defenseStartZPosition);
		m_nDefenseStartYRotationType = nationWar.defenseStartYRotationType;
		m_flDefenseStartYRotation = nationWar.defenseStartYRotation;
		m_flDefenseStartRadius = nationWar.defenseStartRadius;
		m_nFreeTransmissionCount = nationWar.freeTransmissionCount;
		m_nNationCallCount = nationWar.nationCallCount;
		m_nNationCallCoolTime = nationWar.nationCallCoolTime;
		m_nNationCallLifetime = nationWar.nationCallLifetime;
		m_flNationCallRadius = nationWar.nationCallRadius;
		m_nConvergingAttackCount = nationWar.convergingAttackCount;
		m_nConvergingAttackCoolTime = nationWar.convergingAttackCoolTime;
		m_nConvergingAttackLifetime = nationWar.convergingAttackLifetime;
		m_csItemReward1WinNation = CsGameData.Instance.GetItemReward(nationWar.winNationItemRewardId1);
		m_csItemReward2WinNation = CsGameData.Instance.GetItemReward(nationWar.winNationItemRewardId2);
		m_csItemRewardId1LoseNation = CsGameData.Instance.GetItemReward(nationWar.loseNationItemRewardId1);
		m_csItemRewardId2LoseNation = CsGameData.Instance.GetItemReward(nationWar.loseNationItemRewardId2);
		m_csExploitPointRewardWinNation = CsGameData.Instance.GetExploitPointReward(nationWar.winNationExploitPointRewardId);
		m_csExploitPointRewardLoseNation = CsGameData.Instance.GetExploitPointReward(nationWar.loseNationExploitPointRewardId);
		m_csItemRewardWinNationAlliance = CsGameData.Instance.GetItemReward(nationWar.winNationAllianceItemRewardId);
		m_csItemRewardLoseNationAlliance = CsGameData.Instance.GetItemReward(nationWar.loseNationAllianceItemRewardId);

		m_listCsNationWarNpc = new List<CsNationWarNpc>();
		m_listCsNationWarAvailableDayOfWeek = new List<CsNationWarAvailableDayOfWeek>();
		m_listCsNationWarMonsterArrange = new List<CsNationWarMonsterArrange>();
		m_listCsNationWarHeroObjectiveEntry = new List<CsNationWarHeroObjectiveEntry>();
		m_listCsNationWarPaidTransmission = new List<CsNationWarPaidTransmission>();
		m_listCsNationWarExpReward = new List<CsNationWarExpReward>();

	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarNpc GetNationWarNpc(int nNpcId)
	{
		for (int i = 0; i < m_listCsNationWarNpc.Count; i++)
		{
			if (m_listCsNationWarNpc[i].NpcId == nNpcId)
				return m_listCsNationWarNpc[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarMonsterArrange GetNationWarMonsterArrange(int nArrangeId)
	{
		for (int i = 0; i < m_listCsNationWarMonsterArrange.Count; i++)
		{
			if (m_listCsNationWarMonsterArrange[i].ArrangeId == nArrangeId)
				return m_listCsNationWarMonsterArrange[i];
		}

		return null;
	}
}
