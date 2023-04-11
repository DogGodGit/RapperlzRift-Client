using System.Collections.Generic;
using ClientCommon;
using System;
using UnityEngine;

public class CsEliteManager
{
	List<CsHeroEliteMonsterKill> m_listCsHeroEliteMonsterKill;
	List<int> m_listSpawnedEliteMonster;
	int m_nDailyEliteDungeonPlayCount;
	DateTime m_dtEliteDungeonPlayCountDate;

	//---------------------------------------------------------------------------------------------------
	public static CsEliteManager Instance
	{
		get { return CsSingleton<CsEliteManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<int, Vector3> EventAutoMoveToEliteMonster;

	public event Delegate EventEliteMonsterSpawn;
	public event Delegate EventEliteMonsterRemoved;
	public event Delegate EventEliteMonsterKillCountUpdated;

	//---------------------------------------------------------------------------------------------------
    public int DailyEliteDungeonPlayCount
    {
        get { return m_nDailyEliteDungeonPlayCount; }
        set { m_nDailyEliteDungeonPlayCount = value; }
    }

    public DateTime EliteDungeonPlayCountDate
    {
        get { return m_dtEliteDungeonPlayCountDate; }
        set { m_dtEliteDungeonPlayCountDate = value; }
    }

	public List<CsHeroEliteMonsterKill> HeroEliteMonsterKillList
	{
		get { return m_listCsHeroEliteMonsterKill; }
	}
	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroEliteMonsterKill[] heroEliteMonsterKills, int[] spawnedEliteMonsters, int dailyEliteDungeonPlayCount, DateTime dtDate)
	{
		UnInit();

		m_listCsHeroEliteMonsterKill = new List<CsHeroEliteMonsterKill>();

		for (int i = 0; i < heroEliteMonsterKills.Length; i++)
		{
			m_listCsHeroEliteMonsterKill.Add(new CsHeroEliteMonsterKill(heroEliteMonsterKills[i]));
		}

		m_listSpawnedEliteMonster = new List<int>(spawnedEliteMonsters);

		m_nDailyEliteDungeonPlayCount = dailyEliteDungeonPlayCount;
		m_dtEliteDungeonPlayCountDate = dtDate;

		// Command
		// Event
		CsRplzSession.Instance.EventEvtEliteMonsterSpawn += OnEventEvtEliteMonsterSpawn;
		CsRplzSession.Instance.EventEvtEliteMonsterRemoved += OnEventEvtEliteMonsterRemoved;
		CsRplzSession.Instance.EventEvtEliteMonsterKillCountUpdated += OnEventEvtEliteMonsterKillCountUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		// Event
		CsRplzSession.Instance.EventEvtEliteMonsterSpawn -= OnEventEvtEliteMonsterSpawn;
		CsRplzSession.Instance.EventEvtEliteMonsterRemoved -= OnEventEvtEliteMonsterRemoved;
		CsRplzSession.Instance.EventEvtEliteMonsterKillCountUpdated -= OnEventEvtEliteMonsterKillCountUpdated;

		m_listCsHeroEliteMonsterKill = null;
		m_listSpawnedEliteMonster = null;
		m_nDailyEliteDungeonPlayCount = 0;
		m_dtEliteDungeonPlayCountDate = DateTime.Now;
	}

	//---------------------------------------------------------------------------------------------------
	public void AutoMoveToEliteMonster(int nContinentId, Vector3 vtPosition)
	{
		if (EventAutoMoveToEliteMonster != null)
		{
			EventAutoMoveToEliteMonster(nContinentId, vtPosition);
		}
	}

	#region Protocol.Command
	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteMonsterSpawn(SEBEliteMonsterSpawnEventBody eventBody)
	{
		//Debug.Log(" #### OnEventEvtEliteMonsterSpawn ####");
        m_listSpawnedEliteMonster.Add(eventBody.eliteMonsterId);

		if (EventEliteMonsterSpawn != null)
		{
			EventEliteMonsterSpawn();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteMonsterRemoved(SEBEliteMonsterRemovedEventBody eventBody)
	{
		//Debug.Log(" #### OnEventEvtEliteMonsterRemoved ####");
        m_listSpawnedEliteMonster.Remove(eventBody.eliteMonsterId);

		if (EventEliteMonsterRemoved != null)
		{
			EventEliteMonsterRemoved();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteMonsterKillCountUpdated(SEBEliteMonsterKillCountUpdatedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtEliteMonsterKillCountUpdated ####");

        bool bMonster = false;

        for (int i = 0; i < m_listCsHeroEliteMonsterKill.Count; i++)
        {
            if (m_listCsHeroEliteMonsterKill[i].EliteMonster.EliteMonsterId == eventBody.eliteMonsterId)
            {
                m_listCsHeroEliteMonsterKill[i].KillCount = eventBody.killCount;
                bMonster = true;
                break;
            }
        }

        if (!bMonster)
        {
            CsHeroEliteMonsterKill csHeroEliteMonsterKill = new CsHeroEliteMonsterKill(eventBody.eliteMonsterId, eventBody.killCount);
            m_listCsHeroEliteMonsterKill.Add(csHeroEliteMonsterKill);
        }

        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

        CsEliteMonster csEliteMonster = CsGameData.Instance.EliteMonsterList.Find(a => a.EliteMonsterId == eventBody.eliteMonsterId);

        if (csEliteMonster != null)
        {
            for (int i = 0; i < csEliteMonster.EliteMonsterKillAttrValueList.Count; i++)
            {
                if (eventBody.killCount == csEliteMonster.EliteMonsterKillAttrValueList[i].KillCount)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, string.Format(CsConfiguration.Instance.GetString("A87_TXT_03001"), csEliteMonster.Attr.Name, csEliteMonster.EliteMonsterKillAttrValueList[i].AttrValue.Value));
                    CsGameData.Instance.MyHeroInfo.UpdateBattlePower();
                    break;
                }
            }
        }

		if (EventEliteMonsterKillCountUpdated != null)
		{
			EventEliteMonsterKillCountUpdated();
		}
	}

	#endregion Protocol.Event

    //---------------------------------------------------------------------------------------------------
    public int MyKillCount(int nEliteMonsterId)
    {
        CsHeroEliteMonsterKill csHeroEliteMonsterKill = m_listCsHeroEliteMonsterKill.Find(a => a.EliteMonster.EliteMonsterId == nEliteMonsterId);

        if (csHeroEliteMonsterKill != null)
        {
            return csHeroEliteMonsterKill.KillCount;
        }

        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    public bool EliteMonsterSpawned(int nEliteMonsterId)
    {
        if (m_listSpawnedEliteMonster.Find(a => a == nEliteMonsterId) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
