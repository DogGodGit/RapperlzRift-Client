using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsQuestInfo : CsPopupSub
{
    Transform m_trBack;

    Text m_textQuestTargetValue;
    Text m_textContentsValue;

    EnQuestType m_EnQuestType;
	int m_nParam = 0;	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 사용
	bool m_bAcceptableSubQuest = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventSelectQuest += OnEventSelectQuest;

        // 일상
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestUpdated += OnEventBountyHunterQuestUpdated;

        // 일일
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated += OnEventHeroDailyQuestProgressCountUpdated;

        // 주간
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated += OnEventWeeklyQuestRoundProgressCountUpdated;

        //길드
        CsGuildManager.Instance.EventUpdateMissionState += OnEventUpdateMissionState;
        CsGuildManager.Instance.EventUpdateGuildHuntingQuestState += OnEventUpdateGuildHuntingQuestState;

		CsGameEventUIToUI.Instance.EventAcceptableSubQuestEmpty += OnEventAcceptableSubQuestEmpty;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventSelectQuest -= OnEventSelectQuest;

        // 일상
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestUpdated -= OnEventBountyHunterQuestUpdated;

        // 일일
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated -= OnEventHeroDailyQuestProgressCountUpdated;

        // 주간
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated -= OnEventWeeklyQuestRoundProgressCountUpdated;

        //길드
        CsGuildManager.Instance.EventUpdateMissionState -= OnEventUpdateMissionState;
        CsGuildManager.Instance.EventUpdateGuildHuntingQuestState -= OnEventUpdateGuildHuntingQuestState;

		CsGameEventUIToUI.Instance.EventAcceptableSubQuestEmpty -= OnEventAcceptableSubQuestEmpty;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectQuest(EnQuestType enQuestType, int nParam, bool bAcceptableSubQuest)
    {
		m_bAcceptableSubQuest = bAcceptableSubQuest;
        m_EnQuestType = enQuestType;
		m_nParam = nParam;
		UpdateQuestInfo(bAcceptableSubQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestUpdated()
    {
        UpdateQuestInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroDailyQuestProgressCountUpdated()
    {
        UpdateQuestInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundProgressCountUpdated()
    {
        UpdateQuestInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateMissionState()
    {
        UpdateQuestInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateGuildHuntingQuestState()
    {
        UpdateQuestInfo();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventAcceptableSubQuestEmpty()
	{
		m_trBack.gameObject.SetActive(false);
		transform.Find("TextNo").gameObject.SetActive(true);

		Text textNo = transform.Find("TextNo").GetComponent<Text>();
		CsUIData.Instance.SetFont(textNo);
		textNo.text = CsConfiguration.Instance.GetString("A38_TXT_00004");
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickAbandonQuest()
    {
        int nHeroDailyQuestIndex = (int)m_EnQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (m_EnQuestType)
        {
            case EnQuestType.ThreatOfFarm:
				CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
                CsThreatOfFarmQuestManager.Instance.AbandonMission();
                break;

            case EnQuestType.BountyHunter:
				CsBountyHunterQuestManager.Instance.StopAutoPlay(this);
                CsBountyHunterQuestManager.Instance.SendBountyHunterQuestAbandon();
                break;

            case EnQuestType.GuildMission:
				CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Mission);
                CsGuildManager.Instance.SendGuildMissionAbandon();
                break;

            case EnQuestType.GuildHunting:
				CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Hunting);
                CsGuildManager.Instance.SendGuildHuntingQuestAbandon();
                break;

            case EnQuestType.DailyQuest01: 
            case EnQuestType.DailyQuest02:
            case EnQuestType.DailyQuest03:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
				CsDailyQuestManager.Instance.StopAutoPlay(this, csHeroDailyQuest.Id);
                CsDailyQuestManager.Instance.SendDailyQuestAbandon(csHeroDailyQuest.Id);
				break;

			case EnQuestType.SubQuest:
				CsSubQuestManager.Instance.StopAutoPlay(this);
				CsSubQuestManager.Instance.SendSubQuestAbandon(m_nParam);
				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuestAutoStart()
    {
		// 수락 가능한 서브 퀘스트의 경우 NPC 길찾기만 수행
		if (m_bAcceptableSubQuest)
		{
			CsSubQuest csSubQuest = CsGameData.Instance.GetSubQuest(m_nParam);

			if (csSubQuest != null)
			{
				CsGameEventToIngame.Instance.OnEventNpcAutoMove(CsGameData.Instance.MyHeroInfo.Nation.NationId, csSubQuest.StartNpc);
				CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.FindingPath);
				CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
			}

			return;
		}

		ResetQuestPanelDisplay(m_EnQuestType, m_nParam);

        int nHeroDailyQuestIndex = (int)m_EnQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (m_EnQuestType)
        {
            case EnQuestType.MainQuest:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                break;

            case EnQuestType.ThreatOfFarm:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.ThreatOfFarm);
                break;

            case EnQuestType.BountyHunter:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Hunter);
                break;

            case EnQuestType.DailyQuest01:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest == null)
                {
                    return;
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(csHeroDailyQuest.Id);
                }
                break;

            case EnQuestType.DailyQuest02:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest == null)
                {
                    return;
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(csHeroDailyQuest.Id);
                }
                break;

            case EnQuestType.DailyQuest03:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest == null)
                {
                    return;
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(csHeroDailyQuest.Id);
                }
                break;

            case EnQuestType.WeeklyQuest:
                break;

            case EnQuestType.SecretLetter:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SecretLetter);
                break;

            case EnQuestType.MysteryBox:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MysteryBox);
                break;

            case EnQuestType.DimensionRaid:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.DimensionRaid);
                break;

            case EnQuestType.HolyWar:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.HolyWar);
                break;

            case EnQuestType.SupplySupport:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SupplySupport);
                break;

            case EnQuestType.GuildMission:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
                break;

            case EnQuestType.GuildSupplySupport:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildSupplySupport);
                break;

            case EnQuestType.GuildHunting:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildHunting);
                break;

			case EnQuestType.TrueHero:
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.TrueHero);
				break;

			case EnQuestType.SubQuest:
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SubQuest, m_nParam);
				break;

			case EnQuestType.Biography:
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Biography, m_nParam);
				break;

			case EnQuestType.CreatureFarm:
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.CreatureFarm);
				break;

            case EnQuestType.JobChange:
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.JobChange);
                break;
        }

        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trBack = transform.Find("InfoBack");

        Text textQuestTarget = m_trBack.Find("TextQuestTarget").GetComponent<Text>();
        CsUIData.Instance.SetFont(textQuestTarget);
        textQuestTarget.text = CsConfiguration.Instance.GetString("A38_TXT_00001");

        m_textQuestTargetValue = m_trBack.Find("TextQuestTargetValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textQuestTargetValue);

        Text textContents = m_trBack.Find("TextContents").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContents);
        textContents.text = CsConfiguration.Instance.GetString("A38_TXT_00002");

        m_textContentsValue = m_trBack.Find("TextContentsValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textContentsValue);

        Text textReward = m_trBack.Find("TextReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReward);
        textReward.text = CsConfiguration.Instance.GetString("A38_TXT_00003");

        Button buttonAbandon = m_trBack.Find("ButtonAbandon").GetComponent<Button>();
        buttonAbandon.onClick.RemoveAllListeners();
        buttonAbandon.onClick.AddListener(OnClickAbandonQuest);
        buttonAbandon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAbandon = buttonAbandon.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAbandon);

        Button buttonAutoStart = m_trBack.Find("ButtonAutoStart").GetComponent<Button>();
        buttonAutoStart.onClick.RemoveAllListeners();
        buttonAutoStart.onClick.AddListener(OnClickQuestAutoStart);
        buttonAutoStart.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAutoStart = buttonAutoStart.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAutoStart);

        m_EnQuestType = EnQuestType.MainQuest;
		
        UpdateQuestInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateQuestInfo(bool bAcceptableSubQuest = false)
    {
		transform.Find("TextNo").gameObject.SetActive(false);

        Transform trRewardList = m_trBack.Find("RewardList");

        // 경험치
        Transform trRewardExp = trRewardList.Find("RewardExp");
        Text textExp = trRewardExp.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExp);

        // 골드
        Transform trRewardGold = trRewardList.Find("RewardGold");
        Text textGold = trRewardGold.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGold);

        // 국가 공적
        Transform trRewardExploit = trRewardList.Find("RewardExploit");
        Text textExploit = trRewardExploit.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExploit);

        // 길드 공헌
        Transform trRewardGuildContribution = trRewardList.Find("RewardGuildContribution");
        Text textGuildContribution = trRewardGuildContribution.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuildContribution);

        // 길드 자금
        Transform trRewardGuildFund = trRewardList.Find("RewardGuildFund");
        Text textGuildFund = trRewardGuildFund.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuildFund);

        // 길드 건설
        Transform trRewardGuildBuilding = trRewardList.Find("RewardGuildBuilding");
        Text textGuildBuilding = trRewardGuildBuilding.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuildBuilding);

        trRewardGold.gameObject.SetActive(false);
        trRewardExploit.gameObject.SetActive(false);
        trRewardGuildContribution.gameObject.SetActive(false);
        trRewardGuildFund.gameObject.SetActive(false);
        trRewardGuildBuilding.gameObject.SetActive(false);
        trRewardExp.gameObject.SetActive(true);

        Transform trAbandon = m_trBack.Find("ButtonAbandon");
        Transform trAutoStart = m_trBack.Find("ButtonAutoStart");

        Text textAbandon = trAbandon.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAbandon);
        textAbandon.text = CsConfiguration.Instance.GetString("A38_BTN_00010");

        Text textAutoStart = trAutoStart.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAutoStart);
        textAutoStart.text = CsConfiguration.Instance.GetString("A38_BTN_00011");

        Transform trItemReward0 = trRewardList.Find("Reward0");
        Transform trItemReward1 = trRewardList.Find("Reward1");

        trItemReward0.gameObject.SetActive(false);
        trItemReward1.gameObject.SetActive(false);

        CsItemReward csItemReward;

        int nHeroDailyQuestIndex = (int)m_EnQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (m_EnQuestType)
        {
            case EnQuestType.MainQuest:
                if (CsMainQuestManager.Instance.MainQuest == null ||
					CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Completed)
                {
                    m_trBack.gameObject.SetActive(false);

                    Text textNo = transform.Find("TextNo").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textNo);
                    textNo.text = CsConfiguration.Instance.GetString("");
                }
                else
                {
                    m_trBack.gameObject.SetActive(true);
                    m_textQuestTargetValue.text = CsMainQuestManager.Instance.ObjectiveMessage;
                    m_textContentsValue.text = CsMainQuestManager.Instance.MainQuest.StartText;

                    //보상세팅
                    textExp.text = CsMainQuestManager.Instance.MainQuest.RewardExp.ToString("#,##0");

                    textGold.text = CsMainQuestManager.Instance.MainQuest.RewardGold.ToString("#,##0");
                    trRewardGold.gameObject.SetActive(true);

                    int nCount = 0;
                    int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

                    for (int i = 0; i < CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList.Count; i++)
                    {
                        if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].JobId != 0 &&
                            CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].JobId != nJobId)
                        {
                            continue;
                        }
                        else
                        {
                            Transform trReward = trRewardList.Find("Reward" + nCount);

                            Image imageIcon = trReward.Find("Image").GetComponent<Image>();
                            Text textName = trReward.Find("Text").GetComponent<Text>();

                            switch ((EnMainQuestRewardType)CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type)
                            {
                                case EnMainQuestRewardType.MainGear:
                                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.Image);
                                    textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.Name;
                                    break;
                                case EnMainQuestRewardType.SubGear:
                                    string strImage = string.Format("sub_{0}_{1}", CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].SubGear.SubGearId, (int)EnSubGearGrade.Normal);
                                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + strImage);
                                    textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].SubGear.Name;
                                    break;
                                case EnMainQuestRewardType.Item:
                                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Item.Image);
                                    textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Item.Name;
                                    break;
                                case EnMainQuestRewardType.CreatureCard:
                                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_card_" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].CreatureCard.CreatureCardGrade.Grade);
                                    textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].CreatureCard.Name;
                                    break;
                            }

                            trReward.gameObject.SetActive(true);
                            nCount++;
                        }
                    }
                    //버튼세팅
                    trAbandon.gameObject.SetActive(false);
                    //Text textAbandon = trAbandon.Find("Text").GetComponent<Text>();
                    
                    trAutoStart.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.ThreatOfFarm:
                m_trBack.gameObject.SetActive(true);

                if (CsThreatOfFarmQuestManager.Instance.QuestState != EnQuestState.Complete)
                {
                    if (CsThreatOfFarmQuestManager.Instance.Monster == null)
                    {
                        m_textQuestTargetValue.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.TargetMovingText, CsThreatOfFarmQuestManager.Instance.Mission.TargetPositionName);
                        m_textContentsValue.text = CsThreatOfFarmQuestManager.Instance.Quest.TargetMovingDescription;

                        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
                        {
                            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                            m_IEnumeratorThreatOfFameMonsterInfo = null;
                        }
                    }
                    else
                    {
                        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
                        {
                            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                            m_IEnumeratorThreatOfFameMonsterInfo = null;
                        }

                        StartCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                        m_textContentsValue.text = CsThreatOfFarmQuestManager.Instance.Quest.TargetKillDescription;
                    }
                }
                else
                {
                    // Complete
                    m_textQuestTargetValue.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.CompletionText, CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc.Name);
                    m_textContentsValue.text = CsThreatOfFarmQuestManager.Instance.Quest.TargetMovingDescription;
                }

                textExp.text = CsThreatOfFarmQuestManager.Instance.Quest.GetThreatOfFarmQuestReward(CsGameData.Instance.MyHeroInfo.Level).MissionCompletionExpReward.Value.ToString("#,##0");

                csItemReward = CsThreatOfFarmQuestManager.Instance.Quest.GetThreatOfFarmQuestReward(CsGameData.Instance.MyHeroInfo.Level).QuestCompletionItemReward;

                trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);
                trItemReward0.Find("Text").GetComponent<Text>().text = csItemReward.Item.Name;
                trItemReward0.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(true);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.BountyHunter:
                m_trBack.gameObject.SetActive(true);

                m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(CsBountyHunterQuestManager.Instance.BountyHunterQuest.TargetContent), CsBountyHunterQuestManager.Instance.ProgressCount, CsBountyHunterQuestManager.Instance.BountyHunterQuest.TargetCount);
                m_textContentsValue.text = CsBountyHunterQuestManager.Instance.BountyHunterQuest.Description;

                textExp.text = CsGameData.Instance.GetBountyHunterQuestReward(CsBountyHunterQuestManager.Instance.ItemGrade, CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");

                trAbandon.gameObject.SetActive(true);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.DailyQuest01:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest.IsAccepted)
                {
                    m_trBack.gameObject.SetActive(true);

                    if (csHeroDailyQuest.DailyQuestMission.Type == 1)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }
                    else if (csHeroDailyQuest.DailyQuestMission.Type == 2)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }
                    
                    m_textContentsValue.text = csHeroDailyQuest.DailyQuestMission.TargetTitle;

                    trAbandon.gameObject.SetActive(true);
                    trAutoStart.gameObject.SetActive(true);
                }
                else
                {
                    return;
                }

                break;

            case EnQuestType.DailyQuest02:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest.IsAccepted)
                {
                    m_trBack.gameObject.SetActive(true);

                    if (csHeroDailyQuest.DailyQuestMission.Type == 1)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }
                    else if (csHeroDailyQuest.DailyQuestMission.Type == 2)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }

                    m_textContentsValue.text = csHeroDailyQuest.DailyQuestMission.TargetTitle;

                    trAbandon.gameObject.SetActive(true);
                    trAutoStart.gameObject.SetActive(true);
                }
                else
                {
                    return;
                }

                break;

            case EnQuestType.DailyQuest03:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];

                if (csHeroDailyQuest.IsAccepted)
                {
                    m_trBack.gameObject.SetActive(true);

                    if (csHeroDailyQuest.DailyQuestMission.Type == 1)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }
                    else if (csHeroDailyQuest.DailyQuestMission.Type == 2)
                    {
                        m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                    }

                    m_textContentsValue.text = csHeroDailyQuest.DailyQuestMission.TargetTitle;

                    trAbandon.gameObject.SetActive(true);
                    trAutoStart.gameObject.SetActive(true);
                }
                else
                {
                    return;
                }
                break;

            case EnQuestType.WeeklyQuest:
                m_trBack.gameObject.SetActive(true);

                if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
                {
                    switch ((EnWeeklyQuestType)CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.Type)
                    {
                        case EnWeeklyQuestType.Move:
                            m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent));
                            break;

                        case EnWeeklyQuestType.Monster:
                            m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetMonster.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetCount);
                            break;

                        case EnWeeklyQuestType.Collect:
                            m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContinentObject.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetCount);
                            break;
                    }

                    m_textContentsValue.text = CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetDescription;

                    trAbandon.gameObject.SetActive(false);
                    trAutoStart.gameObject.SetActive(true);
                }
                else
                {
                    return;
                }

                break;

            case EnQuestType.SecretLetter:
                m_trBack.gameObject.SetActive(true);

                CsNation csNation = CsGameData.Instance.GetNation(CsSecretLetterQuestManager.Instance.TargetNationId);

                if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Accepted)
                {
                    m_textQuestTargetValue.text = string.Format(CsSecretLetterQuestManager.Instance.SecretLetterQuest.TargetContent, csNation.Name);
                }
                else
                {
                    m_textQuestTargetValue.text = string.Format(CsSecretLetterQuestManager.Instance.SecretLetterQuest.CompletionText, CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo.Name);
                }

                m_textContentsValue.text = string.Format(CsSecretLetterQuestManager.Instance.SecretLetterQuest.Description, csNation.Name);

                //보상세팅
                if (CsSecretLetterQuestManager.Instance.PickedLetterGrade == 0)
                {
                    CsSecretLetterQuestReward csSecretLetterQuestReward = CsGameData.Instance.SecretLetterQuest.SecretLetterQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.SecretLetterGrade.Grade == CsGameData.Instance.SecretLetterGradeList[0].Grade);
                    textExp.text = csSecretLetterQuestReward.ExpReward.Value.ToString("#,##0");
                    textExploit.text = CsGameData.Instance.SecretLetterGradeList[0].ExploitPointReward.Value.ToString("#,##0");
                }
                else
                {
                    CsSecretLetterQuestReward csSecretLetterQuestReward = CsGameData.Instance.SecretLetterQuest.SecretLetterQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.SecretLetterGrade.Grade == CsSecretLetterQuestManager.Instance.PickedLetterGrade);

                    textExp.text = csSecretLetterQuestReward.ExpReward.Value.ToString("#,##0");
                    textExploit.text = CsGameData.Instance.GetSecretLetterGrade(CsSecretLetterQuestManager.Instance.PickedLetterGrade).ExploitPointReward.Value.ToString("#,##0");
                }

                trRewardExploit.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(false);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.MysteryBox:
                m_trBack.gameObject.SetActive(true);

                if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Accepted)
                {
                    m_textQuestTargetValue.text = CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.TargetContent;
                }
                else
                {
                    m_textQuestTargetValue.text = string.Format(CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.CompletionText, CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo.Name);
                }

                m_textContentsValue.text = CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.Description;

                if (CsMysteryBoxQuestManager.Instance.PickedBoxGrade == 0)
                {
                    CsMysteryBoxQuestReward csMysteryBoxQuestReward = CsGameData.Instance.MysteryBoxQuest.MysteryBoxQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.MysteryBoxGrade.Grade == CsGameData.Instance.MysteryBoxGradeList[0].Grade);

                    textExp.text = csMysteryBoxQuestReward.ExpReward.Value.ToString("#,##0");
                    textExploit.text = CsGameData.Instance.MysteryBoxGradeList[0].ExploitPointReward.Value.ToString("#,##0");
                }
                else
                {
                    CsMysteryBoxQuestReward csMysteryBoxQuestReward = CsGameData.Instance.MysteryBoxQuest.MysteryBoxQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.MysteryBoxGrade.Grade == CsMysteryBoxQuestManager.Instance.PickedBoxGrade);

                    textExp.text = csMysteryBoxQuestReward.ExpReward.Value.ToString("#,##0");
                    textExploit.text = CsGameData.Instance.GetMysteryBoxGrade(CsMysteryBoxQuestManager.Instance.PickedBoxGrade).ExploitPointReward.Value.ToString("#,##0");
                }

                trRewardExploit.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(false);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.DimensionRaid:
                m_trBack.gameObject.SetActive(true);

                if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
                {
                    m_textQuestTargetValue.text = string.Format(CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.CompletionText, CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo.Name);
                }
                else
                {
                    CsDimensionRaidQuestStep csDimensionRaidQuestStep = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(CsDimensionRaidQuestManager.Instance.Step);
                    m_textQuestTargetValue.text = string.Format(csDimensionRaidQuestStep.TargetContent, csDimensionRaidQuestStep.TargetNpcInfo.Name);
                }

                m_textContentsValue.text = CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.Content;

                textExp.text = CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");

                textExploit.text = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExploitPointReward.Value.ToString("#,##0");
                trRewardExploit.gameObject.SetActive(true);

                csItemReward = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level).ItemRewardId;
                trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);
                trItemReward0.Find("Text").GetComponent<Text>().text = csItemReward.Item.Name;
                trItemReward0.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(false);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.HolyWar:
                m_trBack.gameObject.SetActive(true);

                CsHolyWarQuestGloryLevel csHolyWarQuestGloryLevel = null;

				for (int i = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList.Count - 1; i >= 0; i--)
				{
					if (CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList[i].RequiredKillCount <= CsHolyWarQuestManager.Instance.KillCount)
					{
						csHolyWarQuestGloryLevel = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList[i];
						break;
					}
				}

                if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
                {
                    m_textQuestTargetValue.text = string.Format(CsHolyWarQuestManager.Instance.HolyWarQuest.CompletionText, CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo.Name);
                }
                else
                {
					m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString(CsHolyWarQuestManager.Instance.HolyWarQuest.TargetContent), CsHolyWarQuestManager.Instance.KillCount, csHolyWarQuestGloryLevel == null ? 0 : csHolyWarQuestGloryLevel.GloryLevel);
                }

                m_textContentsValue.text = CsHolyWarQuestManager.Instance.HolyWarQuest.Description;

                long lExpReward = 0;
                CsHolyWarQuestReward csHolyWarQuestReward = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level);

                if (csHolyWarQuestReward == null)
                {
                    lExpReward = 0;
                }
                else
                {
                    lExpReward = csHolyWarQuestReward.ExpReward.Value;
                }

                textExp.text = lExpReward.ToString("#,##0");

				if (csHolyWarQuestGloryLevel == null)
				{
					textExploit.text = "0";
				}
				else
				{
					textExploit.text = csHolyWarQuestGloryLevel.ExploitPointReward.Value.ToString("#,##0");
				}

                trRewardExploit.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(false);
                trAutoStart.gameObject.SetActive(true);
                break;

            case EnQuestType.SupplySupport:
                m_trBack.gameObject.SetActive(true);

                m_textQuestTargetValue.text = CsConfiguration.Instance.GetString(CsSupplySupportQuestManager.Instance.SupplySupportQuest.TargetContent);
                m_textContentsValue.text = CsConfiguration.Instance.GetString(CsSupplySupportQuestManager.Instance.SupplySupportQuest.StartDialogue);

                CsSupplySupportQuestReward csSupplySupportQuestReward = CsSupplySupportQuestManager.Instance.SupplySupportQuestCart.SupplySupportQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level);

                trAbandon.gameObject.SetActive(false);
                trAutoStart.gameObject.SetActive(true);

                if (csSupplySupportQuestReward == null)
                {
                    break;
                }
                else
                {
                    textExp.text = csSupplySupportQuestReward.ExpReward.Value.ToString("#,##0");

                    textGold.text = csSupplySupportQuestReward.GoldReward.Value.ToString("#,##0");
                    textExploit.text = csSupplySupportQuestReward.ExploitPointReward.Value.ToString("#,##0");

                    trRewardGold.gameObject.SetActive(true);
                    trRewardExploit.gameObject.SetActive(true);
                }
                break;

            case EnQuestType.GuildMission:
				m_trBack.gameObject.SetActive(true);
                CsGuildMission csGuildMission = CsGuildManager.Instance.GuildMission;
                if (CsGuildManager.Instance.GuildMission == null)
                {
                    return;
                }

                switch (csGuildMission.Type)
                {
                    case 1:
                        m_textQuestTargetValue.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetNpc.Name);
                        m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString(CsGuildManager.Instance.GuildMission.TargetDescription), csGuildMission.TargetNpc.Name);
                        break;
                    case 2:
                        m_textQuestTargetValue.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetMonster.Name, CsGuildManager.Instance.MissionProgressCount, csGuildMission.TargetCount);
                        m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString(CsGuildManager.Instance.GuildMission.TargetDescription), csGuildMission.TargetMonster.Name);
                        break;
                    case 3:
                        m_textQuestTargetValue.text = csGuildMission.TargetContent;

                        CsMonsterArrange csMonsterArrange = CsGameData.Instance.GetMonsterArrange(csGuildMission.TargetSummonMonsterArrangeId);
                        if (csMonsterArrange != null)
                        {
                            CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(csMonsterArrange.MonsterId);
                            
                            if (csMonsterInfo != null)
                            {
                                m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString(CsGuildManager.Instance.GuildMission.TargetDescription), csMonsterInfo.Name);
                            }
                            else
                            {

                                m_textContentsValue.text = "";
                            }
                        }
                        else
                        {
                            m_textContentsValue.text = "";
                        }
                        break;
                    case 4:
                        m_textQuestTargetValue.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetContinent.Name);
                        m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString(CsGuildManager.Instance.GuildMission.TargetDescription), csGuildMission.TargetContinent.Name);
                        break;
                }

                textExp.text = CsGameData.Instance.GuildMissionQuest.GetGuildMissionQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
                textGuildContribution.text = csGuildMission.GuildContributionPointReward.Value.ToString("#,##0");
                trRewardGuildContribution.gameObject.SetActive(true);

                textGuildFund.text = csGuildMission.GuildFundReward.Value.ToString("#,##0");
                trRewardGuildFund.gameObject.SetActive(true);

                textGuildBuilding.text = csGuildMission.GuildBuildingPointReward.Value.ToString("#,##0");
                trRewardGuildBuilding.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(true);
                break;

            case EnQuestType.GuildSupplySupport:
				m_trBack.gameObject.SetActive(true);
                CsGuildSupplySupportQuest csGuildSupplySupportQuest = CsGuildManager.Instance.GuildSupplySupportQuest;
                m_textQuestTargetValue.text = CsGuildManager.Instance.GuildSupplySupportQuest.Name;
                m_textContentsValue.text = csGuildSupplySupportQuest.Description;
                textExp.text = CsGameData.Instance.GuildSupplySupportQuest.GetGuildSupplySupportQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");

                textGuildFund.text = csGuildSupplySupportQuest.GuildFundReward.Value.ToString("#,##0");
                trRewardGuildFund.gameObject.SetActive(true);

                textGuildBuilding.text = csGuildSupplySupportQuest.GuildBuildingPointReward.Value.ToString("#,##0");
                trRewardGuildBuilding.gameObject.SetActive(true);

                break;

            case EnQuestType.GuildHunting:
				m_trBack.gameObject.SetActive(true);
                CsGuildHuntingQuestObjective csGuildHuntingQuestObjective = CsGuildManager.Instance.GuildHuntingQuestObjective;

                if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Accepted)
                {
                    if (csGuildHuntingQuestObjective.Type == 1)
                    {
                        m_textQuestTargetValue.text = string.Format(csGuildHuntingQuestObjective.TargetContent, csGuildHuntingQuestObjective.TargetMonster.Name, CsGuildManager.Instance.HeroGuildHuntingQuest.ProgressCount, CsGuildManager.Instance.GuildHuntingQuestObjective.TargetCount);
                    }
                    else if (csGuildHuntingQuestObjective.Type == 2)
                    {
                        m_textQuestTargetValue.text = string.Format(CsGuildManager.Instance.GuildHuntingQuestObjective.TargetContent, csGuildHuntingQuestObjective.TargetContinentObject.Name, CsGuildManager.Instance.HeroGuildHuntingQuest.ProgressCount, CsGuildManager.Instance.GuildHuntingQuestObjective.TargetCount);
                    }
                }
                else if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Executed)
                {
                    m_textQuestTargetValue.text = CsGuildManager.Instance.GuildHuntingQuest.CompletionDialogue;
                }

                m_textContentsValue.text = csGuildHuntingQuestObjective.TargetDescription;

                csItemReward = CsGuildManager.Instance.GuildHuntingQuest.ItemReward;
                trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);
                trItemReward0.Find("Text").GetComponent<Text>().text = csItemReward.Item.Name;
                trItemReward0.gameObject.SetActive(true);
                trRewardExp.gameObject.SetActive(false);
                trAbandon.gameObject.SetActive(true);
                break;

            case EnQuestType.GuildAlterDefence:
				m_trBack.gameObject.SetActive(true);
                m_textQuestTargetValue.text = CsConfiguration.Instance.GetString("A68_TXT_00003");
                m_textContentsValue.text = CsConfiguration.Instance.GetString("A68_TXT_00004");

                trRewardExp.gameObject.SetActive(false);
                trAbandon.gameObject.SetActive(false);

                break;

            case EnQuestType.GuildFarm:
				m_trBack.gameObject.SetActive(true);
                m_textQuestTargetValue.text = CsGameData.Instance.GuildFarmQuest.Name;
                
                switch (CsGuildManager.Instance.GuildFarmQuestState)
	            {
                    case EnGuildFarmQuestState.Accepted:
                        m_textContentsValue.text = CsGameData.Instance.GuildFarmQuest.TargetContent;
                        break;
                    case EnGuildFarmQuestState.Executed:
                        m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString(CsGameData.Instance.GuildFarmQuest.TargetCompletion), CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc.Name);
                        break;
	            }

                csItemReward = CsGameData.Instance.GuildFarmQuest.CompletionItemReward;
                trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);
                trItemReward0.Find("Text").GetComponent<Text>().text = csItemReward.Item.Name;
                trItemReward0.gameObject.SetActive(true);

                textExp.text = CsGameData.Instance.GuildFarmQuest.GuildFarmQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
                
                textGuildContribution.text = CsGameData.Instance.GuildFarmQuest.CompletionGuildContributionPointReward.Value.ToString("#,##0");
                trRewardGuildContribution.gameObject.SetActive(true);

                textGuildBuilding.text = CsGameData.Instance.GuildFarmQuest.CompletionGuildBuildingPointReward.Value.ToString("#,##0");
                trRewardGuildBuilding.gameObject.SetActive(true);

                trAbandon.gameObject.SetActive(false);

                break;

			case EnQuestType.TrueHero:
				m_trBack.gameObject.SetActive(true);
				switch (CsTrueHeroQuestManager.Instance.TrueHeroQuestState)
				{
					case EnTrueHeroQuestState.Accepted:
						// 버티기
						if (CsTrueHeroQuestManager.Instance.Interacted)
						{
							m_textQuestTargetValue.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.StepNo);
							m_textContentsValue.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetContent;
						}
						// 상호작용
						else
						{
							m_textQuestTargetValue.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.StepNo);
							m_textContentsValue.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.TargetContent, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.TargetContinent.Name);
						}
						break;

					case EnTrueHeroQuestState.Executed:
						m_textQuestTargetValue.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.LastStepNo);
						m_textContentsValue.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.CompletionText, CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc.Name);
						break;
				}

				CsTrueHeroQuestReward csTrueHeroReward = CsTrueHeroQuestManager.Instance.TrueHeroQuest.GetTrueHeroReward(CsGameData.Instance.MyHeroInfo.Level);
				textExp.text = csTrueHeroReward.ExpReward.Value.ToString("#,##0");
				textExploit.text = csTrueHeroReward.ExploitPointReward.Value.ToString("#,##0");

                trRewardExploit.gameObject.SetActive(true);

				trAbandon.gameObject.SetActive(false);
				break;

			case EnQuestType.SubQuest:
				m_trBack.gameObject.SetActive(true);

				CsSubQuest csSubQuest = CsGameData.Instance.GetSubQuest(m_nParam);
				CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(m_nParam);

				if (csHeroSubQuest == null)
				{
					m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csSubQuest.Title);

					switch (csSubQuest.Type)
					{
						case 1:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, csSubQuest.TargetContinentObject.Name, 0, csSubQuest.TargetCount);
							break;
						case 2:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, csSubQuest.TargetMonster.Name, 0, csSubQuest.TargetCount);
							break;
						case 3:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, 0, csSubQuest.TargetCount);
							break;
						default:
							break;
					}
				}
				else if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Acception)
				{
					m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csSubQuest.Title);

					switch (csSubQuest.Type)
					{
						case 1:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, csSubQuest.TargetContinentObject.Name, csHeroSubQuest.ProgressCount, csSubQuest.TargetCount);
							break;
						case 2:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, csSubQuest.TargetMonster.Name, csHeroSubQuest.ProgressCount, csSubQuest.TargetCount);
							break;
						case 3:
							m_textContentsValue.text = string.Format(csSubQuest.TargetText, csHeroSubQuest.ProgressCount, csSubQuest.TargetCount);
							break;
						default:
							break;
					}
				}
				else if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
				{
					m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csSubQuest.Title);
					m_textContentsValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csSubQuest.CompletionNpc.Name);
				}
				
				textExp.text = csSubQuest.ExpReward.Value.ToString("#,##0");
				textGold.text = csSubQuest.GoldReward.Value.ToString("#,##0");

				trRewardGold.gameObject.SetActive(true);

				for (int i = 0; i < 2; i++)
				{
					if (i + 1 > csSubQuest.SubQuestRewardList.Count)
					{
						break;
					}

					var item = csSubQuest.SubQuestRewardList[i].ItemReward.Item;

					if (i == 0)
					{
						trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + item.Image);
						trItemReward0.Find("Text").GetComponent<Text>().text = item.Name;
						trItemReward0.gameObject.SetActive(true);
					}
					else
					{
						trItemReward1.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + item.Image);
						trItemReward1.Find("Text").GetComponent<Text>().text = item.Name;
						trItemReward1.gameObject.SetActive(true);
					}
				}

				trAbandon.gameObject.SetActive(csSubQuest.RequiredConditionType == 1 && csSubQuest.ReacceptanceEnabled && !bAcceptableSubQuest);

				break;

			case EnQuestType.Biography:
				m_trBack.gameObject.SetActive(true);

				CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(m_nParam);

				if (csHeroBiographyQuest != null)
				{
					CsBiography csBiography = CsGameData.Instance.GetBiography(m_nParam);
					CsBiographyQuest csBiographyQuest = csHeroBiographyQuest.BiographyQuest;
					
					if (csHeroBiographyQuest.Excuted)
					{
						if (csBiographyQuest.CompletionNpc != null)
						{
							m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csBiographyQuest.CompletionNpc.Name);
						}
					}
					else
					{
						switch (csBiographyQuest.Type)
						{
							case 1:
								m_textQuestTargetValue.text = csBiographyQuest.TargetContent;
								break;
							case 2:
								m_textQuestTargetValue.text = string.Format(csBiographyQuest.TargetContent, csBiographyQuest.TargetMonster.Name, csHeroBiographyQuest.ProgressCount, csBiographyQuest.TargetCount);
								break;
							case 3:
								m_textQuestTargetValue.text = string.Format(csBiographyQuest.TargetContent, csBiographyQuest.TargetContinentObject.Name, csHeroBiographyQuest.ProgressCount, csBiographyQuest.TargetCount);
								break;
							case 4:
								m_textQuestTargetValue.text = string.Format(csBiographyQuest.TargetContent, csBiographyQuest.TargetNpc.Name, csHeroBiographyQuest.ProgressCount, csBiographyQuest.TargetCount);
								break;
							case 5:
								m_textQuestTargetValue.text = string.Format(csBiographyQuest.TargetContent, CsGameData.Instance.GetBiographyQuestDungeon(csBiographyQuest.TargetDungeonId).Name);
								break;
						}
					}

					m_textContentsValue.text = csBiographyQuest.StartDialogue;

					textExp.text = csBiographyQuest.ExpReward.Value.ToString("#,##0");
				}
				
				trAbandon.gameObject.SetActive(false);
				break;

			case EnQuestType.CreatureFarm:
				m_trBack.gameObject.SetActive(true);

				CsCreatureFarmQuest csCreatureFarmQuest = CsCreatureFarmQuestManager.Instance.CreatureFarmQuest;

				if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
				{
					m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csCreatureFarmQuest.CompletionNpc.Name);
				}
				else
				{
					CsHeroCreatureFarmQuest csHeroCreatureFarmQuest = CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest;

					switch (csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType)
					{
						case 1:
							// 이동
							m_textQuestTargetValue.text = csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent;

							break;

						case 2:
							// 상호작용
							m_textQuestTargetValue.text = string.Format(csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent, csHeroCreatureFarmQuest.CreatureFarmQuestMission.ContinentObjectTarget.Name);

							break;

						case 3:
							// 몬스터 처치
							m_textQuestTargetValue.text = csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent;

							break;
					}

				}

				m_textContentsValue.text = csCreatureFarmQuest.StartDialogue;

				CsCreatureFarmQuestExpReward csCreatureFarmQuestExpReward = csCreatureFarmQuest.GetCreatureFarmQuestExpReward(CsGameData.Instance.MyHeroInfo.Level);

				if (csCreatureFarmQuestExpReward != null)
				{
					textExp.text = csCreatureFarmQuestExpReward.ExpReward.Value.ToString("#,##0");
					trRewardExp.gameObject.SetActive(true);
				}

				int nRewardCount = 0;
				foreach (CsCreatureFarmQuestItemReward csCreatureFarmQuestItemReward in csCreatureFarmQuest.CreatureFarmQuestItemRewardList)
				{
					if (nRewardCount < 2)
					{
						if (nRewardCount == 0)
						{
							trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csCreatureFarmQuestItemReward.ItemReward.Item.Image);
							trItemReward0.Find("Text").GetComponent<Text>().text = csCreatureFarmQuestItemReward.ItemReward.Item.Name;
							trItemReward0.gameObject.SetActive(true);
						}
						else
						{
							trItemReward1.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csCreatureFarmQuestItemReward.ItemReward.Item.Image);
							trItemReward1.Find("Text").GetComponent<Text>().text = csCreatureFarmQuestItemReward.ItemReward.Item.Name;
							trItemReward1.gameObject.SetActive(true);
						}
					}

					nRewardCount++;
				}
				
				trAbandon.gameObject.SetActive(false);
				break;

            case EnQuestType.JobChange:

                m_trBack.gameObject.SetActive(true);

                CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;
                CsJobChangeQuest csJobChangeQuest = null;

                if (csHeroJobChangeQuest == null)
                {
                    // 퀘스트 수락 가능
                    csJobChangeQuest = CsGameData.Instance.JobChangeQuestList.First();
                    m_textContentsValue.text = csJobChangeQuest.Title;
                    m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);
                }
                else
                {
                    csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);
                    m_textContentsValue.text = csJobChangeQuest.Title;

                    if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                    {
                        // 퀘스트 완료
                    }
                    else
                    {
                        switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                        {
                            case EnJobChangeQuestStaus.Accepted:

                                string strContentsValue = string.Empty;

                                switch (csJobChangeQuest.Type)
                                {
                                    case 1:

                                        strContentsValue = string.Format((csJobChangeQuest.TargetContent), csJobChangeQuest.TargetMonster.Name, csHeroJobChangeQuest.ProgressCount, csJobChangeQuest.TargetCount);

                                        break;

                                    case 2:

                                        strContentsValue = string.Format((csJobChangeQuest.TargetContent), csJobChangeQuest.TargetContinentObject.Name, csHeroJobChangeQuest.ProgressCount, csJobChangeQuest.TargetCount);

                                        break;

                                    case 3: 

                                        strContentsValue = csJobChangeQuest.TargetContent;

                                        break;
                                }

                                if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                {
                                    m_textQuestTargetValue.text = strContentsValue;
                                }
                                else
                                {
                                    m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);
                                }

                                break;

                            case EnJobChangeQuestStaus.Completed:

                                m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);

                                break;

                            case EnJobChangeQuestStaus.Failed:

                                m_textQuestTargetValue.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);

                                break;
                        }
                    }
                }

                csItemReward = csJobChangeQuest.CompletionItemReward;

                if (csItemReward == null)
                {
                    
                }
                else
                {
                    trItemReward0.Find("Image").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);
                    trItemReward0.Find("Text").GetComponent<Text>().text = csItemReward.Item.Name;
                    trItemReward0.gameObject.SetActive(true);
                }

                trAbandon.gameObject.SetActive(false);

                trRewardExp.gameObject.SetActive(false);

                break;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateQuestInfoEmpty()
	{
		
	}

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void ResetQuestPanelDisplay(EnQuestType enQuestType, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

		if (PlayerPrefs.HasKey(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName))
        {
			PlayerPrefs.SetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName, 1);
        }

		CsGameEventUIToUI.Instance.OnEventDisplayQuestPanel(enQuestType, true, nParam);
    }

    IEnumerator m_IEnumeratorThreatOfFameMonsterInfo;

    //---------------------------------------------------------------------------------------------------
    IEnumerator UpdateThreatOfFameMonsterInfo()
    {
        IMonsterObjectInfo iMonsterObjectInfo = CsGameData.Instance.ListMonsterObjectInfo.Find(a => a.GetInstanceId() == CsThreatOfFarmQuestManager.Instance.Monster.InstanceId);
        yield return iMonsterObjectInfo;

        CsMonsterInfo csMonsterInfo = iMonsterObjectInfo.GetMonsterInfo();
        m_textQuestTargetValue.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.TargetKillText, csMonsterInfo.Name);

        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
        {
            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
            m_IEnumeratorThreatOfFameMonsterInfo = null;
        }
    }
}