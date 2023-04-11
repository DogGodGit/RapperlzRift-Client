using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationWarResult : MonoBehaviour
{
    [SerializeField] GameObject m_goNationWarRankingItem;

    Transform m_trNationWarResult;
    Transform m_trNationWarRanking;

    Button m_buttonClose;

    int m_nWinNationId;
    bool m_bFirst = true;

    enum EnNationWarHeroObjective
    {
        HeroObjectiveWin = 1,
        HeroObjectiveLose = 2,
        HeroObjectiveKill = 3,
        HeroObjectiveImmediateRevival = 4,
    }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsNationWarManager.Instance.EventNationWarResult += OnEventNationWarResult;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsNationWarManager.Instance.EventNationWarResult -= OnEventNationWarResult;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarResult(int nWinNationId, long lRewardedExp, ClientCommon.PDNationWarRanking[] nationWarRankingOffense, ClientCommon.PDNationWarRanking[] nationWarRankingDefense)
    {
        m_nWinNationId = nWinNationId;

        List<ClientCommon.PDNationWarRanking> listRankingOffense = new List<ClientCommon.PDNationWarRanking>(nationWarRankingOffense);
        List<ClientCommon.PDNationWarRanking> listRankingDefense = new List<ClientCommon.PDNationWarRanking>(nationWarRankingDefense);

        UpdateNationWarResult(lRewardedExp);
        UpdateNationWarRanking(listRankingOffense, listRankingDefense);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonClose()
    {
        if (m_bFirst)
        {
            m_trNationWarResult.gameObject.SetActive(false);
            m_trNationWarRanking.gameObject.SetActive(true);

            m_bFirst = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trNationWarResult = transform.Find("NationWarResult");

        m_trNationWarRanking = transform.Find("NationWarRanking");

        m_buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        m_buttonClose.onClick.RemoveAllListeners();
        m_buttonClose.onClick.AddListener(OnClickButtonClose);
        m_buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayNationWarResult()
    {
        m_bFirst = true;
        CsNationWarManager.Instance.SendNationWarResult();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarResult(long lRewardedExp)
    {
        Image imageNationWarResult = m_trNationWarResult.Find("ImageResult").GetComponent<Image>();
        UpdateImageNationWarResult(imageNationWarResult);
        imageNationWarResult.gameObject.SetActive(true);

        Transform trNationWarReward = m_trNationWarResult.Find("NationWarReward");

        Transform trNationWarRewardItem = trNationWarReward.Find("NationWarRewardItem");
        Transform trNationWarRewardAcquire = trNationWarReward.Find("NationWarRewardAcquire");
        Transform trNationWarRewardHeroObjective = trNationWarReward.Find("NationWarRewardHeroObjective");

        Text textNationWarRewardItem = trNationWarRewardItem.Find("Text").GetComponent<Text>();
        Text textNationWarRewardAcquire = trNationWarRewardAcquire.Find("Text").GetComponent<Text>();
        Text textNationWarRewardHeroObjective = trNationWarRewardHeroObjective.Find("Text").GetComponent<Text>();

        CsUIData.Instance.SetFont(textNationWarRewardItem);
        CsUIData.Instance.SetFont(textNationWarRewardAcquire);
        CsUIData.Instance.SetFont(textNationWarRewardHeroObjective);

        textNationWarRewardItem.text = CsConfiguration.Instance.GetString("A70_TXT_00009");
        textNationWarRewardAcquire.text = CsConfiguration.Instance.GetString("A70_TXT_00010");
        textNationWarRewardHeroObjective.text = CsConfiguration.Instance.GetString("A70_TXT_00011");

        for (int i = 0; i < 2; i++)
        {
            CsItemReward csItemReward = null;

            if (m_nWinNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                if (i == 0)
                {
                    csItemReward = CsGameData.Instance.NationWar.WinNationItemReward1;
                }
                else
                {
                    csItemReward = CsGameData.Instance.NationWar.WinNationItemReward2;
                }
            }
            else
            {
                if (i == 0)
                {
                    csItemReward = CsGameData.Instance.NationWar.LoseNationItemRewardId1;
                }
                else
                {
                    csItemReward = CsGameData.Instance.NationWar.LoseNationItemRewardId2;
                }
            }

            Transform trItemReward = trNationWarRewardItem.Find("ItemReward" + i);

            Image imageItem = trItemReward.Find("ImageItem").GetComponent<Image>();
            imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);

            Text textName = trItemReward.Find("Text").GetComponent<Text>();
            Text textValue = trItemReward.Find("TextValue").GetComponent<Text>();

            CsUIData.Instance.SetFont(textName);
            CsUIData.Instance.SetFont(textValue);

            textName.text = csItemReward.Item.Name;
            textValue.text = csItemReward.ItemCount.ToString("#,##0");
        }

        Transform trNationWarRewardExploit = trNationWarRewardAcquire.Find("NationWarRewardExploit");

        Text textExploit = trNationWarRewardExploit.Find("Text").GetComponent<Text>();
        Text textExploitValue = trNationWarRewardExploit.Find("Text").GetComponent<Text>();

        CsUIData.Instance.SetFont(textExploit);
        CsUIData.Instance.SetFont(textExploitValue);

        textExploit.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_CP");

        if (m_nWinNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
        {
            textExploitValue.text = CsGameData.Instance.NationWar.WinNationExploitPointReward.Value.ToString("#,##0");
        }
        else
        {
            textExploitValue.text = CsGameData.Instance.NationWar.LoseNationExploitPointReward.Value.ToString("#,##0");
        }

        Transform trNationWarRewardExp = trNationWarRewardAcquire.Find("NationWarRewardExp");

        Text textExp = trNationWarRewardExp.Find("Text").GetComponent<Text>();
        Text textExpValue = trNationWarRewardExp.Find("Text").GetComponent<Text>();

        CsUIData.Instance.SetFont(textExploit);
        CsUIData.Instance.SetFont(textExploitValue);

        textExp.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_EXP");
        textExpValue.text = lRewardedExp.ToString("#,##0");

        List<CsNationWarHeroObjectiveEntry> listNationWarHeroObjectiveEntry = new List<CsNationWarHeroObjectiveEntry>();

        Transform trHeroObjective = null;

        foreach (EnNationWarHeroObjective enumItem in System.Enum.GetValues(typeof(EnNationWarHeroObjective)))
        {
            CsNationWarHeroObjectiveEntry csNationWarHeroObjectiveEntry = null;

            switch (enumItem)
            {
                case EnNationWarHeroObjective.HeroObjectiveWin:
                    csNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.Find(a => a.Type == (int)enumItem);
                    break;
                case EnNationWarHeroObjective.HeroObjectiveLose:
                    csNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.Find(a => a.Type == (int)enumItem);
                    break;
                case EnNationWarHeroObjective.HeroObjectiveKill:
                    listNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.FindAll(a => a.Type == (int)enumItem);
                    break;
                case EnNationWarHeroObjective.HeroObjectiveImmediateRevival:
                    listNationWarHeroObjectiveEntry = CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.FindAll(a => a.Type == (int)enumItem);
                    break;
            }

            if (listNationWarHeroObjectiveEntry.Count > 0)
            {
                for (int i = listNationWarHeroObjectiveEntry.Count - 1; i >= 0; i--)
                {
                    if (CsNationWarManager.Instance.NationWarImmediateRevivalCount >= listNationWarHeroObjectiveEntry[i].ObjectiveCount)
                    {
                        csNationWarHeroObjectiveEntry = listNationWarHeroObjectiveEntry[i];
                        break;
                    }
                }
            }

            trHeroObjective = trNationWarRewardHeroObjective.Find("HeroObjectiveReward" + (int)enumItem);

            Image imageGoods = trHeroObjective.Find("ImageGoods").GetComponent<Image>();

            Text textHeroObjective = trHeroObjective.Find("TextHeroObjective").GetComponent<Text>();
            Text textGoodsName = trHeroObjective.Find("Text").GetComponent<Text>();
            Text textGoodsValue = trHeroObjective.Find("Text").GetComponent<Text>();

            CsUIData.Instance.SetFont(textHeroObjective);
            CsUIData.Instance.SetFont(textGoodsName);
            CsUIData.Instance.SetFont(textGoodsValue);

            if (csNationWarHeroObjectiveEntry == null)
            {
                trHeroObjective.gameObject.SetActive(false);
            }
            else
            {
                textHeroObjective.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_02004"), csNationWarHeroObjectiveEntry.Name);

                if (csNationWarHeroObjectiveEntry.RewardType == 1)
                {
                    imageGoods.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods06");

                    textGoodsName.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_CP");
                    textGoodsValue.text = csNationWarHeroObjectiveEntry.ExploitPointReward.Value.ToString("#,##0");
                }
                else
                {
                    imageGoods.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods02");

                    textGoodsName.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_DIA");
                    textGoodsValue.text = csNationWarHeroObjectiveEntry.OwnDiaReward.Value.ToString("#,##0");
                }

                if (m_nWinNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                {
                    if (enumItem == EnNationWarHeroObjective.HeroObjectiveLose)
                    {
                        trHeroObjective.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (enumItem == EnNationWarHeroObjective.HeroObjectiveWin)
                    {
                        trHeroObjective.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarRanking(List<ClientCommon.PDNationWarRanking> listRankingOffense, List<ClientCommon.PDNationWarRanking> listRankingDefense)
    {
        Image imageNationWarResult = m_trNationWarRanking.Find("ImageResult").GetComponent<Image>();
        UpdateImageNationWarResult(imageNationWarResult);

        Transform trMyHeroNationWarObjective = m_trNationWarRanking.Find("MyHeroNationWarObjective");

        Text textMyHeroKillCount = trMyHeroNationWarObjective.Find("TextMyHeroKillCount").GetComponent<Text>();
        Text textKillRanking = trMyHeroNationWarObjective.Find("TextMyHeroKillRank").GetComponent<Text>();
        Text textMyHeroAssistCount = trMyHeroNationWarObjective.Find("TextMyHeroAssistCount").GetComponent<Text>();
        Text textMyHeroDeadCount = trMyHeroNationWarObjective.Find("TextMyHeroDeadCount").GetComponent<Text>();

        CsUIData.Instance.SetFont(textMyHeroKillCount);
        CsUIData.Instance.SetFont(textKillRanking);
        CsUIData.Instance.SetFont(textMyHeroAssistCount);
        CsUIData.Instance.SetFont(textMyHeroDeadCount);

        textMyHeroKillCount.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01002"), CsNationWarManager.Instance.NationWarKillCount);

        if (CsNationWarManager.Instance.GetMyHeroNationWarDeclaration() != null)
        {
            ClientCommon.PDNationWarRanking nationWarRanking = null;

            if (CsNationWarManager.Instance.GetMyHeroNationWarDeclaration().NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                nationWarRanking = listRankingOffense.Find(a => a.heroId == CsGameData.Instance.MyHeroInfo.HeroId);
            }
            else
            {
                nationWarRanking = listRankingDefense.Find(a => a.heroId == CsGameData.Instance.MyHeroInfo.HeroId);
            }

            if (nationWarRanking != null)
            {
                textKillRanking.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01003"), nationWarRanking.ranking);
            }
            else
            {
                textKillRanking.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01003"), "-");
            }
        }

        textMyHeroAssistCount.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01004"), CsNationWarManager.Instance.NationWarAssistCount);
        textMyHeroDeadCount.text = string.Format(CsConfiguration.Instance.GetString("A70_TXT_01005"), CsNationWarManager.Instance.NationWarDeadCount);

        if (CsNationWarManager.Instance.GetMyHeroNationWarDeclaration() != null)
        {
            Image imageNationOffense = m_trNationWarRanking.Find("ImageNationOffense").GetComponent<Image>();
            imageNationOffense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + CsNationWarManager.Instance.GetMyHeroNationWarDeclaration().NationId);

            Text textNationOffense = imageNationOffense.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationOffense);
            textNationOffense.text = CsGameData.Instance.GetNation(CsNationWarManager.Instance.GetMyHeroNationWarDeclaration().NationId).Name;

            Image imageNationDefense = m_trNationWarRanking.Find("ImageNationDefense").GetComponent<Image>();
            imageNationDefense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + CsNationWarManager.Instance.GetMyHeroNationWarDeclaration().TargetNationId);

            Text textNationDefense = imageNationDefense.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationDefense);
            textNationDefense.text = CsGameData.Instance.GetNation(CsNationWarManager.Instance.GetMyHeroNationWarDeclaration().TargetNationId).Name;
        }

        Transform trImageBgOffenseRanking = m_trNationWarRanking.Find("ImageBgOffenseRanking");
        Transform trImageBgDefenseRanking = m_trNationWarRanking.Find("ImageBgDefenseRanking");

        UpdateNationWarRanking(listRankingOffense, trImageBgOffenseRanking);
        UpdateNationWarRanking(listRankingDefense, trImageBgDefenseRanking);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageNationWarResult(Image imageNationWarResult)
    {
        if (m_nWinNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
        {
            imageNationWarResult.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/frm_dungeon_win");
        }
        else
        {
            imageNationWarResult.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupNationWar/frm_dungeon_lose");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationWarRanking(List<ClientCommon.PDNationWarRanking> listRanking, Transform trImageBgRanking)
    {
        Transform trNationWarRank = trImageBgRanking.Find("NationWarRank");

        Text textRank = trNationWarRank.Find("TextRank").GetComponent<Text>();
        Text textName = trNationWarRank.Find("TextName").GetComponent<Text>();
        Text textGuildName = trNationWarRank.Find("TextGuildName").GetComponent<Text>();
        Text textKillCount = trNationWarRank.Find("TextKillCount").GetComponent<Text>();

        CsUIData.Instance.SetFont(textRank);
        CsUIData.Instance.SetFont(textName);
        CsUIData.Instance.SetFont(textGuildName);
        CsUIData.Instance.SetFont(textKillCount);

        textRank.text = CsConfiguration.Instance.GetString("A70_TXT_00012");
        textName.text = CsConfiguration.Instance.GetString("A70_TXT_00013");
        textGuildName.text = CsConfiguration.Instance.GetString("A70_TXT_00014");
        textKillCount.text = CsConfiguration.Instance.GetString("A70_TXT_00015");

        Transform trNationWarRankItem = null;

        int nCount = 0;

        if (listRanking.Count <= 10)
        {
            nCount = listRanking.Count;
        }
        else
        {
            nCount = 10;
        }

        for (int i = 0; i < nCount; i++)
        {
            trNationWarRankItem = Instantiate(m_goNationWarRankingItem, trImageBgRanking).transform;
            trNationWarRankItem.name = "NationWarRankItem" + i;

            textRank = trNationWarRankItem.Find("TextRank").GetComponent<Text>();
            textName = trNationWarRankItem.Find("TextName").GetComponent<Text>();
            textGuildName = trNationWarRankItem.Find("TextGuildName").GetComponent<Text>();
            textKillCount = trNationWarRankItem.Find("TextKillCount").GetComponent<Text>();

            CsUIData.Instance.SetFont(textRank);
            CsUIData.Instance.SetFont(textName);
            CsUIData.Instance.SetFont(textGuildName);
            CsUIData.Instance.SetFont(textKillCount);

            if (i < 3)
            {
                Color32 color = new Color32(255, 214, 80, 255);

                textRank.color = color;
                textName.color = color;
                textGuildName.color = color;
                textKillCount.color = color;
            }

            textRank.text = listRanking[i].ranking.ToString("#,##0");
            textName.text = listRanking[i].heroName;

            if (listRanking[i].guildId == System.Guid.Empty)
            {
                // 길드 없음
                textGuildName.text = "";
            }
            else
            {
                textGuildName.text = listRanking[i].guildName;
            }

            textKillCount.text = listRanking[i].killCount.ToString("#,##0");
        }
    }
}
