using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-04)
//---------------------------------------------------------------------------------------------------
public class CsPopupFriendBless : CsPopupSub
{
	GameObject m_goProspectQuestItem;

	Transform m_trPopupFriendBlessReward;
	
	Transform m_trContentLeft;
	Transform m_trFrameLeftBottom;
	Transform m_trTextNoUserLeft;

	Transform m_trContentRight;
	Transform m_trFrameRightBottom;
	Transform m_trTextNoUserRight;

	CsHeroProspectQuest m_csHeroProspectQuest = null;

	float m_flTime = 0.0f;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestRewardReceive += OnEventOwnerProspectQuestRewardReceive;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestRewardReceiveAll += OnEventOwnerProspectQuestRewardReceiveAll;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestRewardReceive += OnEventTargetProspectQuestRewardReceive;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestRewardReceiveAll += OnEventTargetProspectQuestRewardReceiveAll;

		CsBlessingQuestManager.Instance.EventOwnerProspectQuestCompleted += OnEventOwnerProspectQuestCompleted;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestFailed += OnEventOwnerProspectQuestFailed;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestTargetLevelUpdated += OnEventOwnerProspectQuestTargetLevelUpdated;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestStarted += OnEventTargetProspectQuestStarted;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestCompleted += OnEventTargetProspectQuestCompleted;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestFailed += OnEventTargetProspectQuestFailed;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestRewardReceive -= OnEventOwnerProspectQuestRewardReceive;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestRewardReceiveAll -= OnEventOwnerProspectQuestRewardReceiveAll;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestRewardReceive -= OnEventTargetProspectQuestRewardReceive;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestRewardReceiveAll -= OnEventTargetProspectQuestRewardReceiveAll;

