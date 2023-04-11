using System.Collections.Generic;
using ClientCommon;
using UnityEngine;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroSubGear : CsHeroObject
{
	CsSubGear m_csSubGear;					// 보조장비
	int m_nLevel;							// 레벨
	int m_nQuality;							// 품빌
	bool m_bEquipped;						// 장착여부

	int m_nAttributesBattlePower = 0;       // 옵션전투력
	int m_nSoulstoneBattlePower = 0;		// 소울스톤 전투력
	int m_nRuneBattlePower = 0;				// 룬 전투력	

	CsSubGearLevel m_csSubGearLevel;		// 보조장비레벨
	CsSubGearName m_csSubGearName;			// 보조장비이름

	List<CsHeroSubGearRuneSocket> m_listCsHeroSubGearRuneSocket;			// 보조장비 룬소켓 리스트
	List<CsHeroSubGearSoulstoneSocket> m_listCsHeroSubGearSoulstoneSocket;	// 보조장비 소울스톤소켓 리스트

	List<CsAttrValue> m_listCsAttrValue = new List<CsAttrValue>();

	//---------------------------------------------------------------------------------------------------
	public string Image
	{
		get { return string.Format("sub_{0}_{1}", m_csSubGear.SubGearId, m_csSubGearLevel.SubGearGrade.Grade); }
	}

	public CsSubGear SubGear
	{
		get { return m_csSubGear; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set
		{
			m_nLevel = value;
			m_csSubGearLevel = m_csSubGear.GetSubGearLevel(m_nLevel);
			m_csSubGearName = m_csSubGear.GetSubGearName(m_csSubGearLevel.SubGearGrade.Grade);
		}
	}

	public int Quality
	{
		get { return m_nQuality; }
		set
		{
			m_nQuality = value;
			UpdateAttributeValueList();
		}
	}

	public bool Equipped
	{
		get { return m_bEquipped; }
		set { m_bEquipped = value; }
	}

	public int BattlePower
	{
		get { return m_nAttributesBattlePower + m_nSoulstoneBattlePower + m_nRuneBattlePower; }
	}

	public string Name
	{
		get { return string.Format("<color={0}>{1}</color>", m_csSubGearName.SubGearGrade.ColorCode, m_csSubGearName.Name); }
	}

	public CsSubGearLevel SubGearLevel
	{
		get { return m_csSubGearLevel; }
	}

	public List<CsAttrValue> AttrValueList
	{
		get { return m_listCsAttrValue; }
	}

	public List<CsHeroSubGearRuneSocket> RuneSocketList
	{
		get { return m_listCsHeroSubGearRuneSocket; }
	}

	public List<CsHeroSubGearSoulstoneSocket> SoulstoneSocketList
	{
		get { return m_listCsHeroSubGearSoulstoneSocket; }
	}

	public EnNextStep NextStep
	{
		get
		{
			CsSubGearLevelQuality csSubGearLevelQuality = m_csSubGearLevel.GetSubGearLevelQuality(m_nQuality + 1);
			CsSubGearLevel csSubGearLevelNext = m_csSubGear.GetSubGearLevel(m_nLevel + 1);

			if (csSubGearLevelQuality == null)
			{
				if (csSubGearLevelNext == null)
				{
					// 최대레벨
					return EnNextStep.MaxLevel;
				}
				else
				{
					if (m_csSubGearLevel.SubGearGrade.Grade == csSubGearLevelNext.SubGearGrade.Grade)
					{
						// 레벨업
						return EnNextStep.Level;
					}
					else
					{
						// 등급업
						return EnNextStep.Grade;
					}
				}
			}
			else
			{
				// 품질업
				return EnNextStep.Quality;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear(PDFullHeroSubGear heroSubGear)
		: base(EnHeroObjectType.SubGear)
	{
		m_csSubGear = CsGameData.Instance.GetSubGear(heroSubGear.subGearId);
		Level = heroSubGear.level;
		Quality = heroSubGear.quality;
		m_bEquipped = heroSubGear.equipped;

		m_listCsHeroSubGearRuneSocket = new List<CsHeroSubGearRuneSocket>();

		for (int i = 0; i < heroSubGear.equippedRuneSockets.Length; i++)
		{
			CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = new CsHeroSubGearRuneSocket(heroSubGear.equippedRuneSockets[i]);
			m_listCsHeroSubGearRuneSocket.Add(csHeroSubGearRuneSocket);

			m_nRuneBattlePower += csHeroSubGearRuneSocket.BattlePowerValue;
		}

		m_listCsHeroSubGearSoulstoneSocket = new List<CsHeroSubGearSoulstoneSocket>();

		for (int i = 0; i < heroSubGear.equippedSoulstoneSockets.Length; i++)
		{
			CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = new CsHeroSubGearSoulstoneSocket(heroSubGear.equippedSoulstoneSockets[i]);
			m_listCsHeroSubGearSoulstoneSocket.Add(csHeroSubGearSoulstoneSocket);

			m_nSoulstoneBattlePower += csHeroSubGearSoulstoneSocket.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear(int nSubGearId, int nLevel, int nQuality, PDHeroSubGearRuneSocket[] equippedRuneSockets, PDHeroSubGearSoulstoneSocket[] equippedSoulstoneSockets)
		: base(EnHeroObjectType.SubGear)
	{
		m_csSubGear = CsGameData.Instance.GetSubGear(nSubGearId);
		Level = nLevel;
		Quality = nQuality;
		m_bEquipped = false;

		m_listCsHeroSubGearRuneSocket = new List<CsHeroSubGearRuneSocket>();

		for (int i = 0; i < equippedRuneSockets.Length; i++)
		{
			CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = new CsHeroSubGearRuneSocket(equippedRuneSockets[i]);
			m_listCsHeroSubGearRuneSocket.Add(csHeroSubGearRuneSocket);

			m_nRuneBattlePower += csHeroSubGearRuneSocket.BattlePowerValue;
		}

		m_listCsHeroSubGearSoulstoneSocket = new List<CsHeroSubGearSoulstoneSocket>();

		for (int i = 0; i < equippedSoulstoneSockets.Length; i++)
		{
			CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = new CsHeroSubGearSoulstoneSocket(equippedSoulstoneSockets[i]);
			m_listCsHeroSubGearSoulstoneSocket.Add(csHeroSubGearSoulstoneSocket);

			m_nSoulstoneBattlePower += csHeroSubGearSoulstoneSocket.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateAttributeValueList()
	{
		m_listCsAttrValue.Clear();
		m_nAttributesBattlePower = 0;

		for (int i = 0; i < m_csSubGear.SubGearAttrList.Count; i++)
		{
			CsSubGearAttrValue csSubGearAttrValue = m_csSubGear.SubGearAttrList[i].GetSubGearAttrValue(m_nLevel, m_nQuality);

			int nValue = 0;

			if (csSubGearAttrValue != null)
			{
				nValue = csSubGearAttrValue.Value;
			}

			CsAttrValue csAttrValue = new CsAttrValue(csSubGearAttrValue.Attr, nValue);
			m_listCsAttrValue.Add(csAttrValue);

			m_nAttributesBattlePower += csAttrValue.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearSoulstoneSocket GetHeroSubGearSoulstoneSocket(int nIndex)
	{
		for (int i = 0; i < m_listCsHeroSubGearSoulstoneSocket.Count; i++)
		{
			if (m_listCsHeroSubGearSoulstoneSocket[i].Index == nIndex)
				return m_listCsHeroSubGearSoulstoneSocket[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddSoulstoneSocket(int nIndex, int nItemId)
	{
		CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = GetHeroSubGearSoulstoneSocket(nIndex);
	
		if (csHeroSubGearSoulstoneSocket == null)
		{
			CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocketNew = new CsHeroSubGearSoulstoneSocket(nIndex, nItemId);
			m_listCsHeroSubGearSoulstoneSocket.Add(csHeroSubGearSoulstoneSocketNew);
			m_nSoulstoneBattlePower += csHeroSubGearSoulstoneSocketNew.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveSoulstoneSocket(int nIndex)
	{
		CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = GetHeroSubGearSoulstoneSocket(nIndex);

		if (csHeroSubGearSoulstoneSocket != null)
		{
			m_listCsHeroSubGearSoulstoneSocket.Remove(csHeroSubGearSoulstoneSocket);
			m_nSoulstoneBattlePower -= csHeroSubGearSoulstoneSocket.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGearRuneSocket GetHeroSubGearRuneSocket(int nIndex)
	{
		for (int i = 0; i < m_listCsHeroSubGearRuneSocket.Count; i++)
		{
			if (m_listCsHeroSubGearRuneSocket[i].Index == nIndex)
				return m_listCsHeroSubGearRuneSocket[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddRuneSocket(int nIndex, int nItemId)
	{
		CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = new CsHeroSubGearRuneSocket(nIndex, nItemId);
		m_listCsHeroSubGearRuneSocket.Add(csHeroSubGearRuneSocket);

		m_nRuneBattlePower += csHeroSubGearRuneSocket.BattlePowerValue;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveRuneSocket(int nIndex)
	{
		CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = GetHeroSubGearRuneSocket(nIndex);

		if (csHeroSubGearRuneSocket != null)
		{
			m_listCsHeroSubGearRuneSocket.Remove(csHeroSubGearRuneSocket);
			m_nRuneBattlePower -= csHeroSubGearRuneSocket.BattlePowerValue;
		}
	}
	
}
/*
*/ 
public enum EnNextStep
{
	MaxLevel = 0,
	Quality = 1,
	Level = 2,
	Grade = 3,
}
