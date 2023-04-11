using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNestTrap
{
	int m_nTrapId;
	string m_strPrefabName;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_flDamage;
	int m_nActivationStepNo;
	int m_nDeactivationStepNo;

	//---------------------------------------------------------------------------------------------------
	public int TrapId
	{
		get { return m_nTrapId; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float XPosition
	{
		get { return m_flXPosition; }
	}

	public float YPosition
	{
		get { return m_flYPosition; }
	}

	public float ZPosition
	{
		get { return m_flZPosition; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int Damage
	{
		get { return m_flDamage; }
	}

	public int ActivationStepNo
	{
		get { return m_nActivationStepNo; }
	}

	public int DeactivationStepNo
	{
		get { return m_nDeactivationStepNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDragonNestTrap(WPDDragonNestTrap dragonNestTrap)
	{
		m_nTrapId = dragonNestTrap.trapId;
		m_strPrefabName = dragonNestTrap.prefabName;
		m_flXPosition = dragonNestTrap.xPosition;
		m_flYPosition = dragonNestTrap.yPosition;
		m_flZPosition = dragonNestTrap.zPosition;
		m_flRadius = dragonNestTrap.radius;
		m_flDamage = dragonNestTrap.damage;
		m_nActivationStepNo = dragonNestTrap.activationStepNo;
		m_nDeactivationStepNo = dragonNestTrap.deactivationStepNo;
	}
}
