using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsWing
{
	int m_nWingId;
	string m_strName;
	string m_strPrefabName;
	CsAttr m_csAttr;
	CsAttrValueInfo m_csAttrValueInfo;
	string m_strAcquisitionText;
	bool m_bMemoryPieceInstallationEnabled;

	List<CsWingMemoryPieceSlot> m_listCsWingMemoryPieceSlot;
	List<CsWingMemoryPieceStep> m_listCsWingMemoryPieceStep;

	//---------------------------------------------------------------------------------------------------
	public int WingId
	{
		get { return m_nWingId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public CsAttrValueInfo AttrValueInfo
	{
		get { return m_csAttrValueInfo; }
	}

	public string AcquisitionText
	{
		get { return m_strAcquisitionText; }
	}

	public int BattlePower
	{
		get { return m_csAttrValueInfo.Value * m_csAttr.BattlePowerFactor; }
	}

	public bool MemoryPieceInstallationEnabled
	{
		get { return m_bMemoryPieceInstallationEnabled; }
	}

	public List<CsWingMemoryPieceSlot> WingMemoryPieceSlotList
	{
		get { return m_listCsWingMemoryPieceSlot; }
	}

	public List<CsWingMemoryPieceStep> WingMemoryPieceStepList
	{
		get { return m_listCsWingMemoryPieceStep; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public CsWing(WPDWing wing)
	{
		m_nWingId = wing.wingId;
		m_strName = CsConfiguration.Instance.GetString(wing.nameKey);
		m_strPrefabName = wing.prefabName;
		m_csAttr = CsGameData.Instance.GetAttr(wing.attrId);
		m_csAttrValueInfo = CsGameData.Instance.GetAttrValueInfo(wing.attrValueId);
		m_strAcquisitionText = CsConfiguration.Instance.GetString(wing.acquisitionTextKey);
		m_bMemoryPieceInstallationEnabled = wing.memoryPieceInstallationEnabled;

		m_listCsWingMemoryPieceSlot = new List<CsWingMemoryPieceSlot>();
		m_listCsWingMemoryPieceStep = new List<CsWingMemoryPieceStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceSlot GetWingMemoryPieceSlot(int nSlotIndex)
	{
		for (int i = 0; i < m_listCsWingMemoryPieceSlot.Count; i++)
		{
			if (m_listCsWingMemoryPieceSlot[i].SlotIndex == nSlotIndex)
				return m_listCsWingMemoryPieceSlot[i];
		}

		return null;
	}
}
