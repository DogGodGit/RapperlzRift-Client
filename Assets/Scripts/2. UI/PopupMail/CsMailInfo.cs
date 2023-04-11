using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-08)
//---------------------------------------------------------------------------------------------------

public class CsMailInfo : CsPopupSub
{
    [SerializeField] GameObject m_goItemSlot;

    Transform m_trMailInfo;

    Button m_buttonMailReceive;
    Button m_buttonMailDelete;

    CsMail m_csMail;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMailSelected += OnEventMailSelected;
        CsGameEventUIToUI.Instance.EventMailReceive += OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll += OnEventMailReceiveAll;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMailSelected -= OnEventMailSelected;
        CsGameEventUIToUI.Instance.EventMailReceive -= OnEventMailReceive;
        CsGameEventUIToUI.Instance.EventMailReceiveAll -= OnEventMailReceiveAll;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventMailSelected(Guid guidMail)
    {
        if (Guid.Empty == guidMail)
        {
            m_trMailInfo.gameObject.SetActive(false);
        }
        else
        {
            m_trMailInfo.gameObject.SetActive(true);
            UpdateMailInfo(guidMail);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceive(Guid guidMail)
    {
        UpdateMailInfo(guidMail);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMailReceiveAll(Guid[] aguidMails)
    {
        List<Guid> listGuidMail = new List<Guid>(aguidMails);
        
        if (listGuidMail.Find(a => a == m_csMail.Id) == null)
        {
            return;
        }
        else
        {
            UpdateMailInfo(m_csMail.Id);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMailReceive()
    {
        int nCount = 0;
        bool bReceive = true;

        for (int i = 0; i < m_csMail.MailAttachmentList.Count; i++)
        {
            CsMailAttachment csMailAttachment = m_csMail.MailAttachmentList[i];

            if (csMailAttachment.Count <= CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(csMailAttachment.Item.ItemId, csMailAttachment.Owned))
            {
                continue;
            }
            else
            {
                if ((CsGameData.Instance.MyHeroInfo.InventorySlotList.Count + nCount) < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                {
                    nCount++;
                }
                else
                {
                    bReceive = false;
                    break;
                }
            }
        }

        if (bReceive)
        {
            CsCommandEventManager.Instance.SendMailReceive(m_csMail.Id);
        }
        else
        {
            //실패 토스트 출력
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A03_TXT_02001"));
        }

        //if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count < CsGameData.Instance.MyHeroInfo.InventorySlotCount)
        //{
        //    // 한자리라도 남을때
        //    CsCommandEventManager.Instance.SendMailReceive(m_csMail.Id);
        //    return;
        //}
        //else
        //{
        //    int nRemainingItemCount = CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csMail.AttachmentItem.ItemId, m_csMail.AttachmentItemOwned);

        //    if (m_csMail.AttachmentItemCount <= nRemainingItemCount)
        //    {
        //        CsCommandEventManager.Instance.SendMailReceive(m_csMail.Id);
        //        return;
        //    }
        //}

        ////실패 토스트 출력
        //CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A03_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMailDelete()
    {
        CsCommandEventManager.Instance.SendMailDelete(m_csMail.Id);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trMailInfo = transform.Find("MailInfo");

        m_buttonMailReceive = m_trMailInfo.Find("ButtonReceive").GetComponent<Button>();
        m_buttonMailReceive.onClick.RemoveAllListeners();
        m_buttonMailReceive.onClick.AddListener(OnClickMailReceive);
        m_buttonMailReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMailReceive = m_buttonMailReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonMailReceive);
        textButtonMailReceive.text = CsConfiguration.Instance.GetString("A03_BTN_00005");

        m_buttonMailDelete = m_trMailInfo.Find("ButtonDelete").GetComponent<Button>();
        m_buttonMailDelete.onClick.RemoveAllListeners();
        m_buttonMailDelete.onClick.AddListener(OnClickMailDelete);
        m_buttonMailDelete.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMailDelete = m_buttonMailDelete.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonMailDelete);
        textButtonMailDelete.text = CsConfiguration.Instance.GetString("A03_BTN_00004");

        if (CsGameData.Instance.MyHeroInfo.MailList.Count > 0)
        {
            UpdateMailInfo(CsGameData.Instance.MyHeroInfo.MailList[0].Id);
        }
        else
        {
            m_trMailInfo.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMailInfo(Guid guidMail)
    {
        m_csMail = CsGameData.Instance.MyHeroInfo.GetMail(guidMail);

        // Title
        Text textTitle = m_trMailInfo.Find("TextMailName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTitle);
        textTitle.text = m_csMail.Title;

        //Content 
        Text textDesc = m_trMailInfo.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDesc);
        textDesc.text = m_csMail.Content;

        Transform trScrollView = m_trMailInfo.Find("Scroll View");
        Transform trContentItemList = trScrollView.Find("Viewport/Content");
        Transform trItemSlot = null;
        
        // 아이템 슬롯 초기화
        for (int i = 0; i < trContentItemList.childCount; i++)
        {
            trContentItemList.GetChild(i).gameObject.SetActive(false);
        }

        if (m_csMail.Received)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMailDelete, true);
            CsUIData.Instance.DisplayButtonInteractable(m_buttonMailReceive, false);

            trScrollView.gameObject.SetActive(false);
        }
        else
        {
            if (m_csMail.MailAttachmentList.Count > 0)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonMailDelete, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonMailReceive, true);

                // 아이템 슬롯 표시
                for (int i = 0; i < m_csMail.MailAttachmentList.Count; i++)
                {
                    trItemSlot = trContentItemList.Find("ItemSlot" + i);

                    if (trItemSlot == null)
                    {
                        trItemSlot = Instantiate(m_goItemSlot, trContentItemList).transform;
                        trItemSlot.name = "ItemSlot" + i;
                    }

                    CsMailAttachment csMailAttachment = m_csMail.MailAttachmentList[i];
                    CsUIData.Instance.DisplayItemSlot(trItemSlot, csMailAttachment.Item, csMailAttachment.Owned, csMailAttachment.Count, csMailAttachment.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
                    trItemSlot.gameObject.SetActive(true);
                }

                trScrollView.gameObject.SetActive(true);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonMailDelete, true);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonMailReceive, false);

                trScrollView.gameObject.SetActive(false);
            }
        }
    }
}
