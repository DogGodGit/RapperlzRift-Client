using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMainGear
{
	int m_nMainGearId;									// 메인장비ID
	string m_strName;									// 이름
	CsMainGearType m_csMainGearType;					// 장비타입
	CsJob m_csJob;										// 직업
	CsMainGearTier m_csMainGearTier;					// 티어
	CsMainGearGrade m_csMainGearGrade;					// 등급
	CsMainGearQuality m_csMainGearQuality;              // 품질
	int m_nSaleGold;                                    // 판매골드
	string m_strPrefabName;                             // 프리팹이름

	List<CsMainGearBaseAttr> m_listCsMainGearBaseAttr;  // 메인장비 기본속성 목록

	int m_nJobId;
	//---------------------------------------------------------------------------------------------------
	public string Image
	{
		get
		{
			if (m_csJob == null)
			{
				return string.Format("main_{0}_{1}_{2}", (int)EnJob.Common, m_csMainGearType.MainGearType, m_csMainGearTier.Tier);
			}
			else
			{
				return string.Format("main_{0}_{1}_{2}", m_csJob.JobId, m_csMainGearType.MainGearType, m_csMainGearTier.Tier);
			}
		}
	}

	public int MainGearId
	{
		get { return m_nMainGearId; }
	}

	public CsMainGearType MainGearType
	{
		get { return m_csMainGearType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public CsMainGearTier MainGearTier
	{
		get { return m_csMainGearTier; }
	}

	public CsMainGearGrade MainGearGrade
	{
		get { return m_csMainGearGrade; }
	}

	public CsMainGearQuality MainGearQuality
	{
		get { return m_csMainGearQuality; }
	}

	public List<CsMainGearBaseAttr> MainGearBaseAttrList
	{
		get { return m_listCsMainGearBaseAttr; }
	}

	public int SaleGold
	{
		get { return m_nSaleGold; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public int JobId
	{
		get { return m_nJobId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGear(WPDMainGear mainGear)
	{
		m_nMainGearId = mainGear.mainGearId;
		m_strName = CsConfiguration.Instance.GetString(mainGear.nameKey);

		m_csMainGearType = CsGameData.Instance.GetMainGearType(mainGear.mainGearType);
		m_csJob = CsGameData.Instance.GetJob(mainGear.jobId);
		m_nJobId = mainGear.jobId;
		m_csMainGearTier = CsGameData.Instance.GetMainGearTier(mainGear.tier);
		m_csMainGearGrade = CsGameData.Instance.GetMainGearGrade(mainGear.grade);
		m_csMainGearQuality = CsGameData.Instance.GetMainGearQuality(mainGear.quality);
		m_nSaleGold = mainGear.saleGold;
		m_strPrefabName = mainGear.prefabName;

		m_listCsMainGearBaseAttr = new List<CsMainGearBaseAttr>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearBaseAttr GetMainGearBaseAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsMainGearBaseAttr.Count; i++)
		{
			if (m_listCsMainGearBaseAttr[i].Attr.AttrId == nAttrId)
				return m_listCsMainGearBaseAttr[i];
		}

		return null;
	}

	
}
