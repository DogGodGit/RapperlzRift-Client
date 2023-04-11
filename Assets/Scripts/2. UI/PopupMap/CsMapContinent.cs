using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsMapContinent : CsPopupSub
{
    Transform m_trPlayerPosition;

    bool m_bFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {

    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
        }
        else
        {
            DisplayPlayer();
        }
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

    //---------------------------------------------------------------------------------------------------
    void OnClickArea(int nLocationId)
    {
        CsGameEventUIToUI.Instance.OnEventMiniMapSelected(nLocationId);
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPlayerPosition = transform.Find("ImagePlayerPosition");
        Image imageNation = transform.Find("PanelNation/ImageNation").GetComponent<Image>();
        imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + CsGameData.Instance.MyHeroInfo.Nation.NationId);
        Text textNation = transform.Find("PanelNation/TextName").GetComponent<Text>();
        textNation.text = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam).Name;
        CsUIData.Instance.SetFont(textNation);

        //1. 마루두카 산맥
        //2. 얼라이언스
        //3. 팔미르 협곡
        //4. 세리우 사막
        //5. 사이라그
        //6. 론도
        //7. 발모어

        for (int i = 0; i < CsGameData.Instance.ContinentList.Count; ++i)
        {
            CsContinent csContinent = CsGameData.Instance.ContinentList[i];

            if (csContinent == null)
            {
                continue;
            }
            else
            {
                Transform trButtonContinent = transform.Find("Button" + csContinent.ContinentId);

                if (trButtonContinent == null)
                {
                    continue;
                }
                else
                {
                    Button buttonContinet = trButtonContinent.GetComponent<Button>();

                    if (buttonContinet == null)
                    {
                        continue;
                    }
                    else
                    {
                        buttonContinet.onClick.RemoveAllListeners();
                        int nLocationId = csContinent.LocationId;
                        buttonContinet.onClick.AddListener(() => { OnClickArea(nLocationId); });
                        buttonContinet.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                        Text textContinet = buttonContinet.transform.Find("Text").GetComponent<Text>();
                        textContinet.text = csContinent.Name;
                        CsUIData.Instance.SetFont(textContinet);
                    }
                }
            }
        }

        DisplayPlayer();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayPlayer()
    {
        CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId);

        if (csContinent == null)
        {
            m_trPlayerPosition.gameObject.SetActive(false);
        }
        else
        {
            Transform trParent = transform.Find("Button" + csContinent.ContinentId);
            m_trPlayerPosition.parent = trParent;
            m_trPlayerPosition.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 60f);
            m_trPlayerPosition.gameObject.SetActive(true);
        }
    }
}
