using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarMonsterArrange
{
	int m_nArrangeId;
	long m_lMonsterArrangeId;
	int m_nType;                // 1:총사령관,2:위저드,3:엔젤,4:드래곤,5:암석	
	CsContinent m_csContinent;
    Vector3 m_vtPosition;
	float m_flYRotation;
	CsNationWarNpc m_csNationWarNpc;
	int m_nRegenTime;
    Vector3 m_vtTransmissionPosition;
	float m_flTransmissionRadius;
	int m_nTransmissionYRotationType;
	float m_flTransmissionYRotation;

	List<CsNationWarRevivalPointActivationCondition> m_listCsNationWarRevivalPointActivationCondition;

	//---------------------------------------------------------------------------------------------------
	public int ArrangeId
	{
		get { return m_nArrangeId; }
	}

	public long MonsterArrangeId
	{
		get { return m_lMonsterArrangeId; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsContinent Continent
	{
		get { return m_csContinent; }
	}

    public Vector3 Position
    {
        get { return m_vtPosition; }
    }

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public CsNationWarNpc NationWarNpc
	{
		get { return m_csNationWarNpc; }
	}

	public int RegenTime
	{
		get { return m_nRegenTime; }
	}

    public Vector3 TransmissionPosition
    {
        get { return m_vtTransmissionPosition; }
    }

	public float TransmissionRadius
	{
		get { return m_flTransmissionRadius; }
	}

	public int TransmissionYRotationType
	{
		get { return m_nTransmissionYRotationType; }
	}

	public float TransmissionYRotation
	{
		get { return m_flTransmissionYRotation; }
	}

	public List<CsNationWarRevivalPointActivationCondition> NationWarRevivalPointActivationConditionList
	{
		get { return m_listCsNationWarRevivalPointActivationCondition; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarMonsterArrange(WPDNationWarMonsterArrange nationWarMonsterArrange)
	{
		m_nArrangeId = nationWarMonsterArrange.arrangeId;
		m_lMonsterArrangeId = nationWarMonsterArrange.monsterArrangeId;
		m_nType = nationWarMonsterArrange.type;
		m_csContinent = CsGameData.Instance.GetContinent(nationWarMonsterArrange.continentId);
        m_vtPosition = new Vector3(nationWarMonsterArrange.xPosition, nationWarMonsterArrange.yPosition, nationWarMonsterArrange.zPosition);
		m_flYRotation = nationWarMonsterArrange.yPosition;
		m_csNationWarNpc = CsGameData.Instance.NationWar.GetNationWarNpc(nationWarMonsterArrange.nationWarNpcId);
		m_nRegenTime = nationWarMonsterArrange.regenTime;
        m_vtTransmissionPosition = new Vector3(nationWarMonsterArrange.transmissionXPosition, nationWarMonsterArrange.transmissionYPosition, nationWarMonsterArrange.transmissionZPosition);
		m_flTransmissionRadius = nationWarMonsterArrange.transmissionRadius;
		m_nTransmissionYRotationType = nationWarMonsterArrange.transmissionYRotationType;
		m_flTransmissionYRotation = nationWarMonsterArrange.transmissionYRotation;

		m_listCsNationWarRevivalPointActivationCondition = new List<CsNationWarRevivalPointActivationCondition>();
	}
}
