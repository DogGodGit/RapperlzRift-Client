using UnityEngine;
using ClientCommon;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-21)
//---------------------------------------------------------------------------------------------------

public class CsHeroOrdealQuestSlot
{
	int m_nIndex;
	CsOrdealQuestMission m_csOrdealQuestMission;
	int m_nProgressCount;
	float m_flRemainingTime;
	
	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public CsOrdealQuestMission OrdealQuestMission
	{
		get { return m_csOrdealQuestMission; }
		set { m_csOrdealQuestMission = value; }
	}

	public int ProgressCount
	{
		get 
		{
			switch ((EnOrdealQuestMissionType)m_csOrdealQuestMission.Type)
			{
				case EnOrdealQuestMissionType.SoulStoneLevel:
					
					int nTotalLevel = 0;

					foreach (CsHeroSubGear csHeroSubGear in CsGameData.Instance.MyHeroInfo.HeroSubGearList)
					{
						foreach (CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket in csHeroSubGear.SoulstoneSocketList)
						{
							if (csHeroSubGearSoulstoneSocket.Item != null)
							{
								nTotalLevel += csHeroSubGearSoulstoneSocket.Item.Level;
							}
						}
					}

					return nTotalLevel;

				case EnOrdealQuestMissionType.MountLevel:
					CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(CsGameData.Instance.MyHeroInfo.EquippedMountId);

					if (csHeroMount != null)
					{
						return csHeroMount.Level;
					}

					break;

				case EnOrdealQuestMissionType.CreatureLevel:

					return CsCreatureManager.Instance.HeroCreatureList.Max(creature => creature.Level);

				case EnOrdealQuestMissionType.WingLevel:

					return CsGameData.Instance.MyHeroInfo.WingLevel;

				case EnOrdealQuestMissionType.Rank:

					return CsGameData.Instance.MyHeroInfo.RankNo;

				case EnOrdealQuestMissionType.AchievePoint:

					int nTotalPoint = 0;

                    /*
					var res = from accomplishment in CsGameData.Instance.AccomplishmentList
							  join rewarded in CsAccomplishmentManager.Instance.RewardedAccomplishmentList on accomplishment.AccomplishmentId equals rewarded
							  select accomplishment.Point;
					
					foreach (var point in res)
					{
						nTotalPoint += point;
					}
                    */

					return nTotalPoint;

				default:
					return m_nProgressCount;
			}

			return 0;
		}
		set { m_nProgressCount = value; }
	}

	public float RemainingTime
	{
		get 
		{
			float flRemainingTime = m_flRemainingTime - Time.realtimeSinceStartup;

			if (flRemainingTime < 0)
				flRemainingTime = 0f;

			return flRemainingTime; 
		}
		set
		{
			if (value == 0)
				m_flRemainingTime = value;
			else
				m_flRemainingTime = value + Time.realtimeSinceStartup;
		}
	}

	public bool IsCompleted
	{
		get { return m_csOrdealQuestMission == null; }
	}

	public bool Receivable
	{
		get 
		{
			if (m_csOrdealQuestMission == null)
				return false;

			if (m_csOrdealQuestMission.AutoCompletable &&
				m_flRemainingTime - Time.realtimeSinceStartup <= 0)
			{
				return true;
			}



			return ProgressCount >= m_csOrdealQuestMission.TargetCount; 
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroOrdealQuestSlot(CsOrdealQuest csOrdealQuest, PDHeroOrdealQuestSlot heroOrdealQuestSlot)
	{
		m_nIndex = heroOrdealQuestSlot.index;
		m_csOrdealQuestMission = csOrdealQuest.GetOrdealQuestMission(m_nIndex, heroOrdealQuestSlot.missionNo);
		m_nProgressCount = heroOrdealQuestSlot.progressCount;

		if (heroOrdealQuestSlot.remainingTime == 0)
			m_flRemainingTime = 0;
		else
			m_flRemainingTime = heroOrdealQuestSlot.remainingTime + Time.realtimeSinceStartup;
	}
}
