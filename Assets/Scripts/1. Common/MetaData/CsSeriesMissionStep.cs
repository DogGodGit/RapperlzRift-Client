using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsSeriesMissionStep
{
	int m_nMissionId;
	int m_nStep;
	int m_nTargetCount;

	List<CsSeriesMissionStepReward> m_listCsSeriesMissionStepReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public List<CsSeriesMissionStepReward> SeriesMissionStepRewardList
	{
		get { return m_listCsSeriesMissionStepReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSeriesMissionStep(WPDSeriesMissionStep seriesMissionStep)
	{
		m_nMissionId = seriesMissionStep.missionId;
		m_nStep = seriesMissionStep.step;
		m_nTargetCount = seriesMissionStep.targetCount;

		m_listCsSeriesMissionStepReward = new List<CsSeriesMissionStepReward>();
	}
}
