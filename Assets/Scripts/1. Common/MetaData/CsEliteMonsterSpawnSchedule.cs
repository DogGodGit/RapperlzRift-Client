using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteMonsterSpawnSchedule
{
	int m_nEliteMonsterMasterId;
	int m_nScheduleNo;
	int m_nSpawnTime;

	//---------------------------------------------------------------------------------------------------
	public int EliteMonsterMasterId
	{
		get { return m_nEliteMonsterMasterId; }
	}

	public int ScheduleNo
	{
		get { return m_nScheduleNo; }
	}

	public int SpawnTime
	{
		get { return m_nSpawnTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterSpawnSchedule(WPDEliteMonsterSpawnSchedule eliteMonsterSpawnSchedule)
	{
		m_nEliteMonsterMasterId = eliteMonsterSpawnSchedule.eliteMonsterMasterId;
		m_nScheduleNo = eliteMonsterSpawnSchedule.scheduleNo;
		m_nSpawnTime = eliteMonsterSpawnSchedule.spawnTime;
	}
}
