using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMazeFloor
{
	int m_nFloor;
	string m_strName;

	List<CsUndergroundMazePortal> m_listCsUndergroundMazePortal;
	List<CsUndergroundMazeMapMonster> m_listCsUndergroundMazeMapMonster;
	List<CsUndergroundMazeNpc> m_listCsUndergroundMazeNpc;
	List<CsUndergroundMazeMonsterArrange> m_listCsUndergroundMazeMonsterArrange;

	//---------------------------------------------------------------------------------------------------
	public int Floor
	{
		get { return m_nFloor; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsUndergroundMazePortal> UndergroundMazePortalList
	{
		get { return m_listCsUndergroundMazePortal; }
	}

	public List<CsUndergroundMazeMapMonster> UndergroundMazeMapMonsterList
	{
		get { return m_listCsUndergroundMazeMapMonster; }
	}

	public List<CsUndergroundMazeNpc> UndergroundMazeNpcList
	{
		get { return m_listCsUndergroundMazeNpc; }
	}

	public List<CsUndergroundMazeMonsterArrange> UndergroundMazeMonsterArrangeList
	{
		get { return m_listCsUndergroundMazeMonsterArrange; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeFloor(WPDUndergroundMazeFloor undergroundMazeFloor)
	{
		m_nFloor = undergroundMazeFloor.floor;
		m_strName = CsConfiguration.Instance.GetString(undergroundMazeFloor.nameKey);

		m_listCsUndergroundMazePortal = new List<CsUndergroundMazePortal>();
		m_listCsUndergroundMazeMapMonster = new List<CsUndergroundMazeMapMonster>();
		m_listCsUndergroundMazeNpc = new List<CsUndergroundMazeNpc>();
		m_listCsUndergroundMazeMonsterArrange = new List<CsUndergroundMazeMonsterArrange>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeNpc GetUndergroundMazeNpc(int nNpcId)
	{
		for (int i = 0; i < m_listCsUndergroundMazeNpc.Count; i++)
		{
			if (m_listCsUndergroundMazeNpc[i].NpcId == nNpcId)
				return m_listCsUndergroundMazeNpc[i];
		}

		return null;
	}
}
