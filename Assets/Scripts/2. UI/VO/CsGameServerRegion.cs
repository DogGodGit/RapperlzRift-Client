using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-11)
//---------------------------------------------------------------------------------------------------

public class CsGameServerRegion 
{
	int m_nRegionId;
	string m_strName;

	List<CsGameServerGroup> m_csGameServerGroupList;

	//---------------------------------------------------------------------------------------------------
	public int RegionId
	{
		get { return m_nRegionId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsGameServerGroup> GameServerGroupList
	{
		get { return m_csGameServerGroupList; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGameServerRegion(int nRegionId, string strNameKey)
	{
		m_nRegionId = nRegionId;

		if (CsConfiguration.Instance.Dic.ContainsKey(strNameKey))
		{
			m_strName = CsConfiguration.Instance.GetString(strNameKey);
		}
		else
		{
			Debug.Log("CsGameServerRegion     CsConfiguration.Instance.Dic.ContainsKey(strNameKey) == null, strNameKey: " + strNameKey);
		}

		m_csGameServerGroupList = new List<CsGameServerGroup>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGameServerGroup GetGameServerGroup(int nGameServerGroupId)
	{
		for (int i = 0; i < m_csGameServerGroupList.Count; i++)
		{
			if (m_csGameServerGroupList[i].GroupId == nGameServerGroupId)
			{
				return m_csGameServerGroupList[i];
			}
		}

		return null;
	}
}
