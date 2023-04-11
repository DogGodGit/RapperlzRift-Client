using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------------------------------------------------
//작성: 최민수 (2018-10-05)
//-------------------------------------------------------------------------------------------------------

/**
 * 1. 크리쳐 양육
 * 2. 크리쳐 변이
 * 3. 크리쳐 주입
 * 4. 날개 장착
 * 5. 날개 육성
 * 6. 메인 장비 세련
 * 7. 소울 스톤 장착
 * 8. 별자리
 * 9. 탈 것 양육
 * 10. 메인 장비 강화
 * 11. 보조장비 레벨업
 * 12. 계급
 * 13. 컬렉션 활성화
 * 14. 충전
 * 15. 달성도 보상
 * 16. 업적
 * 17. 연속 미션
 * 18. 지하 미로
 * 19. 필드 보스
 * 20. 영혼을 탐하는 자
 * 21. 용맹의 증명
 * 22. 카드 뽑기
 * 23. 컬렉션 상점
 * 24. 주간 퀘스트
 * 25. 상점
 * 26. 월드 드랍
 * 27. 아이템 합성
 * 28. 출석 보상
 * 29. 접속 보상
 * 30. 차원 퀘스트
 * 31. 차원전
 * 32. 고대 유물의 방
 * 33. 농장의 위협
 * 34. 용의 둥지
 * 35. 크리쳐 농장
 * 36. 반역자의 은신처
 * 37. 전쟁의 기억
 * 38. PK
 * 39. 차원 헌납
 * 40. 길드 미션
 * 41. 길드 농장
 * 42. 길드 헌납
 */

public enum EnImproveContent
{
	CreatureTraining = 1,
	CreatureComposition = 2,
	CreatureInjection = 3,
	WingEquipment = 4,
	WingEnchant = 5,
	MainGearRefine = 6,
	MainGearTransit = 7,
	Constellation = 8,
	MountLevelUp = 9,
	MainGearEnchant = 10,
	SubGearLevelUp = 11,
	Class = 12,
	CardCollection = 13,
	DiaCharging = 14,
	TodayTask = 15,
	AchevementAccomplishment = 16,
	SeriesMission = 17,
	UndergroundMaze = 18,
	FieldBoss = 19,
	SoulCoveter = 20,
	ProofOfValor = 21,
	LuckyShop = 22,
	CardShop = 23,
	WeeklyQuest = 24,
	DiaShop = 25,
	WorldDrop = 26,
	ItemCompose = 27,
	AttendReward = 28,
	AccessReward = 29,
	NationQuest = 30,
	NationDiplomacy = 31,
	AncientRelic = 32,
	FarmThreat = 33,
	DragonNest = 34,
	CreatureFarm = 35,
	TraitorsHideout = 36,
	MemoryOfWar = 37,
	PK = 38,
	NationOblation = 39,
	GuildMission = 40,
	GuildFarm = 41,
	GuildOblation = 42,
}

public class CsPopupImprovement : MonoBehaviour 
{
	Transform m_trMainCategory;
	Transform m_trContent;
	Transform m_trCanvas2;

	GameObject m_goListEmpty;

	string m_strImagePath;

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Transform trBackground = transform.Find("ImageBackground");
		m_trContent = transform.Find("ImageBackground/ContentScrollView/Viewport/Content");
		m_trCanvas2 = GameObject.Find("Canvas2").transform;

		CsUIData.Instance.SetText(trBackground.Find("TextTitle"), CsConfiguration.Instance.GetString("A150_TITLE_00001"), true);

		CsUIData.Instance.SetButton(trBackground.Find("ButtonEsc"), OnClickExit);	

		CsRecommendBattlePowerLevel csRecommendBattlePowerLevel = CsGameData.Instance.GetRecommendBattlePowerLevel(CsGameData.Instance.MyHeroInfo.Level);

		string strImagePath;
		long lMyBattlePowerPercent = (CsGameData.Instance.MyHeroInfo.BattlePower / csRecommendBattlePowerLevel.SRankBattlePower) * 100;

