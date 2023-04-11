using System.Collections.Generic;
using WebCommon;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTemple
{
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nRequiredStamina;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartYRotation;
	CsItem m_csAvailableRewardItem;
	string m_strGuideImageName;
	int m_nColorMatchingPoint;
	int m_nColorMatchingObjectivePoint;
	int m_nColorMatchingMonsterSpawnInterval;
	CsMonsterArrange m_csMonsterArrangeColorMatching;
	float m_flColorMatchingMonsterXPosition;
	float m_flColorMatchingMonsterYPosition;
	float m_flColorMatchingMonsterZPosition;
	int m_nColorMatchingMonsterYRotationType;
	float m_flColorMatchingMonsterYRotation;
	int m_nColorMatchingMonsterKillPoint;
	int m_nColorMatchingMonsterKillObjectId;
	string m_strColorMatchingMonsterSpawnGuideTitle;
	string m_strColorMatchingMonsterSpawnGuideContent;
	CsMonsterArrange m_csMonsterArrangeFindTreasureBox;
	string m_strFindTreasureBoxSuccessGuideTitle;
	string m_strFindTreasureBoxSuccessGuideContent;
	string m_strPuzzleRewardTargetTitle;
	string m_strPuzzleRewardTargetContent;
	string m_strPuzzleRewardGuideTitle;
	string m_strPuzzleRewardGuideContent;
	string m_strPuzzleRewardObjectPrefabName;
	float m_flPuzzleRewardObjectInteractionDuration;
	float m_flPuzzleRewardObjectInteractionMaxRange;
	float m_flPuzzleRewardObjectScale;
	int m_nPuzzleRewardObjectHeight;
	float m_flPuzzleRewardObjectRadius;
	string m_strQuizRightAnswerGuideTitle;
	string m_strQuizRightAnswerGuideContent;
	string m_strQuizWrongAnswerGuideTitle;
	string m_strQuizWrongAnswerGuideContent;
	int m_nBossMonsterSpawnDelayTime;
	CsMonsterArrange m_csMonsterArrangeBoss;
	float m_flBossMonsterXPosition;
	float m_flBossMonsterYPosition;
	float m_flBossMonsterZPosition;
	float m_flBossMonsterYRotation;
	string m_strBossMonsterTargetTitle;
	string m_strBossMonsterTargetContent;
	string m_strBossMonsterSpawnGuideTitle;
	string m_strBossMonsterSpawnGuideContent;
	CsItemReward m_csItemRewardBossMonsterKill;
	CsItemReward m_csItemRewardSweep;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	DateTime m_dtPlayDate;
	int m_nDailyWisdomTemplePlayCount;
	bool m_bWisdomTempleCleared;

	List<CsWisdomTempleMonsterAttrFactor> m_listCsWisdomTempleMonsterAttrFactor;
	List<CsWisdomTempleColorMatchingObject> m_listCsWisdomTempleColorMatchingObject;
	List<CsWisdomTempleArrangePosition> m_listCsWisdomTempleArrangePosition;
	List<CsWisdomTempleSweepReward> m_listCsWisdomTempleSweepReward;
	List<CsWisdomTempleStep> m_listCsWisdomTempleStep;
	List<CsWisdomTemplePuzzle> m_listCsWisdomTemplePuzzle;
	List<CsWisdomTempleStepReward> m_listCsWisdomTempleStepReward;

	//---------------------------------------------------------------------------------------------------
	public DateTime PlayDate
	{
		get { return m_dtPlayDate; }
		set { m_dtPlayDate = value; }
	}

	public int DailyWisdomTemplePlayCount 
	{
		get { return m_nDailyWisdomTemplePlayCount; }
		set { m_nDailyWisdomTemplePlayCount = value; }
	}
	public bool WisdomTempleCleared 
	{ 
		get { return m_bWisdomTempleCleared; }
		set { m_bWisdomTempleCleared = value; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
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

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public CsItem AvailableRewardItem
	{
		get { return m_csAvailableRewardItem; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public int ColorMatchingPoint
	{
		get { return m_nColorMatchingPoint; }
	}

	public int ColorMatchingObjectivePoint
	{
		get { return m_nColorMatchingObjectivePoint; }
	}

	public int ColorMatchingMonsterSpawnInterval
	{
		get { return m_nColorMatchingMonsterSpawnInterval; }
	}

	public CsMonsterArrange ColorMatchingMonsterArrange
	{
		get { return m_csMonsterArrangeColorMatching; }
	}

	public float ColorMatchingMonsterXPosition
	{
		get { return m_flColorMatchingMonsterXPosition; }
	}

	public float ColorMatchingMonsterYPosition
	{
		get { return m_flColorMatchingMonsterYPosition; }
	}

	public float ColorMatchingMonsterZPosition
	{
		get { return m_flColorMatchingMonsterZPosition; }
	}

	public int ColorMatchingMonsterYRotationType
	{
		get { return m_nColorMatchingMonsterYRotationType; }
	}

	public float ColorMatchingMonsterYRotation
	{
		get { return m_flColorMatchingMonsterYRotation; }
	}

	public int ColorMatchingMonsterKillPoint
	{
		get { return m_nColorMatchingMonsterKillPoint; }
	}

	public int ColorMatchingMonsterKillObjectId
	{
		get { return m_nColorMatchingMonsterKillObjectId; }
	}

	public string ColorMatchingMonsterSpawnGuideTitle
	{
		get { return m_strColorMatchingMonsterSpawnGuideTitle; }
	}

	public string ColorMatchingMonsterSpawnGuideContent
	{
		get { return m_strColorMatchingMonsterSpawnGuideContent; }
	}

	public CsMonsterArrange FindTreasureBoxMonsterArrange
	{
		get { return m_csMonsterArrangeFindTreasureBox; }
	}

	public string FindTreasureBoxSuccessGuideTitle
	{
		get { return m_strFindTreasureBoxSuccessGuideTitle; }
	}

	public string FindTreasureBoxSuccessGuideContent
	{
		get { return m_strFindTreasureBoxSuccessGuideContent; }
	}

	public string PuzzleRewardTargetTitle
	{
		get { return m_strPuzzleRewardTargetTitle; }
	}

	public string PuzzleRewardTargetContent
	{
		get { return m_strPuzzleRewardTargetContent; }
	}

	public string PuzzleRewardGuideTitle
	{
		get { return m_strPuzzleRewardGuideTitle; }
	}

	public string PuzzleRewardGuideContent
	{
		get { return m_strPuzzleRewardGuideContent; }
	}

	public string PuzzleRewardObjectPrefabName
	{
		get { return m_strPuzzleRewardObjectPrefabName; }
	}

	public float PuzzleRewardObjectInteractionDuration
	{
		get { return m_flPuzzleRewardObjectInteractionDuration; }
	}

	public float PuzzleRewardObjectInteractionMaxRange
	{
		get { return m_flPuzzleRewardObjectInteractionMaxRange; }
	}

	public float PuzzleRewardObjectScale
	{
		get { return m_flPuzzleRewardObjectScale; }
	}

	public int PuzzleRewardObjectHeight
	{
		get { return m_nPuzzleRewardObjectHeight; }
	}

	public float PuzzleRewardObjectRadius
	{
		get { return m_flPuzzleRewardObjectRadius; }
	}

	public string QuizRightAnswerGuideTitle
	{
		get { return m_strQuizRightAnswerGuideTitle; }
	}

	public string QuizRightAnswerGuideContent
	{
		get { return m_strQuizRightAnswerGuideContent; }
	}

	public string QuizWrongAnswerGuideTitle
	{
		get { return m_strQuizWrongAnswerGuideTitle; }
	}

	public string QuizWrongAnswerGuideContent
	{
		get { return m_strQuizWrongAnswerGuideContent; }
	}

	public int BossMonsterSpawnDelayTime
	{
		get { return m_nBossMonsterSpawnDelayTime; }
	}

	public CsMonsterArrange BossMonsterArrange
	{
		get { return m_csMonsterArrangeBoss; }
	}

	public float BossMonsterXPosition
	{
		get { return m_flBossMonsterXPosition; }
	}

	public float BossMonsterYPosition
	{
		get { return m_flBossMonsterYPosition; }
	}

	public float BossMonsterZPosition
	{
		get { return m_flBossMonsterZPosition; }
	}

	public float BossMonsterYRotation
	{
		get { return m_flBossMonsterYRotation; }
	}

	public string BossMonsterTargetTitle
	{
		get { return m_strBossMonsterTargetTitle; }
	}

	public string BossMonsterTargetContent
	{
		get { return m_strBossMonsterTargetContent; }
	}

	public string BossMonsterSpawnGuideTitle
	{
		get { return m_strBossMonsterSpawnGuideTitle; }
	}

	public string BossMonsterSpawnGuideContent
	{
		get { return m_strBossMonsterSpawnGuideContent; }
	}

	public CsItemReward BossMonsterKillItemReward
	{
		get { return m_csItemRewardBossMonsterKill; }
	}

	public CsItemReward SweepItemReward
	{
		get { return m_csItemRewardSweep; }
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

	public List<CsWisdomTempleMonsterAttrFactor> WisdomTempleMonsterAttrFactorList
	{
		get { return m_listCsWisdomTempleMonsterAttrFactor; }
	}

	public List<CsWisdomTempleColorMatchingObject> WisdomTempleColorMatchingObjectList
	{
		get { return m_listCsWisdomTempleColorMatchingObject; }
	}

	public List<CsWisdomTempleArrangePosition> WisdomTempleArrangePositionList
	{
		get { return m_listCsWisdomTempleArrangePosition; }
	}

	public List<CsWisdomTempleSweepReward> WisdomTempleSweepRewardList
	{
		get { return m_listCsWisdomTempleSweepReward; }
	}

	public List<CsWisdomTempleStep> WisdomTempleStepList
	{
		get { return m_listCsWisdomTempleStep; }
	}

	public List<CsWisdomTemplePuzzle> WisdomTemplePuzzleList
	{
		get { return m_listCsWisdomTemplePuzzle; }
	}

	public List<CsWisdomTempleStepReward> WisdomTempleStepRewardList
	{
		get { return m_listCsWisdomTempleStepReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTemple(WPDWisdomTemple wisdomTemple)
	{
		m_strName = CsConfiguration.Instance.GetString(wisdomTemple.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(wisdomTemple.descriptionKey);
		m_strSceneName = wisdomTemple.sceneName;
		m_nRequiredConditionType = wisdomTemple.requiredConditionType;
		m_nRequiredMainQuestNo = wisdomTemple.requiredMainQuestNo;
		m_nRequiredHeroLevel = wisdomTemple.requiredHeroLevel;
		m_nRequiredStamina = wisdomTemple.requiredStamina;
		m_nStartDelayTime = wisdomTemple.startDelayTime;
		m_nLimitTime = wisdomTemple.limitTime;
		m_nExitDelayTime = wisdomTemple.exitDelayTime;
		m_flStartXPosition = wisdomTemple.startXPosition;
		m_flStartYPosition = wisdomTemple.startYPosition;
		m_flStartZPosition = wisdomTemple.startZPosition;
		m_flStartYRotation = wisdomTemple.startYRotation;
		m_csAvailableRewardItem = CsGameData.Instance.GetItem(wisdomTemple.availableRewardItemId);
		m_strGuideImageName = wisdomTemple.guideImageName;
		m_nColorMatchingPoint = wisdomTemple.colorMatchingPoint;
		m_nColorMatchingObjectivePoint = wisdomTemple.colorMatchingObjectivePoint;
		m_nColorMatchingMonsterSpawnInterval = wisdomTemple.colorMatchingMonsterSpawnInterval;
		m_csMonsterArrangeColorMatching = CsGameData.Instance.GetMonsterArrange(wisdomTemple.colorMatchingMonsterArrangeId);
		m_flColorMatchingMonsterXPosition = wisdomTemple.colorMatchingMonsterXPosition;
		m_flColorMatchingMonsterYPosition = wisdomTemple.colorMatchingMonsterYPosition;
		m_flColorMatchingMonsterZPosition = wisdomTemple.colorMatchingMonsterZPosition;
		m_nColorMatchingMonsterYRotationType = wisdomTemple.colorMatchingMonsterYRotationType;
		m_flColorMatchingMonsterYRotation = wisdomTemple.colorMatchingMonsterYRotation;
		m_nColorMatchingMonsterKillPoint = wisdomTemple.colorMatchingMonsterKillPoint;
		m_nColorMatchingMonsterKillObjectId = wisdomTemple.colorMatchingMonsterKillObjectId;
		m_strColorMatchingMonsterSpawnGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.colorMatchingMonsterSpawnGuideTitleKey);
		m_strColorMatchingMonsterSpawnGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.colorMatchingMonsterSpawnGuideContentKey);
		m_csMonsterArrangeFindTreasureBox = CsGameData.Instance.GetMonsterArrange(wisdomTemple.findTreasureBoxMonsterArrangeId);
		m_strFindTreasureBoxSuccessGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.findTreasureBoxSuccessGuideTitleKey);
		m_strFindTreasureBoxSuccessGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.findTreasureBoxSuccessGuideContentKey);
		m_strPuzzleRewardTargetTitle = CsConfiguration.Instance.GetString(wisdomTemple.puzzleRewardTargetTitleKey);
		m_strPuzzleRewardTargetContent = CsConfiguration.Instance.GetString(wisdomTemple.puzzleRewardTargetContentKey);
		m_strPuzzleRewardGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.puzzleRewardGuideTitleKey);
		m_strPuzzleRewardGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.puzzleRewardGuideContentKey);
		m_strPuzzleRewardObjectPrefabName = wisdomTemple.puzzleRewardObjectPrefabName;
		m_flPuzzleRewardObjectInteractionDuration = wisdomTemple.puzzleRewardObjectInteractionDuration;
		m_flPuzzleRewardObjectInteractionMaxRange = wisdomTemple.puzzleRewardObjectInteractionMaxRange;
		m_flPuzzleRewardObjectScale = wisdomTemple.puzzleRewardObjectScale;
		m_nPuzzleRewardObjectHeight = wisdomTemple.puzzleRewardObjectHeight;
		m_flPuzzleRewardObjectRadius = wisdomTemple.puzzleRewardObjectRadius;
		m_strQuizRightAnswerGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.quizRightAnswerGuideTitleKey);
		m_strQuizRightAnswerGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.quizRightAnswerGuideContentKey);
		m_strQuizWrongAnswerGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.quizWrongAnswerGuideTitleKey);
		m_strQuizWrongAnswerGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.quizWrongAnswerGuideContentKey);
		m_nBossMonsterSpawnDelayTime = wisdomTemple.bossMonsterSpawnDelayTime;
		m_csMonsterArrangeBoss = CsGameData.Instance.GetMonsterArrange(wisdomTemple.bossMonsterArrangeId);
		m_flBossMonsterXPosition = wisdomTemple.bossMonsterXPosition;
		m_flBossMonsterYPosition = wisdomTemple.bossMonsterYPosition;
		m_flBossMonsterZPosition = wisdomTemple.bossMonsterZPosition;
		m_flBossMonsterYRotation = wisdomTemple.bossMonsterYRotation;
		m_strBossMonsterTargetTitle = CsConfiguration.Instance.GetString(wisdomTemple.bossMonsterTargetTitleKey);
		m_strBossMonsterTargetContent = CsConfiguration.Instance.GetString(wisdomTemple.bossMonsterTargetContentKey);
		m_strBossMonsterSpawnGuideTitle = CsConfiguration.Instance.GetString(wisdomTemple.bossMonsterSpawnGuideTitleKey);
		m_strBossMonsterSpawnGuideContent = CsConfiguration.Instance.GetString(wisdomTemple.bossMonsterSpawnGuideContentKey);
		m_csItemRewardBossMonsterKill = CsGameData.Instance.GetItemReward(wisdomTemple.bossMonsterKillItemRewardId);
		m_csItemRewardSweep = CsGameData.Instance.GetItemReward(wisdomTemple.sweepItemRewardId);
		m_csLocation = CsGameData.Instance.GetLocation(wisdomTemple.locationId);
		m_flX = wisdomTemple.x;
		m_flZ = wisdomTemple.z;
		m_flXSize = wisdomTemple.xSize;
		m_flZSize = wisdomTemple.zSize;

		m_listCsWisdomTempleMonsterAttrFactor = new List<CsWisdomTempleMonsterAttrFactor>();
		m_listCsWisdomTempleColorMatchingObject = new List<CsWisdomTempleColorMatchingObject>();
		m_listCsWisdomTempleArrangePosition = new List<CsWisdomTempleArrangePosition>();
		m_listCsWisdomTempleSweepReward = new List<CsWisdomTempleSweepReward>();
		m_listCsWisdomTempleStep = new List<CsWisdomTempleStep>();
		m_listCsWisdomTemplePuzzle = new List<CsWisdomTemplePuzzle>();
		m_listCsWisdomTempleStepReward = new List<CsWisdomTempleStepReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleStep GetWisdomTempleStep(int nStepNo)
	{
		for (int i = 0; i < m_listCsWisdomTempleStep.Count; i++)
		{
			if (m_listCsWisdomTempleStep[i].StepNo == nStepNo)
				return m_listCsWisdomTempleStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTemplePuzzle GetWisdomTemplePuzzle(int nPuzzleId)
	{
		for (int i = 0; i < m_listCsWisdomTemplePuzzle.Count; i++)
		{
			if (m_listCsWisdomTemplePuzzle[i].PuzzleId == nPuzzleId)
			{
				return m_listCsWisdomTemplePuzzle[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWisdomTempleColorMatchingObject GetWisdomTempleColorMatchingObject(int nObjectId)
	{
		for (int i = 0; i < m_listCsWisdomTempleColorMatchingObject.Count; i++)
		{
			if (m_listCsWisdomTempleColorMatchingObject[i].ObjectId == nObjectId)
			{
				return m_listCsWisdomTempleColorMatchingObject[i];
			}
		}

		return null;
	}
}
