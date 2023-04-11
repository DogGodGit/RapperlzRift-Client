using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarStage
{
	int m_nStageId;
	string m_strName;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strSceneName;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	int m_nStartYRotationType;
	float m_flStartYRotation;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsFearAltarStageWave> m_listCsFearAltarStageWave;

	//---------------------------------------------------------------------------------------------------
	public int StageId
	{
		get { return m_nStageId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public float StartXPosition
	{
		get { return m_flStartXPosition; }
	}

	public float StartYPosition
	{
		get { return m_flStartYPosition; }
	}

	public float StartZPosition
	{
		get { return m_flStartZPosition; }
	}

	public float StartRadius
	{
		get { return m_flStartRadius; }
	}

	public int StartYRotationType
	{
		get { return m_nStartYRotationType; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public CsLocation Location
	{
		get { return m_csLocation; }
	}

	public float X
	{
		get { return m_flX; }
	}

	public float Z
	{
		get { return m_flZ; }
	}

	public float XSize
	{
		get { return m_flXSize; }
	}

	public float ZSize
	{
		get { return m_flZSize; }
	}

	public List<CsFearAltarStageWave> FearAltarStageWaveList
	{
		get { return m_listCsFearAltarStageWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarStage(WPDFearAltarStage fearAltarStage)
	{
		m_nStageId = fearAltarStage.stageId;
		m_strName = CsConfiguration.Instance.GetString(fearAltarStage.nameKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(fearAltarStage.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(fearAltarStage.targetContentKey);
		m_strSceneName = fearAltarStage.sceneName;
		m_flStartXPosition = fearAltarStage.startXPosition;
		m_flStartYPosition = fearAltarStage.startYPosition;
		m_flStartZPosition = fearAltarStage.startZPosition;
		m_flStartRadius = fearAltarStage.startRadius;
		m_nStartYRotationType = fearAltarStage.startYRotationType;
		m_flStartYRotation = fearAltarStage.startYRotation;
		m_csLocation = CsGameData.Instance.GetLocation(fearAltarStage.locationId);
		m_flX = fearAltarStage.x;
		m_flZ = fearAltarStage.z;
		m_flXSize = fearAltarStage.xSize;
		m_flZSize = fearAltarStage.zSize;

		m_listCsFearAltarStageWave = new List<CsFearAltarStageWave>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarStageWave GetFearAltarStageWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsFearAltarStageWave.Count; i++)
		{
			if (m_listCsFearAltarStageWave[i].WaveNo == nWaveNo)
				return m_listCsFearAltarStageWave[i];
		}

		return null;
	}
}
