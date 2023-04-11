using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupBlackList : CsPopupSub
{
    Transform m_trDeadRecordContent;
    Transform m_trBlackListContent;

    Text m_textNoDeadRecord;
    Text m_textNoBlackList;

    GameObject m_goBlackListItem;
    
    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsFriendManager.Instance.EventDeadRecordAdded += OnEventDeadRecordAdded;
        CsFriendManager.Instance.EventBlacklistEntryAdd += OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventBlacklistEntryDelete += OnEventBlacklistEntryDelete;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsFriendManager.Instance.EventDeadRecordAdded -= OnEventDeadRecordAdded;
        CsFriendManager.Instance.EventBlacklistEntryAdd -= OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventBlacklistEntryDelete -= OnEventBlacklistEntryDelete;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventDeadRecordAdded()
    {
        UpdatePopupDeadRecord();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBlacklistEntryAdd()
    {
        UpdatePopupBlackList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBlacklistEntryDelete()
    {
        UpdatePopupBlackList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        CsDeadRecord csDeadRecord = CsFriendManager.Instance.DeadRecordList.Find(a => a.Id == guidHeroId);

        if (csDeadRecord != null)
        {
            UpdateDeadRecordItem(csDeadRecord);
        }

        CsBlacklistEntry csBlacklistEntry = CsFriendManager.Instance.BlacklistEntryList.Find(a => a.HeroId == guidHeroId);

        if (csBlacklistEntry != null)
        {
            UpdateBlackListItem(csBlacklistEntry);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBlackListEntryDelete(CsBlacklistEntry csBlacklistEntry)
    {
        CsFriendManager.Instance.SendBlacklistEntryDelete(csBlacklistEntry.HeroId);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textDeadRecord = transform.Find("ImageDeadRecord/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeadRecord);
        textDeadRecord.text = CsConfiguration.Instance.GetString("A108_TXT_00011");

        Text textBlackList = transform.Find("ImageBlackList/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBlackList);
        textBlackList.text = CsConfiguration.Instance.GetString("A108_TXT_00012");

        m_trDeadRecordContent = transform.Find("DeadRecordList/Viewport/Content");
        m_trBlackListContent = transform.Find("BlackList/Viewport/Content");

        m_textNoDeadRecord = transform.Find("DeadRecordList/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoDeadRecord);
        m_textNoDeadRecord.text = CsConfiguration.Instance.GetString("A108_TXT_00013");

        m_textNoBlackList = transform.Find("BlackList/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoBlackList);
        m_textNoBlackList.text = CsConfiguration.Instance.GetString("A108_TXT_00014");

        DisplayPopupBlackList();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPopupBlackList()
    {
        if (m_goBlackListItem == null)
        {
            StartCoroutine(LoadBlackListItem());
        }
        else
        {
            UpdatePopupDeadRecord();
            UpdatePopupBlackList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadBlackListItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/BlackListItem");
        yield return resourceRequest;

        m_goBlackListItem = (GameObject)resourceRequest.asset;

        UpdatePopupDeadRecord();
        UpdatePopupBlackList();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupDeadRecord()
    {
        // 데드 레코드 초기화
        for (int i = 0; i < m_trDeadRecordContent.childCount; i++)
        {
            m_trDeadRecordContent.GetChild(i).gameObject.SetActive(false);
        }

        if (CsFriendManager.Instance.DeadRecordList.Count == 0)
        {
            m_textNoDeadRecord.gameObject.SetActive(true);
            return;
        }
        else
        {
            m_textNoDeadRecord.gameObject.SetActive(false);
        }

        Transform trBlackListItem = null;

        for (int i = 0; i < CsFriendManager.Instance.DeadRecordList.Count; i++)
        {
            CsDeadRecord csDeadRecord = CsFriendManager.Instance.DeadRecordList[i];

            trBlackListItem = m_trDeadRecordContent.Find("BlackListItem" + csDeadRecord.Id);

            if (trBlackListItem == null)
            {
                trBlackListItem = Instantiate(m_goBlackListItem, m_trDeadRecordContent).transform;
                trBlackListItem.name = "BlackListItem" + csDeadRecord.Id;
            }
            else
            {
                trBlackListItem.gameObject.SetActive(true);
            }

            UpdateDeadRecordItem(csDeadRecord);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDeadRecordItem(CsDeadRecord csDeadRecord)
    {
        Transform trDeadRecordItem = m_trDeadRecordContent.Find("BlackListItem" + csDeadRecord.Id);

        if (trDeadRecordItem == null)
        {
            return;
        }
        else
        {
            Image imageJob = trDeadRecordItem.Find("ImageJob").GetComponent<Image>();
            imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csDeadRecord.Job.JobId);

            Text textLevelName = trDeadRecordItem.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
            textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01024"), csDeadRecord.Level, csDeadRecord.Name);

            Transform trDeadRecordFrame = trDeadRecordItem.Find("DeadRecordFrame");
            Transform trBlackListFrame = trDeadRecordItem.Find("BlackListFrame");

            trDeadRecordFrame.gameObject.SetActive(true);
            trBlackListFrame.gameObject.SetActive(false);

            Text textTimer = trDeadRecordFrame.Find("TextTimer").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTimer);

            string strTime = "";
            System.TimeSpan timespanRegtime = System.TimeSpan.FromSeconds(csDeadRecord.RegElapsedTime);

            if ((int)timespanRegtime.TotalDays > 0)
            {
                int diffMonth = 0;
                DateTime datetimeAdded = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(timespanRegtime);

                while (true)
                {
                    datetimeAdded = datetimeAdded.AddMonths(1);

                    if (DateTime.Compare(datetimeAdded, CsGameData.Instance.MyHeroInfo.CurrentDateTime) == 1)
                    {
                        break;
                    }
                    else
                    {
                        diffMonth++;
                    }
                }

                if (diffMonth > 12)
                {
                    int diffYear = 0;
                    datetimeAdded = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(timespanRegtime);

                    while (true)
                    {
                        datetimeAdded = datetimeAdded.AddYears(1);

                        if (DateTime.Compare(datetimeAdded, CsGameData.Instance.MyHeroInfo.CurrentDateTime) == 1)
                        {
                            break;
                        }
                        else
                        {
                            diffYear++;
                        }
                    }

                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_5"), diffYear);
                }
                else if (diffMonth < 1)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_3"), (int)timespanRegtime.TotalDays);
                }
                else
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_4"), diffMonth);
                }
            }
            else
            {
                if ((int)timespanRegtime.TotalHours > 0)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_2"), (int)timespanRegtime.TotalHours);
                }
                else if (timespanRegtime.TotalMinutes > 0)
                {
                    strTime = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TIME_1"), (int)timespanRegtime.TotalMinutes);
                }
            }

            textTimer.text = strTime;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupBlackList()
    {
        // 블랙 리스트 초기화
        for (int i = 0; i < m_trBlackListContent.childCount; i++)
        {
            m_trBlackListContent.GetChild(i).gameObject.SetActive(false);
        }

        if (CsFriendManager.Instance.BlacklistEntryList.Count == 0)
        {
            m_textNoBlackList.gameObject.SetActive(true);
            return;
        }
        else
        {
            m_textNoBlackList.gameObject.SetActive(false);
        }

        Transform trBlackListItem = null;

        for (int i = 0; i < CsFriendManager.Instance.BlacklistEntryList.Count; i++)
        {
            CsBlacklistEntry csBlacklistEntry = CsFriendManager.Instance.BlacklistEntryList[i];

            trBlackListItem = m_trBlackListContent.Find("BlackListItem" + csBlacklistEntry.HeroId);

            if (trBlackListItem == null)
            {
                trBlackListItem = Instantiate(m_goBlackListItem, m_trBlackListContent).transform;
                trBlackListItem.name = "BlackListItem" + csBlacklistEntry.HeroId;
            }
            else
            {
                trBlackListItem.gameObject.SetActive(true);
            }

            UpdateBlackListItem(csBlacklistEntry);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBlackListItem(CsBlacklistEntry csBlacklistEntry)
    {
        Transform trBlackListItem = m_trBlackListContent.Find("BlackListItem" + csBlacklistEntry.HeroId);

        if (trBlackListItem == null)
        {
            return;
        }
        else
        {
            Image imageJob = trBlackListItem.Find("ImageJob").GetComponent<Image>();
            imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csBlacklistEntry.Job.JobId);

            Text textLevelName = trBlackListItem.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
            textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01024"), csBlacklistEntry.Level, csBlacklistEntry.Name);

            Transform trDeadRecordFrame = trBlackListItem.Find("DeadRecordFrame");
            Transform trBlackListFrame = trBlackListItem.Find("BlackListFrame");

            trDeadRecordFrame.gameObject.SetActive(false);
            trBlackListFrame.gameObject.SetActive(true);

            Button buttonDelete = trBlackListFrame.Find("ButtonDelete").GetComponent<Button>();
            buttonDelete.onClick.RemoveAllListeners();
            buttonDelete.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonDelete.onClick.AddListener(() => OnClickBlackListEntryDelete(csBlacklistEntry));

            Text textButtonDelete = buttonDelete.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonDelete);
            textButtonDelete.text = CsConfiguration.Instance.GetString("A108_BTN_00033");
        }
    }
}
