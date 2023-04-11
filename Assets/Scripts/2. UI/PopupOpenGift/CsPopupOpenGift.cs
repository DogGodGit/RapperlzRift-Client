using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsPopupOpenGift : CsUpdateableMonoBehaviour
{
	[SerializeField]
	GameObject goItemSlot;
	GameObject m_goPopupItemInfo;

	Transform m_trContent;
	Transform m_trPopupList;
	Transform m_trItemInfo;

	CsPopupItemInfo m_csPopupItemInfo;

	int m_nToday = 0;

	bool ReceivableOpenGift
	{
		get
		{
			return CsGameData.Instance.MyHeroInfo.ReceivedOpenGiftRewardList.Count < CsGameData.Instance.OpenGiftRewardList.Max(gift => gift.Day); 
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		if (!ReceivableOpenGift)
		{
			PopupClose();
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventOpenGiftReceive += OnEventOpenGiftReceive;

		m_nToday = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.Subtract(CsGameData.Instance.MyHeroInfo.RegDate).TotalDays + 1;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventOpenGiftReceive -= OnEventOpenGiftReceive;
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
		m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;

		StringBuilder sb = new StringBuilder();

		Transform trImageBackground = transform.Find("ImageBackground");

		Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = CsConfiguration.Instance.GetString("A99_NAME_00001");

		m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");

		Transform trButtonClose = trImageBackground.Find("ButtonClose");
		Button buttonClose = trButtonClose.GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClickPopupClose);

		GameObject goOpenGiftItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupOpenGift/OpenGiftItem");

		int nOpenGiftLastDay = CsGameData.Instance.OpenGiftRewardList.Max(gift => gift.Day);

		for (int i = 0; i < nOpenGiftLastDay; i++)
		{
			int nDay = i + 1;

			sb.Length = 0;
			sb.Append("Day");

			Transform trOpenGiftItem = Instantiate(goOpenGiftItem, m_trContent).transform;
			trOpenGiftItem.name = sb.Append(nDay).ToString();

			Text textDate = trOpenGiftItem.Find("FrameDate/TextDate").GetComponent<Text>();
			CsUIData.Instance.SetFont(textDate);
			textDate.text = string.Format(CsConfiguration.Instance.GetString("A99_TXT_01002"), nDay);

			Text textToday = trOpenGiftItem.Find("FrameDate/TextToday").GetComponent<Text>();
			CsUIData.Instance.SetFont(textToday);
			textToday.text = CsConfiguration.Instance.GetString("A99_TXT_00001");

			Text textButtonReceive = trOpenGiftItem.Find("ButtonReceive/TextReceive").GetComponent<Text>();
			CsUIData.Instance.SetFont(textButtonReceive);
			textButtonReceive.text = CsConfiguration.Instance.GetString("A99_BTN_00001");

			Button buttonReceive = trOpenGiftItem.Find("ButtonReceive").GetComponent<Button>();
			buttonReceive.onClick.RemoveAllListeners();
			buttonReceive.onClick.AddListener(() => OnClickOpenGiftReceive(nDay));
			buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			Text textReceived = trOpenGiftItem.Find("TextReceived").GetComponent<Text>();
			CsUIData.Instance.SetFont(textReceived);
			textReceived.text = CsConfiguration.Instance.GetString("A99_TXT_00002");

			Transform trFrameItemSlot = trOpenGiftItem.Find("FrameItemSlot");

			var openGifts = from gift in CsGameData.Instance.OpenGiftRewardList
							where gift.Day == nDay
							orderby gift.RewardNo
							select gift;

			foreach (var gift in openGifts)
			{
				sb.Length = 0;
				sb.Append("Reward");

				Transform trItemSlot = Instantiate(goItemSlot, trFrameItemSlot).transform;
				trItemSlot.name = sb.Append(gift.RewardNo).ToString();

				CsUIData.Instance.DisplayItemSlot(trItemSlot, gift.ItemReward.Item, gift.ItemReward.ItemOwned, gift.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);

				Button buttonItemSlot = trItemSlot.GetComponent<Button>();
				buttonItemSlot.onClick.RemoveAllListeners();
				buttonItemSlot.onClick.AddListener(() => OnClickItemInfo(gift.ItemReward.Item));
				buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
			}

			UpdateContent(nDay);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPopupClose()
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickOpenGiftReceive(int nDay)
	{
		var listOpenGift = from openGift in CsGameData.Instance.OpenGiftRewardList
						   where openGift.Day == nDay
						   select openGift.ItemReward;

		if (!CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listOpenGift))
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A99_TXT_03001"));
			return;
		}

		CsCommandEventManager.Instance.SendOpenGiftReceive(nDay);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickItemInfo(CsItem csItem)
	{
		StartCoroutine(LoadPopupItemInfo(csItem));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenGiftReceive(int nDay)
	{
		if (!ReceivableOpenGift)
		{
			PopupClose();
			return;
		}

		UpdateContent(nDay);
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupItemInfo(CsItem csitem)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
		yield return resourceRequest;
		m_goPopupItemInfo = (GameObject)resourceRequest.asset;

		OpenPopupItemInfo(csitem);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo(CsItem csitem)
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

		m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, false, 0, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
	{
		m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
		Destroy(m_trItemInfo.gameObject);
		m_csPopupItemInfo = null;
		m_trItemInfo = null;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateContent(int nDay)
	{
		if (nDay < 1)
			return;

		if (m_trContent != null)
		{
			string strFmt = string.Format("Day{0}", nDay);
			
			Transform trOpenGiftItem = m_trContent.Find(strFmt);

			bool bIsReceived = CsGameData.Instance.MyHeroInfo.IsReceivedOpenGiftReward(nDay);
			bool bIsReceivable = nDay <= m_nToday && CsGameConfig.Instance.OpenGiftRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level;

			trOpenGiftItem.Find("FrameDate/TextToday").gameObject.SetActive(nDay == m_nToday);

			trOpenGiftItem.Find("ImageReceivable").gameObject.SetActive(bIsReceivable);
			trOpenGiftItem.Find("ImageNonReceivable").gameObject.SetActive(!bIsReceivable);

			Button buttonReceive = trOpenGiftItem.Find("ButtonReceive").GetComponent<Button>();
			buttonReceive.interactable = bIsReceivable;

			trOpenGiftItem.Find("ButtonReceive").gameObject.SetActive(!bIsReceived);
			trOpenGiftItem.Find("TextReceived").gameObject.SetActive(bIsReceived);

			trOpenGiftItem.Find("ImageReceived").gameObject.SetActive(bIsReceived);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		int nOpenGiftLastDay = CsGameData.Instance.OpenGiftRewardList.Max(gift => gift.Day);

		m_nToday = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.Subtract(CsGameData.Instance.MyHeroInfo.RegDate).TotalDays + 1;

		if (m_nToday <= nOpenGiftLastDay)
		{
			UpdateContent(m_nToday - 1);
			UpdateContent(m_nToday);
		}
	}
}