		CsBlessingQuestManager.Instance.EventOwnerProspectQuestCompleted -= OnEventOwnerProspectQuestCompleted;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestFailed -= OnEventOwnerProspectQuestFailed;
		CsBlessingQuestManager.Instance.EventOwnerProspectQuestTargetLevelUpdated -= OnEventOwnerProspectQuestTargetLevelUpdated;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestStarted -= OnEventTargetProspectQuestStarted;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestCompleted -= OnEventTargetProspectQuestCompleted;
		CsBlessingQuestManager.Instance.EventTargetProspectQuestFailed -= OnEventTargetProspectQuestFailed;
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime) 
	{
		if (m_flTime + flTime < Time.time)
		{
			foreach (CsHeroProspectQuest csHeroProspectOwnerQuest in CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList)
			{
				UpdateProspectQuestItem(csHeroProspectOwnerQuest, true);
			}

			UpdateButtonReceiveAll(true);

			foreach (CsHeroProspectQuest csHeroProspectTargetQuest in CsBlessingQuestManager.Instance.HeroProspectQuestTargetList)
			{
				UpdateProspectQuestItem(csHeroProspectTargetQuest, false);
			}

			UpdateButtonReceiveAll(false);

			m_flTime = Time.time;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		// Left
		Transform trFrameLeft = transform.Find("FrameLeft");

		Text textTitleLeft = trFrameLeft.Find("ImageTitle/TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitleLeft);
		textTitleLeft.text = CsConfiguration.Instance.GetString("A108_TXT_00004");

		m_trContentLeft = trFrameLeft.Find("FrameList/Viewport/Content");
		m_trTextNoUserLeft = trFrameLeft.Find("FrameList/Viewport/TextNoUser");
		m_trFrameLeftBottom = trFrameLeft.Find("FrameBottom");

		Text textNoUserLeft = m_trTextNoUserLeft.GetComponent<Text>();
		CsUIData.Instance.SetFont(textNoUserLeft);
		textNoUserLeft.text = CsConfiguration.Instance.GetString("A108_TXT_00006");

		Text textLeftBottomTitle = m_trFrameLeftBottom.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textLeftBottomTitle);
		textLeftBottomTitle.text = CsConfiguration.Instance.GetString("A108_TXT_00008");

		Text textLeftBottomContent = m_trFrameLeftBottom.Find("TextContent").GetComponent<Text>();
		CsUIData.Instance.SetFont(textLeftBottomContent);
		textLeftBottomContent.text = CsConfiguration.Instance.GetString("A108_TXT_00017");

		Button buttonReceiveAllOwnerProspect = m_trFrameLeftBottom.Find("ButtonReceive").GetComponent<Button>();
		buttonReceiveAllOwnerProspect.onClick.RemoveAllListeners();
		buttonReceiveAllOwnerProspect.onClick.AddListener(() => OnClickButtonPopupReceiveAll(true));

		// Right
		Transform trFrameRight = transform.Find("FrameRight");

		Text textTitleRight = trFrameRight.Find("ImageTitle/TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitleRight);
		textTitleRight.text = CsConfiguration.Instance.GetString("A108_TXT_00005");

		m_trContentRight = trFrameRight.Find("FrameList/Viewport/Content");
		m_trTextNoUserRight = trFrameRight.Find("FrameList/Viewport/TextNoUser");
		m_trFrameRightBottom = trFrameRight.Find("FrameBottom");

		Text textNoUserRight = m_trTextNoUserRight.GetComponent<Text>();
		CsUIData.Instance.SetFont(textNoUserRight);
		textNoUserRight.text = CsConfiguration.Instance.GetString("A108_TXT_00007");

		Text textRightBottomTitle = m_trFrameRightBottom.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textRightBottomTitle);
		textRightBottomTitle.text = CsConfiguration.Instance.GetString("A108_TXT_01017");

		Button buttonReceiveAllTargetProspect = m_trFrameRightBottom.Find("ButtonReceive").GetComponent<Button>();
		buttonReceiveAllTargetProspect.onClick.RemoveAllListeners();
		buttonReceiveAllTargetProspect.onClick.AddListener(() => OnClickButtonPopupReceiveAll(false));

		// PopupBless
		m_trPopupFriendBlessReward = transform.Find("PopupFriendBlessReward");
		
		Button buttonPopupFriendBlessReward = m_trPopupFriendBlessReward.GetComponent<Button>();
		buttonPopupFriendBlessReward.onClick.RemoveAllListeners();
		buttonPopupFriendBlessReward.onClick.AddListener(ClosePopupReceiveAll);

		Text textTitlePopupFriendBlessReward = m_trPopupFriendBlessReward.Find("ImageBackground/TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitlePopupFriendBlessReward);
		textTitlePopupFriendBlessReward.text = CsConfiguration.Instance.GetString("A108_TXT_02002");

		Text textReceive = m_trPopupFriendBlessReward.Find("ImageBackground/ButtonReceiveAll/TextReceiveAll").GetComponent<Text>();
		CsUIData.Instance.SetFont(textReceive);
		textReceive.text = CsConfiguration.Instance.GetString("A108_BTN_00041");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		if (m_goProspectQuestItem == null)
		{
			m_goProspectQuestItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupFriend/ProspectQuestItem");
		}

		SetProspectQuestAll();
	}

	//---------------------------------------------------------------------------------------------------
	// 퀘스트 전체 업데이트
	void SetProspectQuestAll()
	{
		SetProspectQuest(true);
		SetProspectQuest(false);
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트 or 대상유망자퀘스트 전체 세팅
	void SetProspectQuest(bool bOwnerProespectQuest)
	{
		// 내가 관심있는 유저
		if (bOwnerProespectQuest)
		{
			m_trContentLeft.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0);
			m_trTextNoUserLeft.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count <= 0);
			m_trFrameLeftBottom.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0);

			if (CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0)
			{
				for (int i = 0; i < m_trContentLeft.childCount; i++)
				{
					m_trContentLeft.GetChild(i).gameObject.SetActive(false);
					m_trContentLeft.GetChild(i).name = "";
				}

				int nCount = 0;
				foreach (CsHeroProspectQuest csHeroProspectQuest in CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList)
				{
					Transform trProspectQuestItem = null;

					if (nCount < m_trContentLeft.childCount)
					{
						trProspectQuestItem = m_trContentLeft.GetChild(nCount);
						trProspectQuestItem.gameObject.SetActive(true);
					}
					else
					{
						trProspectQuestItem = Instantiate(m_goProspectQuestItem, m_trContentLeft).transform;
					}

					trProspectQuestItem.name = csHeroProspectQuest.InstanceId.ToString();

					SetProspectQuestItem(csHeroProspectQuest, true);

					nCount++;
				}
			}
		}
		// 나에게 관심있는 유저
		else
		{
			m_trContentRight.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0);
			m_trTextNoUserRight.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count <= 0);
			m_trFrameRightBottom.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0);

			if (CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0)
			{
				for (int i = 0; i < m_trContentRight.childCount; i++)
				{
					m_trContentRight.GetChild(i).gameObject.SetActive(false);
					m_trContentRight.GetChild(i).name = "";
				}

				int nCount = 0;
				foreach (CsHeroProspectQuest csHeroProspectQuest in CsBlessingQuestManager.Instance.HeroProspectQuestTargetList)
				{
					Transform trProspectQuestItem = null;

					if (nCount < m_trContentRight.childCount)
					{
						trProspectQuestItem = m_trContentRight.GetChild(nCount);
						trProspectQuestItem.gameObject.SetActive(true);
					}
					else
					{
						trProspectQuestItem = Instantiate(m_goProspectQuestItem, m_trContentRight).transform;
					}

					trProspectQuestItem.name = csHeroProspectQuest.InstanceId.ToString();

					SetProspectQuestItem(csHeroProspectQuest, false);

					nCount++;
				}

				Text textRightBottomContent = m_trFrameRightBottom.Find("TextContent").GetComponent<Text>();
				CsUIData.Instance.SetFont(textRightBottomContent);

				int nObjectiveLevel = CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Max(quest => quest.BlessingTargetLevel.ProspectQuestObjectiveLevel);
				textRightBottomContent.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01031"), nObjectiveLevel);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유망자 퀘스트 내용 세팅(처음 1회)
	void SetProspectQuestItem(CsHeroProspectQuest csHeroProspectQuest, bool bOwnerProespectQuest)
	{
		// 내가 관심있는 유저
		if (bOwnerProespectQuest && m_trContentLeft != null)
		{
			Transform trProspectQuestItem = m_trContentLeft.Find(csHeroProspectQuest.InstanceId.ToString());

			if (trProspectQuestItem != null)
			{
				Image imageIcon = trProspectQuestItem.Find("ImageIcon").GetComponent<Image>();
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroProspectQuest.TargetJob.JobId.ToString());

				Transform trFrameOwnerProspect = trProspectQuestItem.Find("FrameOwnerProspect");
				trFrameOwnerProspect.gameObject.SetActive(true);

				Text textUserName = trFrameOwnerProspect.Find("TextUserName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textUserName);
				textUserName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01014"), csHeroProspectQuest.TargetLevel, csHeroProspectQuest.TargetName);

				Text textTarget = trFrameOwnerProspect.Find("TextTarget").GetComponent<Text>();
				CsUIData.Instance.SetFont(textTarget);
				textTarget.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01029"), csHeroProspectQuest.BlessingTargetLevel.ProspectQuestObjectiveLevel);

				Transform trRemainingTime = trFrameOwnerProspect.Find("RemainingTime");

				Text textRemainingTime = trRemainingTime.Find("TextRemainingTime").GetComponent<Text>();
				CsUIData.Instance.SetFont(textRemainingTime);
				textRemainingTime.text = CsConfiguration.Instance.GetString("A108_TXT_00016");

				Text textFinish = trRemainingTime.Find("TextFinish").GetComponent<Text>();
				CsUIData.Instance.SetFont(textFinish);
				textFinish.text = CsConfiguration.Instance.GetString("A108_TXT_00018");

				textFinish.gameObject.SetActive(csHeroProspectQuest.IsCompleted);

				Button buttonBless = trFrameOwnerProspect.Find("ButtonBless").GetComponent<Button>();
				buttonBless.onClick.RemoveAllListeners();
				buttonBless.onClick.AddListener(() => OnClickButtonBless(csHeroProspectQuest));

				Text textBless = trFrameOwnerProspect.Find("ButtonBless/TextBless").GetComponent<Text>();
				CsUIData.Instance.SetFont(textBless);
				textBless.text = CsConfiguration.Instance.GetString("A108_BTN_00024");

				Button buttonReceive = trFrameOwnerProspect.Find("ButtonReceive").GetComponent<Button>();
				buttonReceive.onClick.RemoveAllListeners();
				buttonReceive.onClick.AddListener(() => OnClickButtonReceiveOwnerProspect(csHeroProspectQuest));

				Text textReceive = trFrameOwnerProspect.Find("ButtonReceive/TextReceive").GetComponent<Text>();
				CsUIData.Instance.SetFont(textReceive);
				textReceive.text = CsConfiguration.Instance.GetString("A108_BTN_00034");
			}
		}
		
		// 나에게 관심있는 유저
		if (!bOwnerProespectQuest && m_trContentRight != null)
		{
			Transform trProspectQuestItem = m_trContentRight.Find(csHeroProspectQuest.InstanceId.ToString());

			if (trProspectQuestItem != null)
			{
				Image imageIcon = trProspectQuestItem.Find("ImageIcon").GetComponent<Image>();
				imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroProspectQuest.OwnerJob.JobId.ToString());

				Transform trFrameTargetProspect = trProspectQuestItem.Find("FrameTargetProspect");
				trFrameTargetProspect.gameObject.SetActive(true);

				Text textUserName = trFrameTargetProspect.Find("TextUserName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textUserName);
				textUserName.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01016"), csHeroProspectQuest.OwnerLevel, csHeroProspectQuest.OwnerName);

				Transform trRemainingTime = trFrameTargetProspect.Find("RemainingTime");

				Text textRemainingTime = trRemainingTime.Find("TextRemainingTime").GetComponent<Text>();
				CsUIData.Instance.SetFont(textRemainingTime);
				textRemainingTime.text = CsConfiguration.Instance.GetString("A108_TXT_00016");

				Text textFinish = trRemainingTime.Find("TextFinish").GetComponent<Text>();
				CsUIData.Instance.SetFont(textFinish);
				textFinish.text = CsConfiguration.Instance.GetString("A108_TXT_00018");
				
				Button buttonAddFriend = trFrameTargetProspect.Find("ButtonAddFriend").GetComponent<Button>();
				buttonAddFriend.onClick.RemoveAllListeners();
				buttonAddFriend.onClick.AddListener(() => OnClickButtonAddFriend(csHeroProspectQuest));

				Text textAddFriend = trFrameTargetProspect.Find("ButtonAddFriend/TextAddFriend").GetComponent<Text>();
				CsUIData.Instance.SetFont(textAddFriend);
				textAddFriend.text = CsConfiguration.Instance.GetString("A108_BTN_00025");

				Button buttonReceive = trFrameTargetProspect.Find("ButtonReceive").GetComponent<Button>();
				buttonReceive.onClick.RemoveAllListeners();
				buttonReceive.onClick.AddListener(() => OnClickButtonReceiveTargetProspect(csHeroProspectQuest));

				Text textReceive = trFrameTargetProspect.Find("ButtonReceive/TextReceive").GetComponent<Text>();
				CsUIData.Instance.SetFont(textReceive);
				textReceive.text = CsConfiguration.Instance.GetString("A108_BTN_00034");
			}
		}

		UpdateProspectQuestItem(csHeroProspectQuest, bOwnerProespectQuest);
	}

	//---------------------------------------------------------------------------------------------------
	// 전체 보상 팝업 열기
	void OpenPopupReceiveAll(bool bOwnerProspect)
	{
		if (m_trPopupFriendBlessReward != null)
		{
			RectTransform rtfPopupFirendBlessReward = m_trPopupFriendBlessReward.Find("ImageBackground").GetComponent<RectTransform>();

			Transform trFrameReward = m_trPopupFriendBlessReward.Find("ImageBackground/FrameReward");

			Dictionary<int, int> dicCsItemReward = new Dictionary<int,int>();

			for (int i = 0; i < trFrameReward.childCount; i++)
			{
				trFrameReward.GetChild(i).gameObject.SetActive(false);
			}

			if (bOwnerProspect)
			{
				rtfPopupFirendBlessReward.anchoredPosition = new Vector2(-100, -50);

				foreach (CsHeroProspectQuest csHeroProspectQuest in CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList)
				{
					if (!csHeroProspectQuest.IsCompleted)
						continue;

					foreach (CsProspectQuestOwnerReward ownerReward in csHeroProspectQuest.BlessingTargetLevel.ProspectQuestOwnerRewardList)
					{
						if (dicCsItemReward.ContainsKey(ownerReward.ItemReward.Item.ItemId))
						{
							dicCsItemReward[ownerReward.ItemReward.Item.ItemId] += ownerReward.ItemReward.ItemCount;
						}	
						else
						{
							dicCsItemReward.Add(ownerReward.ItemReward.Item.ItemId, ownerReward.ItemReward.ItemCount);
						}
					}
				}
			}
			else
			{
				rtfPopupFirendBlessReward.anchoredPosition = new Vector2(390, -50);

				foreach (CsHeroProspectQuest csHeroProspectQuest in CsBlessingQuestManager.Instance.HeroProspectQuestTargetList)
				{
					if (!csHeroProspectQuest.IsCompleted)
						continue;

					foreach (CsProspectQuestTargetReward targetReward in csHeroProspectQuest.BlessingTargetLevel.ProspectQuestTargetRewardList)
					{
						if (dicCsItemReward.ContainsKey(targetReward.ItemReward.Item.ItemId))
						{
							dicCsItemReward[targetReward.ItemReward.Item.ItemId] += targetReward.ItemReward.ItemCount;
						}
						else
						{
							dicCsItemReward.Add(targetReward.ItemReward.Item.ItemId, targetReward.ItemReward.ItemCount);
						}
					}
				}
			}

			var enumerator = dicCsItemReward.GetEnumerator();

			int nIndex = 0;
			while (enumerator.MoveNext())
			{
				CsItem csItem = CsGameData.Instance.GetItem(enumerator.Current.Key);

				if (csItem != null)
				{
					Transform trSlot = trFrameReward.GetChild(nIndex);
					trSlot.gameObject.SetActive(true);

					CsUIData.Instance.DisplayItemSlot(trSlot, csItem, false, enumerator.Current.Value, false, EnItemSlotSize.Medium, false);
				}

				nIndex++;
			}

			Button buttonReceiveAll = m_trPopupFriendBlessReward.Find("ImageBackground/ButtonReceiveAll").GetComponent<Button>();
			buttonReceiveAll.onClick.RemoveAllListeners();
			buttonReceiveAll.onClick.AddListener(() => OnClickButtonReceiveAll(bOwnerProspect));

			m_trPopupFriendBlessReward.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전체 보상 팝업 닫기
	void ClosePopupReceiveAll()
	{
		m_trPopupFriendBlessReward.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	// 유망자 퀘스트 내용 업데이트
	void UpdateProspectQuestItem(CsHeroProspectQuest csHeroProspectQuest, bool bOwnerProspectQuest)
	{
		Transform trFrameProspectItem = bOwnerProspectQuest ?
			m_trContentLeft.Find(csHeroProspectQuest.InstanceId.ToString()) :
			m_trContentRight.Find(csHeroProspectQuest.InstanceId.ToString());

		if (trFrameProspectItem == null ||
			!trFrameProspectItem.gameObject.activeSelf)
			return;

		Transform trRemainingTime = null;

		// 내가 관심있는 유저
		if (bOwnerProspectQuest)
		{
			Transform trFrameOwnerProspect = trFrameProspectItem.Find("FrameOwnerProspect");

			if (trFrameOwnerProspect != null)
			{
				trFrameOwnerProspect.Find("ButtonBless").gameObject.SetActive(!csHeroProspectQuest.IsCompleted);
				trFrameOwnerProspect.Find("ButtonReceive").gameObject.SetActive(csHeroProspectQuest.IsCompleted);

				trRemainingTime = trFrameOwnerProspect.Find("RemainingTime");
			}
		}
		// 나에게 관심있는 유저
		else
		{
			Transform trFrameTargetProspect = trFrameProspectItem.Find("FrameTargetProspect");

			if (trFrameTargetProspect != null)
			{
				CsFriend csFriend = CsFriendManager.Instance.FriendList.Find(friend => friend.Id.CompareTo(csHeroProspectQuest.OwnerId) == 0);

				trFrameTargetProspect.Find("ButtonAddFriend").gameObject.SetActive(csFriend == null && !csHeroProspectQuest.IsCompleted);
				trFrameTargetProspect.Find("ButtonReceive").gameObject.SetActive(csHeroProspectQuest.IsCompleted);

				trRemainingTime = trFrameTargetProspect.Find("RemainingTime");
			}
		}

		if (trRemainingTime != null)
		{
			trRemainingTime.Find("TextRemainingTime").gameObject.SetActive(!csHeroProspectQuest.IsCompleted);
			trRemainingTime.Find("TextTime").gameObject.SetActive(!csHeroProspectQuest.IsCompleted);
			trRemainingTime.Find("TextFinish").gameObject.SetActive(csHeroProspectQuest.IsCompleted);

			if (!csHeroProspectQuest.IsCompleted)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(csHeroProspectQuest.RemainingTime);

				Text textTime = trRemainingTime.Find("TextTime").GetComponent<Text>();
				CsUIData.Instance.SetFont(textTime);
				textTime.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01030"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 퀘스트를 성공 혹은 실패했을 경우 제거
	void RemoveProspectQuestItem(CsHeroProspectQuest csHeroProspectQuest, bool bOwnerProspectQuest)
	{
		Transform trProspectQuestItem = null;

		if (bOwnerProspectQuest)
		{
			trProspectQuestItem = m_trContentLeft.Find(csHeroProspectQuest.InstanceId.ToString());

			m_trContentLeft.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0);
			m_trTextNoUserLeft.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count <= 0);
			m_trFrameLeftBottom.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0);
		}
		else
		{
			trProspectQuestItem = m_trContentRight.Find(csHeroProspectQuest.InstanceId.ToString());

			m_trContentRight.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0);
			m_trTextNoUserRight.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count <= 0);
			m_trFrameRightBottom.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0);
		}

		if (trProspectQuestItem != null)
		{
			trProspectQuestItem.gameObject.SetActive(false);
			trProspectQuestItem.name = "";
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 보상 전체받기 버튼 업데이트
	void UpdateButtonReceiveAll(bool bOwnerProspectQuest)
	{
		Transform trImageNonReceived = null;
		Transform trButtonReceive = null;
		Transform trImageReceived = null;

		if (bOwnerProspectQuest)
		{
			trImageNonReceived = m_trFrameLeftBottom.Find("ImageNonReceived");
			trButtonReceive = m_trFrameLeftBottom.Find("ButtonReceive");
			trImageReceived = m_trFrameLeftBottom.Find("ImageReceived");

			if (CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count > 0)
			{
				if (CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Find(quest => quest.IsCompleted) == null)
				{
					trImageNonReceived.gameObject.SetActive(true);
					trButtonReceive.gameObject.SetActive(false);
					trImageReceived.gameObject.SetActive(false);
				}
				else
				{
					trImageNonReceived.gameObject.SetActive(false);
					trButtonReceive.gameObject.SetActive(true);
					trImageReceived.gameObject.SetActive(false);
				}
			}
			else
			{
				trImageNonReceived.gameObject.SetActive(false);
				trButtonReceive.gameObject.SetActive(false);
				trImageReceived.gameObject.SetActive(true);
			}
		}
		else
		{
			trImageNonReceived = m_trFrameRightBottom.Find("ImageNonReceived");
			trButtonReceive = m_trFrameRightBottom.Find("ButtonReceive");
			trImageReceived = m_trFrameRightBottom.Find("ImageReceived");

			if (CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Count > 0)
			{
				if (CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Find(quest => quest.IsCompleted) == null)
				{
					trImageNonReceived.gameObject.SetActive(true);
					trButtonReceive.gameObject.SetActive(false);
					trImageReceived.gameObject.SetActive(false);
				}
				else
				{
					trImageNonReceived.gameObject.SetActive(false);
					trButtonReceive.gameObject.SetActive(true);
					trImageReceived.gameObject.SetActive(false);
				}
			}
			else
			{
				trImageNonReceived.gameObject.SetActive(false);
				trButtonReceive.gameObject.SetActive(false);
				trImageReceived.gameObject.SetActive(true);
			}
		}
	}

	#region Event

	//---------------------------------------------------------------------------------------------------
	// 축복(귓속말)
	void OnClickButtonBless(CsHeroProspectQuest csHeroProspectQuest)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		// %%딜레이

		string[] asMessage = { CsConfiguration.Instance.GetString("A108_TXT_00024") };
		CsCommandEventManager.Instance.SendChattingMessageSend((int)EnChattingType.OneToOne, asMessage, null, csHeroProspectQuest.TargetId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonReceiveOwnerProspect(CsHeroProspectQuest csHeroProspectQuest)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		m_csHeroProspectQuest = csHeroProspectQuest;

		CsBlessingQuestManager.Instance.SendOwnerProspectQuestRewardReceive(csHeroProspectQuest.InstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonAddFriend(CsHeroProspectQuest csHeroProspectQuest)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		if (CsFriendManager.Instance.FriendList.Find(friend => friend.Id == csHeroProspectQuest.OwnerId) != null)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02007"));
			return;
		}

		m_csHeroProspectQuest = csHeroProspectQuest;

		CsFriendManager.Instance.SendFriendApply(csHeroProspectQuest.OwnerId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonReceiveTargetProspect(CsHeroProspectQuest csHeroProspectQuest)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		m_csHeroProspectQuest = csHeroProspectQuest;

		CsBlessingQuestManager.Instance.SendTargetProspectQuestRewardReceive(csHeroProspectQuest.InstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPopupReceiveAll(bool bOwnerProspectQuest)
	{
		OpenPopupReceiveAll(bOwnerProspectQuest);
	}

	//---------------------------------------------------------------------------------------------------
	// 전체 보상 받기
	void OnClickButtonReceiveAll(bool bOwnerProspectQuest)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		ClosePopupReceiveAll();

		if (bOwnerProspectQuest)
		{
			CsBlessingQuestManager.Instance.SendOwnerProspectQuestRewardReceiveAll();
		}
		else
		{
			CsBlessingQuestManager.Instance.SendTargetProspectQuestRewardReceiveAll();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOwnerProspectQuestRewardReceive()
	{
		UpdateButtonReceiveAll(true);
		RemoveProspectQuestItem(m_csHeroProspectQuest, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOwnerProspectQuestRewardReceiveAll()
	{
		// 전체삭제
		UpdateButtonReceiveAll(true);
		SetProspectQuest(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTargetProspectQuestRewardReceive()
	{
		UpdateButtonReceiveAll(false);
		RemoveProspectQuestItem(m_csHeroProspectQuest, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTargetProspectQuestRewardReceiveAll()
	{
		// 전체삭제
		UpdateButtonReceiveAll(false);
		SetProspectQuest(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOwnerProspectQuestCompleted(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateButtonReceiveAll(true);
		UpdateProspectQuestItem(csHeroProspectQuest, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOwnerProspectQuestFailed(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateButtonReceiveAll(true);
		RemoveProspectQuestItem(csHeroProspectQuest, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOwnerProspectQuestTargetLevelUpdated(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateProspectQuestItem(csHeroProspectQuest, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTargetProspectQuestStarted(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateButtonReceiveAll(false);

		// 새로 받은 대상유망자퀘스트 추가 처리
		Transform trProspectQuestItem = null;

		for (int i = 0; i < m_trContentRight.childCount; i++)
		{
			GameObject goItem = m_trContentRight.GetChild(i).gameObject;
			
			if (!goItem.activeSelf && goItem.name.Length <= 0)
			{
				trProspectQuestItem = goItem.transform;
				goItem.SetActive(true);
			}
		}

		if (trProspectQuestItem == null)
		{
			trProspectQuestItem = Instantiate(m_goProspectQuestItem, m_trContentRight).transform;
		}

		trProspectQuestItem.name = csHeroProspectQuest.InstanceId.ToString();

		SetProspectQuestItem(csHeroProspectQuest, false);

		Text textRightBottomContent = m_trFrameRightBottom.Find("TextContent").GetComponent<Text>();
		CsUIData.Instance.SetFont(textRightBottomContent);

		int nObjectiveLevel = CsBlessingQuestManager.Instance.HeroProspectQuestTargetList.Max(quest => quest.BlessingTargetLevel.ProspectQuestObjectiveLevel);
		textRightBottomContent.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01031"), nObjectiveLevel);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTargetProspectQuestCompleted(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateButtonReceiveAll(false);
		UpdateProspectQuestItem(csHeroProspectQuest, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTargetProspectQuestFailed(CsHeroProspectQuest csHeroProspectQuest)
	{
		UpdateButtonReceiveAll(false);
		RemoveProspectQuestItem(csHeroProspectQuest, false);
	}

	//---------------------------------------------------------------------------------------------------
	#endregion Event
}
