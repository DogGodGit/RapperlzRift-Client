using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-31)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonTrap
{
	int m_nDungeonNo;
	int m_nDifficulty;
	int m_nTrapId;
	string m_strPrefabName;
	float m_flXPosition;
	float m_flYPosition;
	float m_flZPosition;
	float m_flYRotation;
	float m_flWidth;
	float m_flHeight;
	float m_flHitAreaOffset;
	float m_flStartDelay;
	float m_flCastingStartDelay;
	int m_nCastingDuration;
	int m_nHitCount;
	float m_flCastingTerm;
	float m_flDamage;
	float m_flPrefabScale;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

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

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public float Width
	{
		get { return m_flWidth; }
	}

	public float Height
	{
		get { return m_flHeight; }
	}

	public float HitAreaOffset
	{
		get { return m_flHitAreaOffset; }
	}

	public float StartDelay
	{
		get { return m_flStartDelay; }
	}

	public float CastingStartDelay
	{
		get { return m_flCastingStartDelay; }
	}

	public int CastingDuration
	{
		get { return m_nCastingDuration; }
	}

	public int HitCount
	{
		get { return m_nHitCount; }
	}

	public float CastingTerm
	{
		get { return m_flCastingTerm; }
	}

	public float Damage
	{
		get { return m_flDamage; }
	}

	public float PrefabScale
	{
		get { return m_flPrefabScale; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonTrap(WPDStoryDungeonTrap storyDungeonTrap)
	{
		m_nDungeonNo = storyDungeonTrap.dungeonNo;
		m_nDifficulty = storyDungeonTrap.difficulty;
		m_nTrapId = storyDungeonTrap.trapId;
		m_strPrefabName = storyDungeonTrap.prefabName;
		m_flXPosition = storyDungeonTrap.xPosition;
		m_flYPosition = storyDungeonTrap.yPosition;
		m_flZPosition = storyDungeonTrap.zPosition;
		m_flYRotation = storyDungeonTrap.yRotation;
		m_flWidth = storyDungeonTrap.width;
		m_flHeight = storyDungeonTrap.height;
		m_flHitAreaOffset = storyDungeonTrap.hitAreaOffset;
		m_flStartDelay = storyDungeonTrap.startDelay;
		m_flCastingStartDelay = storyDungeonTrap.castingStartDelay;
		m_nCastingDuration = storyDungeonTrap.castingDuration;
		m_nHitCount = storyDungeonTrap.hitCount;
		m_flCastingTerm = storyDungeonTrap.castingTerm;
		m_flDamage = storyDungeonTrap.damage;
		m_flPrefabScale = storyDungeonTrap.prefabScale;
	}
}
