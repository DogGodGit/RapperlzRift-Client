using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsArtifact
{
	int m_nArtifactNo;
	string m_strName;
	string m_strPrefabName;
	int m_nSafeRevivalAdditionalHpRecoveryRate;				// 안전부활추가HP회복율
	int m_nFreeImmediateRevivalAdditionalDailyCount;        // 무료즉시부활추가일일횟수	
	int m_nSafeRevivalWaitingDecRate;                       // 안전부활대기감소율

	List<CsArtifactAttr> m_listCsArtifactAttr;
	List<CsArtifactLevel> m_listCsArtifactLevel;

	//---------------------------------------------------------------------------------------------------
	public int ArtifactNo
	{
		get { return m_nArtifactNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public int SafeRevivalAdditionalHpRecoveryRate
	{
		get { return m_nSafeRevivalAdditionalHpRecoveryRate; }
	}

	public int FreeImmediateRevivalAdditionalDailyCount
	{
		get { return m_nFreeImmediateRevivalAdditionalDailyCount; }
	}

	public int SafeRevivalWaitingDecRate
	{
		get { return m_nSafeRevivalWaitingDecRate; }
	}

	public List<CsArtifactAttr> ArtifactAttrList
	{
		get { return m_listCsArtifactAttr; }
	}

	public List<CsArtifactLevel> ArtifactLevelList
	{
		get { return m_listCsArtifactLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifact(WPDArtifact artifact)
	{
		m_nArtifactNo = artifact.artifactNo;
		m_strName = CsConfiguration.Instance.GetString(artifact.nameKey);
		m_strPrefabName = artifact.prefabName;

		m_listCsArtifactAttr = new List<CsArtifactAttr>();
		m_listCsArtifactLevel = new List<CsArtifactLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactLevel GetArtifactLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsArtifactLevel.Count; i++)
		{
			if (m_listCsArtifactLevel[i].Level == nLevel)
				return m_listCsArtifactLevel[i];
		}

		return null;
	}
}
