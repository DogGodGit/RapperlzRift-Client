using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupFieldBoss : CsUpdateableMonoBehaviour {

	[SerializeField]
	GameObject m_goItemSlot;
	CsFieldBoss m_csFieldBoss;

	GameObject m_goPopupItemInfo;
	Transform m_trPopupList;
	Transform m_trItemInfo;
	CsPopupItemInfo m_csPopupItemInfo;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		InitializeUI();
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

		Text textTitle = transform.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A115_TXT_00001");

		Button buttonEscape = transform.Find("ButtonEscape").GetComponent<Button>();
		buttonEscape.onClick.RemoveAllListeners();
		buttonEscape.onClick.AddListener(PopupClose);
		buttonEscape.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		// 몬스터 리스트
		GameObject goMonsterItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupFieldBoss/MonsterItem");

		Transform trContent = transform.Find("Scroll View/Viewport/Content");
		ToggleGroup toggleGroup = trContent.GetComponent<ToggleGroup>();
		
		foreach (var fieldBoss in CsGameData.Instance.FieldBossEvent.FieldBossList)
		{
			Transform trFieldBossItem = Instantiate(goMonsterItem, trContent).transform;

			trFieldBossItem.name = "FieldBoss" + fieldBoss.FieldBossId;

			Image imageMonsterIcon = trFieldBossItem.Find("ImageFrame/ImageMonsterIcon").GetComponent<Image>();
			imageMonsterIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + fieldBoss.ImageName);

			Text textMonsterLevel = trFieldBossItem.Find("TextMonsterLv").GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterLevel);
			textMonsterLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"),
				CsGameData.Instance.GetMonsterInfo(fieldBoss.MonsterArrange.MonsterId).Level);

			Text textMonsterName = trFieldBossItem.Find("TextMonsterName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterName);
			textMonsterName.text = string.Format(CsConfiguration.Instance.GetString("A115_TXT_01002"), fieldBoss.Name);

			Toggle toggle = trFieldBossItem.GetComponent<Toggle>();
			toggle.group = toggleGroup;

			toggle.isOn = false;
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((bIsOn) => OnValueChangedFieldBossToggle(bIsOn, fieldBoss));
		}

		if (trContent.childCount > 0)
		{
			trContent.GetChild(0).GetComponent<Toggle>().isOn = true;
		}

		// 출현 지역
		Text textSpawnLocate = transform.Find("TextSpawnLocate").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSpawnLocate);
		textSpawnLocate.text = CsConfiguration.Instance.GetString("A115_TXT_00002");

		// 출현 시간
		Text textSpawnTime = transform.Find("TextSpawnTime").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSpawnTime);
		textSpawnTime.text = CsConfiguration.Instance.GetString("A115_TXT_00003");

		Transform trSpawnTimeDetail = transform.Find("SpawnTimeDetail");

		foreach (var openSchedule in CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList)
		{
			TimeSpan tsStartTime = TimeSpan.FromSeconds(openSchedule.StartTime);
			
			Transform trText = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupFieldBoss/TextSpawnTimeDetail"), trSpawnTimeDetail).transform;

			Text text = trText.GetComponent<Text>();
			CsUIData.Instance.SetFont(text);
			text.text = string.Format(CsConfiguration.Instance.GetString("A115_TXT_00004"), tsStartTime.Hours.ToString("00"), tsStartTime.Minutes.ToString("00"));
		}

		// 획득가능 아이템
		Text textItemList = transform.Find("TextItemList").GetComponent<Text>();
		CsUIData.Instance.SetFont(textItemList);
		textItemList.text = CsConfiguration.Instance.GetString("A115_TXT_00005");

		Transform trItemList = transform.Find("ItemList");

		var AvailableRewardList = CsGameData.Instance.FieldBossEvent.FieldBossEventAvailableRewardList;

		foreach (var availableReward in AvailableRewardList)
		{
			Transform trItem = Instantiate(m_goItemSlot, trItemList).transform;
			trItem.name = "Item" + availableReward.Item.ItemId;

			CsUIData.Instance.DisplaySmallItemSlot(trItem, availableReward.Item, false, 0);
		}

		Button buttonStart = transform.Find("ButtonStart").GetComponent<Button>();
		buttonStart.onClick.RemoveAllListeners();
		buttonStart.onClick.AddListener(OnClickButtonStart);
		buttonStart.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textStart = transform.Find("ButtonStart/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textStart);
		textStart.text = CsConfiguration.Instance.GetString("A115_TXT_00006");
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonStart()
	{
		Vector3 vtPosition = new Vector3(m_csFieldBoss.XPosition, m_csFieldBoss.YPosition, m_csFieldBoss.ZPosition);

		CsGameEventToIngame.Instance.OnEventMapMove(m_csFieldBoss.Continent.ContinentId, CsGameData.Instance.MyHeroInfo.Nation.NationId, vtPosition);

		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedFieldBossToggle(bool bIsOn, CsFieldBoss csFieldBoss)
	{
		if (bIsOn)
		{
			m_csFieldBoss = csFieldBoss;

			CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(csFieldBoss.MonsterArrange.MonsterId);

			Text textMonsterLevel = transform.Find("TextMonsterLv").GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterLevel);
			textMonsterLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csMonsterInfo.Level);

			Text textMonsterName = transform.Find("TextMonsterName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textMonsterName);
			textMonsterName.text = string.Format(CsConfiguration.Instance.GetString("A115_TXT_01002"), csFieldBoss.Name);

			// 출현 지역
			Text textSpawmLocateDetail = transform.Find("TextSpawnLocateDetail").GetComponent<Text>();
			CsUIData.Instance.SetFont(textSpawmLocateDetail);
			textSpawmLocateDetail.text = string.Format(CsConfiguration.Instance.GetString("A115_TXT_01003"), csFieldBoss.Continent.Name);

			LoadMonsterModel(csMonsterInfo.MonsterCharacter);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void LoadMonsterModel(CsMonsterCharacter csMonsterCharacter)
	{
		Transform tr3DMosnter = transform.Find("3DMonster");
		
		for (int i = 0; i < tr3DMosnter.childCount; ++i)
        {
            if (tr3DMosnter.GetChild(i).GetComponent<Camera>() != null)
            {
                continue;
            }

            tr3DMosnter.GetChild(i).gameObject.SetActive(false);
        }

		if (csMonsterCharacter == null)
		{
		    return;
		}

		Transform trMountModel = transform.Find("3DMonster/" + csMonsterCharacter.PrefabName);

		if (trMountModel == null)
		{
			StartCoroutine(LoadMonsterModelCoroutine(csMonsterCharacter));
		}
		else
		{
			trMountModel.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadMonsterModelCoroutine(CsMonsterCharacter csMonsterCharacter)
	{
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + csMonsterCharacter.PrefabName);
		yield return resourceRequest;

		transform.Find("3DMonster/UIChar_Camera").gameObject.SetActive(true);

		GameObject goMonster = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DMonster"));

		int nLayer = LayerMask.NameToLayer("UIChar");

		Transform[] atrMount = goMonster.GetComponentsInChildren<Transform>();

		for (int i = 0; i < atrMount.Length; ++i)
		{
			atrMount[i].gameObject.layer = nLayer;
		}

		goMonster.transform.localPosition = new Vector3(0, -150, 500);
		goMonster.transform.eulerAngles = new Vector3(0, 180, 0);

		goMonster.transform.localScale = new Vector3(150, 150, 150);
		goMonster.name = csMonsterCharacter.PrefabName;
		goMonster.tag = "Untagged";
		goMonster.gameObject.SetActive(true);

	}
}
