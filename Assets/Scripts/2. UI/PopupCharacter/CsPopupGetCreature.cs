using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-10)
//---------------------------------------------------------------------------------------------------

public class CsPopupGetCreature : CsUpdateableMonoBehaviour
{
	List<CsHeroCreature> m_listCsHeroCreature;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Text textTitle = transform.Find("ImageFrameTitle/TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A146_TXT_00016");

		Button buttonOK = transform.Find("ButtonOK").GetComponent<Button>();
		buttonOK.onClick.RemoveAllListeners();
		buttonOK.onClick.AddListener(OnClickButtonOK);

		Text textOK = buttonOK.transform.Find("TextOK").GetComponent<Text>();
		CsUIData.Instance.SetFont(textOK);
		textOK.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");
	}

	//---------------------------------------------------------------------------------------------------
	public void DisplayCreatures(List<CsHeroCreature> heroCreatureList)
	{
		m_listCsHeroCreature = heroCreatureList;

		if (m_listCsHeroCreature.Count > 0)
		{
			DisplayCreature();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DisplayCreature()
	{
		if (m_listCsHeroCreature == null || m_listCsHeroCreature.Count <= 0)
			return;

		CsHeroCreature csHeroCreature = m_listCsHeroCreature[0];

		// 몬스터 모델링, 이름, 평점 세팅
		Text textName = transform.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name);

		Text textGrade = transform.Find("TextGrade").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGrade);
		textGrade.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_00030"), csHeroCreature.GetCreatureGrade().ToString("#,##0"));

		LoadCreatureModel(m_listCsHeroCreature[0].Creature);

		// 사용한 몬스터 정보 삭제
		m_listCsHeroCreature.RemoveAt(0);
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonOK()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		// 다음 몬스터가 있을 경우 출력
		if (m_listCsHeroCreature.Count > 0)
		{
			DisplayCreature();
		}
		else
		{
			PopupClose();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void LoadCreatureModel(CsCreature csCreature)
	{
		Transform tr3DCreature = transform.Find("3DCreature");

		for (int i = 0; i < tr3DCreature.childCount; ++i)
		{
			if (tr3DCreature.GetChild(i).GetComponent<Camera>() != null)
			{
				continue;
			}

			tr3DCreature.GetChild(i).gameObject.SetActive(false);
		}

		if (csCreature == null)
		{
			return;
		}

		Transform trMountModel = transform.Find("3DCreature/" + csCreature.CreatureCharacter.PrefabName);

		if (trMountModel == null)
		{
			StartCoroutine(LoadCreatureModelCoroutine(csCreature));
		}
		else
		{
			trMountModel.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadCreatureModelCoroutine(CsCreature csCreature)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + csCreature.CreatureCharacter.PrefabName);
		yield return resourceRequest;

		transform.Find("3DCreature/UIChar_Camera").gameObject.SetActive(true);

		GameObject goMonster = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DCreature"));

		int nLayer = LayerMask.NameToLayer("UIChar");

		Transform[] atrMount = goMonster.GetComponentsInChildren<Transform>();

		for (int i = 0; i < atrMount.Length; ++i)
		{
			atrMount[i].gameObject.layer = nLayer;
		}

		goMonster.transform.localPosition = new Vector3(0, -110, 500);
		goMonster.transform.eulerAngles = new Vector3(0, 180, 0);

		goMonster.transform.localScale = new Vector3(150, 150, 150);
		goMonster.name = csCreature.CreatureCharacter.PrefabName;
		goMonster.tag = "Untagged";
		goMonster.gameObject.SetActive(true);

	}
}
