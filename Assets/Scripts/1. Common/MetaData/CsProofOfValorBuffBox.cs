using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorBuffBox
{
	int m_nBuffBoxId;
	string m_strPrefabName;
	float m_flOffenseFactor;
	float m_flPhysicalDefenseFactor;
	float m_flHpRecoveryFactor;
	string m_strUseGuideTitle;
	string m_strUseGuideContent;

	List<CsProofOfValorBuffBoxArrange> m_listCsProofOfValorBuffBoxArrange;

	//---------------------------------------------------------------------------------------------------
	public int BuffBoxId
	{
		get { return m_nBuffBoxId; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float OffenseFactor
	{
		get { return m_flOffenseFactor; }
	}

	public float PhysicalDefenseFactor
	{
		get { return m_flPhysicalDefenseFactor; }
	}

	public float HpRecoveryFactor
	{
		get { return m_flHpRecoveryFactor; }
	}

	public string UseGuideTitle
	{
		get { return m_strUseGuideTitle; }
	}

	public string UseGuideContent
	{
		get { return m_strUseGuideContent; }
	}

	public List<CsProofOfValorBuffBoxArrange> ProofOfValorBuffBoxArrangeList
	{
		get { return m_listCsProofOfValorBuffBoxArrange; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorBuffBox(WPDProofOfValorBuffBox proofOfValorBuffBox)
	{
		m_nBuffBoxId = proofOfValorBuffBox.buffBoxId;
		m_strPrefabName = proofOfValorBuffBox.prefabName;
		m_flOffenseFactor = proofOfValorBuffBox.offenseFactor;
		m_flPhysicalDefenseFactor = proofOfValorBuffBox.physicalDefenseFactor;
		m_flHpRecoveryFactor = proofOfValorBuffBox.hpRecoveryFactor;
		m_strUseGuideTitle = CsConfiguration.Instance.GetString(proofOfValorBuffBox.useGuideTitleKey);
		m_strUseGuideContent = CsConfiguration.Instance.GetString(proofOfValorBuffBox.useGuideContentKey);

		m_listCsProofOfValorBuffBoxArrange = new List<CsProofOfValorBuffBoxArrange>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorBuffBoxArrange GetProofOfValorBuffBoxArrange(int nBuffBoxArrangeId)
	{
		for (int i = 0; i < m_listCsProofOfValorBuffBoxArrange.Count; i++)
		{
			if (m_listCsProofOfValorBuffBoxArrange[i].ArrangeId == nBuffBoxArrangeId)
				return m_listCsProofOfValorBuffBoxArrange[i];
		}

		return null;
	}
}
