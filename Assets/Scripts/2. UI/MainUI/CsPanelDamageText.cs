using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextFx;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-02-22)
//---------------------------------------------------------------------------------------------------

public enum EnDamageTextType
{
    None = 0,
    Blocked = 1,
    Critical = 2,
    Penetration = 3,
    Immortal = 4,
}

public class CsPanelDamageText : MonoBehaviour
{
    GameObject m_goUpText;
	GameObject m_goPlayer;	// 피격당했을 경우
	GameObject m_goEnemy;	// 공격했을 경우
	GameObject m_goGuard;
	GameObject m_goExp;
	GameObject m_goHp;
	
    Sprite m_spriteCritical;
    Sprite m_spriteBlocked;
    Sprite m_spritePenetration;
    Sprite m_spriteExp;

    Transform m_trImageCombo;
    Text m_textComboCount;

    IEnumerator m_iEnumeratorFadeOut;
    int m_nComboCount = 0; // 콤보 개수

	const int m_nFontSize = 42;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventToUI.Instance.EventCreatDamageText += OnEventCreatDamageText;

        CsGameEventUIToUI.Instance.EventHpPotionUse += OnEventHpPotionUse;
        CsDungeonManager.Instance.EventProofOfValorBuffBoxAcquire += OnEventProofOfValorBuffBoxAcquire;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxAcquire += OnEventInfiniteWarBuffBoxAcquire;
        CsGameEventUIToUI.Instance.EventExpAcuisitionText += OnEventExpAcuisitionText;

        CsGameEventToUI.Instance.EventHitCombo += OnEventHitCombo;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventToUI.Instance.EventCreatDamageText -= OnEventCreatDamageText;

        CsGameEventUIToUI.Instance.EventHpPotionUse -= OnEventHpPotionUse;
        CsDungeonManager.Instance.EventProofOfValorBuffBoxAcquire -= OnEventProofOfValorBuffBoxAcquire;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxAcquire -= OnEventInfiniteWarBuffBoxAcquire;
        CsGameEventUIToUI.Instance.EventExpAcuisitionText -= OnEventExpAcuisitionText;

