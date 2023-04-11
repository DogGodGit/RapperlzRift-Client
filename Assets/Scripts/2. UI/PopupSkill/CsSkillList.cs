using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-07)
//---------------------------------------------------------------------------------------------------

public class CsSkillList : CsPopupSub
{
    Transform m_trSkillList;
    int m_nSelectedSkillId;

    float m_flTime = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSkillLevelUp += OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally += OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventExpPotionUse += OnEventExpPotionUse;
        CsGameEventUIToUI.Instance.EventExpAcquisition += OnEventExpAcquisition;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {
            UpdateNotice();

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventSkillLevelUp -= OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally -= OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventExpPotionUse -= OnEventExpPotionUse;
        CsGameEventUIToUI.Instance.EventExpAcquisition -= OnEventExpAcquisition;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcquisition(long lExp, bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateSkillAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        if (bLevelUp)
        {
            UpdateSkillAll();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpPotionUse(bool bLevelUp, long lAcquiredExp)
    {
        if (bLevelUp)
        {
            UpdateSkillAll();
        }
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUp(int nSkillId)
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.SkillLevelup);
        UpdateSkillSlot(nSkillId);
        UpdateNotice();
        CsGameEventUIToUI.Instance.OnEventSkillSelected(nSkillId);
        UpdateIsLevelUpSkillAutoSelected();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUpTotally(int nSkillId)
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.SkillLevelup);
        UpdateSkillSlot(nSkillId);
        UpdateNotice();
        CsGameEventUIToUI.Instance.OnEventSkillSelected(nSkillId);
        UpdateIsLevelUpSkillAutoSelected();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSkill(Toggle toggle, int nSkillId)
    {
        m_nSelectedSkillId = nSkillId;

        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGameEventUIToUI.Instance.OnEventSkillSelected(nSkillId);
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trSkillList = transform.Find("SkillList");

        m_nSelectedSkillId = 1;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];

            if (csHeroSkill.IsLevelUp)
            {
                m_nSelectedSkillId = csHeroSkill.JobSkill.SkillId;
                break;
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            int nSlotIndex = csHeroSkill.JobSkill.SkillId;

            Toggle toggleSkillSlot = m_trSkillList.Find("ToggleSkillList" + nSlotIndex).GetComponent<Toggle>();
            toggleSkillSlot.onValueChanged.RemoveAllListeners();

            if (nSlotIndex == m_nSelectedSkillId)
            {
                toggleSkillSlot.isOn = true;
                m_nSelectedSkillId = csHeroSkill.JobSkill.SkillId;
            }

            toggleSkillSlot.onValueChanged.AddListener((ison) => OnValueChangedSkill(toggleSkillSlot, csHeroSkill.JobSkill.SkillId));

            UpdateSkillSlot(csHeroSkill.JobSkill.SkillId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSkillAll()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            UpdateSkillSlot(CsGameData.Instance.MyHeroInfo.HeroSkillList[i].JobSkill.SkillId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSkillSlot(int nSkillId)
    {
        CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(nSkillId);

        Transform trSkillSlot = m_trSkillList.Find("ToggleSkillList" + csHeroSkill.JobSkill.SkillId);

        //스킬아이콘
        Image imageSkillIcon = trSkillSlot.Find("ImageSkillIcon").GetComponent<Image>();

        imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.JobId + "_" + csHeroSkill.JobSkill.SkillId);

        Transform trSkillIconFrame = trSkillSlot.Find("ImageSkillIconFrame");

        if (nSkillId == 1)
        {
            trSkillIconFrame.gameObject.SetActive(false);
        }
        else
        {
            trSkillIconFrame.gameObject.SetActive(true);
        }

        //스킬레벨
        Text textLevel = trSkillSlot.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLevel);
        textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroSkill.JobSkillLevel.Level);

        //스킬이름
        Text textName = trSkillSlot.Find("TextSkillName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csHeroSkill.JobSkill.Name;

        //스킬오픈조건이 나오면 사용할 잠금표시.
        Transform trLock = trSkillSlot.Find("ImageLcok");

        Text textLock = trLock.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLock);

        //레벨업 가능시 표시되는 레드닷
        Transform trNotice = trSkillSlot.Find("ImageNotice");
        trNotice.gameObject.SetActive(csHeroSkill.IsLevelUp);

        CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest(csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo);

        if (CsMainQuestManager.Instance.MainQuest == null)
        {
            //열림
            trLock.gameObject.SetActive(false);
        }
        else
        {
            if (csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
            {
                //열림
                trLock.gameObject.SetActive(false);
            }
            else
            {
                //잠금
                trLock.gameObject.SetActive(true);
                textLock.text = string.Format(CsConfiguration.Instance.GetString("A14_TXT_01003"), csMainQuest.Title);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNotice()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            Transform trSkillSlot = m_trSkillList.Find("ToggleSkillList" + csHeroSkill.JobSkill.SkillId);

            Transform trNotice = trSkillSlot.Find("ImageNotice");

            if (trNotice.gameObject.activeSelf != csHeroSkill.IsLevelUp)
            {
                trNotice.gameObject.SetActive(csHeroSkill.IsLevelUp);

                if (m_nSelectedSkillId == csHeroSkill.JobSkill.SkillId)
                {
                    CsGameEventUIToUI.Instance.OnEventSkillSelected(csHeroSkill.JobSkill.SkillId);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateIsLevelUpSkillAutoSelected()
    {
        CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(m_nSelectedSkillId);

        if (csHeroSkill == null)
        {
            return;
        }
        else
        {
            if (csHeroSkill.IsLevelUp)
            {
                return;
            }
            else
            {
                for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
                {
                    csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
                    Toggle toggleSkillSlot = m_trSkillList.Find("ToggleSkillList" + csHeroSkill.JobSkill.SkillId).GetComponent<Toggle>();

                    if (csHeroSkill != null)
                    {
                        if (csHeroSkill.IsLevelUp)
                        {
                            toggleSkillSlot.isOn = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
