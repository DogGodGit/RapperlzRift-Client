using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupJobChange : MonoBehaviour 
{
    Transform m_trCanvas2;
    Transform m_trJobChangeList;
    Transform m_trJobInfoFrame;

    Button m_buttonJobChange;

    Text m_textLimitLevel;

    CsJob m_csJob = null;

    Camera m_uiCamera;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopupClose();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChange()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJobChange()
    {
        CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.JobChangeRequiredItemId);

        if (csItem == null)
        {
            return;
        }
        else
        {
            string strMessage = string.Format(CsConfiguration.Instance.GetString("A154_TXT_00005"), csItem.Name, m_csJob.Name);
            CsGameEventUIToUI.Instance.OnEventConfirm(strMessage,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsJobChangeManager.Instance.SendHeroJobChange(m_csJob.JobId),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSelectJob(bool bIson, CsJob csJob, Transform trImageGlow, Text textJobName, Image imageJobIcon)
    {
        if (bIson)
        {
            m_csJob = csJob;

            trImageGlow.gameObject.SetActive(true);
            textJobName.color = CsUIData.Instance.ColorWhite;
            imageJobIcon.color = new Color32(255, 255, 255, 255);

            UpdateJobInfoFrame();
        }
        else
        {
            trImageGlow.gameObject.SetActive(false);
            textJobName.color = CsUIData.Instance.ColorGray;
            imageJobIcon.color = new Color32(255, 255, 255, 200);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;

        Transform trImageBackground = transform.Find("ImageBackground");
        m_uiCamera = trImageBackground.Find("3DCharacter/UIChar_Camera").GetComponent<Camera>();

        Transform trTopFrame = trImageBackground.Find("TopFrame");

        Text textPopupName = trTopFrame.Find("ImageLineLeft/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A154_TXT_00003");

        Button buttonClose = trTopFrame.Find("ImageLineRight/ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickPopupClose);

        Text textSelectJob = trImageBackground.Find("TextSelectJob").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSelectJob);
        textSelectJob.text = CsConfiguration.Instance.GetString("A154_TXT_00002");

        m_trJobChangeList = trImageBackground.Find("JobChangeList");
        
        UpdateJobChangeList();

        m_buttonJobChange = trImageBackground.Find("ButtonJobChange").GetComponent<Button>();
        m_buttonJobChange.onClick.RemoveAllListeners();
        m_buttonJobChange.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonJobChange.onClick.AddListener(OnClickJobChange);

        Text textJobChange = m_buttonJobChange.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textJobChange);
        textJobChange.text = CsConfiguration.Instance.GetString("A154_TXT_00003");

        Image imageIcon = m_buttonJobChange.transform.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + CsGameConfig.Instance.JobChangeRequiredItemId);

        m_textLimitLevel = trImageBackground.Find("TextLimitLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLimitLevel);
        m_textLimitLevel.text = string.Format(CsConfiguration.Instance.GetString("A154_TXT_00004"), CsGameConfig.Instance.JobChangeRequiredHeroLevel);

        UpdateButtonJobChange();

        m_trJobInfoFrame = trImageBackground.Find("JobInfoFrame");

        UpdateJobInfoFrame();

        LoadCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateJobChangeList()
    {
        int nMyHeroJobId = CsGameData.Instance.MyHeroInfo.JobId;
        List<CsJob> listCsJob = new List<CsJob>(CsGameData.Instance.JobList).FindAll(a => a.ParentJobId == nMyHeroJobId && a.JobId != nMyHeroJobId);

        Transform trJobChange = null;

        for (int i = 0; i < m_trJobChangeList.childCount; i++)
        {
            trJobChange = m_trJobChangeList.GetChild(i);

            if (i < listCsJob.Count)
            {
                CsJob csJob = listCsJob[i];

                Transform trImageGlow = trJobChange.Find("ImageGlow");

                Text textJobName = trJobChange.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textJobName);
                textJobName.text = csJob.Name;

                Image imageJobIcon = trJobChange.Find("ImageIcon").GetComponent<Image>();
                imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csJob.JobId);

                Toggle toggleJobChange = trJobChange.GetComponent<Toggle>();
                toggleJobChange.onValueChanged.RemoveAllListeners();

                if (i == 0)
                {
                    m_csJob = csJob;
                    toggleJobChange.isOn = true;
                }
                else
                {
                    toggleJobChange.isOn = false;

                    trImageGlow.gameObject.SetActive(false);
                    textJobName.color = CsUIData.Instance.ColorGray;
                    imageJobIcon.color = new Color32(255, 255, 255, 200);
                }

                toggleJobChange.onValueChanged.AddListener((ison) => OnValueChangedSelectJob(ison, csJob, trImageGlow, textJobName, imageJobIcon));

                trJobChange.gameObject.SetActive(true);
            }
            else
            {
                trJobChange.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonJobChange()
    {
        Text textItemCount = m_buttonJobChange.transform.Find("ImageIcon/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemCount);
        textItemCount.text = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.JobChangeRequiredItemId).ToString("#,##0");

        bool bInteractable = false;

        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
        {
            m_buttonJobChange.gameObject.SetActive(false);
            m_textLimitLevel.gameObject.SetActive(true);
        }
        else
        {
            if (0 < CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.JobChangeRequiredItemId))
            {
                bInteractable = true;
            }
            else
            {
                bInteractable = false;
            }

            m_textLimitLevel.gameObject.SetActive(false);
            m_buttonJobChange.gameObject.SetActive(true);
        }

        m_buttonJobChange.interactable = bInteractable;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateJobInfoFrame()
    {
        Image imageJobIcon = m_trJobInfoFrame.Find("ImageJobIcon").GetComponent<Image>();
        imageJobIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + m_csJob.JobId);

        Text textJobName = m_trJobInfoFrame.Find("TextJobName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textJobName);
        textJobName.text = m_csJob.Name;

        Text textJobInfo = m_trJobInfoFrame.Find("TextJobInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textJobInfo);
        textJobInfo.text = m_csJob.Description;

        Transform trImageJobChangeBack = m_trJobInfoFrame.Find("ImageJobChangeBack");

        Text textCurrentJobName = trImageJobChangeBack.Find("ImageCurrentJob/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCurrentJobName);
        textCurrentJobName.text = CsGameData.Instance.MyHeroInfo.Job.Name;

        Text textJobChangeName = trImageJobChangeBack.Find("ImageJobChange/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textJobChangeName);
        textJobChangeName.text = m_csJob.Name;

        Transform trCurrentJobAttrList = trImageJobChangeBack.Find("CurrentJobAttrList");
        Transform trCurrentJobAttr = null;
        
        Text textAttrName = null;
        Text textAttrValue = null;

        CsAttr csAttr = null;
        CsJobLevel csJobLevelCurrent = CsGameData.Instance.GetJob(CsGameData.Instance.MyHeroInfo.JobId).JobLevelList.Find(a => a.Level == CsGameConfig.Instance.JobChangeRequiredHeroLevel);

        for (int i = 0; i < trCurrentJobAttrList.childCount; i++)
        {
            csAttr = CsGameData.Instance.GetAttr(i + 1);

            if (csAttr == null)
            {
                continue;
            }
            else
            {
                trCurrentJobAttr = trCurrentJobAttrList.Find("CurrentJobAttr" + i);

                textAttrName = trCurrentJobAttr.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrName);
                textAttrName.text = csAttr.Name;

                textAttrValue = trCurrentJobAttr.Find("TextValue").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttrValue);

                if (csJobLevelCurrent == null)
                {
                    textAttrValue.text = "0";
                }
                else
                {
                    switch (csAttr.EnAttr)
                    {
                        case EnAttr.MaxHp:
                            textAttrValue.text = csJobLevelCurrent.MaxHp.ToString("#,##0");
                            break;
                        case EnAttr.PhysicalOffense:
                            textAttrValue.text = csJobLevelCurrent.PhysicalOffense.ToString("#,##0");
                            break;
                        case EnAttr.MagicalOffense:
                            textAttrValue.text = csJobLevelCurrent.MagicalOffense.ToString("#,##0");
                            break;
                        case EnAttr.PhysicalDefense:
                            textAttrValue.text = csJobLevelCurrent.PhysicalDefense.ToString("#,##0");
                            break;
                        case EnAttr.MagicalDefense:
                            textAttrValue.text = csJobLevelCurrent.MagicalDefense.ToString("#,##0");
                            break;
                    }
                }
            }
        }

        Transform trJobChangeAttrList = trImageJobChangeBack.Find("JobChangeAttrList");
        Text textJobChangeAttrValue = null;
        CsJobLevel csJobLevelChange = m_csJob.JobLevelList.Find(a => a.Level == CsGameConfig.Instance.JobChangeRequiredHeroLevel);

        for (int i = 0; i < trJobChangeAttrList.childCount; i++)
        {
            csAttr = CsGameData.Instance.GetAttr(i + 1);

            if (csAttr == null)
            {
                continue;
            }
            else
            {
                int nAttrValue = 0;

                textJobChangeAttrValue = trJobChangeAttrList.Find("TextAttr" + i).GetComponent<Text>();
                CsUIData.Instance.SetFont(textJobChangeAttrValue);

                if (csJobLevelChange == null)
                {
                    textJobChangeAttrValue.text = "+" + nAttrValue.ToString();
                }
                else
                {
                    switch (csAttr.EnAttr)
                    {
                        case EnAttr.MaxHp:
                            nAttrValue = csJobLevelChange.MaxHp - csJobLevelCurrent.MaxHp;
                            break;
                        case EnAttr.PhysicalOffense:
                            nAttrValue = csJobLevelChange.PhysicalOffense - csJobLevelCurrent.PhysicalOffense;
                            break;
                        case EnAttr.MagicalOffense:
                            nAttrValue = csJobLevelChange.MagicalOffense - csJobLevelCurrent.MagicalOffense;
                            break;
                        case EnAttr.PhysicalDefense:
                            nAttrValue = csJobLevelChange.PhysicalDefense - csJobLevelCurrent.PhysicalDefense;
                            break;
                        case EnAttr.MagicalDefense:
                            nAttrValue = csJobLevelChange.MagicalDefense - csJobLevelCurrent.MagicalDefense;
                            break;
                    }

                    textJobChangeAttrValue.text = "+" + nAttrValue.ToString();
                }
            }
        }

        Transform trChangeAttr = m_trJobInfoFrame.Find("ChangeAttr");

        Text textAttr = trChangeAttr.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttr);
        textAttr.text = CsConfiguration.Instance.GetString("A154_TXT_00001");

        Image imageBaseAttr = trChangeAttr.Find("ImageBaseAttr/Image").GetComponent<Image>();
        imageBaseAttr.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSkill/ico_skill_property0" + CsGameData.Instance.MyHeroInfo.Job.Elemental.ElementalId);

        Text textBaseAttr = trChangeAttr.Find("ImageBaseAttr/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBaseAttr);
        textBaseAttr.text = CsGameData.Instance.MyHeroInfo.Job.Elemental.Name;

        Image imageChangeAttr = trChangeAttr.Find("ImageChangeAttr/Image").GetComponent<Image>();
        imageChangeAttr.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSkill/ico_skill_property0" + m_csJob.Elemental.ElementalId);

        Text textChangeAttr = trChangeAttr.Find("ImageChangeAttr/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textChangeAttr);
        textChangeAttr.text = m_csJob.Elemental.Name;
    }

    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(gameObject);
    }

    #region 3DModel

    //---------------------------------------------------------------------------------------------------
    void LoadCharacterModel()		//캐릭터모델 동적로드함수
    {
		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;
		Transform trCharacterModel = transform.Find("ImageBackground/3DCharacter/Character" + nJobId);

        if (trCharacterModel == null)
        {
            StartCoroutine(LoadCharacterModelCoroutine());
        }
        else
        {
            trCharacterModel.gameObject.SetActive(true);
            UpdateCharacterModel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCharacterModelCoroutine()
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
        yield return resourceRequest;
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("ImageBackground/3DCharacter"));

        float flScale = 1 / m_trCanvas2.GetComponent<RectTransform>().localScale.x;

		switch (nJobId)
        {
            case (int)EnJob.Gaia:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Asura:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 185, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Deva:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 175, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Witch:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
        }

        goCharacter.GetComponent<CsUICharcterRotate>().UICamera = m_uiCamera;

		goCharacter.name = "Character" + nJobId;
        goCharacter.gameObject.SetActive(true);

        UpdateCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel()
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = transform.Find("ImageBackground/3DCharacter/Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
            CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

            csEquipment.MidChangeEquipments(csHeroCustomData, false);
            csEquipment.CreateWing(csHeroCustomData, null, true);
        }
    }

    #endregion 3DModel
}