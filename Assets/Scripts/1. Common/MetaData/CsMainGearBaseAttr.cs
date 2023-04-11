using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMainGearBaseAttr
{
	int m_nMainGearId;                                                          // 메인장비ID
	CsAttr m_csAttr;                                                            // 속성

	List<CsMainGearBaseAttrEnchantLevel> m_listCsMainGearBaseAttrEnchantLevel;  // 메인장비 기본속성 강화레벨 목록

	//---------------------------------------------------------------------------------------------------
	public int MainGearId
	{
		get { return m_nMainGearId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public List<CsMainGearBaseAttrEnchantLevel> MainGearBaseAttrEnchantLevelList
	{
		get { return m_listCsMainGearBaseAttrEnchantLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearBaseAttr(WPDMainGearBaseAttr mainGearBaseAttr)
	{
		m_nMainGearId = mainGearBaseAttr.mainGearId;
		m_csAttr = CsGameData.Instance.GetAttr(mainGearBaseAttr.attrId);

		m_listCsMainGearBaseAttrEnchantLevel = new List<CsMainGearBaseAttrEnchantLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearBaseAttrEnchantLevel GetMainGearBaseAttrEnchantLevel(int nEnchantLevel)
	{
		for (int i = 0; i < m_listCsMainGearBaseAttrEnchantLevel.Count; i++)
		{
			if (m_listCsMainGearBaseAttrEnchantLevel[i].EnchantLevel == nEnchantLevel)
				return m_listCsMainGearBaseAttrEnchantLevel[i];
		}

		return null;
	}
}
