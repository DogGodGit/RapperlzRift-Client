using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimTrap
{
	int m_nTrapId;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flRadius;
	int m_nDamage;

	//---------------------------------------------------------------------------------------------------
	public int TrapId
	{
		get { return m_nTrapId; }
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
		get { return m_nDamage; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimTrap(WPDRuinsReclaimTrap ruinsReclaimTrap)
	{
		m_nTrapId = ruinsReclaimTrap.trapId;
		m_flXPosition = ruinsReclaimTrap.xPosition;
		m_flYPosition = ruinsReclaimTrap.yPosition;
		m_flZPosition = ruinsReclaimTrap.zPosition;
		m_flRadius = ruinsReclaimTrap.radius;
		m_nDamage = ruinsReclaimTrap.damage;
	}
}
