using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2017-04-11)
//---------------------------------------------------------------------------------------------------

public class CsPanelNationWar : MonoBehaviour
{
    GameObject m_goPopupNationWarResult;
    GameObject m_goPopupNationWarRanking;
    GameObject m_goNationWarGuide;

    Transform m_trPopupNationWarResult;
    Transform m_trPopupNationWarRanking;
    Transform m_trNationWarGuide;
    Transform m_trPopupList;
    Transform m_trToast;

    Image m_imageNationWarMonster;

    Queue<string> m_queueStrMessage = new Queue<string>();
    IEnumerator m_iEnumeratorNationWarGuide;

    int m_nCount;

    enum EnNationWarMonsterType
    {
        Commander = 1,
        Wizard = 2,
        Angel = 3,
        Dragon = 4,
        Rock = 5,
    }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsNationWarManager.Instance.EventNationWarConvergingAttack += OnEventNationWarConvergingAttack;
        CsNationWarManager.Instance.EventMyNationWarConvergingAttack += OnEventMyNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttackFinished += OnEventNationWarConvergingAttackFinished;
        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;

        CsNationWarManager.Instance.EventMoveToNationWarMonster += OnEventMoveToNationWarMonster;
        CsNationWarManager.Instance.EventNationTransmission += OnEventNationTransmission;

        CsNationWarManager.Instance.EventNationWarFinished += OnEventNationWarFinished;

        //
        CsNationWarManager.Instance.EventNationWarMonsterEmergency += OnEventNationWarMonsterEmergency;
        CsNationWarManager.Instance.EventNationWarMonsterDead += OnEventNationWarMonsterDead;

        CsNationWarManager.Instance.EventNationWarWin += OnEventNationWarWin;
        CsNationWarManager.Instance.EventNationWarLose += OnEventNationWarLose;

        CsGameEventUIToUI.Instance.EventOpenPopupNationWarResult += OnEventOpenPopupNationWarResult;

        CsNationWarManager.Instance.EventNationWarMultiKill += OnEventNationWarMultiKill;
        CsNationWarManager.Instance.EventNationWarNoblesseKill += OnEventNationWarNoblesseKill;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsNationWarManager.Instance.EventMyNationWarConvergingAttack -= OnEventMyNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttack -= OnEventNationWarConvergingAttack;
        CsNationWarManager.Instance.EventNationWarConvergingAttackFinished -= OnEventNationWarConvergingAttackFinished;
        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;

        CsNationWarManager.Instance.EventMoveToNationWarMonster -= OnEventMoveToNationWarMonster;
        CsNationWarManager.Instance.EventNationTransmission -= OnEventNationTransmission;

        CsNationWarManager.Instance.EventNationWarFinished -= OnEventNationWarFinished;
        
        //
        CsNationWarManager.Instance.EventNationWarMonsterEmergency -= OnEventNationWarMonsterEmergency;
        CsNationWarManager.Instance.EventNationWarMonsterDead -= OnEventNationWarMonsterDead;

        CsNationWarManager.Instance.EventNationWarWin -= OnEventNationWarWin;
        CsNationWarManager.Instance.EventNationWarLose -= OnEventNationWarLose;

        CsGameEventUIToUI.Instance.EventOpenPopupNationWarResult -= OnEventOpenPopupNationWarResult;

        CsNationWarManager.Instance.EventNationWarMultiKill -= OnEventNationWarMultiKill;
        CsNationWarManager.Instance.EventNationWarNoblesseKill -= OnEventNationWarNoblesseKill;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventMyNationWarConvergingAttack()
    {
        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);

        CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(CsGameData.Instance.NationWar.GetNationWarMonsterArrange(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId).MonsterArrangeId).MonsterId);
        m_queueStrMessage.Enqueue(string.Format(CsConfiguration.Instance.GetString("A70_TXT_04018"), csMonsterInfo.Name));

        if (m_iEnumeratorNationWarGuide != null)
        {
            return;
        }
        else
        {
            m_iEnumeratorNationWarGuide = NationWarGuide();
            StartCoroutine(m_iEnumeratorNationWarGuide);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarConvergingAttack(int nMonsterArrangeId)
    {
        UpdateNationWarConvergingAttack(nMonsterArrangeId);

        CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(CsGameData.Instance.NationWar.GetNationWarMonsterArrange(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId).MonsterArrangeId).MonsterId);
        m_queueStrMessage.Enqueue(string.Format(CsConfiguration.Instance.GetString("A70_TXT_04018"), csMonsterInfo.Name));
        
        if (m_iEnumeratorNationWarGuide != null)
        {
            return;
        }
        else
        {
            m_iEnumeratorNationWarGuide = NationWarGuide();
            StartCoroutine(m_iEnumeratorNationWarGuide);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarConvergingAttackFinished()
    {
        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarConvergingAttack()
    {
        CsNationWarManager.Instance.MoveToNationWarMonster(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);
        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.NationWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMoveToNationWarMonster(int nNationId, int nArrangeId, Vector3 vtPos)
    {
        CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.NationWar);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationTransmission()
    {
        if (CsNationWarManager.Instance.MyNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

            if (csNpcInfo == null)
                return;

            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
            {
                // 국가 이동 실패
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
            }
            else
            {
                CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarFinished(System.Guid guidDeclarationId, int nWinNationId)
    {
        UpdateNationWarConvergingAttack();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMonsterEmergency(int nArrangeId)
    {
        CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nArrangeId);

        if (csNationWarMonsterArrange == null)
        {
            return;
        }
        else
        {
            CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(csNationWarMonsterArrange.MonsterArrangeId).MonsterId);
            m_queueStrMessage.Enqueue(string.Format(CsConfiguration.Instance.GetString("A70_TXT_04001"), csMonsterInfo.Name));
        
            if (m_iEnumeratorNationWarGuide != null)
            {
                return;
            }
            else
            {
                m_iEnumeratorNationWarGuide = NationWarGuide();
                StartCoroutine(m_iEnumeratorNationWarGuide);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMonsterDead(int nArrangeId)
    {
        CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nArrangeId);
        CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(csNationWarMonsterArrange.MonsterArrangeId).MonsterId);

        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.MyNationWarDeclaration;

        if (csNationWarMonsterArrange == null || csNationWarDeclaration == null || csMonsterInfo == null)
        {
            return;
        }
        else
        {
            string strMessage = "";

            switch (csNationWarMonsterArrange.Type)
            {
                case (int)EnNationWarMonsterType.Commander:
                    return;

                case (int)EnNationWarMonsterType.Wizard:
                    // 공격 수비측 위자드 점령
                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04024"), csMonsterInfo.Name);
                    }
                    else
                    {
                        strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04011"), csMonsterInfo.Name);
                    }

                    m_queueStrMessage.Enqueue(strMessage);

                    // 점령 후 데이터 확인
                    int nCount = 0;

                    for (int i = 0; i < CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance.Count; i++)
                    {
                        if (CsGameData.Instance.NationWar.GetNationWarMonsterArrange(CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].monsterArrangeId).Type == (int)EnNationWarMonsterType.Wizard &&
                            CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].nationId == CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId)
                        {
                            nCount++;
                        }
                    }

                    // 공격 수비측 위자드 점령
                    if (nCount == 2)
                    {
                        // 총사령관의 무적이 복구되었습니다.
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04017");
                    }
                    else if (nCount == 0)
                    {
                        if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                        {
                            // 위자드를 모두 처치하여 얼라이언스 부활 지점을 점령하였습니다.
                            strMessage = CsConfiguration.Instance.GetString("A70_TXT_04012");
                        }
                        else
                        {
                            // 위자드를 모두 처치당해 얼라이언스 부활 지점을 점령당했습니다.
                            strMessage = CsConfiguration.Instance.GetString("A70_TXT_04013");
                        }
                    }
                    else
                    {
                        // 공격측 >> 수비측
                        if (m_nCount == 0)
                        {
                            // 얼라이언스 부활 지점이 복구되었습니다.
                            strMessage = CsConfiguration.Instance.GetString("A70_TXT_04014");
                        }
                        // 수비측 >> 공격측
                        else
                        {
                            if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                            {
                                // 총사령관의 무적이 해제되었습니다. 공격하세요
                                strMessage = CsConfiguration.Instance.GetString("A70_TXT_04015");
                            }
                            else
                            {
                                // 총사령관의 무적이 해제되었습니다. 보호하세요
                                strMessage = CsConfiguration.Instance.GetString("A70_TXT_04016");
                            }
                        }
                    }

                    m_nCount = nCount;
                    m_queueStrMessage.Enqueue(strMessage);

                    break;
                case (int)EnNationWarMonsterType.Angel:
                    if (csNationWarMonsterArrange.ArrangeId == 4)
                    {
                        if (csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Count > 0)
                        {
                            CsNationWarRevivalPointActivationCondition csNationWarRevivalPointActivationCondition = null;
                            csNationWarRevivalPointActivationCondition = csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Find(a => a.ArrangeId == csNationWarMonsterArrange.ArrangeId);

                            if (csNationWarRevivalPointActivationCondition == null)
                            {
                                break;
                            }
                            else
                            {
                                if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                                {
                                    strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04026"), csMonsterInfo.Name, csNationWarRevivalPointActivationCondition.RevivalPoint.Name);
                                }
                                else
                                {
                                    strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04027"), csMonsterInfo.Name, csNationWarRevivalPointActivationCondition.RevivalPoint.Name);
                                }

                                m_queueStrMessage.Enqueue(strMessage);
                            }
                        }
                    }
                    else
                    {
                        if (csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Count > 0)
                        {
                            CsNationWarRevivalPointActivationCondition csNationWarRevivalPointActivationCondition = null;
                            csNationWarRevivalPointActivationCondition = csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Find(a => a.ArrangeId == csNationWarMonsterArrange.ArrangeId);

                            if (csNationWarRevivalPointActivationCondition == null)
                            {
                                break;
                            }
                            else
                            {
                                if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                                {
                                    strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04026"), csMonsterInfo.Name, csNationWarRevivalPointActivationCondition.RevivalPoint.Name);
                                }
                                else
                                {
                                    strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_04027"), csMonsterInfo.Name, csNationWarRevivalPointActivationCondition.RevivalPoint.Name);
                                }

                                m_queueStrMessage.Enqueue(strMessage);
                            }
                        }
                    }

                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04028");
                    }
                    else
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04029");
                    }

                    m_queueStrMessage.Enqueue(strMessage);

                    break;
                case (int)EnNationWarMonsterType.Dragon:
                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04004");
                    }
                    else
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04005");
                    }

                    m_queueStrMessage.Enqueue(strMessage);

                    break;
                case (int)EnNationWarMonsterType.Rock:
                    if (csNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04002");
                    }
                    else
                    {
                        strMessage = CsConfiguration.Instance.GetString("A70_TXT_04003");
                    }

                    m_queueStrMessage.Enqueue(strMessage);

                    break;
            }
        }

        if (m_iEnumeratorNationWarGuide != null)
        {
            return;
        }
        else
        {
            m_iEnumeratorNationWarGuide = NationWarGuide();
            StartCoroutine(m_iEnumeratorNationWarGuide);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarWin(ClientCommon.PDItemBooty[] joinBooties, long joinAcquiredExp, int joinAcquiredExploitPoint, int objectiveAchievementAcquiredExploitPoint, ClientCommon.PDItemBooty rankingBooty, ClientCommon.PDItemBooty luckyBooty, bool bLevelUp)
    {
        OpenPopupNationWarResult();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarLose(ClientCommon.PDItemBooty[] joinBooties, long joinAcquiredExp, int joinAcquiredExploitPoint, int objectiveAchievementAcquiredExploitPoint, ClientCommon.PDItemBooty rankingBooty, ClientCommon.PDItemBooty luckyBooty, bool bLevelUp)
    {
        OpenPopupNationWarResult();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupNationWarResult()
    {
        OpenPopupNationWarResult();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMultiKill(System.Guid guidHeroId, string strHeroName, int nationId, int nKillCount)
    {
        string strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_02002"), CsGameData.Instance.GetNation(nationId).Name, strHeroName, nKillCount);
        CsGameEventUIToUI.Instance.OnEventToastSystem(strMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarNoblesseKill(System.Guid guidKillerId, string strKillerName, int nKillerNationId, System.Guid guidDeadHeroId, string strDeadHeroName, int nDeadNationId, int nDeadNoblesseId)
    {
        Debug.Log("==>>OnEventNationWarNoblesseKill<<==");
        string strMessage = string.Format(CsConfiguration.Instance.GetString("A70_TXT_02003"), CsGameData.Instance.GetNation(nKillerNationId).Name, strKillerName, CsGameData.Instance.GetNation(nDeadNationId).Name, CsGameData.Instance.GetNationNoblesse(nDeadNoblesseId).Name, strDeadHeroName);
        CsGameEventUIToUI.Instance.OnEventToastSystem(strMessage);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_imageNationWarMonster = transform.Find("ImageNationWarMonster").GetComponent<Image>();

        Button buttonConvergingAttack = m_imageNationWarMonster.transform.Find("ButtonConvergingAttackIcon").GetComponent<Button>();
        buttonConvergingAttack.onClick.RemoveAllListeners();
        buttonConvergingAttack.onClick.AddListener(OnClickNationWarConvergingAttack);

        UpdateNationWarConvergingAttack(CsNationWarManager.Instance.NationWarConvergingAttackTargetArrangeId);

        if (CsNationWarManager.Instance.MyNationWarDeclaration == null)
        {
            m_nCount = 0;
        }
        else
        {
            for (int i = 0; i < CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance.Count; i++)
            {
                if (CsGameData.Instance.NationWar.GetNationWarMonsterArrange(CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].monsterArrangeId).Type == (int)EnNationWarMonsterType.Wizard &&
                    CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance[i].nationId == CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId)
                {
                    m_nCount++;
                }
            }
        }

        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");
        m_trToast = trCanvas2.Find("PanelToast");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarConvergingAttack(int nMonsterArrangeId = 0)
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration == null)
        {
            m_imageNationWarMonster.gameObject.SetActive(false);
            return;
        }
        else
        {
            CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
            CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nMonsterArrangeId);

            if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && csNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam &&
                csContinent != null && csContinent.IsNationWarTarget && csNationWarDeclaration.Status == EnNationWarDeclaration.Current && csNationWarMonsterArrange != null)
            {
                // 보여줌
                if (csNationWarMonsterArrange.Type == (int)EnNationWarMonsterType.Rock)
                {
                    m_imageNationWarMonster.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_7_on");
                }
                else
                {
                    m_imageNationWarMonster.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_world_war_position_" + csNationWarMonsterArrange.ArrangeId);
                }

                Text textTargetMonster = m_imageNationWarMonster.transform.Find("TextTargetMonster").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTargetMonster);
                textTargetMonster.text = CsGameData.Instance.GetMonsterInfo(CsGameData.Instance.GetMonsterArrange(csNationWarMonsterArrange.MonsterArrangeId).MonsterId).Name;

                Image imageNation = textTargetMonster.transform.Find("Image").GetComponent<Image>();
                int nMyHeroNationId = CsGameData.Instance.MyHeroInfo.Nation.NationId;

                if (csNationWarDeclaration.TargetNationId == nMyHeroNationId)
                {
                    imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_mini_world_war_defence");
                }
                else if (csNationWarDeclaration.NationId == nMyHeroNationId)
                {
                    imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/ico_mini_world_war_attack");
                }

                m_imageNationWarMonster.gameObject.SetActive(true);
            }
            else
            {
                // 안보여줌
                m_imageNationWarMonster.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator NationWarGuide()
    {
        OpenNationWarGuide(m_queueStrMessage.Dequeue());

        yield return new WaitForSeconds(3.0f);

        m_trNationWarGuide.gameObject.SetActive(false);

        if (m_queueStrMessage.Count > 0)
        {
            if (m_iEnumeratorNationWarGuide != null)
            {
                StopCoroutine(m_iEnumeratorNationWarGuide);
                m_iEnumeratorNationWarGuide = null;
            }

            m_iEnumeratorNationWarGuide = NationWarGuide();
            StartCoroutine(m_iEnumeratorNationWarGuide);
        }
        else
        {
            DestroyNationWarGuide();
            m_iEnumeratorNationWarGuide = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarGuide(string strMessage)
    {
        Text textNationWarGuide = m_trNationWarGuide.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationWarGuide);
        textNationWarGuide.text = strMessage;

        m_trNationWarGuide.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupNationWarResult()
    {
        if (CsNationWarManager.Instance.NationWarJoined)
        {
            if (m_goPopupNationWarResult == null)
            {
                StartCoroutine(LoadPopupNationWarResult());
            }
            else
            {
                UpdatePopupNationWarResult();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupNationWarResult()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNationWar/PopupNationWarResult");
        yield return resourceRequest;
        m_goPopupNationWarResult = (GameObject)resourceRequest.asset;

        UpdatePopupNationWarResult();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupNationWarResult()
    {
        m_trPopupNationWarResult = Instantiate(m_goPopupNationWarResult, m_trPopupList).transform;
        m_trPopupNationWarResult.name = "PopupNationwarResult";

        CsPopupNationWarResult csPopupNationWarResult = m_trPopupNationWarResult.GetComponent<CsPopupNationWarResult>();

        if (csPopupNationWarResult == null)
        {
            return;
        }
        else
        {
            csPopupNationWarResult.DisplayNationWarResult();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenNationWarGuide(string strMessage)
    {
        if (m_trNationWarGuide == null)
        {
            if (m_goNationWarGuide == null)
            {
                StartCoroutine(LoadNationWarGuide(strMessage));
            }
            else
            {
                CreateNationWarGuide(strMessage);
            }
        }
        else
        {
            UpdateNationWarGuide(strMessage);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadNationWarGuide(string strMessage)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNationWar/ImageNationWarGuide");
        yield return resourceRequest;
        m_goNationWarGuide = (GameObject)resourceRequest.asset;

        CreateNationWarGuide(strMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateNationWarGuide(string strMessage)
    {
        m_trNationWarGuide = Instantiate(m_goNationWarGuide, m_trToast).transform;
        m_trNationWarGuide.name = "NationWarGuide";
        m_trNationWarGuide.SetAsFirstSibling();

        UpdateNationWarGuide(strMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void DestroyNationWarGuide()
    {
        if (m_trNationWarGuide == null)
        {
            return;
        }
        else
        {
            Destroy(m_trNationWarGuide.gameObject);
            m_trNationWarGuide = null;
        }
    }
}