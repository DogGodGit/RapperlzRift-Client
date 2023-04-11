using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport
{
    [SerializeField] GameObject m_goSeriesMissionItem;

    Transform m_trSeriesMission;
    Transform m_trSeriesMissionList;

    Text m_textAllMissionClear;

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventSeriesMissionRewardReceive(CsHeroSeriesMission csHeroSeriesMission)
    {
        UpdateSeriesMissionItem();
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSeriesMissionUpdated(CsHeroSeriesMission csHeroSeriesMission)
    {
        UpdateSeriesMissionItem();
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceiveSeriesMissionReward(int nMissionId, int nStep)
    {
        CsCommandEventManager.Instance.SendSeriesMissionRewardReceive(nMissionId, nStep);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeSeriesMission(Transform trSeriesMission)
    {
        // UI Initialize
        m_trSeriesMission = trSeriesMission;
        m_trSeriesMissionList = m_trSeriesMission.Find("Scroll View/Viewport/Content");

        Text textInfo = m_trSeriesMission.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfo);
        textInfo.text = CsConfiguration.Instance.GetString("A37_TXT_00007");

        m_textAllMissionClear = m_trSeriesMission.Find("TextAllMissionClear").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAllMissionClear);
        m_textAllMissionClear.text = CsConfiguration.Instance.GetString("A37_TXT_01009");

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList.Count; i++)
        {
            CreateSeriesMissionItem(i);
        }

        UpdateSeriesMissionItem();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSeriesMissionItem(int nIndex)
    {
        CsHeroSeriesMission csHeroSeriesMission = CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList[nIndex];

        Transform trSeriesMissionItem = Instantiate(m_goSeriesMissionItem, m_trSeriesMissionList).transform;
        trSeriesMissionItem.name = "SeriesMissionItem" + csHeroSeriesMission.SeriesMission.MissionId;

        Text textMissionStep = trSeriesMissionItem.Find("TextMissionStep").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMissionStep);

        Text textMissionInfo = trSeriesMissionItem.Find("TextMissionInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMissionInfo);

        Text textProgress = trSeriesMissionItem.Find("TextProgress").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgress);
        textProgress.text = CsConfiguration.Instance.GetString("A37_TXT_01006");

        Text textProgressCount = trSeriesMissionItem.Find("TextProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgressCount);

        Button buttonReceive = trSeriesMissionItem.Find("ButtonReceive").GetComponent<Button>();

        Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A37_BTN_00001");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSeriesMissionItem()
    {
        if (CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList.Count == 0)
        {
            m_textAllMissionClear.gameObject.SetActive(true);
        }
        else
        {
            // 초기화
            for (int i = 0; i < m_trSeriesMissionList.childCount; i++)
            {
                Transform trSeriesMissionItem = m_trSeriesMissionList.GetChild(i);
                trSeriesMissionItem.gameObject.SetActive(false);
            }

            // 미션 정렬 리스트
            List<CsSeriesMission> listCsSeriesMissionProgress = new List<CsSeriesMission>();
            List<CsSeriesMission> listCsSeriesMissionSuccess = new List<CsSeriesMission>();

            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList.Count; i++)
            {
                CsHeroSeriesMission csHeroSeriesMission = CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList[i];
                Transform trSeriesMissionItem = m_trSeriesMissionList.Find("SeriesMissionItem" + csHeroSeriesMission.SeriesMission.MissionId);

                UpdateTextMission(trSeriesMissionItem, csHeroSeriesMission);
                UpdateItemSlot(trSeriesMissionItem, csHeroSeriesMission);
                UpdateTextProgressCount(trSeriesMissionItem, csHeroSeriesMission);
                UpdateButtonReceive(trSeriesMissionItem, csHeroSeriesMission);

                if (csHeroSeriesMission.ProgressCount < csHeroSeriesMission.SeriesMissionStep.TargetCount)
                {
                    listCsSeriesMissionProgress.Add(csHeroSeriesMission.SeriesMission);
                }
                else
                {
                    listCsSeriesMissionSuccess.Add(csHeroSeriesMission.SeriesMission);
                }

                trSeriesMissionItem.gameObject.SetActive(true);
            }

            listCsSeriesMissionSuccess.Sort(CompareTo);
            listCsSeriesMissionProgress.Sort(CompareTo);

            for (int i = 0; i < listCsSeriesMissionSuccess.Count; i++)
            {
                Transform trSeriesMissionItem = m_trSeriesMissionList.Find("SeriesMissionItem" + listCsSeriesMissionSuccess[i].MissionId);
                trSeriesMissionItem.SetSiblingIndex(i);
            }
            for (int i = listCsSeriesMissionSuccess.Count; i < listCsSeriesMissionSuccess.Count + listCsSeriesMissionProgress.Count; i++)
            {
                int nIndex = i - listCsSeriesMissionSuccess.Count;
                Transform trSeriesMissionItem = m_trSeriesMissionList.Find("SeriesMissionItem" + listCsSeriesMissionProgress[nIndex].MissionId);
                trSeriesMissionItem.SetSiblingIndex(i);
            }

            m_textAllMissionClear.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextMission(Transform trSeriesMissionItem, CsHeroSeriesMission csHeroSeriesMission)
    {
        Text textMissionStep = trSeriesMissionItem.Find("TextMissionStep").GetComponent<Text>();
        textMissionStep.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01004"), csHeroSeriesMission.SeriesMission.Name, csHeroSeriesMission.CurrentStep);

        Text textMissionInfo = trSeriesMissionItem.Find("TextMissionInfo").GetComponent<Text>();
        textMissionInfo.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01005"), csHeroSeriesMission.SeriesMission.Name, csHeroSeriesMission.SeriesMissionStep.TargetCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateItemSlot(Transform trSeriesMissionItem, CsHeroSeriesMission csHeroSeriesMission)
    {
        for (int i = 0; i < csHeroSeriesMission.SeriesMissionStep.SeriesMissionStepRewardList.Count; i++)
        {
            CsItemReward csItemReward = csHeroSeriesMission.SeriesMissionStep.SeriesMissionStepRewardList[i].ItemReward;

            Transform trItemSlot = trSeriesMissionItem.Find("ItemSlot" + i);
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(csItemReward));
            buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextProgressCount(Transform trSeriesMissionItem, CsHeroSeriesMission csHeroSeriesMission)
    {
        Text textProgressCount = trSeriesMissionItem.Find("TextProgressCount").GetComponent<Text>();

        if (csHeroSeriesMission.ProgressCount > csHeroSeriesMission.SeriesMissionStep.TargetCount)
        {
            csHeroSeriesMission.ProgressCount = csHeroSeriesMission.SeriesMissionStep.TargetCount;
        }

        textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01007"), csHeroSeriesMission.ProgressCount, csHeroSeriesMission.SeriesMissionStep.TargetCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonReceive(Transform trSeriesMissionItem, CsHeroSeriesMission csHeroSeriesMission)
    {
        Button buttonReceive = trSeriesMissionItem.Find("ButtonReceive").GetComponent<Button>();
        buttonReceive.onClick.RemoveAllListeners();
        buttonReceive.onClick.AddListener(() => OnClickReceiveSeriesMissionReward(csHeroSeriesMission.SeriesMission.MissionId, csHeroSeriesMission.CurrentStep));
        buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        if (csHeroSeriesMission.ProgressCount >= csHeroSeriesMission.SeriesMissionStep.TargetCount)
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(CsSeriesMission x, CsSeriesMission y)
    {
        if (x.SortNo > y.SortNo) return 1;
        else if (x.SortNo < y.SortNo) return -1;
        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeSeriesMission()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList.Count; i++)
        {
            CsHeroSeriesMission csHeroSeriesMission = CsGameData.Instance.MyHeroInfo.HeroSeriesMissionList[i];

            if (csHeroSeriesMission.SeriesMissionStep.TargetCount <= csHeroSeriesMission.ProgressCount)
            {
                return true;
            }
        }

        return false;
    }
}