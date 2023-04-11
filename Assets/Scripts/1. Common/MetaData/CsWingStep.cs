using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsWingStep
{
	int m_nStep;
	string m_strName;
	string m_strColorCode;
	int m_nEnchantMaterialItemCount;
	int m_nRewardWingId;

	List<CsWingStepLevel> m_listCsWingStepLevel;
	List<CsWingStepPart> m_listCsWingStepPart;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public int EnchantMaterialItemCount
	{
		get { return m_nEnchantMaterialItemCount; }
	}

	public int RewardWingId
	{
		get { return m_nRewardWingId; }
	}

	public List<CsWingStepLevel> WingStepLevelList
	{
		get { return m_listCsWingStepLevel; }
	}

	public List<CsWingStepPart> WingStepPartList
	{
		get { return m_listCsWingStepPart; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingStep(WPDWingStep wingStep)
	{
		m_nStep = wingStep.step;
		m_strName = CsConfiguration.Instance.GetString(wingStep.nameKey);
		m_strColorCode = wingStep.colorCode;
		m_nEnchantMaterialItemCount = wingStep.enchantMaterialItemCount;
		m_nRewardWingId = wingStep.rewardWingId;

		m_listCsWingStepLevel = new List<CsWingStepLevel>();
		m_listCsWingStepPart = new List<CsWingStepPart>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingStepPart GetWingStepPart(int nPartId)
	{
		for (int i = 0; i < m_listCsWingStepPart.Count; i++)
		{
			if (m_listCsWingStepPart[i].WingPart.PartId == nPartId)
				return m_listCsWingStepPart[i];
		}

		return null;
	}

}