		if (CsGameData.Instance.MyHeroInfo.BattlePower >= csRecommendBattlePowerLevel.SRankBattlePower)
		{
			strImagePath = "GUI/PopupImprovement/frm_dungeon_rank_5";
		}
		else if (lMyBattlePowerPercent >= 10)
		{
			strImagePath = "GUI/PopupImprovement/frm_dungeon_rank_4";
		}
		else if (lMyBattlePowerPercent >= 20 && lMyBattlePowerPercent < 10)
		{
			strImagePath = "GUI/PopupImprovement/frm_dungeon_rank_3";
		}
		else if (lMyBattlePowerPercent >= 30 && lMyBattlePowerPercent < 20)
		{
			strImagePath = "GUI/PopupImprovement/frm_dungeon_rank_2";
		}
		else
		{
			strImagePath = "GUI/PopupImprovement/frm_dungeon_rank_1";
		}

		CsUIData.Instance.SetImage(trBackground.Find("ImageRank"), strImagePath);

		CsUIData.Instance.SetText(trBackground.Find("BattlePower/TextMyBattlePower"), string.Format(CsConfiguration.Instance.GetString("A150_TXT_00001"), CsGameData.Instance.MyHeroInfo.BattlePower), true);
		CsUIData.Instance.SetText(trBackground.Find("BattlePower/TextRecommandBattlePower"), string.Format(CsConfiguration.Instance.GetString("A150_TXT_00002"), csRecommendBattlePowerLevel.SRankBattlePower), true);

		m_trMainCategory = trBackground.Find("CategoryScrollView/Viewport/Content");

		m_goListEmpty = trBackground.Find("ListEmpty").gameObject;
		CsUIData.Instance.SetText(m_goListEmpty.transform.Find("Text"), CsConfiguration.Instance.GetString("A150_TXT_00003"), true);
		m_goListEmpty.SetActive(false);
		
		for (int i = 0; i < CsGameData.Instance.ImprovementMainCategoryList.Count; i++)
		{
			List<CsImprovementSubCategory> listCsImprovementSubCategory = CsGameData.Instance.ImprovementMainCategoryList[i].ImprovementSubCategoryList;

			if (listCsImprovementSubCategory.Count == 0)
			{
				GameObject goToggleMainCategory = Instantiate<GameObject>(Resources.Load<GameObject>("GUI/PopupImprovement/MainCategoryToggle"), m_trMainCategory);
				goToggleMainCategory.name = CsGameData.Instance.ImprovementMainCategoryList[i].MainCategoryId.ToString();

				CsUIData.Instance.SetText(goToggleMainCategory.transform.Find("Label"), CsGameData.Instance.ImprovementMainCategoryList[i].Name, false);

				int nMainCategoryId;

				if (int.TryParse(goToggleMainCategory.name.ToString(), out nMainCategoryId))
				{
					Toggle toggle = goToggleMainCategory.GetComponent<Toggle>();
					toggle.group = m_trMainCategory.GetComponent<ToggleGroup>();
					toggle.onValueChanged.RemoveAllListeners();
					toggle.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
					toggle.onValueChanged.AddListener((ison) => OnClickCategory(ison, nMainCategoryId, false));

					toggle.isOn = false;
				}
			}
			else
			{
				GameObject goToggleParentCategory = Instantiate<GameObject>(Resources.Load<GameObject>("GUI/PopupImprovement/ParentCategory"), m_trMainCategory);

				CsParentCategory csParentCategory = goToggleParentCategory.GetComponent<CsParentCategory>();
				csParentCategory.MainCategoryId = CsGameData.Instance.ImprovementMainCategoryList[i].MainCategoryId;

				goToggleParentCategory.name = csParentCategory.MainCategoryId.ToString();

				CsUIData.Instance.SetText(goToggleParentCategory.transform.Find("Text"), CsGameData.Instance.ImprovementMainCategoryList[i].Name, false);

				Image imageArrow = goToggleParentCategory.transform.Find("ImageArrow").GetComponent<Image>();

				Button buttonCategory = goToggleParentCategory.GetComponent<Button>();
				buttonCategory.onClick.RemoveAllListeners();
				buttonCategory.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
				buttonCategory.onClick.AddListener(() => OnClickParentCategory(csParentCategory, imageArrow));

				int[] anChildCategoryId = new int[listCsImprovementSubCategory.Count];

				for (int j = 0; j < listCsImprovementSubCategory.Count; j++)
				{
					int nSubCategoryId = listCsImprovementSubCategory[j].SubCategoryId;
					anChildCategoryId[j] = nSubCategoryId;

					GameObject goSubCategory = Instantiate<GameObject>(Resources.Load<GameObject>("GUI/PopupImprovement/MainCategoryToggle"), m_trMainCategory);
					goSubCategory.name = "Sub" + nSubCategoryId.ToString();

					CsUIData.Instance.SetText(goSubCategory.transform.Find("Label"), listCsImprovementSubCategory[j].Name, false);

					Toggle toggleChild = goSubCategory.GetComponent<Toggle>();
					toggleChild.group = m_trMainCategory.GetComponent<ToggleGroup>();
					toggleChild.onValueChanged.RemoveAllListeners();
					toggleChild.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
					toggleChild.onValueChanged.AddListener((ison) => OnClickCategory(ison, csParentCategory.MainCategoryId, true, nSubCategoryId));

					goSubCategory.SetActive(false);
					toggleChild.isOn = false;
				}

				csParentCategory.ChildCategoryId = anChildCategoryId;
			}
		}

