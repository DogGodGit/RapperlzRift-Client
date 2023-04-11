using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationWarHeroObjective : CsPopupSub
{
    [SerializeField] GameObject m_goNationWarHeroObjectiveItem;
    Transform m_trContent;

    bool m_bFirst = true;

    void Awake()
    {
        CsNationWarManager.Instance.EventNationWarKillCountUpdated += OnEventNationWarKillCountUpdated;
        CsNationWarManager.Instance.EventNationWarImmediateRevivalCountUpdated += OnEventNationWarImmediateRevivalCountUpdated;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsNationWarManager.Instance.EventNationWarKillCountUpdated -= OnEventNationWarKillCountUpdated;
        CsNationWarManager.Instance.EventNationWarImmediateRevivalCountUpdated -= OnEventNationWarImmediateRevivalCountUpdated;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarKillCountUpdated()
    {
        Debug.Log("OnEventNationWarKillCountUpdated");
        UpdateNationWarHeroObjectiveItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarImmediateRevivalCountUpdated()
    {
        Debug.Log("OnEventNationWarImmediateRevivalCountUpdated");
        UpdateNationWarHeroObjectiveItem();
    }

    //---------------------------------------------------------------------------------------------------

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        m_trContent.localPosition = new Vector3(0, 0, 0);
        UpdateNationWarHeroObjectiveItem();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CreateNationWarHeroObjectiveItem();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateNationWarHeroObjectiveItem()
    {
        m_trContent = transform.Find("Scroll View/Viewport/Content");

        Transform trNationWarHeroObjectiveItem = null;

        for (int i = 0; i < CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.Count; i++)
        {
            CsNationWarHeroObjectiveEntry csNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList[i];

            trNationWarHeroObjectiveItem = transform.Find("NationWarHeroObjectiveItem" + i);

            if (trNationWarHeroObjectiveItem == null)
            {
                trNationWarHeroObjectiveItem = Instantiate(m_goNationWarHeroObjectiveItem, m_trContent).transform;
                trNationWarHeroObjectiveItem.name = "NationWarHeroObjectiveItem" + i;
            }

            Image imageIcon = trNationWarHeroObjectiveItem.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_achievements_" + csNationWarHeroObjectiveEntry.Type);

            Transform trNationWarHeroObjectiveInfo = trNationWarHeroObjectiveItem.Find("NationWarHeroObjectiveInfo");

            Text textName = trNationWarHeroObjectiveInfo.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csNationWarHeroObjectiveEntry.Name;

            Text textDescription = trNationWarHeroObjectiveInfo.Find("TextDescription").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDescription);
            textDescription.text = csNationWarHeroObjectiveEntry.Description;

            Slider sliderAchievement = trNationWarHeroObjectiveInfo.Find("Slider").GetComponent<Slider>();
            sliderAchievement.maxValue = csNationWarHeroObjectiveEntry.ObjectiveCount;

            switch (i)
            {
                // 국전 승리
                case 0:
                    sliderAchievement.value = 0;
                    break;
                // 국전 패배
                case 1:
                    sliderAchievement.value = 0;
                    break;
                // 국전 5명 제거
                case 2:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                    break;
                // 국전 10명 제거
                case 3:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                    break;
                // 국전 20명 제거
                case 4:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                    break;
                // 국전 50명 이상 제거
                case 5:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                    break;
                // 국전 즉시 부활 5회
                case 6:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                    break;
                // 국전 즉시 부활 10회
                case 7:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                    break;
                // 국전 즉시 부활 20회
                case 8:
                    sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                    break;
            }

            Text textProgress = sliderAchievement.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textProgress);
            textProgress.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), sliderAchievement.value, sliderAchievement.maxValue);

            Image imageGood = trNationWarHeroObjectiveItem.Find("ImageGoods/Image").GetComponent<Image>();

            switch (csNationWarHeroObjectiveEntry.RewardType)
            {
                case 1: // 공적
                    imageGood.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods06");
                    break;
                case 2: // 다이아
                    imageGood.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods02");
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarHeroObjectiveItem()
    {
        for (int i = 0; i < CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.Count; i++)
        {
            Transform trNationWarHeroObjectiveItem = m_trContent.Find("NationWarHeroObjectiveItem" + i);

            if (trNationWarHeroObjectiveItem == null)
            {
                continue;
            }
            else
            {
                CsNationWarHeroObjectiveEntry csNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList[i];

                Transform trNationWarHeroObjectiveInfo = trNationWarHeroObjectiveItem.Find("NationWarHeroObjectiveInfo");

                Slider sliderAchievement = trNationWarHeroObjectiveInfo.Find("Slider").GetComponent<Slider>();
                sliderAchievement.maxValue = csNationWarHeroObjectiveEntry.ObjectiveCount;

                switch (i)
                {
                    // 국전 승리
                    case 0:
                        sliderAchievement.value = 0;
                        break;
                    // 국전 패배
                    case 1:
                        sliderAchievement.value = 0;
                        break;
                    // 국전 5명 제거
                    case 2:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                        break;
                    // 국전 10명 제거
                    case 3:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                        break;
                    // 국전 20명 제거
                    case 4:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                        break;
                    // 국전 50명 이상 제거
                    case 5:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarKillCount;
                        break;
                    // 국전 즉시 부활 5회
                    case 6:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                        break;
                    // 국전 즉시 부활 10회
                    case 7:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                        break;
                    // 국전 즉시 부활 20회
                    case 8:
                        sliderAchievement.value = CsNationWarManager.Instance.NationWarImmediateRevivalCount;
                        break;
                }

                Text textProgress = sliderAchievement.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textProgress);
                textProgress.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), sliderAchievement.value, sliderAchievement.maxValue);
            }
        }
    }
}