using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-07-19)
//---------------------------------------------------------------------------------------------------

enum EnWealthType
{
	Dia = 1,
	Gold = 2,
}

public class CsPopupRetrieval : CsUpdateableMonoBehaviour
{
	[SerializeField]
	GameObject m_goItemSlot64;
	GameObject m_goRetrievalItem;

	Transform m_trContent;
	Transform m_trScrollView;
	Transform m_trTextEmpty;
	
	Button m_buttonTotalGold;
	Button m_buttonTotalDia;

	Text m_textMyDia;
	Text m_textMyGold;
	Text m_textTotalDia;
	Text m_textTotalGold;

	Transform m_trRetrievalItem;
	CsRetrieval m_csRetrieval;

	int m_nTotalDia;
	long m_lTotalGold;

	bool m_bReceiveMail = false;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventRetrieveGold += OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll += OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia += OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll += OnEventRetrieveDiaAll;
		
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventRetrieveGold -= OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll -= OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia -= OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll -= OnEventRetrieveDiaAll;
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PopupClose();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_goRetrievalItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupRetrieval/RetrievalItem");

		Transform trImageBackground = transform.Find("ImageBackground");

		// 타이틀
		Text textTitle = trImageBackground.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A107_TITLE_00001");	// 타이틀