        CsGameEventToUI.Instance.EventHitCombo -= OnEventHitCombo;
    }


    #region EventHandler

    //---------------------------------------------------------------------------------------------------
	// bMyHero가 true이면 피격당한 경우, false 이면 공격한 경우
    void OnEventCreatDamageText(EnDamageTextType enDamageTextType, int nValue, Vector2 vector2, bool bMyHero)
    {
        /*
        if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon)
        {
            CreateDungeonDamageText(enDamageTextType, vector2, nValue, bMyHero);
        }
        else
        {
            CreateDamageText(enDamageTextType, vector2, nValue, bMyHero);
        }
        */

        CreateDamageText(enDamageTextType, vector2, nValue, bMyHero);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHpPotionUse(int nRecoveryHp)
    {
        CreateHpUpText(nRecoveryHp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBuffBoxAcquire(int nRecoveryHp)
    {
        if (0 < nRecoveryHp)
        {
            CreateHpUpText(nRecoveryHp);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBuffBoxAcquire(int nRecoveryHp)
    {
        if (0 < nRecoveryHp)
        {
            CreateHpUpText(nRecoveryHp);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcuisitionText(long lAcquiredExp)
    {
        CreateExpUpText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHitCombo()
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon)
        {
            m_nComboCount++;
            m_textComboCount.text = m_nComboCount.ToString();

            StartFadeOut(0.3f);

            Animation anim = m_trImageCombo.GetComponent<Animation>();
            anim.Stop();
            anim.Play();
        }
        else
        {
            return;
        }
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
		m_goPlayer = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Player");
		m_goEnemy = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Enemy");
		m_goGuard = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Guard");
		m_goExp = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Exp");
		m_goHp = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Hp");

        m_trImageCombo = transform.Find("ImageCombo");

        Text textCombo = m_trImageCombo.Find("TextCombo").GetComponent<Text>();
        m_textComboCount = m_trImageCombo.Find("TextCount").GetComponent<Text>();

        CsUIData.Instance.SetDungeonDamageFont(textCombo);
        CsUIData.Instance.SetDungeonDamageFont(m_textComboCount);

        textCombo.text = CsConfiguration.Instance.GetString("A13_TXT_00007");
    }

    //---------------------------------------------------------------------------------------------------
	// bMyHero가 true이면 피격당한 경우, false 이면 공격한 경우
    void CreateDamageText(EnDamageTextType enDamageTextType, Vector2 vector2, int nValue, bool bMyHero)
    {
        if (enDamageTextType == EnDamageTextType.Immortal)
        {
            return;
        }

		/*
		if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyBattleText))
		{
			if (PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyBattleText) != 1)
			{
				return;
			}
		}
		*/

		#region old
		//if (m_goUpText == null)
		//{
		//    m_goUpText = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Up");
		//}

		//GameObject goText = Instantiate(m_goUpText, transform);
		//RectTransform rectTransform = goText.GetComponent<RectTransform>();
		//goText.name = "DamageText";

		//Text textDamage = goText.transform.Find("Text").GetComponent<Text>();
		//CsUIData.Instance.SetDamageFont(textDamage);
		//textDamage.text = nValue.ToString("#,##0");

		//Image imageDamageType = textDamage.transform.Find("ImageDamageType").GetComponent<Image>();

		//CsGradient selectGradient = textDamage.GetComponent<CsGradient>();

		//// 피격당했을 경우
		//if (bMyHero)
		//{
		//    Vector2 m_vtCanvasResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
		//    rectTransform.anchoredPosition = new Vector2((m_vtCanvasResolution.x / 2), ((m_vtCanvasResolution.y / 2) + (m_vtCanvasResolution.y / 10)));
		//    //rectTransform.anchoredPosition = new Vector2(960, 700); // 1920x1080  + 40

		//    switch (enDamageTextType)
		//    {
		//        case EnDamageTextType.None:
		//            imageDamageType.gameObject.SetActive(false);
		//            selectGradient.StartColor = new Color32(238, 116, 91, 255);
		//            selectGradient.EndColor = new Color32(197, 52, 25, 255);
		//            break;

		//        case EnDamageTextType.Blocked:
		//            imageDamageType.gameObject.SetActive(false);
		//            selectGradient.StartColor = new Color32(65, 135, 243, 255);
		//            selectGradient.EndColor = new Color32(15, 76, 171, 255);
		//            break;

		//        case EnDamageTextType.Critical:
		//            imageDamageType.gameObject.SetActive(false);
		//            selectGradient.StartColor = new Color32(199, 56, 29, 255);
		//            selectGradient.EndColor = new Color32(123, 38, 15, 255);
		//            break;

		//        case EnDamageTextType.Penetration:
		//            imageDamageType.gameObject.SetActive(false);
		//            selectGradient.StartColor = new Color32(235, 122, 31, 255);
		//            selectGradient.EndColor = new Color32(194, 90, 14, 255);
		//            break;

		//        case EnDamageTextType.Immortal:
		//            break;
		//    }
		//}
		//// 공격했을 경우
		//else
		//{
		//    rectTransform.anchoredPosition = vector2 / CsGameConfig.Instance.ScaleFactor;

		//    switch (enDamageTextType)
		//    {
		//        case EnDamageTextType.None:
		//            imageDamageType.gameObject.SetActive(false);
		//            selectGradient.StartColor = new Color32(234, 234, 234, 255);
		//            selectGradient.EndColor = new Color32(180, 180, 180, 255);
		//            break;

		//        case EnDamageTextType.Blocked:

		//            if (m_spriteBlocked == null)
		//            {
		//                m_spriteBlocked = CsUIData.Instance.LoadAssetNation<Sprite>("text_guard");
		//            }
		//            imageDamageType.sprite = m_spriteBlocked;
		//            imageDamageType.gameObject.SetActive(true);
		//            imageDamageType.SetNativeSize();

		//            selectGradient.StartColor = new Color32(132, 206, 240, 255);
		//            selectGradient.EndColor = new Color32(41, 116, 240, 255);
		//            break;

		//        case EnDamageTextType.Critical:

		//            if (m_spriteCritical == null)
		//            {
		//                m_spriteCritical = CsUIData.Instance.LoadAssetNation<Sprite>("text_critical");
		//            }
		//            imageDamageType.sprite = m_spriteCritical;
		//            imageDamageType.gameObject.SetActive(true);
		//            imageDamageType.SetNativeSize();

		//            selectGradient.StartColor = new Color32(255, 222, 91, 255);
		//            selectGradient.EndColor = new Color32(224, 61, 26, 255);
		//            break;

		//        case EnDamageTextType.Penetration:

		//            if (m_spritePenetration == null)
		//            {
		//                m_spritePenetration = CsUIData.Instance.LoadAssetNation<Sprite>("text_smash");
		//            }
		//            imageDamageType.sprite = m_spritePenetration;
		//            imageDamageType.gameObject.SetActive(true);
		//            imageDamageType.SetNativeSize();

		//            selectGradient.StartColor = new Color32(250, 239, 219, 255);
		//            selectGradient.EndColor = new Color32(246, 204, 91, 255);
		//            break;

		//        case EnDamageTextType.Immortal:
		//            break;
		//    }
		//}

		//Animation animation = textDamage.GetComponent<Animation>();
		//animation.Play();
		//Destroy(goText.gameObject, animation.GetClipCount());
		#endregion old

		if (m_goPlayer == null)
		{
			m_goPlayer = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Player");
		}

		if (m_goEnemy == null)
		{
			m_goEnemy = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Enemy");
		}

		if (m_goGuard == null)
		{
			m_goGuard = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Player");
		}

		Transform trDamageText = null;
		TextFxUGUI textFxUGUIDamage = null;
		TextFxUGUI textFxUGUISpecial = null;
		Animation animation = null;

		switch (enDamageTextType)
		{
			case EnDamageTextType.None:

				trDamageText = Instantiate(bMyHero ? m_goPlayer : m_goEnemy, transform).transform;

				animation = trDamageText.Find("Damage").GetComponent<Animation>();
				textFxUGUIDamage = trDamageText.Find("Damage/BasicDamage").GetComponent<TextFxUGUI>();
				
				break;
			case EnDamageTextType.Blocked:

				trDamageText = Instantiate(bMyHero ? m_goGuard : m_goEnemy, transform).transform;

				animation = trDamageText.Find("Damage").GetComponent<Animation>();

				if (bMyHero)
				{
					textFxUGUIDamage = trDamageText.Find("Damage/BasicDamage").GetComponent<TextFxUGUI>();

					textFxUGUISpecial = trDamageText.Find("Text/GuardText").GetComponent<TextFxUGUI>();
					textFxUGUISpecial.text = CsConfiguration.Instance.GetString("PUBLIC_DMG_GUARD");
				}
				else
				{
					
					textFxUGUIDamage = trDamageText.Find("Damage/BasicDamage").GetComponent<TextFxUGUI>();
				}

				break;
			case EnDamageTextType.Critical:

				trDamageText = Instantiate(bMyHero ? m_goPlayer : m_goEnemy, transform).transform;
				
				animation = trDamageText.Find("Damage").GetComponent<Animation>();

				if (bMyHero)
				{
					textFxUGUIDamage = trDamageText.Find("Damage/BasicDamage").GetComponent<TextFxUGUI>();
				}
				else
				{
					textFxUGUIDamage = trDamageText.Find("Damage/CriticalDamage").GetComponent<TextFxUGUI>();
				}
				
				textFxUGUISpecial = trDamageText.Find("Text/CriticalText").GetComponent<TextFxUGUI>();
				textFxUGUISpecial.text = CsConfiguration.Instance.GetString("PUBLIC_DMG_CRITICAL");

				break;
			case EnDamageTextType.Penetration:

				trDamageText = Instantiate(bMyHero ? m_goPlayer : m_goEnemy, transform).transform;

				animation = trDamageText.Find("Damage").GetComponent<Animation>();
				textFxUGUIDamage = trDamageText.Find("Damage/BasicDamage").GetComponent<TextFxUGUI>();

				textFxUGUISpecial = trDamageText.Find("Text/CriticalText").GetComponent<TextFxUGUI>();
				textFxUGUISpecial.text = CsConfiguration.Instance.GetString("PUBLIC_DMG_SMASH");

				break;
			default:
				break;
		}

		if (trDamageText != null)
		{
			RectTransform rectTransform = trDamageText.GetComponent<RectTransform>();
			trDamageText.name = "DamageText";

			if (bMyHero)
			{
				Vector2 m_vtCanvasResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
				rectTransform.anchoredPosition = new Vector2((m_vtCanvasResolution.x / 2), ((m_vtCanvasResolution.y / 2)));
			}
			else
			{
				rectTransform.anchoredPosition = vector2 / CsGameConfig.Instance.ScaleFactor;
			}
		}

		if (textFxUGUIDamage != null)
		{
			textFxUGUIDamage.text = nValue.ToString("#,##0");
			textFxUGUIDamage.fontSize = m_nFontSize;

			textFxUGUIDamage.gameObject.SetActive(true);
			textFxUGUIDamage.AnimationManager.PlayAnimation();
		}

		if (textFxUGUISpecial != null)
		{
			textFxUGUISpecial.fontSize = m_nFontSize;

			textFxUGUISpecial.gameObject.SetActive(true);
			textFxUGUISpecial.AnimationManager.PlayAnimation();
		}

		if (animation != null)
		{
			animation.Play();
		}

		if (trDamageText != null)
		{
			Destroy(trDamageText.gameObject, 2.5f);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void CreateDungeonDamageText(EnDamageTextType enDamageTextType, Vector2 vector2, int nValue, bool bMyHero)
    {
        if (enDamageTextType == EnDamageTextType.Immortal)
        {
            return;
        }

        if (m_goUpText == null)
        {
            m_goUpText = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Up");
        }

        GameObject goText = Instantiate(m_goUpText, transform);
        RectTransform rectTransform = goText.GetComponent<RectTransform>();
        goText.name = "DamageText";

        Transform trTextDamage = goText.transform.Find("Text");
        trTextDamage.gameObject.SetActive(false);

        Text textDungeonDamage = goText.transform.Find("TextDungeon").GetComponent<Text>();
        CsUIData.Instance.SetDungeonDamageFont(textDungeonDamage);
        textDungeonDamage.text = nValue.ToString();

        Image imageDamageType = textDungeonDamage.transform.Find("ImageDamageType").GetComponent<Image>();

        CsGradient selectGradient = textDungeonDamage.GetComponent<CsGradient>();
        Outline outlineDamageText = textDungeonDamage.GetComponent<Outline>();

        if (bMyHero)
        {
            Vector2 m_vtCanvasResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
            rectTransform.anchoredPosition = new Vector2((m_vtCanvasResolution.x / 2), ((m_vtCanvasResolution.y / 2) + (m_vtCanvasResolution.y / 10)));
            //rectTransform.anchoredPosition = new Vector2(960, 700); // 1920x1080  + 40

            switch (enDamageTextType)
            {
                case EnDamageTextType.None:
                    imageDamageType.gameObject.SetActive(false);

                    selectGradient.StartColor = new Color32(245, 74, 56, 255);
                    selectGradient.EndColor = new Color32(188, 44, 25, 255);

                    outlineDamageText.effectColor = new Color32(63, 31, 19, 255);
                    break;

                case EnDamageTextType.Blocked: 
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back3");

                    selectGradient.StartColor = new Color32(101, 163, 255, 255);
                    selectGradient.EndColor = new Color32(28, 98, 208, 255);

                    outlineDamageText.effectColor = new Color32(153, 50, 96, 255);
                    break;

                case EnDamageTextType.Critical:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back2");

                    selectGradient.StartColor = new Color32(236, 135, 42, 255);
                    selectGradient.EndColor = new Color32(230, 53, 13, 255);

                    textDungeonDamage.fontSize = 54;
                    outlineDamageText.effectColor = new Color32(97, 39, 17, 255);
                    break;

                case EnDamageTextType.Penetration:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back");

                    selectGradient.StartColor = new Color32(248, 135, 42, 255);
                    selectGradient.EndColor = new Color32(230, 53, 13, 255);

                    outlineDamageText.effectColor = new Color32(99, 64, 13, 255);
                    break;

                case EnDamageTextType.Immortal:
                    break;
            }
        }
        else
        {
            rectTransform.anchoredPosition = vector2 / CsGameConfig.Instance.ScaleFactor;

            switch (enDamageTextType)
            {
                case EnDamageTextType.None:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back");
                    
                    selectGradient.StartColor = new Color32(234, 234, 234, 255);
                    selectGradient.EndColor = new Color32(180, 180, 180, 255);

                    outlineDamageText.effectColor = new Color32(99, 64, 13, 255);
                    break;

                case EnDamageTextType.Blocked:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back3");
                    selectGradient.StartColor = new Color32(132, 206, 240, 255);
                    selectGradient.EndColor = new Color32(41, 116, 240, 255);

                    outlineDamageText.effectColor = new Color32(153, 50, 96, 255);
                    break;

                case EnDamageTextType.Critical:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back2");
                    
                    selectGradient.StartColor = new Color32(255, 222, 91, 255);
                    selectGradient.EndColor = new Color32(224, 61, 26, 255);

                    textDungeonDamage.fontSize = 54;
                    outlineDamageText.effectColor = new Color32(97, 39, 17, 255);
                    break;

                case EnDamageTextType.Penetration:
                    imageDamageType.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_damage_back");
                    
                    selectGradient.StartColor = new Color32(248, 197, 108, 255);
                    selectGradient.EndColor = new Color32(245, 118, 37, 255);

                    outlineDamageText.effectColor = new Color32(99, 64, 13, 255);
                    break;

                case EnDamageTextType.Immortal:
                    break;
            }
        }

        textDungeonDamage.gameObject.SetActive(true);

        Animation animation = textDungeonDamage.GetComponent<Animation>();
        animation.Play();
        Destroy(goText.gameObject, animation.GetClipCount());
    }

    //---------------------------------------------------------------------------------------------------
    void CreateHpUpText(int nValue)
    {
		/*
		if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyBattleText))
		{
			if (PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyBattleText) != 1)
			{
				return;
			}
		}
		*/

		#region old
		//if (m_goUpText == null)
		//{
		//    m_goUpText = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Up");
		//}


		//GameObject goText = Instantiate(m_goUpText, transform);
		//RectTransform rectTransform = goText.GetComponent<RectTransform>();
		//rectTransform.anchoredPosition = new Vector2(800, 360); // 1920x1080  + 40

		//goText.name = "DamageText";

		//Text textDamage = goText.transform.Find("Text").GetComponent<Text>();
		//CsUIData.Instance.SetDamageFont(textDamage);
		//textDamage.text = string.Format(CsConfiguration.Instance.GetString("INPUT_PLUS"), nValue.ToString("#,###"));

		//Image imageDamageType = textDamage.transform.Find("ImageDamageType").GetComponent<Image>();
		//imageDamageType.gameObject.SetActive(false);

		//CsGradient selectGradient = textDamage.GetComponent<CsGradient>();
		//selectGradient.StartColor = new Color32(104, 184, 21, 255);
		//selectGradient.EndColor = new Color32(18, 148, 0, 255);

		//Animation animation = textDamage.GetComponent<Animation>();
		//animation.Play();
		//Destroy(goText.gameObject, animation.GetClipCount());
		#endregion old

		if (m_goHp == null)
		{
			m_goHp = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Hp");
		}

		Transform trHpText = null;
		TextFxUGUI textFxUGUIHp = null;
		Animation animation = null;

		trHpText = Instantiate(m_goHp, transform).transform;
		trHpText.name = "HpText";

		textFxUGUIHp =  trHpText.Find("Hp").GetComponent<TextFxUGUI>();
		animation = trHpText.Find("Hp").GetComponent<Animation>();

		if (trHpText != null)
		{
			RectTransform rectTransform = trHpText.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(800, 360);
		}
	
		if (textFxUGUIHp != null)
		{
			textFxUGUIHp.gameObject.SetActive(true);
			textFxUGUIHp.text = nValue.ToString();
			textFxUGUIHp.fontSize = m_nFontSize;
			textFxUGUIHp.AnimationManager.PlayAnimation();
		}

		if (animation != null)
		{
			animation.Play();
		}

		if (trHpText == null)
		{
			Destroy(trHpText.gameObject, 2.5f);
		}
		else
		{
			if (animation != null)
			{
				Destroy(trHpText.gameObject, animation.GetClipCount());
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void CreateExpUpText(long nValue)
    {
		/*
		if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyBattleText))
		{
			if (PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyBattleText) != 1)
			{
				return;
			}
		}
		*/

		#region old
		//if (m_goUpText == null)
		//{
		//    m_goUpText = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Up");
		//}

		//GameObject goText = Instantiate(m_goUpText, transform);
		//RectTransform rectTransform = goText.GetComponent<RectTransform>();
		//rectTransform.anchoredPosition = new Vector2(800, 360); // 1920x1080  + 40

		//goText.name = "DamageText";

		//Text textDamage = goText.transform.Find("Text").GetComponent<Text>();
		//CsUIData.Instance.SetDamageFont(textDamage);
		//textDamage.text = string.Format(CsConfiguration.Instance.GetString("INPUT_PLUS"), nValue.ToString("#,###"));

		//Image imageDamageType = textDamage.transform.Find("ImageDamageType").GetComponent<Image>();

		//if (m_spriteExp == null)
		//{
		//    m_spriteExp = CsUIData.Instance.LoadAssetNation<Sprite>("text_exp");
		//}

		//imageDamageType.sprite = m_spriteExp;
		//imageDamageType.gameObject.SetActive(true);
		//imageDamageType.SetNativeSize();

		//CsGradient selectGradient = textDamage.GetComponent<CsGradient>();
		//selectGradient.StartColor = new Color32(22, 179, 195, 255);
		//selectGradient.EndColor = new Color32(13, 127, 138, 255);

		//Animation animation = textDamage.GetComponent<Animation>();
		//animation.Play();
		//Destroy(goText.gameObject, animation.GetClipCount());
		#endregion old

		if (m_goExp == null)
		{
			m_goExp = CsUIData.Instance.LoadAsset<GameObject>("GUI/Common/Exp");
		}

		Transform trExpText = null;
		TextFxUGUI textFxUGUIExp = null;
		Animation animation = null;

		trExpText = Instantiate(m_goExp, transform).transform;
		trExpText.name = "ExpText";

		textFxUGUIExp = trExpText.Find("Exp").GetComponent<TextFxUGUI>();
		animation = trExpText.Find("Exp").GetComponent<Animation>();

		if (trExpText != null)
		{
			RectTransform rectTransform = trExpText.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(900, 200);
		}

		if (textFxUGUIExp != null)
		{
			textFxUGUIExp.gameObject.SetActive(true);
			textFxUGUIExp.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_DMG_EXP"), nValue.ToString());
			textFxUGUIExp.fontSize = m_nFontSize;
			//textFxUGUIExp.AnimationManager.PlayAnimation();
		}

		if (animation != null)
		{
			animation.Play();
		}

		if (trExpText == null)
		{
			Destroy(trExpText.gameObject, 2.5f);
		}
		else
		{
			if (animation != null)
			{
				Destroy(trExpText.gameObject, animation.GetClipCount());
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void StartFadeOut(float flDuration)
    {
        if (m_iEnumeratorFadeOut != null)
        {
            StopCoroutine(m_iEnumeratorFadeOut);
            m_iEnumeratorFadeOut = null;
        }

        m_iEnumeratorFadeOut = FadeOutCoroutine(flDuration);
        StartCoroutine(m_iEnumeratorFadeOut);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeInCoroutine(float flDuration)
    {
        //서서히 밝게 한다.
        CanvasGroup canvasGroup = m_trImageCombo.GetComponent<CanvasGroup>();

        yield return new WaitUntil(() => canvasGroup.alpha == 1);
        //yield return new WaitForSeconds(0.5f);

        for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - (fl / flDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeOutCoroutine(float flDuration)
    {
        //서서히 어둡게 한다
        CanvasGroup canvasGroup = m_trImageCombo.GetComponent<CanvasGroup>();

        if (canvasGroup.alpha < 1)
        {
            for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
            {
                canvasGroup.alpha = fl / flDuration;
                yield return null;
            }

            canvasGroup.alpha = 1;
        }

        yield return new WaitForSeconds(CsDungeonManager.Instance.StoryDungeon.ComboDuration);
        m_nComboCount = 0;
        StartCoroutine(FadeInCoroutine(0.3f));
    }
}