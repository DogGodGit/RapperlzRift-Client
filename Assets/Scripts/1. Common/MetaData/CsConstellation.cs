using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsConstellation
{
	int m_nConstellationId;
	string m_strName;
	int m_nRequiredConditionType;	// 1 : 영웅레벨, 2 : 메인퀘스트
	int m_nRequiredConditionValue;

	List<CsConstellationStep> m_listCsConstellationStep;

	//---------------------------------------------------------------------------------------------------
	public int ConstellationId
	{
		get { return m_nConstellationId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredConditionValue
	{
		get { return m_nRequiredConditionValue; }
	}

	public List<CsConstellationStep> ConstellationStepList
	{
		get { return m_listCsConstellationStep; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellation(WPDConstellation constellation)
	{
		m_nConstellationId = constellation.constellationId;
		m_strName = CsConfiguration.Instance.GetString(constellation.nameKey);
		m_nRequiredConditionType = constellation.requiredConditionType;
		m_nRequiredConditionValue = constellation.requiredConditionValue;

		m_listCsConstellationStep = new List<CsConstellationStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellationStep GetConstellationStep(int nStep)
	{
		for (int i = 0; i < m_listCsConstellationStep.Count; i++)
		{
			if (m_listCsConstellationStep[i].Step == nStep)
				return m_listCsConstellationStep[i];
		}

		return null;
	}
}
