using System.Collections.Generic;
using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimStep
{
	int m_nStepNo;
	int m_nType;
	string m_strTargetTitle;
	string m_strTargetContent;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	int m_nRemoveObstacleId;
	int m_nActivationPortalId;
	int m_nDeactivationPortalId;
	int m_nRevivalPointId;

	List<CsRuinsReclaimObjectArrange> m_listCsRuinsReclaimObjectArrange;
	List<CsRuinsReclaimStepReward> m_listCsRuinsReclaimStepReward;
	List<CsRuinsReclaimStepWave> m_listCsRuinsReclaimStepWave;
	List<CsRuinsReclaimStepWaveSkill> m_listCsRuinsReclaimStepWaveSkill;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public Vector3 TargetPosition
	{
		get { return m_vtTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int RemoveObstacleId
	{
		get { return m_nRemoveObstacleId; }
	}

	public int ActivationPortalId
	{
		get { return m_nActivationPortalId; }
	}

	public int DeactivationPortalId
	{
		get { return m_nDeactivationPortalId; }
	}

	public int RevivalPointId
	{
		get { return m_nRevivalPointId; }
	}

	public List<CsRuinsReclaimObjectArrange> RuinsReclaimObjectArrangeList
	{
		get { return m_listCsRuinsReclaimObjectArrange; }
	}

	public List<CsRuinsReclaimStepReward> RuinsReclaimStepRewardList
	{
		get { return m_listCsRuinsReclaimStepReward; }
	}

	public List<CsRuinsReclaimStepWave> RuinsReclaimStepWaveList
	{
		get { return m_listCsRuinsReclaimStepWave; }
	}

	public List<CsRuinsReclaimStepWaveSkill> RuinsReclaimStepWaveSkillList
	{
		get { return m_listCsRuinsReclaimStepWaveSkill; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStep(WPDRuinsReclaimStep ruinsReclaimStep)
	{
		m_nStepNo = ruinsReclaimStep.stepNo;
		m_nType = ruinsReclaimStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(ruinsReclaimStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(ruinsReclaimStep.targetContentKey);
		m_vtTargetPosition = new Vector3(ruinsReclaimStep.targetXPosition, ruinsReclaimStep.targetYPosition, ruinsReclaimStep.targetZPosition);
		m_flTargetRadius = ruinsReclaimStep.targetRadius;
		m_nRemoveObstacleId = ruinsReclaimStep.removeObstacleId;
		m_nActivationPortalId = ruinsReclaimStep.activationPortalId;
		m_nDeactivationPortalId = ruinsReclaimStep.deactivationPortalId;
		m_nRevivalPointId = ruinsReclaimStep.revivalPointId;

		m_listCsRuinsReclaimObjectArrange = new List<CsRuinsReclaimObjectArrange>();
		m_listCsRuinsReclaimStepReward = new List<CsRuinsReclaimStepReward>();
		m_listCsRuinsReclaimStepWave = new List<CsRuinsReclaimStepWave>();
		m_listCsRuinsReclaimStepWaveSkill = new List<CsRuinsReclaimStepWaveSkill>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStepWave GetRuinsReclaimStepWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsRuinsReclaimStepWave.Count; i++)
		{
			if (m_listCsRuinsReclaimStepWave[i].WaveNo == nWaveNo)
			{
				return m_listCsRuinsReclaimStepWave[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimObjectArrange GetRuinsReclaimObjectArrange(int nArrangeNo)
	{
		for (int i = 0; i < m_listCsRuinsReclaimObjectArrange.Count; i++)
		{
			if (m_listCsRuinsReclaimObjectArrange[i].ArrangeNo == nArrangeNo)
			{
				return m_listCsRuinsReclaimObjectArrange[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStepWaveSkill GetRuinsReclaimStepWaveSkill(int nWaveNo)
	{
		for (int i = 0; i < m_listCsRuinsReclaimStepWaveSkill.Count; i++)
		{
			if (m_listCsRuinsReclaimStepWaveSkill[i].WaveNo == nWaveNo)
			{
				return m_listCsRuinsReclaimStepWaveSkill[i];
			}
		}

		return null;
	}
}
