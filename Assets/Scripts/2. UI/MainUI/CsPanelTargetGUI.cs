using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-02-26)
//---------------------------------------------------------------------------------------------------

public class CsPanelTargetGUI : MonoBehaviour
{
    Transform m_trButtonTargetGUI;
    Transform m_trMonsterTargetGUI;

    Sprite m_spriteMonster;
    Sprite m_job1;
    Sprite m_job2;
    Sprite m_job3;
    Sprite m_job4;
    Sprite m_job5;
    Sprite m_job6;
    Sprite m_job7;
    Sprite m_job8;
    Sprite m_job9;
    Sprite m_job10;
    Sprite m_job11;
    Sprite m_job12;

    Text m_textLevelInfo;
    Text m_textSlier;

    Button m_buttonHeroInfo;

    CsHeroBase m_csHeroBase = null;
    IMonsterObjectInfo m_iMonsterObjectInfo = null;
    ICartObjectInfo m_iCartObjectInfo = null;

    bool m_bIsFirst = true;
	int m_nMonsterHpLineCount = 1;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trButtonTargetGUI = transform.Find("ButtonTargetGUI");
        m_trMonsterTargetGUI = transform.Find("ImageMonsterTargetGUI");

        CsGameEventToUI.Instance.EventSelectHeroInfo += OnEventSelectHeroInfo;
        CsGameEventToUI.Instance.EventSelectHeroInfoStop += OnEventSelectHeroInfoStop;

        CsGameEventToUI.Instance.EventSelectMonsterInfo += OnEventSelectMonsterInfo;
        CsGameEventToUI.Instance.EventSelectMonsterInfoStop += OnEventSelectMonsterInfoStop;

        CsGameEventToUI.Instance.EventSelectCartInfo += OnEventSelectCartInfo;
        CsGameEventToUI.Instance.EventSelectCartInfoStop += OnEventSelectCartInfoStop;
        
        CsGameEventToUI.Instance.EventSelectInfoUpdate += OnEventSelectInfoUpdate;
        CsGameEventToUI.Instance.EventCreateBossMonster += OnEventCreateBossMonster;
        CsDungeonManager.Instance.EventStoryDungeonClear += OnEventStoryDungeonClear;

        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventToUI.Instance.EventSelectHeroInfo -= OnEventSelectHeroInfo;
        CsGameEventToUI.Instance.EventSelectHeroInfoStop -= OnEventSelectHeroInfoStop;

        CsGameEventToUI.Instance.EventSelectMonsterInfo -= OnEventSelectMonsterInfo;
        CsGameEventToUI.Instance.EventSelectMonsterInfoStop -= OnEventSelectMonsterInfoStop;

        CsGameEventToUI.Instance.EventSelectCartInfo -= OnEventSelectCartInfo;
        CsGameEventToUI.Instance.EventSelectCartInfoStop -= OnEventSelectCartInfoStop;

