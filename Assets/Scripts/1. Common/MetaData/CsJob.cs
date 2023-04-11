using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public enum EnJob
{
	Common = 0,
	Gaia = 1,
	Asura = 2,
	Deva = 3,
	Witch = 4
}

public enum EnOffenseType
{
	Physical = 1,
	Magical = 2,
}

public class CsJob
{
	int m_nJobId;               // 직업ID
	string m_strName;			// 이름
	string m_strDescription;    // 설명
	int m_nMoveSpeed;           // 이동속도
	int m_nOffenseType;         // 공격타입		
	CsElemental m_csElemental;  // 원소ID
	float m_flRadius;           // 반지름
	int m_nWalkMoveSpeed;
	int m_nParentJobId;			

	List<CsJobLevel> m_listJobLevel;

	//---------------------------------------------------------------------------------------------------
	public int JobId
	{
		get { return m_nJobId; }
	}

	public EnJob EnJobId
	{
		get { return (EnJob)m_nJobId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int MoveSpeed
	{
		get { return m_nMoveSpeed; }
	}

	public EnOffenseType OffenseType
	{
		get { return (EnOffenseType)m_nOffenseType; }
	}
	
	public CsElemental Elemental
	{
		get { return m_csElemental; }
	}

	public List<CsJobLevel> JobLevelList
	{
		get { return m_listJobLevel; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int WalkMoveSpeed
	{
		get { return m_nWalkMoveSpeed; }
	}

	public int ParentJobId
	{
		get { return m_nParentJobId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJob(WPDJob job)
	{
		m_nJobId = job.jobId;
		m_strName = CsConfiguration.Instance.GetString(job.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(job.descriptionKey);
		m_nMoveSpeed = job.moveSpeed;
		m_nOffenseType = job.offenseType;
		m_csElemental = CsGameData.Instance.GetElemental(job.elementalId);
		m_flRadius = job.radius;
		m_nWalkMoveSpeed = job.walkMoveSpeed;
		m_nParentJobId = job.parentJobId;

		m_listJobLevel = new List<CsJobLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobLevel GetJobLevel(int nLevel)
	{
		for (int i = 0; i < m_listJobLevel.Count; i++)
		{
			if (m_listJobLevel[i].Level == nLevel)
				return m_listJobLevel[i];
		}

		return null;
	}
}
