using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMount
{
	int m_nMountId;
	string m_strName;
	string m_strAcquisitionText;
	float m_flMoveSpeed;

	List<CsMountLevel> m_listCsMountLevel;
	List<CsMountQuality> m_listCsMountQuality;
	List<CsMountAwakeningLevel> m_listCsMountAwakeningLevel;

	//---------------------------------------------------------------------------------------------------
	public int MountId
	{
		get { return m_nMountId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string AcquisitionText
	{
		get { return m_strAcquisitionText; }
	}

	public float MoveSpeed
	{
		get { return m_flMoveSpeed; }
	}

	public List<CsMountLevel> MountLevelList
	{
		get { return m_listCsMountLevel; }
	}

	public List<CsMountQuality> MountQualityList
	{
		get { return m_listCsMountQuality; }
	}

	public List<CsMountAwakeningLevel> MountAwakeningLevelList
	{
		get { return m_listCsMountAwakeningLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMount(WPDMount mount)
	{
		m_nMountId = mount.mountId;
		m_strName = CsConfiguration.Instance.GetString(mount.nameKey);
		m_flMoveSpeed = mount.moveSpeed;
		m_strAcquisitionText = CsConfiguration.Instance.GetString(mount.acquisitionTextKey);

		m_listCsMountLevel = new List<CsMountLevel>();
		m_listCsMountQuality = new List<CsMountQuality>();
		m_listCsMountAwakeningLevel = new List<CsMountAwakeningLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountLevel GetMountLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsMountLevel.Count; i++)
		{
			if (m_listCsMountLevel[i].MountLevelMaster.Level == nLevel)
				return m_listCsMountLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountQuality GetMountQuality(int nQuality)
	{
		for (int i = 0; i < m_listCsMountQuality.Count; i++)
		{
			if (m_listCsMountQuality[i].MountQualityMaster.Quality == nQuality)
				return m_listCsMountQuality[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountAwakeningLevel GetMountAwakeningLevel(int nAwakeningLevel)
	{
		for (int i = 0; i < m_listCsMountAwakeningLevel.Count; i++)
		{
			if (m_listCsMountAwakeningLevel[i].MountAwakeningLevelMaster.AwakeningLevel == nAwakeningLevel)
				return m_listCsMountAwakeningLevel[i];
		}

		return null;
	}
}