        CsGameEventToUI.Instance.EventSelectInfoUpdate -= OnEventSelectInfoUpdate;
        CsGameEventToUI.Instance.EventCreateBossMonster -= OnEventCreateBossMonster;
        CsDungeonManager.Instance.EventStoryDungeonClear -= OnEventStoryDungeonClear;

        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTargetGUI()
    {
        //영웅이면 영웅정보조회 팝업 출력
        if (m_csHeroBase != null)
        {
            CsGameEventUIToUI.Instance.OnEventOpenUserReference(m_csHeroBase);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectHeroInfo(Guid guid)
    {
        IHeroObjectInfo iHeroObjectInfo = CsGameData.Instance.ListHeroObjectInfo.Find(a => a.GetHeroId() == guid);
        m_csHeroBase = iHeroObjectInfo.GetHeroBase();
        UpdateTargetGUI(m_csHeroBase.Job.JobId, m_csHeroBase.Level, m_csHeroBase.Name, m_csHeroBase.Hp, m_csHeroBase.MaxHp);
        m_trButtonTargetGUI.gameObject.SetActive(true);

        m_buttonHeroInfo.transition = Selectable.Transition.ColorTint;
        m_buttonHeroInfo.interactable = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectHeroInfoStop()
    {
        m_csHeroBase = null;
        m_trButtonTargetGUI.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectMonsterInfo(long lMonsterId, int nHpLineCount)
    {
        m_iMonsterObjectInfo = CsGameData.Instance.ListMonsterObjectInfo.Find(a => a.GetInstanceId() == lMonsterId);

        if (m_iMonsterObjectInfo == null)
        {
            return;
        }
        else
        {
            CsMonsterInfo csMonsterInfo = m_iMonsterObjectInfo.GetMonsterInfo();

			m_nMonsterHpLineCount = nHpLineCount;

            if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon)
            {
                // 스토리 던전일 때
                if (csMonsterInfo.TamingEnabled)
                {
                    UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp(), m_iMonsterObjectInfo.GetMentalStrength(), m_iMonsterObjectInfo.GetMaxMentalStrength());
                    m_trMonsterTargetGUI.gameObject.SetActive(true);
                }
            }
            else
            {
                if (csMonsterInfo.TamingEnabled)
                {
                    UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp(), m_iMonsterObjectInfo.GetMentalStrength(), m_iMonsterObjectInfo.GetMaxMentalStrength());
                }
                else
                {
                    UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp());
                }

                m_trMonsterTargetGUI.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectMonsterInfoStop()
    {
        if (m_iMonsterObjectInfo == null)
        {
            return;
        }
        else
        {
            CsMonsterInfo csMonsterInfo = m_iMonsterObjectInfo.GetMonsterInfo();

            if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon && csMonsterInfo != null && !csMonsterInfo.TamingEnabled)
            {
                return;
            }
            else
            {
				m_nMonsterHpLineCount = 1;

                m_iMonsterObjectInfo = null;
                m_trMonsterTargetGUI.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectCartInfo(long lCartInstanceId)
    {
        m_iCartObjectInfo = CsGameData.Instance.ListCartObjectInfo.Find(a => a.GetInstanceId() == lCartInstanceId);

        if (m_iCartObjectInfo == null)
        {
            return;
        }
        else
        {
            CsCartObject csCartObject = m_iCartObjectInfo.GetCartObject();
            string strCartName = string.Format(CsGameData.Instance.GetCart(csCartObject.Cart.CartId).Name, csCartObject.OwnerName, csCartObject.Cart.CartGrade.Name);

            UpdateTargetGUI(0, csCartObject.Level, strCartName, csCartObject.Hp, csCartObject.MaxHp, true);
        }

        m_buttonHeroInfo.transition = Selectable.Transition.None;
        m_buttonHeroInfo.interactable = false;

        m_trButtonTargetGUI.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectCartInfoStop()
    {
        m_iCartObjectInfo = null;
        m_trButtonTargetGUI.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSelectInfoUpdate()
    {
        if (m_csHeroBase != null)
        {
            UpdateLevelHp(m_csHeroBase.Level, m_csHeroBase.Name, m_csHeroBase.Hp, m_csHeroBase.MaxHp);
        }
        else if (m_iCartObjectInfo != null)
        {
            CsCartObject csCartObject = m_iCartObjectInfo.GetCartObject();
            string strCartName = string.Format(CsGameData.Instance.GetCart(csCartObject.Cart.CartId).Name, csCartObject.OwnerName, csCartObject.Cart.CartGrade.Name);

            UpdateLevelHp(0, strCartName, csCartObject.Hp, csCartObject.MaxHp);
        }
        else
        {
            if (m_iMonsterObjectInfo != null)
            {
                CsMonsterInfo csMonsterInfo = m_iMonsterObjectInfo.GetMonsterInfo();

                if (csMonsterInfo.TamingEnabled)
                {
                    // 테이밍 몬스터 업데이트
                    UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp(), m_iMonsterObjectInfo.GetMentalStrength(), m_iMonsterObjectInfo.GetMaxMentalStrength());
                }
                else
                {
                    UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp());
                }
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
	void OnEventCreateBossMonster(long lInstanceId, CsMonsterInfo csMonsterInfo, int nHpLineCount)
    {
        m_iMonsterObjectInfo = CsGameData.Instance.ListMonsterObjectInfo.Find(a => a.GetInstanceId() == lInstanceId);

        if (m_iMonsterObjectInfo == null)
        {
            return;
        }
        else
        {
			m_nMonsterHpLineCount = nHpLineCount;

            UpdateMonsterTargetGUI(csMonsterInfo.Level, csMonsterInfo.Name, m_iMonsterObjectInfo.GetHp(), m_iMonsterObjectInfo.GetMaxHp());
            m_trMonsterTargetGUI.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonClear(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        Debug.Log("OnEventStoryDungeonClear");
		m_nMonsterHpLineCount = 1;
        m_trMonsterTargetGUI.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        if (m_csHeroBase != null && m_csHeroBase.HeroId == guidHeroId)
        {
            UpdateTargetGUI(m_csHeroBase.Job.JobId, m_csHeroBase.Level, m_csHeroBase.Name, m_csHeroBase.Hp, m_csHeroBase.MaxHp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_buttonHeroInfo = m_trButtonTargetGUI.GetComponent<Button>();
        m_buttonHeroInfo.onClick.RemoveAllListeners();
        m_buttonHeroInfo.onClick.AddListener(OnClickTargetGUI);
        m_buttonHeroInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonHeroInfo.transition = Selectable.Transition.None;

        m_textLevelInfo = m_trButtonTargetGUI.Find("TextLevelName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevelInfo);

        m_textSlier = m_trButtonTargetGUI.Find("Slider/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSlier);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTargetGUI(int nJobId, int Level, string strName, int nHp, int nMaxHp, bool bCart = false)
    {
        if (m_bIsFirst)
        {
            InitializeUI();
            m_bIsFirst = false;
        }

        Image imageJob = m_trButtonTargetGUI.Find("ImageJob").GetComponent<Image>();
        RectTransform rtrimageJob = imageJob.GetComponent<RectTransform>();

        /*
        if (nJobId == 0)
        {
            if (m_spriteMonster == null)
            {
                m_spriteMonster = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_monster");
            }

            imageJob.sprite = m_spriteMonster;
            rtrimageJob.sizeDelta = new Vector2(32, 40);
        }
        */

        if (bCart)
        {
            rtrimageJob.sizeDelta = new Vector2(64, 64);

            imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_cart03");
        }
        else
        {
            rtrimageJob.sizeDelta = new Vector2(56, 56);

            switch (nJobId)
            {
                case 1:
                    if (m_job1 == null)
                    {
                        m_job1 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_1");
                    }

                    imageJob.sprite = m_job1;
                    break;

                case 2:
                    if (m_job2 == null)
                    {
                        m_job2 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_2");
                    }

                    imageJob.sprite = m_job2;
                    break;

                case 3:
                    if (m_job3 == null)
                    {
                        m_job3 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_3");
                    }

                    imageJob.sprite = m_job3;
                    break;

                case 4:
                    if (m_job4 == null)
                    {
                        m_job4 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_4");
                    }

                    imageJob.sprite = m_job4;
                    break;

                case 5:
                    if (m_job5 == null)
                    {
                        m_job5 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_5");
                    }

                    imageJob.sprite = m_job5;
                    break;

                case 6:
                    if (m_job6 == null)
                    {
                        m_job6 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_6");
                    }

                    imageJob.sprite = m_job6;
                    break;

                case 7:
                    if (m_job7 == null)
                    {
                        m_job7 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_7");
                    }

                    imageJob.sprite = m_job7;
                    break;

                case 8:
                    if (m_job8 == null)
                    {
                        m_job8 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_8");
                    }

                    imageJob.sprite = m_job8;
                    break;

                case 9:
                    if (m_job9 == null)
                    {
                        m_job9 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_9");
                    }

                    imageJob.sprite = m_job9;
                    break;

                case 10:
                    if (m_job10 == null)
                    {
                        m_job10 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_10");
                    }

                    imageJob.sprite = m_job10;
                    break;

                case 11:
                    if (m_job11 == null)
                    {
                        m_job11 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_11");
                    }

                    imageJob.sprite = m_job11;
                    break;

                case 12:
                    if (m_job12 == null)
                    {
                        m_job12 = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_12");
                    }

                    imageJob.sprite = m_job12;
                    break;
            }
        }

        UpdateLevelHp(Level, strName, nHp, nMaxHp);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLevelHp(int Level, string strName, int nHp, int nMaxHp)
    {
        m_textLevelInfo.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), Level, strName);
        m_textSlier.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_PER"), (((float)nHp / nMaxHp) * 100).ToString("0"));

        Slider slider = m_trButtonTargetGUI.Find("Slider").GetComponent<Slider>();
        slider.maxValue = nMaxHp;
        slider.value = nHp;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMonsterTargetGUI(int nLevel, string strName, int nHp, int nMaxHp)
    {
		float flHpPerLine = (float)nMaxHp / m_nMonsterHpLineCount;
		int nCurrentLineNo = (int)(nHp / flHpPerLine);
		float flRemainingHp = nHp % flHpPerLine;

		if (flRemainingHp > 0)
		{
			nCurrentLineNo += 1;
		}
		else if (nCurrentLineNo > 0)
		{
			flRemainingHp = flHpPerLine;
		}

		Text textLevelName = m_trMonsterTargetGUI.Find("TextLevelName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLevelName);
        textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), nLevel, strName);

        Slider sliderHp = m_trMonsterTargetGUI.Find("SliderHp").GetComponent<Slider>();
        sliderHp.maxValue = flHpPerLine;
		sliderHp.value = flRemainingHp;

		if (nCurrentLineNo > 0)
		{
			RectTransform rtrFill = m_trMonsterTargetGUI.Find("SliderHp/Fill Area/Fill" + nCurrentLineNo.ToString()).GetComponent<RectTransform>();

			if (sliderHp.fillRect != rtrFill)
			{
				sliderHp.fillRect.gameObject.SetActive(false);
				sliderHp.fillRect = rtrFill;
				sliderHp.fillRect.gameObject.SetActive(true);
			}
		}

		Transform trFrameNextHp = m_trMonsterTargetGUI.Find("SliderHp/FrameNextHp");
		trFrameNextHp.gameObject.SetActive(nCurrentLineNo > 1);

		if (nCurrentLineNo > 1)
		{
			for (int i = 0; i < trFrameNextHp.childCount; i++)
			{
				trFrameNextHp.GetChild(i).gameObject.SetActive(false);
			}

			trFrameNextHp.Find("ImageNextHp" + (nCurrentLineNo - 1).ToString()).gameObject.SetActive(true);
		}

        Text textHp = sliderHp.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textHp);
		textHp.text = string.Format(CsConfiguration.Instance.GetString("A165_TXT_00001"), nCurrentLineNo);
		//textHp.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_PER"), (((float)nHp / nMaxHp) * 100).ToString("0"));

        Transform trSliderMental = m_trMonsterTargetGUI.Find("SliderMental");
        trSliderMental.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
	void UpdateMonsterTargetGUI(int nLevel, string strName, int nHp, int nMaxHp, int nMentalStrength, int nMaxMentalStrength)
    {
		UpdateMonsterTargetGUI(nLevel, strName, nHp, nMaxHp);

        Slider sliderMental = m_trMonsterTargetGUI.Find("SliderMental").GetComponent<Slider>();
        sliderMental.maxValue = nMaxMentalStrength;
        sliderMental.value = nMentalStrength;

        Text textMentalStrength = sliderMental.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMentalStrength);
        textMentalStrength.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nMentalStrength, nMaxMentalStrength);

        sliderMental.gameObject.SetActive(true);
    }
}