		CsImprovementMainCategory csImprovementMainCategory = CsGameData.Instance.ImprovementMainCategoryList[0];
		OpenMainContentMenu(csImprovementMainCategory);

		m_trMainCategory.GetChild(0).GetComponent<Toggle>().isOn = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OpenMainContentMenu(CsImprovementMainCategory csImprovementMainCategory)
	{
		RemoveContents();
		
		for (int i = 0; i < csImprovementMainCategory.ImprovementMainCategoryContentList.Count; i++)
		{
			CsImprovementContent csImprovementContent = csImprovementMainCategory.ImprovementMainCategoryContentList[i].ImprovementContent;

			if (csImprovementContent != null)
			{
				CreateContent(csImprovementContent);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenSubContentMenu(CsImprovementSubCategory csImprovementSubCategory)
	{
		RemoveContents();

		for (int i = 0; i < csImprovementSubCategory.ImprovementSubCategoryContentList.Count; i++)
		{
			CsImprovementContent csImprovementContent = csImprovementSubCategory.ImprovementSubCategoryContentList[i].ImprovementContent;

			if (csImprovementContent != null)
			{
				CreateContent(csImprovementContent);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateContent(CsImprovementContent csImprovementContent)
	{
		GameObject goContent;

		EnMainMenu enMainMenu;
		EnSubMenu enSubMenu;

		if (csImprovementContent.RequiredConditionType == 1)
		{
			if (csImprovementContent.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level)
				return;
		}
		else if (csImprovementContent.RequiredConditionType == 2)
		{
			if (csImprovementContent.RequiredMainQuestNo > CsMainQuestManager.Instance.MainQuest.MainQuestNo)
				return;
		}

		if (csImprovementContent.IsAchievementDisplay)
		{
			goContent = Instantiate<GameObject>(Resources.Load<GameObject>("GUI/PopupImprovement/ImprovementArchiveSlot"), m_trContent);

			SetImageIconAndMenuId(csImprovementContent, out enMainMenu, out enSubMenu);

			CsUIData.Instance.SetImage(goContent.transform.Find("ImageIcon"), m_strImagePath);
			CsUIData.Instance.SetText(goContent.transform.Find("TextTitle"), csImprovementContent.Name, false);

			CsImprovementContentAchievementLevel csImprovementContentAchievementLevel = csImprovementContent.GetImprovementContentAchievementLevel(CsGameData.Instance.MyHeroInfo.Level); 

			if (csImprovementContentAchievementLevel != null)
			{
				float flContentValue = GetContentAchievement(csImprovementContent.ContentId);
				float flAchievementRate = flContentValue / csImprovementContentAchievementLevel.AchievementValue;

				List<CsImprovementContentAchievement> listCsImprovementContentAchievement = CsGameData.Instance.ImprovementContentAchievementList;
				CsImprovementContentAchievement csImprovementContentAchievement = listCsImprovementContentAchievement[0];

				for (int i = 0; i < listCsImprovementContentAchievement.Count; i++)
				{
					if ((float)listCsImprovementContentAchievement[i].AchievementRate / 10000.0f <= flAchievementRate)
					{
						csImprovementContentAchievement = listCsImprovementContentAchievement[i];
					}
				}

				if (csImprovementContentAchievement != null)
				{
					CsUIData.Instance.SetText(goContent.transform.Find("TextDetail"), "<color=" + csImprovementContentAchievement.ColorCode + ">" + csImprovementContentAchievement.Name + "</color>", false);

					Image imageProgress = goContent.transform.Find("ImageProgressBack/ImageProgress").GetComponent<Image>();
					imageProgress.sprite = Resources.Load<Sprite>("GUI/PopupImprovement/frm_guage_creature_" + csImprovementContentAchievement.Achievement); ;
					imageProgress.fillAmount = flAchievementRate;
				}
				
			}
		}
		else
		{
			goContent = Instantiate<GameObject>(Resources.Load<GameObject>("GUI/PopupImprovement/ImprovementSlot"), m_trContent);

			SetImageIconAndMenuId(csImprovementContent, out enMainMenu, out enSubMenu);

			CsUIData.Instance.SetImage(goContent.transform.Find("ImageIcon"), m_strImagePath);
			CsUIData.Instance.SetText(goContent.transform.Find("TextTitle"), csImprovementContent.Name, false);
			CsUIData.Instance.SetText(goContent.transform.Find("TextDetail"), csImprovementContent.Description, false);
		}

		CsUIData.Instance.SetButton(goContent.transform.Find("ButtonMove"), () => OnClickMoveContent(csImprovementContent, enMainMenu, enSubMenu));
		CsUIData.Instance.SetText(goContent.transform.Find("ButtonMove/Text"), CsConfiguration.Instance.GetString("A150_BTN_00001"), true);
	}

	//---------------------------------------------------------------------------------------------------
	float GetContentAchievement(int nContentId)
	{
		float flContentValue = 0;

		EnImproveContent enImproveContent = (EnImproveContent)nContentId;

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsCreatureManager.Instance.ParticipatedCreatureId);

		switch(enImproveContent)
		{
			case EnImproveContent.CreatureTraining:				
				if (csHeroCreature != null)
					flContentValue = csHeroCreature.Level;
				else
					flContentValue = 0;

				break;

			case EnImproveContent.CreatureComposition:
				if (csHeroCreature != null)
					flContentValue = csHeroCreature.Quality / csHeroCreature.Creature.MaxQuality;
				else
					flContentValue = 0;
				break;

			case EnImproveContent.CreatureInjection:
				if (csHeroCreature != null)
					flContentValue = csHeroCreature.InjectionLevel;
				else
					flContentValue = 0;

				break;

			case EnImproveContent.WingEquipment:
				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingList.Count; i++)
				{
					if (CsGameData.Instance.MyHeroInfo.HeroWingList[i].MemoryPieceStep > flContentValue)
						flContentValue = CsGameData.Instance.MyHeroInfo.HeroWingList[i].MemoryPieceStep;
				}

				break;

			case EnImproveContent.WingEnchant:
				flContentValue = CsGameData.Instance.MyHeroInfo.WingLevel;
				break;

			case EnImproveContent.MainGearRefine:
				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
				{
					flContentValue += CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].BattlePower;
				}
				
				break;

			case EnImproveContent.MainGearTransit:
				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSubGearList.Count; i++)
				{
					for (int j = 0; j < CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].SoulstoneSocketList.Count; j++)
					{
						flContentValue += CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].SoulstoneSocketList[j].Item.Level;
					}
				}

				break;

			case EnImproveContent.Constellation:
				flContentValue = CsConstellationManager.Instance.StarEssense;
				break;

			case EnImproveContent.MountLevelUp:
				flContentValue = CsGameData.Instance.MyHeroInfo.GetHeroMount(CsGameData.Instance.MyHeroInfo.EquippedMountId).Level;
				break;

			case EnImproveContent.MainGearEnchant:
				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
				{
					flContentValue += CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].MainGearEnchantLevel.EnchantLevel;
				}

				break;

			case EnImproveContent.SubGearLevelUp:
				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSubGearList.Count; i++)
				{
					flContentValue += CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].Level;
				}
				
				break;

			case EnImproveContent.Class:
				flContentValue = CsGameData.Instance.MyHeroInfo.RankNo;
				break;

			case EnImproveContent.CardCollection:
				flContentValue = CsCreatureCardManager.Instance.CreatureCardCollectionFamePoint;
				break;
		}

		return flContentValue;
	}

	//---------------------------------------------------------------------------------------------------
	void CheckEmptyText()
	{
		if (m_trContent.childCount == 0)
			m_goListEmpty.SetActive(true);
		else
			m_goListEmpty.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveContents()
	{
		foreach (Transform trChild in m_trContent)
		{
			Destroy(trChild.gameObject);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CreateContents(int nMainCategoryId, bool bIsSubCategory, int nSubCategoryId)
	{
		if (bIsSubCategory)
		{
			CsImprovementSubCategory csImrpovementSubCategory = CsGameData.Instance.GetImprovementMainCategory(nMainCategoryId).GetImprovementSubCategory(nSubCategoryId);
			if (csImrpovementSubCategory != null)
			{
				OpenSubContentMenu(csImrpovementSubCategory);
			}
		}
		else
		{
			CsImprovementMainCategory csImprovementMainCategory = CsGameData.Instance.GetImprovementMainCategory(nMainCategoryId);
			if (csImprovementMainCategory != null)
			{
				OpenMainContentMenu(csImprovementMainCategory);
			}
		}
		yield return null;

		CheckEmptyText();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadDungeonSubMenu(EnImproveContent enImproveContent)
	{
		yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory") != null);

		Transform trDungeonSubMenu = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory");
		CsDungeonCartegory csDungeonCartegory = trDungeonSubMenu.GetComponent<CsDungeonCartegory>();
		Debug.Log(enImproveContent);
		switch (enImproveContent)
		{
			case EnImproveContent.UndergroundMaze:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.UndergroundMaze);
				break;

			case EnImproveContent.AncientRelic:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.ArtifactRoom);
				break;

			case EnImproveContent.ProofOfValor:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.ProofOfValor);
				break;

			case EnImproveContent.SoulCoveter:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.SoulCoveter);
				break;

			case EnImproveContent.DragonNest:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.DragonNest);
				break;

			case EnImproveContent.MemoryOfWar:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.WarMemory);
				break;
				
			case EnImproveContent.TraitorsHideout:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.AncientRelic);
				break;
		}

		PopupClose();
	}

	IEnumerator LoadInventoryCompose()
	{
		yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory") != null);

		Transform trInventory = m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory");
		CsInventory csInventory = trInventory.GetComponent<CsInventory>();

		csInventory.OpenComposeToImprovement();

		PopupClose();
	}
	
	//---------------------------------------------------------------------------------------------------
	IEnumerator ContentPopupOpen(CsImprovementContent csImprovementContent, EnMainMenu enMainMenu, EnSubMenu enSubMenu)
	{
		EnImproveContent enImproveContent = (EnImproveContent)csImprovementContent.ContentId;

		switch (enImproveContent)
		{
			case EnImproveContent.DiaCharging:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventSinglePopupOpen(EnMenuId.ChargingEvent);
				PopupClose();
				break;

			case EnImproveContent.TodayTask:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

			case EnImproveContent.SeriesMission:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

			case EnImproveContent.UndergroundMaze:
			case EnImproveContent.SoulCoveter:
			case EnImproveContent.ProofOfValor:
			case EnImproveContent.AncientRelic:
			case EnImproveContent.DragonNest:
			case EnImproveContent.TraitorsHideout:
			case EnImproveContent.MemoryOfWar:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				StartCoroutine(LoadDungeonSubMenu(enImproveContent));
				break;

			case EnImproveContent.FieldBoss:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventOpenPopupFieldBoss();
				PopupClose();
				break;

			case EnImproveContent.WorldDrop:
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A150_TXT_03001"));
				break;

			case EnImproveContent.ItemCompose:
				StartCoroutine(LoadInventoryCompose());
				break;

			case EnImproveContent.AttendReward:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

			case EnImproveContent.AccessReward:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

			case EnImproveContent.FarmThreat:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.ThreatOfFarm);
				PopupClose();
				break;

			case EnImproveContent.CreatureFarm:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.CreatureFarm);
				PopupClose();
				break;

			case EnImproveContent.NationQuest:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

			case EnImproveContent.PK:
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A150_TXT_03002"));
				break;

			case EnImproveContent.GuildMission:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
				PopupClose();
				break;

			case EnImproveContent.GuildFarm:
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

				// 길드 영지 이동
				CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
											  CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
											  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
				PopupClose();
				break;

			case EnImproveContent.GuildOblation:
				if (CsGuildManager.Instance.Guild != null)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
					PopupClose();
				}
				else
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A150_TXT_03003"));
				}
				
				break;

			default:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);
				PopupClose();
				break;

		}

		yield return null;
	}

	//---------------------------------------------------------------------------------------------------
	void SetImageIconAndMenuId(CsImprovementContent csImprovementContent, out EnMainMenu enMainMenu, out EnSubMenu enSubMenu)
	{
		EnImproveContent enImproveContent = (EnImproveContent)csImprovementContent.ContentId;

		switch (enImproveContent)
		{
			case EnImproveContent.CreatureTraining:
				enMainMenu = EnMainMenu.Creature;
				enSubMenu = EnSubMenu.CreatureTraining;
				m_strImagePath = "GUI/Common/ico_menu_all_16";
				break;

			case EnImproveContent.CreatureComposition:
				enMainMenu = EnMainMenu.Creature;
				enSubMenu = EnSubMenu.CreatureComposition;
				m_strImagePath = "GUI/Common/ico_menu_all_16";
				break;

			case EnImproveContent.CreatureInjection:
				enMainMenu = EnMainMenu.Creature;
				enSubMenu = EnSubMenu.CreatureInjection;
				m_strImagePath = "GUI/Common/ico_menu_all_16";
				break;

			case EnImproveContent.WingEquipment:
				enMainMenu = EnMainMenu.Wing;
				enSubMenu = EnSubMenu.WingEquipment;
				m_strImagePath = "GUI/Common/ico_menu_all_13";
				break;

			case EnImproveContent.WingEnchant:
				enMainMenu = EnMainMenu.Wing;
				enSubMenu = EnSubMenu.WingEnchant;
				m_strImagePath = "GUI/Common/ico_menu_all_13";
				break;

			case EnImproveContent.MainGearRefine:
				enMainMenu = EnMainMenu.MainGear;
				enSubMenu = EnSubMenu.MainGearRefine;
				m_strImagePath = "GUI/Common/ico_menu_all_11";
				break;

			case EnImproveContent.MainGearTransit:
				enMainMenu = EnMainMenu.SubGear;
				enSubMenu = EnSubMenu.SubGearSoulstone;
				m_strImagePath = "GUI/Common/ico_menu_all_12";
				break;

			case EnImproveContent.Constellation:
				enMainMenu = EnMainMenu.Constellation;
				enSubMenu = EnSubMenu.Constellation;
				m_strImagePath = "GUI/Common/ico_menu_all_18";
				break;

			case EnImproveContent.MountLevelUp:
				enMainMenu = EnMainMenu.Mount;
				enSubMenu = EnSubMenu.MountLevelUp;
				m_strImagePath = "GUI/Common/ico_menu_all_15";
				break;

			case EnImproveContent.MainGearEnchant:
				enMainMenu = EnMainMenu.MainGear;
				enSubMenu = EnSubMenu.MainGearEnchant;
				m_strImagePath = "GUI/Common/ico_menu_all_11";
				break;

			case EnImproveContent.SubGearLevelUp:
				enMainMenu = EnMainMenu.SubGear;
				enSubMenu = EnSubMenu.SubGearLevelUp;
				m_strImagePath = "GUI/Common/ico_menu_all_12";
				break;

			case EnImproveContent.Class:
				enMainMenu = EnMainMenu.Class;
				enSubMenu = EnSubMenu.Class;
				m_strImagePath = "GUI/Common/ico_menu_all_05";
				break;

			case EnImproveContent.CardCollection:
				enMainMenu = EnMainMenu.Collection;
				enSubMenu = EnSubMenu.CardCollection;
				m_strImagePath = "GUI/Common/ico_menu_all_33";
				break;

			case EnImproveContent.DiaCharging:
				enMainMenu = EnMainMenu.DiaShop;
				enSubMenu = EnSubMenu.DiaShop;
				m_strImagePath = "GUI/Common/ico_menu_all_10";
				break;

			case EnImproveContent.TodayTask:
				enMainMenu = EnMainMenu.TodayTask;
				enSubMenu = EnSubMenu.TodayTaskExp;
				m_strImagePath = "GUI/Common/ico_menu_all_24";
				break;

			case EnImproveContent.AchevementAccomplishment:
				enMainMenu = EnMainMenu.Achievement;
				enSubMenu = EnSubMenu.Accomplishment;
				m_strImagePath = "GUI/Common/ico_menu_all_04";
				break;

			case EnImproveContent.SeriesMission:
				enMainMenu = EnMainMenu.Support;
				enSubMenu = EnSubMenu.SeriesMission;
				m_strImagePath = "GUI/Common/ico_menu_all_24";
				break;

			case EnImproveContent.UndergroundMaze:
				// 지하미로
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.TimeLimitDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.FieldBoss:
				enMainMenu = EnMainMenu.Creature;
				enSubMenu = EnSubMenu.CreatureTraining;
				m_strImagePath = "GUI/Common/todaytask_15";
				break;

			case EnImproveContent.SoulCoveter:
				// 영혼을 탐하는자
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.PartyDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.ProofOfValor:
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.IndividualDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.LuckyShop:
				enMainMenu = EnMainMenu.LuckyShop;
				enSubMenu = EnSubMenu.LuckyShop;
				m_strImagePath = "GUI/Common/ico_menu_all_31";
				break;

			case EnImproveContent.CardShop:
				enMainMenu = EnMainMenu.Collection;
				enSubMenu = EnSubMenu.CardShop;
				m_strImagePath = "GUI/Common/ico_menu_all_10";
				break;

			case EnImproveContent.WeeklyQuest:
				enMainMenu = EnMainMenu.WeeklyQuest;
				enSubMenu = EnSubMenu.WeeklyQuest;
				m_strImagePath = "GUI/Common/ico_menu_all_24";
				break;

			case EnImproveContent.DiaShop:
				enMainMenu = EnMainMenu.DiaShop;
				enSubMenu = EnSubMenu.DiaShop;
				m_strImagePath = "GUI/Common/ico_menu_all_10";
				break;

			case EnImproveContent.WorldDrop:
				// 일반 토스트만 띄울 예정
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.ItemCompose:
				// 아이템 합성창으로 바로 이동
				enMainMenu = EnMainMenu.Character;
				enSubMenu = EnSubMenu.Inventory;
				m_strImagePath = "GUI/Common/ico_menu_all_28";
				break;

			case EnImproveContent.AttendReward:
				enMainMenu = EnMainMenu.Support;
				enSubMenu = EnSubMenu.AttendReward;
				m_strImagePath = "GUI/Common/ico_menu_all_21";
				break;

			case EnImproveContent.AccessReward:
				enMainMenu = EnMainMenu.Support;
				enSubMenu = EnSubMenu.AccessReward;
				m_strImagePath = "GUI/Common/ico_menu_all_21";
				break;

			case EnImproveContent.NationQuest:
				enMainMenu = EnMainMenu.TodayTask;
				enSubMenu = EnSubMenu.TodayTaskExp;
				m_strImagePath = "GUI/Common/ico_menu_all_06";
				break;

			case EnImproveContent.NationDiplomacy:
				enMainMenu = EnMainMenu.Nation;
				enSubMenu = EnSubMenu.NationDiplomacy;
				m_strImagePath = "GUI/Common/ico_menu_all_06";
				break;

			case EnImproveContent.AncientRelic:
				// 고대 유물의 방
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.IndividualDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.FarmThreat:
				// 농장의 위협 (시작 npc 이동)
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/retrieval_10";
				break;

			case EnImproveContent.DragonNest:
				// 용의 둥지
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.PartyDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.CreatureFarm:
				// 크리처 농장 (시작 npc 이동)
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/retrieval_10";
				break;

			case EnImproveContent.TraitorsHideout:
				// 반역자의 은신처
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.PartyDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.MemoryOfWar:
				// 전쟁의 기억
				enMainMenu = EnMainMenu.Dungeon;
				enSubMenu = EnSubMenu.TimeLimitDungeon;
				m_strImagePath = "GUI/Common/ico_menu_all_25";
				break;

			case EnImproveContent.PK:
				// PK 토스트 메시지
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/ico_menu_all_06";
				break;

			case EnImproveContent.NationOblation:
				// 차원 헌납
				enMainMenu = EnMainMenu.Nation;
				enSubMenu = EnSubMenu.NationInfo;
				m_strImagePath = "GUI/Common/ico_menu_all_06";
				break;

			case EnImproveContent.GuildMission:
				// 길드 미션 (시작 npc 경로 표시)
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/ico_menu_all_07";
				break;

			case EnImproveContent.GuildFarm:
				// 길드 농장 (길드 영지 이동 이벤트)
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "GUI/Common/ico_menu_all_07";
				break;

			case EnImproveContent.GuildOblation:
				// 길드 헌납
				enMainMenu = EnMainMenu.Guild;
				enSubMenu = EnSubMenu.GuildMember;
				m_strImagePath = "GUI/Common/ico_menu_all_07";
				break;

			default:
				enMainMenu = 0;
				enSubMenu = 0;
				m_strImagePath = "";
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(this.gameObject);
	}

	#region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnClickExit()
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickCategory(bool bIson, int nMainCategoryId, bool bIsSubCategory, int nSubCategoryId = 0)
	{
		if (bIson)
		{
			StartCoroutine(CreateContents(nMainCategoryId, bIsSubCategory, nSubCategoryId));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickParentCategory(CsParentCategory csParentCategory, Image imageArrow)
	{
		int[] anChildCategoryId = csParentCategory.ChildCategoryId;

		if (!csParentCategory.IsSelect)
		{
			for (int i = 0; i < anChildCategoryId.Length; i++)
			{
				GameObject goSubCategory = m_trMainCategory.Find("Sub" + anChildCategoryId[i].ToString()).gameObject;
				goSubCategory.SetActive(true);
			}
		}
		else
		{
			for (int i = 0; i < anChildCategoryId.Length; i++)
			{
				m_trMainCategory.Find("Sub" + anChildCategoryId[i].ToString()).gameObject.SetActive(false);
			}
		}
		StartCoroutine(ChangeImageArrow(csParentCategory, imageArrow));
	}

	IEnumerator ChangeImageArrow(CsParentCategory csParentCategory, Image imageArrow)
	{
		ResourceRequest resourceRequset;

		if (!csParentCategory.IsSelect)
		{
			resourceRequset = CsUIData.Instance.LoadAssetAsync<Sprite>("GUI/PopupImprovement/ico_arrow01_up");
		}
		else
		{
			resourceRequset = CsUIData.Instance.LoadAssetAsync<Sprite>("GUI/PopupImprovement/ico_arrow01_down");
		}
		yield return resourceRequset;

		imageArrow.sprite = (Sprite)resourceRequset.asset;
		csParentCategory.IsSelect = !csParentCategory.IsSelect;
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickMoveContent(CsImprovementContent csImprovementContent, EnMainMenu enMainMenu, EnSubMenu enSubMenu)
	{
		StartCoroutine(ContentPopupOpen(csImprovementContent, enMainMenu, enSubMenu));
	}

	#endregion EventHandler
}
