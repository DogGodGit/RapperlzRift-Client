using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-23)
//---------------------------------------------------------------------------------------------------

public class CsGameServerGroup
{
    int m_nGroupId;
    string m_strName;
	int m_nRegionId;
	bool m_bRecommendServerAuto;
	int m_nRecommendServerConditionType;
	bool m_bIsAccessRestriction;
	bool m_bAccessAllowed;

    List<CsGameServer> m_csGameServerList;

    //---------------------------------------------------------------------------------------------------
    public int GroupId
    {
        get { return m_nGroupId; }
    }

    public string Name
    {
        get { return m_strName; }
    }

	public int RegionId
	{
		get { return m_nRegionId; }
	}
	
	public bool RecommendServerAuto
	{
		get { return m_bRecommendServerAuto; }
	}

	public int RecommendServerConditionType
	{
		get { return m_nRecommendServerConditionType; }
	}

	public bool IsAccessRestriction
	{
		get { return m_bIsAccessRestriction; }
	}
	
	public bool AccessAllowed
	{
		get { return m_bAccessAllowed; }
	}

    public List<CsGameServer> GameServerList
    {
        get { return m_csGameServerList; }
    }

	public bool Recommendable
	{
		get
		{
			for (int i = 0; i < m_csGameServerList.Count; i++)
			{
				if (m_csGameServerList[i].Recommend ||
					(m_bRecommendServerAuto && m_csGameServerList[i].Recommendable))
				{
					return true;
				}
			}

			return false;
		}
	}

    //---------------------------------------------------------------------------------------------------
    public CsGameServerGroup(int nGroupId, string strNameKey, int nRegionId, bool bRecommendServerAuto,
							 int nRecommendServerConditionType, bool bIsAccessRestriction, bool bAccessAllowed)
    {
        m_nGroupId = nGroupId;

        if (CsConfiguration.Instance.Dic.ContainsKey(strNameKey))
        {
            m_strName = CsConfiguration.Instance.GetString(strNameKey);
        }
        else
        {
            Debug.Log("CsGameServerRegion     CsConfiguration.Instance.Dic.ContainsKey(strNameKey) == null, strNameKey: " + strNameKey);
        }

		m_nRegionId = nRegionId;
		m_bRecommendServerAuto = bRecommendServerAuto;
		m_nRecommendServerConditionType = nRecommendServerConditionType;
		m_bIsAccessRestriction = bIsAccessRestriction;
		m_bAccessAllowed = bAccessAllowed;

        m_csGameServerList = new List<CsGameServer>();
    }

    //---------------------------------------------------------------------------------------------------
    public CsGameServer GetGameServer(int nVirtualGameServerId)
    {
        for (int i = 0; i < m_csGameServerList.Count; i++)
        {
            if (m_csGameServerList[i].VirtualGameServerId == nVirtualGameServerId)
                return m_csGameServerList[i];
        }

        return null;
    }
}

