using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteMonsterMaster
{
	int m_nEliteMonsterMasterId;
	string m_strName;
	int m_nLevel;
	int m_nDisplayMonsterId;
	CsEliteMonsterCategory m_csEliteMonsterCategory;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	int m_nYRotationType;
	float m_flYRotation;

	List<CsEliteMonsterSpawnSchedule> m_listCsEliteMonsterSpawnSchedule;

	//---------------------------------------------------------------------------------------------------
	public int EliteMonsterMasterId
	{
		get { return m_nEliteMonsterMasterId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int DisplayMonsterId
	{
		get { return m_nDisplayMonsterId; }
	}

	public CsEliteMonsterCategory EliteMonsterCategory
	{
		get { return m_csEliteMonsterCategory; }
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

	public int YRotationType
	{
		get { return m_nYRotationType; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public List<CsEliteMonsterSpawnSchedule> EliteMonsterSpawnScheduleList
	{
		get { return m_listCsEliteMonsterSpawnSchedule; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterMaster(WPDEliteMonsterMaster eliteMonsterMaster)
	{
		m_nEliteMonsterMasterId = eliteMonsterMaster.eliteMonsterMasterId;
		m_strName = CsConfiguration.Instance.GetString(eliteMonsterMaster.nameKey);
		m_nLevel = eliteMonsterMaster.level;
		m_nDisplayMonsterId = eliteMonsterMaster.displayMonsterId;
		m_csEliteMonsterCategory = CsGameData.Instance.GetEliteMonsterCategory(eliteMonsterMaster.categoryId);
		m_flXPosition = eliteMonsterMaster.xPosition;
		m_flYPosition = eliteMonsterMaster.yPosition;
		m_flZPosition = eliteMonsterMaster.zPosition;
		m_nYRotationType = eliteMonsterMaster.yRotationType;
		m_flYRotation = eliteMonsterMaster.yRotation;

		m_listCsEliteMonsterSpawnSchedule = new List<CsEliteMonsterSpawnSchedule>();
	}
}
