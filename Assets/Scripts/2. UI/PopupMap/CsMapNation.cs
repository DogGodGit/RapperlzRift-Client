using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsMapNation : CsPopupSub
{

    Vector3 vtTransmissionNpcPos;
    int nTransmissionNpcId;
    int nTransmissionNpcContinentId;


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

    #region Event Handler

    void OnClickNationMove(int nNationId)
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == nNationId)
        {
            return;
        }
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A60_TXT_03001"));
            return;
        }
        else
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(nTransmissionNpcId);
			CsGameEventToIngame.Instance.OnEventMapMove(csNpcInfo.ContinentId, nNationId, csNpcInfo.Position);
            CsUIData.Instance.AutoStateType = EnAutoStateType.Move;
            CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        }
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        List<CsNation> listNation = CsGameData.Instance.NationList;

        for (int i = 0; i < listNation.Count; ++i)
        {
            int nNationId = listNation[i].NationId;
            Transform trNation = transform.Find("PanelNation" + nNationId);
            Button buttonNation = trNation.Find("ButtonNation").GetComponent<Button>();
            Image imageNation = trNation.Find("ImageNationIcon").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + nNationId);
            buttonNation.onClick.RemoveAllListeners();
            buttonNation.onClick.AddListener(() => { OnClickNationMove(nNationId); });
            buttonNation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            Text textName = buttonNation.transform.Find("Text").GetComponent<Text>();
            textName.text = listNation[i].Name;

            if (CsGameData.Instance.MyHeroInfo.Nation.NationId == listNation[i].NationId)
            {
                textName.color = CsUIData.Instance.ColorSkyblue;
            }
            CsUIData.Instance.SetFont(textName);
        }

        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
        nTransmissionNpcId = csNpcInfo.NpcId;
        vtTransmissionNpcPos = csNpcInfo.Position;
        nTransmissionNpcContinentId = csNpcInfo.ContinentId;

        DisplayPlayer();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPlayer()
    {
        List<CsNation> listNation = CsGameData.Instance.NationList;

        for (int i = 0; i < listNation.Count; ++i)
        {
            Transform trPlayer = transform.Find("PanelNation" + listNation[i].NationId + "/ImagePlayerNation");
            if (trPlayer != null)
            {
                if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == listNation[i].NationId)
                {
                    trPlayer.gameObject.SetActive(true);
                }
                else
                {
                    trPlayer.gameObject.SetActive(false);
                }
            }
        }
    }
}
