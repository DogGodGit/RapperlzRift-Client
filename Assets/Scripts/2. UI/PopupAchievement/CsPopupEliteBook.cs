using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsPopupEliteBook : CsPopupSub
{
    GameObject m_goCategory;
    GameObject m_goType;

    Text m_textCompleted;
    Text m_textNextRenewalTime;
    Text m_textRenewal;
    Text m_textCountTime;
    Text m_textEnterEliteDungeonCount;
	Text m_textStamina;
	Text m_textMaxStamina;

    List<CsEliteMonsterMaster> m_listCsEliteMonsterMaster;

    int m_nSelectedCategoryIndex = 0;
    int m_nSelectedMonsterMasterIndex = 0;
    //int m_nSelectedContinentId = 0;

    float m_flDungeonEnterTime;
    float m_flTime;

    bool m_bFirst;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsEliteManager.Instance.EventEliteMonsterKillCountUpdated += OnEventEliteMonsterKillCountUpdated;
        CsEliteManager.Instance.EventEliteMonsterRemoved += OnEventEliteMonsterRemoved;
        CsEliteManager.Instance.EventEliteMonsterSpawn += OnEventEliteMonsterSpawn;

		CsGameEventUIToUI.Instance.EventStaminaBuy += OnEventStaminaBuy;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }
        else
        {
            Transform trCategoryList = transform.Find("CategoryList");

            for (int i = 0; i < trCategoryList.childCount; i++)
            {
                Transform trCategory = trCategoryList.Find("Cartegory" + i);

                Toggle toggle = trCategory.GetComponent<Toggle>();

                if (i == 0)
                {
                    if (toggle.isOn)
                    {
                        m_nSelectedCategoryIndex = 0;
                        UpdateEliteMonsterList();
                        UpdateEliteBookCategoryCompleted();
                    }
                    else
                    {
                        toggle.isOn = true;
                    }
                }
                else
                {
                    toggle.isOn = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            UpdateDungeonEnterButton();
            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsEliteManager.Instance.EventEliteMonsterKillCountUpdated -= OnEventEliteMonsterKillCountUpdated;
        CsEliteManager.Instance.EventEliteMonsterRemoved -= OnEventEliteMonsterRemoved;
        CsEliteManager.Instance.EventEliteMonsterSpawn -= OnEventEliteMonsterSpawn;

		CsGameEventUIToUI.Instance.EventStaminaBuy -= OnEventStaminaBuy;
    }

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnEventEliteMonsterKillCountUpdated()
    {
        UpdateEliteMonsterInfo();
        UpdateEliteBookCategoryCompleted();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteMonsterRemoved()
    {
        UpdateEliteMonsterInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteMonsterSpawn()
    {
        UpdateEliteMonsterInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedEliteMonsterBookCategory(Toggle toggle, int nIndex)
    {
        Text textCategory = toggle.transform.Find("TextContentName").GetComponent<Text>();

        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nSelectedCategoryIndex = nIndex;
            UpdateEliteMonsterList();
            UpdateEliteBookCategoryCompleted();

            textCategory.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textCategory.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedEliteMonseterMaster(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nSelectedMonsterMasterIndex = nIndex;
            UpdateEliteMonsterInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGoMonster()
    {
        int nContinentId = CsGameData.Instance.EliteMonsterCategoryList[m_nSelectedCategoryIndex].Continent.ContinentId;
        Vector3 vtSelectedMonsterPosition = new Vector3(m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].XPosition, m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].YPosition, m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].ZPosition);
        CsUIData.Instance.AutoStateType = EnAutoStateType.Move;

        CsEliteManager.Instance.AutoMoveToEliteMonster(nContinentId, vtSelectedMonsterPosition);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnterEliteDungeon()
    {
        int nCurrentTime = (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour * 3600) + (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute * 60) + CsGameData.Instance.MyHeroInfo.CurrentDateTime.Second;
        int nAddCount = nCurrentTime / CsGameData.Instance.EliteDungeon.EnterCountAddInterval;
        int nCount = CsGameData.Instance.EliteDungeon.BaseEnterCount + nAddCount - CsEliteManager.Instance.DailyEliteDungeonPlayCount;

        if (nCount > 0)
        {
            CsDungeonManager.Instance.SendContinentExitForEliteDungeonEnter(m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterMasterId);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickChargeStamina()
	{
		int nStaminBuyCount = CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount + 1;
		CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);

		if (csStaminaBuyCount == null)
		{
			nStaminBuyCount = CsGameData.Instance.StaminaBuyCountList.Count;
			csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);
		}

		//스테미너 충전 확인창
		string strDes = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03001"), csStaminaBuyCount.RequiredDia, csStaminaBuyCount.Stamina);

		CsGameEventUIToUI.Instance.OnEventConfirm(strDes, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickChargeStaminaOK(nStaminBuyCount), CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickChargeStaminaOK(int nStaminaBuyCount)
	{
		CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminaBuyCount);

		if (csStaminaBuyCount.RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
		{
			CsCommandEventManager.Instance.SendStaminaBuy();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStaminaBuy()
	{
		m_textStamina.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.Stamina, CsGameConfig.Instance.MaxStamina);

		CheckStaminaMax();
	}

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textCompleted = transform.Find("TextCompleted").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCompleted);
        textCompleted.text = CsConfiguration.Instance.GetString("A85_TXT_00001");

        m_textCompleted = transform.Find("TextCompletedValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCompleted);

        m_textNextRenewalTime = transform.Find("TextRenewalTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNextRenewalTime);

        Text textNextRenewal = m_textNextRenewalTime.transform.Find("TextNextRenewal").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNextRenewal);
        textNextRenewal.text = CsConfiguration.Instance.GetString("A85_TXT_00002");

        m_textRenewal = transform.Find("TextRenewal").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textRenewal);
        m_textRenewal.text = CsConfiguration.Instance.GetString("A85_TXT_00004");

        m_textCountTime = transform.Find("TextRemaningTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCountTime);

        Text textActivateAttr = transform.Find("ActivateAttrList/TextAttrActivate").GetComponent<Text>();
        CsUIData.Instance.SetFont(textActivateAttr);
        textActivateAttr.text = CsConfiguration.Instance.GetString("A85_TXT_00003");

        //버튼설정
        Button buttonGoMonster = transform.Find("ButtonShortCut").GetComponent<Button>();
        buttonGoMonster.onClick.RemoveAllListeners();
        buttonGoMonster.onClick.AddListener(OnClickGoMonster);
        buttonGoMonster.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textGoMonster = buttonGoMonster.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGoMonster);
        textGoMonster.text = CsConfiguration.Instance.GetString("A85_BTN_00002");

        Button buttonEnterEliteDungeon = transform.Find("ButtonChallenge").GetComponent<Button>();
        buttonEnterEliteDungeon.onClick.RemoveAllListeners();
        buttonEnterEliteDungeon.onClick.AddListener(OnClickEnterEliteDungeon);
        buttonEnterEliteDungeon.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textEnterEliteDungeon = buttonEnterEliteDungeon.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnterEliteDungeon);
        textEnterEliteDungeon.text = CsConfiguration.Instance.GetString("A85_BTN_00001");

        m_textEnterEliteDungeonCount = buttonEnterEliteDungeon.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEnterEliteDungeonCount);
        m_textEnterEliteDungeonCount.text = "0";

		m_textStamina = transform.Find("Stamina/TextLayout/TextStamina").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textStamina);
		m_textStamina.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.Stamina, CsGameConfig.Instance.MaxStamina);

		m_textMaxStamina = transform.Find("Stamina/TextLayout/TextFull").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textMaxStamina);
		m_textMaxStamina.text = CsConfiguration.Instance.GetString("A13_TITLE_00003");

		Button buttonBuyStamina = transform.Find("Stamina/ButtonPlus").GetComponent<Button>();
		buttonBuyStamina.onClick.RemoveAllListeners();
		buttonBuyStamina.onClick.AddListener(OnClickChargeStamina);
		buttonBuyStamina.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		CheckStaminaMax();
		
        Transform trCategoryList = transform.Find("CategoryList");

        if (m_goCategory == null)
        {
            m_goCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleIllustBookCategory");
        }

        for (int i = 0; i < CsGameData.Instance.EliteMonsterCategoryList.Count; i++)
        {
            int nIndex = i;

            GameObject goCategory = Instantiate(m_goCategory, trCategoryList);
            goCategory.name = "Cartegory" + i;

            Toggle toggle = goCategory.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.group = trCategoryList.GetComponent<ToggleGroup>();

            Text textCategory = goCategory.transform.Find("TextContentName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCategory);
            textCategory.text = string.Format(CsGameData.Instance.EliteMonsterCategoryList[i].Name, CsGameData.Instance.EliteMonsterCategoryList[i].RecommendMinHeroLevel, CsGameData.Instance.EliteMonsterCategoryList[i].RecommendMaxHeroLevel);

            if (i == 0)
            {
                toggle.isOn = true;
                textCategory.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                toggle.isOn = false;
                textCategory.color = CsUIData.Instance.ColorGray;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedEliteMonsterBookCategory(toggle, nIndex));
        }

        m_nSelectedCategoryIndex = 0;
        UpdateEliteMonsterList();
        UpdateEliteBookCategoryCompleted();
    }

	//---------------------------------------------------------------------------------------------------
	void CheckStaminaMax()
	{
		if (CsGameData.Instance.MyHeroInfo.Stamina >= CsGameConfig.Instance.MaxStamina)
		{
			m_textMaxStamina.gameObject.SetActive(true);
		}
		else
		{
			m_textMaxStamina.gameObject.SetActive(false);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateEliteMonsterList()
    {
        Transform trTypeList = transform.Find("Scroll View/Viewport/Content");

        if (m_goType == null)
        {
            m_goType = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleIllustBookContent");
        }

        for (int i = 0; i < trTypeList.childCount; i++)
        {
            trTypeList.GetChild(i).gameObject.SetActive(false);
        }

        m_listCsEliteMonsterMaster = CsGameData.Instance.EliteMonsterMasterList.FindAll(a => a.EliteMonsterCategory.CategoryId == CsGameData.Instance.EliteMonsterCategoryList[m_nSelectedCategoryIndex].CategoryId);

        for (int i = 0; i < m_listCsEliteMonsterMaster.Count; i++)
        {
            int nIndex = i;

            Transform trType = trTypeList.Find("Type" + i);

            if (trType == null)
            {
                GameObject goType = Instantiate(m_goType, trTypeList);
                goType.name = "Type" + i;
                trType = goType.transform;
            }
            else
            {
                trType.gameObject.SetActive(true);
            }

            Toggle toggle = trType.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.group = trTypeList.GetComponent<ToggleGroup>();

            if (i == 0)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedEliteMonseterMaster(toggle, nIndex));

            Text textType = trType.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textType);
            textType.text = string.Format(m_listCsEliteMonsterMaster[i].Name, m_listCsEliteMonsterMaster[i].Level);
        }

        m_nSelectedMonsterMasterIndex = 0;
        UpdateEliteMonsterInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEliteMonsterInfo()
    {
        //속성값 변경
        Transform trAttrList = transform.Find("ActivateAttrList");

        List<CsEliteMonster> listCsEliteMonster = CsGameData.Instance.EliteMonsterList.FindAll(a => a.EliteMonsterMaster.EliteMonsterMasterId == m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterMasterId);
        bool bSpawned = false;

        for (int i = 0; i < listCsEliteMonster.Count; i++)
        {
            Transform trAttr = trAttrList.Find("ActivateAttr" + (listCsEliteMonster[i].StarGrade - 1));

            int nKillCount = CsEliteManager.Instance.MyKillCount(listCsEliteMonster[i].EliteMonsterId);

            Text textCount = trAttr.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);

            Text textAttrName = trAttr.Find("TextAttrName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = listCsEliteMonster[i].Attr.Name;

            Text textPrevAttrValue = trAttr.Find("TextPrevAttrValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPrevAttrValue);

            Text textNextAttrValue = trAttr.Find("TextNextAttrValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNextAttrValue);

            //킬카운트에 따른 값계산
            if (nKillCount < listCsEliteMonster[i].EliteMonsterKillAttrValueList[0].KillCount)
            {
                textPrevAttrValue.text = "0";
                textNextAttrValue.text = listCsEliteMonster[i].EliteMonsterKillAttrValueList[0].AttrValue.Value.ToString("#,##0");
                textCount.text = string.Format(CsConfiguration.Instance.GetString("A85_TXT_01001"), nKillCount, listCsEliteMonster[i].EliteMonsterKillAttrValueList[0].KillCount);
            }
            else
            {
                for (int j = 0; j < listCsEliteMonster[i].EliteMonsterKillAttrValueList.Count; j++)
                {
                    if (j < listCsEliteMonster[i].EliteMonsterKillAttrValueList.Count - 1)
                    {
                        if (listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].KillCount <= nKillCount && listCsEliteMonster[i].EliteMonsterKillAttrValueList[j + 1].KillCount > nKillCount)
                        {
                            textPrevAttrValue.text = listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].AttrValue.Value.ToString("#,##0");
                            textNextAttrValue.text = listCsEliteMonster[i].EliteMonsterKillAttrValueList[j + 1].AttrValue.Value.ToString("#,##0");
                            textCount.text = string.Format(CsConfiguration.Instance.GetString("A85_TXT_01001"), nKillCount, listCsEliteMonster[i].EliteMonsterKillAttrValueList[j + 1].KillCount);
                            break;
                        }
                    }
                    else
                    {
                        textPrevAttrValue.text = listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].AttrValue.Value.ToString("#,##0");
                        textNextAttrValue.text = CsConfiguration.Instance.GetString("A85_TXT_00005");

                        if (listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].KillCount < nKillCount)
                        {
                            nKillCount = listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].KillCount;
                        }

                        textCount.text = string.Format(CsConfiguration.Instance.GetString("A85_TXT_01001"), nKillCount, listCsEliteMonster[i].EliteMonsterKillAttrValueList[j].KillCount);
                    }
                }
            }

            if (CsEliteManager.Instance.EliteMonsterSpawned(listCsEliteMonster[i].EliteMonsterId))
            {
                bSpawned = true;
            }
        }

        //스폰시간 텍스트 변경
        if (bSpawned)
        {
            m_textRenewal.gameObject.SetActive(true);
            m_textNextRenewalTime.gameObject.SetActive(false);
        }
        else
        {
            m_textRenewal.gameObject.SetActive(false);
            m_textNextRenewalTime.gameObject.SetActive(true);

            // 다음스폰시간
            int nCurrentTime = (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour * 3600) + (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute * 60) + CsGameData.Instance.MyHeroInfo.CurrentDateTime.Second;
            TimeSpan timeSpan;

            for (int i = 0; i < m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList.Count; i++)
            {
                if (nCurrentTime < m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[0].SpawnTime ||
                    nCurrentTime > m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList.Count - 1].SpawnTime)
                {
                    timeSpan = TimeSpan.FromSeconds(m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[0].SpawnTime);
                    m_textNextRenewalTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"));
                }
                else
                {
                    if (nCurrentTime > m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[i].SpawnTime && nCurrentTime < m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[i + 1].SpawnTime)
                    {
                        timeSpan = TimeSpan.FromSeconds(m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].EliteMonsterSpawnScheduleList[i + 1].SpawnTime);
                        m_textNextRenewalTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"));
                    }
                }
            }
        }

        //모델변경
        Transform tr3DMonsterList = transform.Find("3DMonster");

        for (int i = 0; i < tr3DMonsterList.childCount; i++)
        {
            if (tr3DMonsterList.GetChild(i).name != "UIChar_Camera_Book")
            {
                tr3DMonsterList.GetChild(i).gameObject.SetActive(false);
            }
        }

        Transform tr3DMonster = tr3DMonsterList.Find("Monster" + m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].DisplayMonsterId);

        if (tr3DMonster == null)
        {
            CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].DisplayMonsterId);

            if (csMonsterInfo != null)
            {
                GameObject goMon = Instantiate(Resources.Load<GameObject>(string.Format("Prefab/MonsterObject/{0:D2}", csMonsterInfo.MonsterCharacter.PrefabName)), tr3DMonsterList) as GameObject;
                Destroy(goMon.GetComponent<CsMonster>());
                goMon.name = "Monster" + m_listCsEliteMonsterMaster[m_nSelectedMonsterMasterIndex].DisplayMonsterId;

                int nLayer = LayerMask.NameToLayer("UIChar");
                Transform[] atrMon = goMon.GetComponentsInChildren<Transform>();

                for (int i = 0; i < atrMon.Length; ++i)
                {
                    atrMon[i].gameObject.layer = nLayer;
                }

                goMon.transform.localPosition = new Vector3(-35, -170, 511);
                goMon.transform.eulerAngles = new Vector3(0, 160f, 0);
                goMon.transform.localScale = new Vector3(150, 150, 150);
                tr3DMonster = goMon.transform;
            }
        }
        else
        {
            tr3DMonster.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEliteBookCategoryCompleted()
    {
        int nTotalCount = 0;
        int nTotalMax = 0;

        for (int i = 0; i < m_listCsEliteMonsterMaster.Count; i++)
        {
            List<CsEliteMonster> listCsEliteMonster = CsGameData.Instance.EliteMonsterList.FindAll(a => a.EliteMonsterMaster.EliteMonsterMasterId == m_listCsEliteMonsterMaster[i].EliteMonsterMasterId);

            for (int j = 0; j < listCsEliteMonster.Count; j++)
            {
                int nKillCount = CsEliteManager.Instance.MyKillCount(listCsEliteMonster[j].EliteMonsterId);
                nTotalMax += listCsEliteMonster[j].EliteMonsterKillAttrValueList.Count;

                if (nKillCount < listCsEliteMonster[j].EliteMonsterKillAttrValueList[0].KillCount)
                {
                    continue;
                }
                else if (nKillCount >= listCsEliteMonster[j].EliteMonsterKillAttrValueList[listCsEliteMonster[j].EliteMonsterKillAttrValueList.Count - 1].KillCount)
                {
                    nTotalCount += listCsEliteMonster[j].EliteMonsterKillAttrValueList.Count;
                }
                else
                {
                    for (int k = 0; k < listCsEliteMonster[j].EliteMonsterKillAttrValueList.Count; k++)
                    {
                        if (nKillCount >= listCsEliteMonster[j].EliteMonsterKillAttrValueList[k].KillCount && nKillCount < listCsEliteMonster[j].EliteMonsterKillAttrValueList[k + 1].KillCount)
                        {
                            nTotalCount += (k + 1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        float flPercent = (float)nTotalCount / nTotalMax * 100;
        m_textCompleted.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_PER"), flPercent.ToString("##0"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDungeonEnterButton()
    {
        //던전 입장 버튼 및 카운트 // 시간 업데이트
        int nCurrentTime = (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour * 3600) + (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute * 60) + CsGameData.Instance.MyHeroInfo.CurrentDateTime.Second;
        int nAddCount = nCurrentTime / CsGameData.Instance.EliteDungeon.EnterCountAddInterval;

        if (m_textEnterEliteDungeonCount != null)
        {
            //버튼에 던전카운트 표시
            m_textEnterEliteDungeonCount.text = (CsGameData.Instance.EliteDungeon.BaseEnterCount + nAddCount - CsEliteManager.Instance.DailyEliteDungeonPlayCount).ToString("#,##0");
        }

        m_flDungeonEnterTime = CsGameData.Instance.EliteDungeon.EnterCountAddInterval - (nCurrentTime % CsGameData.Instance.EliteDungeon.EnterCountAddInterval) + Time.realtimeSinceStartup;
        UpdateDungeonEnterCountRecoveryTime();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDungeonEnterCountRecoveryTime()
    {
        //다음 카운트를 얻을때 까지 남은 시간
        if (m_textCountTime != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(m_flDungeonEnterTime - Time.realtimeSinceStartup);
            m_textCountTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
        }
    }
}
