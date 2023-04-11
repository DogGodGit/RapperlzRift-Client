using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public enum EnSubGearType
{
    Helmet = 1,
    Belt = 2,
    Gloves = 3,
    Shoes = 4,
    Necklace = 5,
    Ring = 6,
}

public class CsSubGear
{
    int m_nSubGearId;                                                   // 보조장비ID
    int m_nSlotIndex;                                                   // 착용슬롯인덱스
    string m_strName;                                                   // 이름

    List<CsSubGearName> m_listCsSubGearName;                            // 보조장비 이름 목록
    List<CsSubGearRuneSocket> m_listCsSubGearRuneSocket;                // 보조장비 명문 소켓 목록
    List<CsSubGearSoulstoneSocket> m_listCsSubGearSoulstoneSocket;      // 보조장비 소울스톤 소켓 목록
    List<CsSubGearAttr> m_listCsSubGearAttr;                            // 보조장비 속성 목록
	List<CsSubGearLevel> m_listCsSubGearLevel;                          // 보조장비 레벨 목록

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

    public EnSubGearType SubGearType
    {
        get { return (EnSubGearType)m_nSubGearId; }
    }

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsSubGearName>SubGearNameList
	{
		get { return m_listCsSubGearName; }
	}

	public List<CsSubGearRuneSocket> SubGearRuneSocketList
	{
		get { return m_listCsSubGearRuneSocket; }
	}

	public List<CsSubGearSoulstoneSocket> SubGearSoulstoneSocketList
	{
		get { return m_listCsSubGearSoulstoneSocket; }
	}

	public List<CsSubGearAttr> SubGearAttrList
	{
		get { return m_listCsSubGearAttr; }
	}

	public List<CsSubGearLevel> SubGearLevelList
	{
		get { return m_listCsSubGearLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGear(WPDSubGear subGear)
	{
		m_nSubGearId = subGear.subGearId;
		m_nSlotIndex = subGear.slotIndex;
		m_strName = CsConfiguration.Instance.GetString(subGear.nameKey);

		m_listCsSubGearName = new List<CsSubGearName>();
		m_listCsSubGearRuneSocket = new List<CsSubGearRuneSocket>();
		m_listCsSubGearSoulstoneSocket = new List<CsSubGearSoulstoneSocket>();
		m_listCsSubGearAttr = new List<CsSubGearAttr>();
		m_listCsSubGearLevel = new List<CsSubGearLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearAttr GetSubGearAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsSubGearAttr.Count; i++)
		{
			if (m_listCsSubGearAttr[i].Attr.AttrId == nAttrId)
				return m_listCsSubGearAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearLevel GetSubGearLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsSubGearLevel.Count; i++)
		{
			if (m_listCsSubGearLevel[i].Level == nLevel)
				return m_listCsSubGearLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearName GetSubGearName(int nGrade)
	{
		for (int i = 0; i < m_listCsSubGearName.Count; i++)
		{
			if (m_listCsSubGearName[i].SubGearGrade.Grade == nGrade)
				return m_listCsSubGearName[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneSocket GetSubGearSoulstoneSocket(int nIndex)
	{
		for (int i = 0; i < m_listCsSubGearSoulstoneSocket.Count; i++)
		{
			if (m_listCsSubGearSoulstoneSocket[i].SocketIndex == nIndex)
				return m_listCsSubGearSoulstoneSocket[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearRuneSocket GetSubGearRuneSocket(int nSocketIndex)
	{
		for (int i = 0; i < m_listCsSubGearRuneSocket.Count; i++)
		{
			if (m_listCsSubGearRuneSocket[i].SocketIndex == nSocketIndex)
				return m_listCsSubGearRuneSocket[i];
		}

		return null;
	}

}
