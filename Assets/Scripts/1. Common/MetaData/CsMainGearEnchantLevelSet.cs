using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsMainGearEnchantLevelSet
{
	int m_nSetNo;
	string m_strName;
	int m_nRequiredTotalEnchantLevel;

	List<CsMainGearEnchantLevelSetAttr> m_listCsMainGearEnchantLevelSetAttr;

	//---------------------------------------------------------------------------------------------------
	public int SetNo
	{
		get { return m_nSetNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredTotalEnchantLevel
	{
		get { return m_nRequiredTotalEnchantLevel; }
	}

	public List<CsMainGearEnchantLevelSetAttr> MainGearEnchantLevelSetAttrList
	{
		get { return m_listCsMainGearEnchantLevelSetAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantLevelSet(WPDMainGearEnchantLevelSet mainGearEnchantLevelSet)
	{
		m_nSetNo = mainGearEnchantLevelSet.setNo;
		m_strName = CsConfiguration.Instance.GetString(mainGearEnchantLevelSet.nameKey);
		m_nRequiredTotalEnchantLevel = mainGearEnchantLevelSet.requiredTotalEnchantLevel;

		m_listCsMainGearEnchantLevelSetAttr = new List<CsMainGearEnchantLevelSetAttr>();
	}
}
