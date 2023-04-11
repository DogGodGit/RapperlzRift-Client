using WebCommon;
using UnityEngine;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildHuntingQuestObjective
{
	int m_nObjectiveId;
    CsContinentObject m_csContinentObjectTarget;
    int m_nMinHeroLevel;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strTargetDescription;
	int m_nType;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPostion;
	float m_flTargetRadius;
	int m_nTargetContinentObjectId;
	CsMonsterInfo m_csMonsterTarget;
	int m_nTargetCount;

	//---------------------------------------------------------------------------------------------------
	public int ObjectiveId
	{
		get { return m_nObjectiveId; }
	}

	public int MinHeroLevel
	{
		get { return m_nMinHeroLevel; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string TargetDescription
	{
		get { return m_strTargetDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosition
	{
		get { return m_vtTargetPostion; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int TargetContinentObjectId
	{
		get { return m_nTargetContinentObjectId; }
	}

	public CsMonsterInfo TargetMonster
	{
		get { return m_csMonsterTarget; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

    public CsContinentObject TargetContinentObject
    {
        get { return m_csContinentObjectTarget; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsGuildHuntingQuestObjective(WPDGuildHuntingQuestObjective guildHuntingQuestObjective)
	{
		m_nObjectiveId = guildHuntingQuestObjective.objectiveId;
		m_nMinHeroLevel = guildHuntingQuestObjective.minHeroLevel;
		m_strTargetTitle = CsConfiguration.Instance.GetString(guildHuntingQuestObjective.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(guildHuntingQuestObjective.targetContentKey);
		m_strTargetDescription = CsConfiguration.Instance.GetString(guildHuntingQuestObjective.targetDescriptionKey);
		m_nType = guildHuntingQuestObjective.type;
		m_csContinentTarget = CsGameData.Instance.GetContinent(guildHuntingQuestObjective.targetContinentId);
		m_vtTargetPostion = new Vector3(guildHuntingQuestObjective.targetXPosition, guildHuntingQuestObjective.targetYPosition, guildHuntingQuestObjective.targetZPosition);
		m_flTargetRadius = guildHuntingQuestObjective.targetRadius;
        m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(guildHuntingQuestObjective.targetContinentObjectId);
        m_nTargetContinentObjectId = guildHuntingQuestObjective.targetContinentObjectId;
        m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(guildHuntingQuestObjective.targetMonsterId);
		m_nTargetCount = guildHuntingQuestObjective.targetCount;
	}
}