		// 보유 재화
		m_textMyGold = trImageBackground.Find("FrameGold/TextGold").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textMyGold);
		m_textMyDia = trImageBackground.Find("FrameDia/TextDia").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textMyDia);
		
		// 닫기 버튼
		Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(PopupClose);
		buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");
		m_trScrollView = trImageBackground.Find("Scroll View");
		m_trTextEmpty = trImageBackground.Find("Scroll View/Viewport/TextEmpty");

		Text textEmpty = m_trTextEmpty.GetComponent<Text>();
		CsUIData.Instance.SetFont(textEmpty);
		textEmpty.text = CsConfiguration.Instance.GetString("A107_TXT_00007"); // 회수 가능한 컨텐츠가 없습니다.

		// 하단 영역
		Transform trFrameBottom = trImageBackground.Find("FrameBottom");
		
		Text textNotice = trFrameBottom.Find("TextNotice").GetComponent<Text>();
		CsUIData.Instance.SetFont(textNotice);
		textNotice.text = CsConfiguration.Instance.GetString("A107_TXT_00006"); // 우측 버튼 터치 시~

		Transform trButtonRetrieveGoldAll = trFrameBottom.Find("ButtonRetrieveGoldAll");
		m_buttonTotalGold = trButtonRetrieveGoldAll.GetComponent<Button>();
		m_buttonTotalGold.onClick.RemoveAllListeners();
		m_buttonTotalGold.onClick.AddListener(OnClickButtonRetrieveGoldAll);
		m_buttonTotalGold.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Transform trButtonRetrieveDiaAll = trFrameBottom.Find("ButtonRetrieveDiaAll");
		m_buttonTotalDia = trButtonRetrieveDiaAll.GetComponent<Button>();
		m_buttonTotalDia.onClick.RemoveAllListeners();
		m_buttonTotalDia.onClick.AddListener(OnClickButtonRetrieveDiaAll);
		m_buttonTotalDia.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_textTotalGold = trButtonRetrieveGoldAll.Find("TextGold").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textTotalGold);

		m_textTotalDia = trButtonRetrieveDiaAll.Find("TextDia").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textTotalDia);

		UpdateWealth();
		UpdateItemAll();
		CheckAndUpdateEmptyContent();
	}

	//---------------------------------------------------------------------------------------------------
	// 재화 업데이트
	void UpdateWealth()
	{
		m_textMyDia.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");
		m_textMyGold.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");
	}

	//---------------------------------------------------------------------------------------------------
	// 전체 항목 업데이트
	void UpdateItemAll()
	{
		m_lTotalGold = 0;
		m_nTotalDia = 0;
		
		for (int i = 0; i < m_trContent.childCount; i++)
		{
			m_trContent.GetChild(i).name = "";
			m_trContent.GetChild(i).gameObject.SetActive(false);
		}

		int nIndex = 0;

		foreach	(var csRetrieval in CsGameData.Instance.RetrievalList)
		{
			int nRemainingRetrievalCount = csRetrieval.GetRemainingCount();

			if (nRemainingRetrievalCount <= 0)
				continue;

			Transform trItem;
			
			if (nIndex < m_trContent.childCount)
			{
				trItem = m_trContent.GetChild(nIndex);
			}
			else
			{
				trItem = Instantiate(m_goRetrievalItem, m_trContent).transform;
				
				Text textReward = trItem.Find("TextReward").GetComponent<Text>();
				CsUIData.Instance.SetFont(textReward);
				textReward.text = CsConfiguration.Instance.GetString("A107_TXT_00003");

				Text textGoldRetrieval = trItem.Find("FrameGoldDescription/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textGoldRetrieval);
				textGoldRetrieval.text = CsConfiguration.Instance.GetString("A107_TXT_00004");

				Text textDiaRetrieval = trItem.Find("FrameDiaDescription/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textDiaRetrieval);
				textDiaRetrieval.text = CsConfiguration.Instance.GetString("A107_TXT_00005");
			}

			UpdateItem(trItem, csRetrieval, nRemainingRetrievalCount);

			// 총 필요재화
			m_lTotalGold += (csRetrieval.GoldRetrievalRequiredGold * nRemainingRetrievalCount);
			m_nTotalDia += (csRetrieval.DiaRetrievalRequiredDia * nRemainingRetrievalCount);
			
			nIndex++;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckAndUpdateEmptyContent()
	{
		int nIndex = 0;

		for (int i = 0; i < m_trContent.childCount; i++)
		{
			if (m_trContent.GetChild(i).gameObject.activeSelf)
			{
				nIndex++;
				break;
			}
		}

		m_trContent.gameObject.SetActive(nIndex > 0);
		m_trTextEmpty.gameObject.SetActive(nIndex <= 0);

		m_buttonTotalGold.interactable = nIndex > 0;
		m_buttonTotalDia.interactable = nIndex > 0;

		m_textTotalGold.text = m_lTotalGold.ToString("#,##0");
		m_textTotalDia.text = m_nTotalDia.ToString("#,##0");
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateItem(Transform trItem, CsRetrieval csRetrieval, int nRemainingRetrievalCount = -1)
	{
		trItem.name = csRetrieval.RetrievalId.ToString();

		if (nRemainingRetrievalCount == -1)
		{
			nRemainingRetrievalCount = csRetrieval.GetRemainingCount();

			if (nRemainingRetrievalCount == 0)
			{
				trItem.gameObject.SetActive(false);
				return;
			}
		}

		Image imageIcon = trItem.Find("ImageIcon").GetComponent<Image>();
		imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/retrieval_" + csRetrieval.RetrievalId);

		Text textName = trItem.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = csRetrieval.Name;

		Text textRemainingCount = trItem.Find("TextRemainingCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(textRemainingCount);
		textRemainingCount.text = string.Format(CsConfiguration.Instance.GetString("A107_TXT_00002"), nRemainingRetrievalCount);

		// 보상
		trItem.Find("FrameRewardExp").gameObject.SetActive(csRetrieval.RewardDisplayType == 1);
		trItem.Find("FrameRewardItem").gameObject.SetActive(csRetrieval.RewardDisplayType == 2);

		if (csRetrieval.RewardDisplayType == 1)
		{
			// 경험치
			Text trText = trItem.Find("FrameRewardExp/Text").GetComponent<Text>();
			CsUIData.Instance.SetFont(trText);

			CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);

			if (csRetrievalReward != null)
			{
				trText.text = csRetrievalReward.DiaExpReward.Value.ToString("#,##0");
			}
		}
		else if (csRetrieval.RewardDisplayType == 2)
		{
			// 아이템
			Transform trFrmaeRewardItem = trItem.Find("FrameRewardItem");
			
			Transform trGoldItemReward;
			Transform trDiaItemReward;

			if (trFrmaeRewardItem.childCount == 0)
			{
				trGoldItemReward = Instantiate(m_goItemSlot64, trFrmaeRewardItem).transform;
				trGoldItemReward.name = "GoldItemReward";

				trDiaItemReward = Instantiate(m_goItemSlot64, trFrmaeRewardItem).transform;
				trDiaItemReward.name = "DiaItemReward";
			}
			else
			{
				trGoldItemReward = trFrmaeRewardItem.Find("GoldItemReward");
				trDiaItemReward = trFrmaeRewardItem.Find("DiaItemReward");
			}

			CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);

			if (csRetrievalReward != null)
			{
				Image imageGoldItemReward = trGoldItemReward.Find("ImageIcon").GetComponent<Image>();
				imageGoldItemReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csRetrievalReward.GoldItemReward.Item.Image);

				Image imageDiaItemReward = trDiaItemReward.Find("ImageIcon").GetComponent<Image>();
				imageDiaItemReward.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csRetrievalReward.DiaItemReward.Item.Image);

				trGoldItemReward.Find("ImageOwn").gameObject.SetActive(csRetrievalReward.GoldItemReward.ItemOwned);
				trDiaItemReward.Find("ImageOwn").gameObject.SetActive(csRetrievalReward.DiaItemReward.ItemOwned);

				Text textGoldItemReward = trGoldItemReward.Find("Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textGoldItemReward);
				textGoldItemReward.text = csRetrievalReward.GoldItemReward.ItemCount.ToString();

				Text textDiaItemReward = trDiaItemReward.Find("Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textDiaItemReward);
				textDiaItemReward.text = csRetrievalReward.DiaItemReward.ItemCount.ToString();
			}
		}

		// 버튼
		Transform trButtonGold = trItem.Find("ButtonGold");

		Button buttonGold = trButtonGold.GetComponent<Button>();
		buttonGold.onClick.RemoveAllListeners();
		buttonGold.onClick.AddListener(() => OnClickButtonRetrieveGold(trItem, csRetrieval));
		buttonGold.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textGold = trButtonGold.Find("TextGold").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGold);
		textGold.text = csRetrieval.GoldRetrievalRequiredGold.ToString();

		Transform trButtonDia = trItem.Find("ButtonDia");

		Button buttonDia = trButtonDia.GetComponent<Button>();
		buttonDia.onClick.RemoveAllListeners();
		buttonDia.onClick.AddListener(() => OnClickButtonRetrieveDia(trItem, csRetrieval));
		buttonDia.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textDia = trButtonDia.Find("TextDia").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDia);
		textDia.text = csRetrieval.DiaRetrievalRequiredDia.ToString();
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		// 전체 업데이트
		UpdateItemAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGold(bool bLevelUp, long lExpAcq)
	{
		ReceiveMailToastMessage();

		UpdateWealth();
		UpdateItem(m_trRetrievalItem, m_csRetrieval);

		m_lTotalGold -= m_csRetrieval.GoldRetrievalRequiredGold;
		m_nTotalDia -= m_csRetrieval.DiaRetrievalRequiredDia;

		m_textTotalGold.text = m_lTotalGold.ToString("#,##0");
		m_textTotalDia.text = m_nTotalDia.ToString("#,##0");

		CheckAndUpdateEmptyContent();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGoldAll(bool bLevelUp, long lExpAcq)
	{
		ReceiveMailToastMessage();

		UpdateWealth();
		UpdateItemAll();

		CheckAndUpdateEmptyContent();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDia(bool bLevelUp, long lExpAcq)
	{
		ReceiveMailToastMessage();

		UpdateWealth();
		UpdateItem(m_trRetrievalItem, m_csRetrieval);

		m_lTotalGold -= m_csRetrieval.GoldRetrievalRequiredGold;
		m_nTotalDia -= m_csRetrieval.DiaRetrievalRequiredDia;

		m_textTotalGold.text = m_lTotalGold.ToString("#,##0");
		m_textTotalDia.text = m_nTotalDia.ToString("#,##0");

		CheckAndUpdateEmptyContent();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDiaAll(bool bLevelUp, long lExpAcq)
	{
		ReceiveMailToastMessage();

		UpdateWealth();
		UpdateItemAll();

		CheckAndUpdateEmptyContent();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRetrieveGoldAll()
	{
		// 골드 체크
		if (CsGameData.Instance.MyHeroInfo.Gold < m_lTotalGold)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03001"));
			return;
		}

		// 인벤토리 부족 시 메일 발송
		List<CsItemReward> listItemRewards = new List<CsItemReward>();

		foreach (CsRetrieval csRetrieval in CsGameData.Instance.RetrievalList)
		{
			if (csRetrieval.RewardDisplayType == 1)
				continue;

			int nRemainingCount = csRetrieval.GetRemainingCount();

			for (int i = 0; i < nRemainingCount; i++)
			{
				CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);

				if (csRetrievalReward != null)
				{
					listItemRewards.Add(csRetrievalReward.GoldItemReward);
				}
			}
		}

		m_bReceiveMail = !CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listItemRewards);

		CsCommandEventManager.Instance.SendRetrieveGoldAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRetrieveDiaAll()
	{
		// 다이아 체크
		if (CsGameData.Instance.MyHeroInfo.Dia < m_nTotalDia)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03002"));
			return;
		}

		// 인벤토리 부족 시 메일 발송
		List<CsItemReward> listItemRewards = new List<CsItemReward>();

		foreach (CsRetrieval csRetrieval in CsGameData.Instance.RetrievalList)
		{
			if (csRetrieval.RewardDisplayType == 1)
				continue;

			int nRemainingCount = csRetrieval.GetRemainingCount();

			for (int i = 0; i < nRemainingCount; i++)
			{
				CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);

				if (csRetrievalReward != null)
				{
					listItemRewards.Add(csRetrievalReward.DiaItemReward);
				}
			}
		}

		m_bReceiveMail = !CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listItemRewards);

		CsCommandEventManager.Instance.SendRetrieveDiaAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRetrieveGold(Transform trItem, CsRetrieval csRetrieval)
	{
		// 골드 체크
		if (CsGameData.Instance.MyHeroInfo.Gold < csRetrieval.GoldRetrievalRequiredGold)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03001"));
			return;
		}

		// 인벤토리 부족 시 메일 발송
		List<CsItemReward> listItemRewards = new List<CsItemReward>();

		if (csRetrieval.RewardDisplayType == 2)
		{
			CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);
			
			if (csRetrievalReward != null)
			{
				listItemRewards.Add(csRetrievalReward.GoldItemReward);
			}
		}

		m_bReceiveMail = !CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listItemRewards);

		m_trRetrievalItem = trItem;
		m_csRetrieval = csRetrieval;

		CsCommandEventManager.Instance.SendRetrieveGold(csRetrieval.RetrievalId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRetrieveDia(Transform trItem, CsRetrieval csRetrieval)
	{
		// 다이아 체크
		if (CsGameData.Instance.MyHeroInfo.Dia < csRetrieval.DiaRetrievalRequiredDia)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03002"));
			return;
		}

		// 인벤토리 부족 시 메일 발송
		List<CsItemReward> listItemRewards = new List<CsItemReward>();

		if (csRetrieval.RewardDisplayType == 2)
		{
			CsRetrievalReward csRetrievalReward = csRetrieval.GetRetrievalReward(CsGameData.Instance.MyHeroInfo.Level);

			if (csRetrievalReward != null)
			{
				listItemRewards.Add(csRetrievalReward.DiaItemReward);
			}
		}

		m_bReceiveMail = !CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listItemRewards);
		
		m_trRetrievalItem = trItem;
		m_csRetrieval = csRetrieval;

		CsCommandEventManager.Instance.SendRetrieveDia(csRetrieval.RetrievalId);
	}

	//---------------------------------------------------------------------------------------------------
	void ReceiveMailToastMessage()
	{
		if (m_bReceiveMail)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03003"));
		}

		m_bReceiveMail = false;
	}
}
