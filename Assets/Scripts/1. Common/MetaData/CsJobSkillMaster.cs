using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-29)
//---------------------------------------------------------------------------------------------------

public class CsJobSkillMaster
{
	int m_nSkillId;						// 스킬ID
	int m_nOpenRequiredMainQuestNo;     // 오픈필요메인퀘스트번호

	List<CsJobSkillLevelMaster> m_listCsJobSkillLevelMaster;

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public int OpenRequiredMainQuestNo
	{
		get { return m_nOpenRequiredMainQuestNo; }
	}

	public List<CsJobSkillLevelMaster> JobSkillLevelMasterList
	{
		get { return m_listCsJobSkillLevelMaster; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillMaster(WPDJobSkillMaster jobSkillMaster)
	{
		m_nSkillId = jobSkillMaster.skillId;
		m_nOpenRequiredMainQuestNo = jobSkillMaster.openRequiredMainQuestNo;

		m_listCsJobSkillLevelMaster = new List<CsJobSkillLevelMaster>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillLevelMaster GetJobSkillLevelMaster(int nLevel)
	{
		for (int i = 0; i < m_listCsJobSkillLevelMaster.Count; i++)
		{
			if (m_listCsJobSkillLevelMaster[i].Level == nLevel)
				return m_listCsJobSkillLevelMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsMaxLevel(int nCurrentLevel)
	{
		if (GetJobSkillLevelMaster(nCurrentLevel + 1) == null)
			return true;
		else
			return false;
	}

}
