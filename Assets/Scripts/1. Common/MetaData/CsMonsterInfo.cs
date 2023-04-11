using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterInfo
{
	int m_nMonsterId;								// 몬스터ID
	CsMonsterCharacter m_csMonsterCharacter;		// 몬스터캐릭터ID
	string m_strName;								// 이름
	int m_nLevel;									// 레벨
	float m_flScale;								// 크기
	int m_nHeight;                                  // 높이(단위cm)
	float m_flRadius;                               // 반지름
	int m_nMoveSpeed;                               // 이동속도
	int m_nBattleMoveSpeed;                         // 전투이동속도
	float m_flVisibilityRange;                      // 가시거리
	float m_flPatrolRange;                          // 정찰거리
	float m_flActiveAreaRadius;                     // 활동영역반지름	
	float m_flReturnCompletionRadius;               // 복귀완료반지름
	float m_flSkillCastingInterval;                 // 스킬시전간격
	int m_nMaxHp;                                   // 최대HP
	int m_nPhysicalOffense;                         // 물리공격력
	float m_flQuestAreaRadius;                      // 퀘스트영역반지름	
	bool m_bMoveEnabled;                            // 이동가능여부
	bool m_bAttackEnabled;                          // 공격가능여부
	float m_flAttackStopRange;                      // 공격중지거리
	bool m_bTamingEnabled;
	int m_nMentalStrength;
	float m_flStealRadius;
	int m_nStealSuccessRate;
	CsItemReward m_csItemRewardSteal;

	List<CsMonsterOwnSkill> m_listCsMonsterOwnSkill;

	//---------------------------------------------------------------------------------------------------
	public int MonsterId
	{
		get { return m_nMonsterId; }
	}

	public CsMonsterCharacter MonsterCharacter
	{
		get { return m_csMonsterCharacter; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public float Scale
	{
		get { return m_flScale; }
	}

	public int Height
	{
		get { return m_nHeight; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	public int MoveSpeed
	{
		get { return m_nMoveSpeed; }
	}

	public int BattleMoveSpeed
	{
		get { return m_nBattleMoveSpeed; }
	}

	public float VisibilityRange
	{
		get { return m_flVisibilityRange; }
	}

	public float PatrolRange
	{
		get { return m_flPatrolRange; }
	}

	public float ActiveAreaRadius
	{
		get { return m_flActiveAreaRadius; }
	}

	public float ReturnCompletionRadius
	{
		get { return m_flReturnCompletionRadius; }
	}

	public float SkillCastingInterval
	{
		get { return m_flSkillCastingInterval; }
	}

	public int MaxHp
	{
		get { return m_nMaxHp; }
	}

	public int PhysicalOffense
	{
		get { return m_nPhysicalOffense; }
	}

	public float QuestAreaRadius
	{
		get { return m_flQuestAreaRadius; }
	}

	public bool MoveEnabled
	{
		get { return m_bMoveEnabled; }
	}

	public bool AttackEnabled
	{
		get { return m_bAttackEnabled; }
	}

	public float AttackStopRange
	{
		get { return m_flAttackStopRange; }
	}

	public List<CsMonsterOwnSkill> MonsterOwnSkillList
	{
		get { return m_listCsMonsterOwnSkill; }
	}

	public bool TamingEnabled
	{
		get { return m_bTamingEnabled; }
	}

	public int MentalStrength
	{
		get { return m_nMentalStrength; }
	}

	public float StealRadius
	{
		get { return m_flStealRadius; }
	}

	public int StealSuccessRate
	{
		get { return m_nStealSuccessRate; }
	}

	public CsItemReward StealItemReward
	{
		get { return m_csItemRewardSteal; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterInfo(WPDMonster monster)
	{
		m_nMonsterId = monster.monsterId;
		m_csMonsterCharacter = CsGameData.Instance.GetMonsterCharacter(monster.monsterCharacterId);
		m_strName = CsConfiguration.Instance.GetString(monster.nameKey);
		m_nLevel = monster.level;
		m_flScale = monster.scale;
		m_nHeight = monster.height;
		m_flRadius = monster.radius;
		m_nMoveSpeed = monster.moveSpeed;
		m_nBattleMoveSpeed = monster.battleMoveSpeed;
		m_flVisibilityRange = monster.visibilityRange;
		m_flPatrolRange = monster.patrolRange;
		m_flActiveAreaRadius = monster.activeAreaRadius;
		m_flReturnCompletionRadius = monster.returnCompletionRadius;
		m_flSkillCastingInterval = monster.skillCastingInterval;
		m_nMaxHp = monster.maxHp;
		m_nPhysicalOffense = monster.physicalOffense;
		m_flQuestAreaRadius = monster.questAreaRadius;
		m_bMoveEnabled = monster.moveEnabled;
		m_bAttackEnabled = monster.attackEnabled;
		m_flAttackStopRange = monster.attackStopRange;
		m_bTamingEnabled = monster.tamingEnabled;
		m_nMentalStrength = monster.mentalStrength;
		m_flStealRadius = monster.stealRadius;
		m_nStealSuccessRate = monster.stealSuccessRate;
		m_csItemRewardSteal = CsGameData.Instance.GetItemReward(monster.stealItemRewardId);

		m_listCsMonsterOwnSkill = new List<CsMonsterOwnSkill>();
	}

	// 2018.05.09 Dev
	//---------------------------------------------------------------------------------------------------
	public CsMonsterInfo(int nMonsterId, string strName, string strPrefabName, int nLevel, float flScale, int nMaxHp, int nPhysicalOffense)
	{
		m_nMonsterId = nMonsterId;
		m_csMonsterCharacter = new CsMonsterCharacter(nMonsterId, strName, strPrefabName);
		m_strName = strName;
		m_nLevel = nLevel;
		m_flScale = flScale;
		m_nHeight = 1;
		m_flRadius = 0.5f;
		m_nMoveSpeed = 150;
		m_nBattleMoveSpeed = 350;
		m_flVisibilityRange = 0f;
		m_flPatrolRange = 5f;
		m_flActiveAreaRadius = 15f;
		m_flSkillCastingInterval = 2.5f;
		m_nMaxHp = nMaxHp;
		m_nPhysicalOffense = nPhysicalOffense;
		m_flQuestAreaRadius = 15;
		m_bMoveEnabled = true;
		m_bAttackEnabled = true;
		m_flAttackStopRange = 2;

		m_listCsMonsterOwnSkill = new List<CsMonsterOwnSkill>();
	}
}
