using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsGoldDungeonStepMonsterArrange
{
	int m_nDifficulty;
	int m_nStep;
	int m_nArrangeNo;
	CsMonsterArrange m_csMonsterArrange;
	int m_nMonsterCount;
	Vector3 m_vtPosition;
	float m_flRadius;
	int m_nYRotationType;
	float m_flYRotation;
	bool m_bIsFugitive;
	int m_nActivationWaveNo;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int ArrangeNo
	{
		get { return m_nArrangeNo; }
	}

	public CsMonsterArrange MonsterArrange
	{
		get { return m_csMonsterArrange; }
	}

	public int MonsterCount
	{
		get { return m_nMonsterCount; }
	}

	public Vector3 Position
	{
		get { return m_vtPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int YRotationType
	{
		get { return m_nYRotationType; }
	}

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public bool IsFugitive
	{
		get { return m_bIsFugitive; }
	}

	public int ActivationWaveNo
	{
		get { return m_nActivationWaveNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonStepMonsterArrange(WPDGoldDungeonStepMonsterArrange goldDungeonStepMonsterArrange)
	{
		m_nDifficulty = goldDungeonStepMonsterArrange.difficulty;
		m_nStep = goldDungeonStepMonsterArrange.step;
		m_nArrangeNo = goldDungeonStepMonsterArrange.arrangeNo;
		m_csMonsterArrange = CsGameData.Instance.GetMonsterArrange(goldDungeonStepMonsterArrange.monsterArrangeId);
		m_nMonsterCount = goldDungeonStepMonsterArrange.monsterCount;
		m_vtPosition = new Vector3(goldDungeonStepMonsterArrange.xPosition, goldDungeonStepMonsterArrange.yPosition, goldDungeonStepMonsterArrange.zPosition);
		m_flRadius = goldDungeonStepMonsterArrange.radius;
		m_nYRotationType = goldDungeonStepMonsterArrange.yRotationType;
		m_flYRotation = goldDungeonStepMonsterArrange.yRotation;
		m_bIsFugitive = goldDungeonStepMonsterArrange.isFugitive;
		m_nActivationWaveNo = goldDungeonStepMonsterArrange.activationWaveNo;
	}
}
