using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientCommon;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsPanelFade : MonoBehaviour
{
    Transform m_trFadeImage;
    Transform m_trSleepImage;
    
    CanvasGroup m_CanvasGroup;
    
    IEnumerator m_iEnumeratorFadeIn;
    IEnumerator m_iEnumeratorFadeOut;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
        //CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter += OnEventContinentExitForMainQuestDungeonEnter;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon += OnEventMainQuestDungeonAbandon;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;

        CsGameEventUIToUI.Instance.EventLoadingSliderComplete += OnEventLoadingSliderComplete;
        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;

        CsGameEventToUI.Instance.EventFade += OnEventFade;

        CsMainQuestDungeonManager.Instance.EventContinentExit += OnEventContinentExit;

        CsGameEventToUI.Instance.EventBossAppear += OnEventBossAppear;
        CsGameEventToIngame.Instance.EventSleepMode += OnEventSleepMode;

        CsGameEventUIToUI.Instance.EventSleepModeReset += OnEventSleepModeReset;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        //CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter -= OnEventContinentExitForMainQuestDungeonEnter;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon -= OnEventMainQuestDungeonAbandon;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;

        CsGameEventUIToUI.Instance.EventLoadingSliderComplete -= OnEventLoadingSliderComplete;
        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;

        CsGameEventToUI.Instance.EventFade -= OnEventFade;

        CsMainQuestDungeonManager.Instance.EventContinentExit -= OnEventContinentExit;

        CsGameEventToUI.Instance.EventBossAppear -= OnEventBossAppear;
        CsGameEventToIngame.Instance.EventSleepMode -= OnEventSleepMode;

        CsGameEventUIToUI.Instance.EventSleepModeReset -= OnEventSleepModeReset;

    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventSleepModeReset()
    {
        m_trSleepImage.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSleepMode(bool bIson)
    {
        m_trSleepImage.gameObject.SetActive(bIson);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBossAppear(string strName, bool bAppear)
    {
        if (bAppear)
        {
            StartFadeIn(0.5f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExit(UnityAction unityAction)
    {
        StartFadeOut(0.6f, unityAction);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFade(bool bFadeStart, bool bFast)
    {
        if (bFast)
		{
			if (bFadeStart)
			{
				m_CanvasGroup.alpha = 1;
				m_trFadeImage.gameObject.SetActive(true);
			}
			else
			{
				m_CanvasGroup.alpha = 0;
				m_trFadeImage.gameObject.SetActive(false);
			}
        }
        else
        {
            if (bFadeStart)
            {
                //어둡게
                StartFadeOut(0.6f);
            }
            else
            {
                Debug.Log("밝게");
                //밝게
                StartFadeIn(0.6f);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bChangeScene)
    {
        if (!bChangeScene)
        {
            StartFadeIn(0.6f, AreaToast);
        }

        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
        {
            CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventLoadingSliderComplete()
    {
        StartFadeIn(0.6f, AreaToast);
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonAbandon(int nContinentId, bool bChangeScene)
    {
        if (!bChangeScene)
        {
            StartFadeOut(0.6f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonBanished(int nContinentId, bool bChangeScene)
    {
        if (!bChangeScene)
        {
            StartFadeOut(0.6f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nContinentId, bool bChangeScene)
    {
        if (!bChangeScene)
        {
            StartFadeOut(0.6f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //void OnEventContinentExitForMainQuestDungeonEnter(bool bChangeScene)
    //{
    //    if (!bChangeScene)
    //    {
    //        StartFadeOut(0.6f);
    //    }
    //}

    #endregion 

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trFadeImage = transform.Find("Image");
        m_CanvasGroup = transform.Find("Image").GetComponent<CanvasGroup>();
        m_trSleepImage = transform.Find("ImageSleep");

        Text textSleep = m_trSleepImage.Find("Text").GetComponent<Text>();
        textSleep.text = CsConfiguration.Instance.GetString("A04_TXT_00006");
        CsUIData.Instance.SetFont(textSleep);

        m_trFadeImage.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void StartFadeIn(float flDuration, UnityAction unityAction = null)
    {
        if (m_iEnumeratorFadeIn != null)
        {
            StopCoroutine(m_iEnumeratorFadeIn);
            m_iEnumeratorFadeIn = null;
        }

        if (m_iEnumeratorFadeOut != null)
        {
            StopCoroutine(m_iEnumeratorFadeOut);
            m_iEnumeratorFadeOut = null;
        }

        m_trFadeImage.gameObject.SetActive(true);
        m_CanvasGroup.alpha = 1;

        m_iEnumeratorFadeIn = FadeInCoroutine(flDuration, unityAction);
        StartCoroutine(m_iEnumeratorFadeIn);
    }

    //---------------------------------------------------------------------------------------------------
    void StartFadeOut(float flDuration)
    {
        if (m_iEnumeratorFadeOut != null)
        {
            StopCoroutine(m_iEnumeratorFadeOut);
            m_iEnumeratorFadeOut = null;
        }

        if (m_iEnumeratorFadeIn != null)
        {
            StopCoroutine(m_iEnumeratorFadeIn);
            m_iEnumeratorFadeIn = null;
        }

        m_trFadeImage.gameObject.SetActive(true);

        m_iEnumeratorFadeOut = FadeOutCoroutine(flDuration);
        StartCoroutine(m_iEnumeratorFadeOut);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeInCoroutine(float flDuration)
    {
        //서서히 밝게 한다.

        yield return new WaitUntil(() => m_CanvasGroup.alpha == 1);

        yield return new WaitForSeconds(0.5f);

        for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
        {
            m_CanvasGroup.alpha = 1 - (fl / flDuration);
            yield return null;
        }

        m_trFadeImage.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeOutCoroutine(float flDuration)
    {
        //서서히 어둡게 한다
        for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
        {
            m_CanvasGroup.alpha = fl / flDuration;
            yield return null;
        }

        m_CanvasGroup.alpha = 1;
    }

    //---------------------------------------------------------------------------------------------------
    void StartFadeOut(float flDuration, UnityAction unityAction)
    {
        if (m_iEnumeratorFadeOut != null)
        {
            StopCoroutine(m_iEnumeratorFadeOut);
            m_iEnumeratorFadeOut = null;
        }

        if (m_iEnumeratorFadeIn != null)
        {
            StopCoroutine(m_iEnumeratorFadeIn);
            m_iEnumeratorFadeIn = null;
        }

        m_trFadeImage.gameObject.SetActive(true);

        m_iEnumeratorFadeOut = FadeOutCoroutine(flDuration, unityAction);
        StartCoroutine(m_iEnumeratorFadeOut);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeOutCoroutine(float flDuration, UnityAction unityAction)
    {
        //서서히 어둡게 한다
        for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
        {
            m_CanvasGroup.alpha = fl / flDuration;
            yield return null;
        }

        m_CanvasGroup.alpha = 1;

        if (unityAction != null)
        {
            unityAction();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeInCoroutine(float flDuration, UnityAction unityAction)
    {
        //서서히 밝게 한다.
        yield return new WaitUntil(() => m_CanvasGroup.alpha == 1);

        yield return new WaitForSeconds(0.5f);

        for (float fl = 0; fl <= flDuration; fl += Time.deltaTime)
        {
            m_CanvasGroup.alpha = 1 - (fl / flDuration);
            yield return null;
        }

        m_trFadeImage.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventHeroSkillReset();

        if (unityAction != null)
        {
            unityAction();
        }
    }

    //지역 토스트
    void AreaToast()
    {
        string strName;

        switch (CsDungeonManager.Instance.DungeonPlay)
        {
            case EnDungeonPlay.None:

                CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);

                if (csContinent != null)
                {
                    CsNation csNation = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam);

                    if (csNation != null)
                    {
                        if (CsGameData.Instance.MyHeroInfo.Nation == csNation)
                        {
                            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, csNation.Name);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, csNation.Name, false);
                        }
                    }
                    else
                    {
                        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == 0)
                        {
                            CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaContinent, csContinent.Name, "");
                        }
                    }
                }

                break;

            case EnDungeonPlay.MainQuest:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsMainQuestDungeonManager.Instance.MainQuestDungeon.Name, CsMainQuestDungeonManager.Instance.MainQuestDungeon.Description);

                break;

            case EnDungeonPlay.Story:

                if (CsDungeonManager.Instance.StoryDungeon == null)
                {
                    return;
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsDungeonManager.Instance.StoryDungeon.Name, CsDungeonManager.Instance.StoryDungeon.SubName);
                }

                break;

            case EnDungeonPlay.Exp:

                CsExpDungeonDifficulty csExpDungeonDifficulty = CsDungeonManager.Instance.ExpDungeon.GetExpDungeonDifficulty(CsDungeonManager.Instance.ExpDungeonDifficultyWave.Difficulty);
                strName = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_DUNLV"), CsDungeonManager.Instance.ExpDungeon.Name, csExpDungeonDifficulty.RequiredHeroLevel);
                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A32_TXT_00007"), strName);

                break;

            case EnDungeonPlay.Gold:
                
                strName = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_DUNLV"), CsDungeonManager.Instance.GoldDungeon.Name, CsDungeonManager.Instance.GoldDungeonDifficulty.RequiredHeroLevel);
                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A39_TXT_00004"), strName);

                break;

            case EnDungeonPlay.UndergroundMaze:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A43_TXT_00003"), CsDungeonManager.Instance.UndergroundMazeFloor.Name);

                break;

            case EnDungeonPlay.ArtifactRoom:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A47_TXT_03001"), CsDungeonManager.Instance.ArtifactRoom.Name);

                break;

            case EnDungeonPlay.AncientRelic:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A40_TXT_00009"), CsDungeonManager.Instance.AncientRelic.Name);

                break;

            case EnDungeonPlay.FieldOfHonor:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A31_TXT_00013"), CsDungeonManager.Instance.FieldOfHonor.Name);

                break;

            case EnDungeonPlay.SoulCoveter:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A74_TXT_00002"), CsDungeonManager.Instance.SoulCoveter.Name);

                break;

            case EnDungeonPlay.Elite:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A87_TXT_00001"), CsDungeonManager.Instance.EliteDungeon.Name);

                break;

            case EnDungeonPlay.ProofOfValor:
                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A89_TXT_03001"), CsDungeonManager.Instance.ProofOfValor.Name);
                break;

            case EnDungeonPlay.WisdomTemple:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A105_TXT_00005"), CsDungeonManager.Instance.RuinsReclaim.Name);

                break;

            case EnDungeonPlay.RuinsReclaim:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A110_TXT_00010"), CsDungeonManager.Instance.RuinsReclaim.Name);

                break;

            case EnDungeonPlay.InfiniteWar:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A112_TXT_00003"), CsDungeonManager.Instance.InfiniteWar.Name);

                break;

            case EnDungeonPlay.FearAltar:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsDungeonManager.Instance.FearAltarStage.Name, CsDungeonManager.Instance.FearAltar.Name);

                break;

            case EnDungeonPlay.WarMemory:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A121_TXT_00006"), CsDungeonManager.Instance.WarMemory.Name);

                break;

            case EnDungeonPlay.OsirisRoom:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A39_TXT_00004"), CsDungeonManager.Instance.OsirisRoom.Name);

                break;

            case EnDungeonPlay.Biography:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsDungeonManager.Instance.BiographyQuestDungeon.Name, CsDungeonManager.Instance.BiographyQuestDungeon.Description);

                break;

            case EnDungeonPlay.DragonNest:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A144_TXT_00016"), CsDungeonManager.Instance.DragonNest.Name);

                break;

            case EnDungeonPlay.AnkouTomb:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A162_TXT_00006"), CsDungeonManager.Instance.AnkouTomb.Name);

                break;

            case EnDungeonPlay.TradeShip:

                CsGameEventUIToUI.Instance.OnEventToastChangeArea(EnToastType.ChangeAreaDungeon, CsConfiguration.Instance.GetString("A164_TXT_00802"), CsDungeonManager.Instance.TradeShip.Name);

                break;
        }
    }
}
