using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CsPopupBattleSetting : CsPopupSub
{
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSetting(bool bIson, EnPlayerPrefsKey enPlayerPrefsKey, int nValue, Text text)
    {
        if (bIson)
        {
            CsConfiguration.Instance.SetPlayerPrefsKey(enPlayerPrefsKey, nValue);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSkill(bool bIson, EnPlayerPrefsKey enPlayerPrefsKey)
    {
        if (bIson)
        {
            CsConfiguration.Instance.SetPlayerPrefsKey(enPlayerPrefsKey, 1);
        }
        else
        {
            CsConfiguration.Instance.SetPlayerPrefsKey(enPlayerPrefsKey, 0);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedItemSet(bool bIson, int nGrade, Text text)
    {
        if (bIson)
        {
            if (CsGameData.Instance.MyHeroInfo.LootingItemMinGrade != nGrade)
            {
                CsCommandEventManager.Instance.SendBattleSettingSet(nGrade);
            }

            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trContent = transform.Find("Scroll View/Viewport/Content");

        //전투 범위
        Transform trCombatRange = trContent.Find("CombatRange");
        Transform trCombatRangeList = trCombatRange.Find("ToggleList");

        Text textCombatRange = trCombatRange.Find("TextName").GetComponent<Text>();
        textCombatRange.text = CsConfiguration.Instance.GetString("A18_TXT_00001");
        CsUIData.Instance.SetFont(textCombatRange);

        for (int i = 0; i < 3; ++i)
        {
            int nValue = i;
            Toggle toggleCombat = trCombatRangeList.Find("Toggle" + i).GetComponent<Toggle>();
            Text textToggle = toggleCombat.transform.Find("TextToggle").GetComponent<Text>();

            toggleCombat.onValueChanged.RemoveAllListeners();
            if(CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.CombatRange) == i)
            {
                toggleCombat.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }

            toggleCombat.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.CombatRange, nValue, textToggle));
            textToggle.text = CsConfiguration.Instance.GetString("A18_BTN_0000" + (i + 1));
            CsUIData.Instance.SetFont(textToggle);
        }

        //자동 물약
        Transform trAutoPortion = trContent.Find("AutoPortion");
        Transform trAutoPortionList = trAutoPortion.Find("ToggleList");

        Text textAutoPortion = trAutoPortion.Find("TextName").GetComponent<Text>();
        textAutoPortion.text = CsConfiguration.Instance.GetString("A18_TXT_00002");
        CsUIData.Instance.SetFont(textAutoPortion);

        Toggle togglePortionOn = trAutoPortionList.Find("Toggle0").GetComponent<Toggle>();
        Text textToggleOn = togglePortionOn.transform.Find("TextToggle").GetComponent<Text>();
        togglePortionOn.onValueChanged.RemoveAllListeners();

        Toggle togglePortionOff = trAutoPortionList.Find("Toggle1").GetComponent<Toggle>();
        Text textToggleOff = togglePortionOff.transform.Find("TextToggle").GetComponent<Text>();
        togglePortionOff.onValueChanged.RemoveAllListeners();

        if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoPortion) == 1)
        {
            togglePortionOn.isOn = true;
            textToggleOn.color = CsUIData.Instance.ColorWhite;
            textToggleOff.color = CsUIData.Instance.ColorGray;
        }
        else
        {
            togglePortionOff.isOn = true;
            textToggleOn.color = CsUIData.Instance.ColorGray;
            textToggleOff.color = CsUIData.Instance.ColorWhite;
        }

        togglePortionOn.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.AutoPortion, 1, textToggleOn));
        togglePortionOff.onValueChanged.AddListener((ison) => OnValueChangedSetting(ison, EnPlayerPrefsKey.AutoPortion, 0, textToggleOff));

        textToggleOn.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ON");
        CsUIData.Instance.SetFont(textToggleOn);

        textToggleOff.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_OFF");
        CsUIData.Instance.SetFont(textToggleOff);

        //아이템 습득
        Transform trItemGet = trContent.Find("ItemGet");
        Transform trItemGetList = trItemGet.Find("ToggleList");

        Text textItemGet = trItemGet.Find("TextName").GetComponent<Text>();
        textItemGet.text = CsConfiguration.Instance.GetString("A18_TXT_00003");
        CsUIData.Instance.SetFont(textItemGet);

        for (int i = 0; i < 5; ++i)
        {
            int nItemGrade = i + 1;

            Toggle toggleItem = trItemGetList.Find("Toggle" + i).GetComponent<Toggle>();
            Text textToggle = toggleItem.transform.Find("TextToggle").GetComponent<Text>();
            toggleItem.onValueChanged.RemoveAllListeners();

            if (nItemGrade == CsGameData.Instance.MyHeroInfo.LootingItemMinGrade)
            {
                toggleItem.isOn = true;
                textToggle.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                textToggle.color = CsUIData.Instance.ColorGray;
            }

            toggleItem.onValueChanged.AddListener((ison) => OnValueChangedItemSet(ison, nItemGrade, textToggle));
            textToggle.text = CsConfiguration.Instance.GetString("IGRADE_NAME_" + (i + 1));
            CsUIData.Instance.SetFont(textToggle);
        }

        //자동 스킬
        Transform trAutoSkill = trContent.Find("AutoSkill");
        Transform trAutoSkillList = trAutoSkill.Find("ToggleList");
        List<CsHeroSkill> listHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList;

        Text textAutoSkill = trAutoSkill.Find("TextName").GetComponent<Text>();
        textAutoSkill.text = CsConfiguration.Instance.GetString("A18_TXT_00004");
        CsUIData.Instance.SetFont(textAutoSkill);

        for (int i = 0; i < listHeroSkill.Count; ++i)
        {
            CsHeroSkill csHeroSkill = listHeroSkill[i];

            if (csHeroSkill.JobSkill.SkillId == 1) continue;

            Toggle toggleSkill = trAutoSkillList.Find("Toggle" + csHeroSkill.JobSkill.SkillId).GetComponent<Toggle>();
            toggleSkill.onValueChanged.RemoveAllListeners();

            EnPlayerPrefsKey enPlayerPrefsKey = (EnPlayerPrefsKey)System.Enum.Parse(typeof(EnPlayerPrefsKey), "AutoSkill" + csHeroSkill.JobSkill.SkillId);

            if (CsConfiguration.Instance.GetSettingKey(enPlayerPrefsKey) == 1)
                toggleSkill.isOn = true;
            else
                toggleSkill.isOn = false;

            toggleSkill.onValueChanged.AddListener((ison) => OnValueChangedSkill(ison, enPlayerPrefsKey));
            toggleSkill.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));



            Image imageToggle = toggleSkill.transform.Find("imageSkill").GetComponent<Image>();
            imageToggle.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + csHeroSkill.JobSkill.JobId + "_" + csHeroSkill.JobSkill.SkillId);
        }
    }
}
