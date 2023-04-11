using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupCreatureCompositionResult : CsUpdateableMonoBehaviour
{

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
		Button buttonBackground = transform.GetComponent<Button>();
		buttonBackground.onClick.RemoveAllListeners();
		buttonBackground.onClick.AddListener(OnClickButtonBackground);

		CsUIData.Instance.SetText(transform.Find("ImageTitle/TextTitle"), "A146_TXT_00048", true);
		CsUIData.Instance.SetText(transform.Find("TextDescription"), "A146_TXT_00049", true);
	}

	//---------------------------------------------------------------------------------------------------
	public void DisplayResult(CsHeroCreature csHeroCreature)
	{
		CsUIData.Instance.SetText(transform.Find("ImageBackground/TextValueLevelName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00027"), csHeroCreature.Level, csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name), false);

		Transform trLayoutLevel = transform.Find("ImageBackground/LayoutLevel");

		SetCreatureLevelStar(trLayoutLevel, csHeroCreature);

		LoadCreatureModel(csHeroCreature.Creature);

		Transform trImageFrameSkill = transform.Find("ImageBackground/ImageFrameSkill");

		for (int i = 0; i < trImageFrameSkill.childCount; i++)
		{
			Transform trSkill = trImageFrameSkill.GetChild(i);

			trSkill.Find("ImageLock").gameObject.SetActive(i >= CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + csHeroCreature.AdditionalOpenSkillSlotCount);

			CsHeroCreatureSkill csHeroCreatureSkill = csHeroCreature.HeroCreatureSkillList.Find(heroCreatureSkill => heroCreatureSkill.SlotIndex == i);
			trSkill.GetComponent<Image>().enabled = csHeroCreatureSkill != null;

			if (csHeroCreatureSkill != null)
			{
				CsUIData.Instance.SetImage(trSkill, "GUI/PopupCreature/" + csHeroCreatureSkill.CreatureSkill.ImageName);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void SetCreatureLevelStar(Transform trLayoutLevel, CsHeroCreature csHeroCreature)
	{
		for (int i = 0; i < trLayoutLevel.childCount; i++)
		{
			trLayoutLevel.GetChild(i).gameObject.SetActive(false);
		}

		GameObject goImageStar = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/ImageStar");

		int nStarLevel = csHeroCreature.InjectionLevel / 10 + 1;
		int nStarCount = csHeroCreature.InjectionLevel % 10;

		if (nStarCount == 0)
		{
			nStarCount += 10;
			nStarLevel -= 1;
		}

		int nFullStarCount = nStarCount / 2;

		Sprite spriteStarFull = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreature/ico_star" + nStarLevel.ToString() + "_equip_on");
		Sprite spriteStarHalf = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreature/ico_star" + nStarLevel.ToString() + "_equip_off");

		// 1개짜리 별
		for (int i = 0; i < nFullStarCount; i++)
		{
			Transform trStar = null;

			if (i < trLayoutLevel.childCount)
			{
				trStar = trLayoutLevel.GetChild(i);
				trStar.gameObject.SetActive(true);
			}
			else
			{
				trStar = Instantiate(goImageStar, trLayoutLevel).transform;
			}

			if (trStar != null)
			{
				trStar.name = "Star" + i.ToString();

				Image imageStar = trStar.GetComponent<Image>();
				imageStar.sprite = spriteStarFull;
			}
		}

		// 반개짜리 별
		if (nStarCount % 2 == 1)
		{
			Transform trStarHalf = null;

			if (nFullStarCount + 1 < trLayoutLevel.childCount)
			{
				trStarHalf = trLayoutLevel.GetChild(nFullStarCount + 1);
				trStarHalf.gameObject.SetActive(true);
			}
			else
			{
				trStarHalf = Instantiate(goImageStar, trLayoutLevel).transform;
			}

			if (trStarHalf != null)
			{
				trStarHalf.name = "Star" + nFullStarCount.ToString();

				Image imageStarHalf = trStarHalf.GetComponent<Image>();
				imageStarHalf.sprite = spriteStarHalf;
			}
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

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonBackground()
	{
		PopupClose();
	}
}
