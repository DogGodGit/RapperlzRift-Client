using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun16_WisdomTemple : CsSceneIngameDungeon
{
	GameObject m_goColorMatchingObject = null;

	Dictionary<string, CsWisdomTempleObject> m_dicWisdomTempleObject = new Dictionary<string, CsWisdomTempleObject>();
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventWisdomTempleEnter += OnEventWisdomTempleEnter;
		m_csDungeonManager.EventWisdomTempleStepStart += OnEventWisdomTempleStepStart;

		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionFinished += OnEventWisdomTempleColorMatchingObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectCheck += OnEventWisdomTempleColorMatchingObjectCheck;

		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterCreated += OnEventWisdomTempleColorMatchingMonsterCreated;
		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterKill += OnEventWisdomTempleColorMatchingMonsterKill;

		m_csDungeonManager.EventWisdomTemplePuzzleCompleted += OnEventWisdomTemplePuzzleCompleted;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionCancel += OnEventWisdomTemplePuzzleRewardObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionFinished += OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;

		m_csDungeonManager.EventWisdomTempleBossMonsterCreated += OnEventWisdomTempleBossMonsterCreated;

		m_goColorMatchingObject = transform.Find("ColorMatchingObject").gameObject;
		m_goColorMatchingObject.SetActive(false);
		m_csDungeonManager.SendWisdomTempleEnter();
		CsIngameData.Instance.InGameCamera.CameraCullDistance(45);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		DungeonExit();
		m_csDungeonManager.EventWisdomTempleEnter -= OnEventWisdomTempleEnter;
		m_csDungeonManager.EventWisdomTempleStepStart -= OnEventWisdomTempleStepStart;

		m_csDungeonManager.EventWisdomTempleColorMatchingObjectInteractionFinished -= OnEventWisdomTempleColorMatchingObjectInteractionFinished;
		m_csDungeonManager.EventWisdomTempleColorMatchingObjectCheck -= OnEventWisdomTempleColorMatchingObjectCheck;

		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterCreated -= OnEventWisdomTempleColorMatchingMonsterCreated;
		m_csDungeonManager.EventWisdomTempleColorMatchingMonsterKill -= OnEventWisdomTempleColorMatchingMonsterKill;

		m_csDungeonManager.EventWisdomTemplePuzzleCompleted -= OnEventWisdomTemplePuzzleCompleted;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionCancel -= OnEventWisdomTemplePuzzleRewardObjectInteractionCancel;
		m_csDungeonManager.EventWisdomTemplePuzzleRewardObjectInteractionFinished -= OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;

		m_csDungeonManager.EventWisdomTempleBossMonsterCreated -= OnEventWisdomTempleBossMonsterCreated;

		m_csDungeonManager.ResetDungeon();

		m_goColorMatchingObject = null;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY)
	{
		SetMyHeroLocation(m_csDungeonManager.WisdomTemple.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepStart(PDWisdomTempleMonsterInstance[] aMonsterInsts, PDWisdomTempleColorMatchingObjectInstance[] colorMatchingObjectInsts, int nQuizNo)
	{
		for (int i = 0; i < aMonsterInsts.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInsts[i]));
		}

		StartCoroutine(CreateColorMatchingObject(colorMatchingObjectInsts));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingObjectInteractionFinished(PDWisdomTempleColorMatchingObjectInstance pDWisdomTempleColorMatchingObjectInstance)
	{
		PDWisdomTempleColorMatchingObjectInstance[] aColorMatchingObjectInst = new PDWisdomTempleColorMatchingObjectInstance[1];
		aColorMatchingObjectInst[0] = pDWisdomTempleColorMatchingObjectInstance;

		StartCoroutine(CreateColorMatchingObject(aColorMatchingObjectInst));
		StartCoroutine(ColorMatchingObjectCheck());
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingObjectCheck(PDWisdomTempleColorMatchingObjectInstance[] colorMatchingObjectInsts, int nColorMatchingPoint)
	{
		if (colorMatchingObjectInsts != null)
		{
			StartCoroutine(CreateColorMatchingObject(colorMatchingObjectInsts, true));

			if (colorMatchingObjectInsts.Length > 0)
			{
				StartCoroutine(ColorMatchingObjectCheck());
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingMonsterCreated(PDWisdomTempleColorMatchingMonsterInstance monsterInst)
	{
		StartCoroutine(AsyncCreateMonster(monsterInst));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingMonsterKill(PDWisdomTempleColorMatchingObjectInstance[] colorMatchingObjectInsts, int nColorMatchingPoint)
	{
		StartCoroutine(CreateColorMatchingObject(colorMatchingObjectInsts, true, true));
		StartCoroutine(ColorMatchingObjectCheck());
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleCompleted(bool b, long l, PDWisdomTemplePuzzleRewardObjectInstance[] pDWisdomTemplePuzzleRewardObjectInstance)
	{
		StopCoroutine("ColorMatchingObjectCheck");
		m_bColorMatching = false;
		StartCoroutine(ColorMathingFinish(pDWisdomTemplePuzzleRewardObjectInstance));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleRewardObjectInteractionCancel()
	{
		StopCoroutine("ColorMatchingObjectCheck");
		m_bColorMatching = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleRewardObjectInteractionFinished(PDItemBooty booty, long lInstanceId)
	{
		if (m_dicWisdomTempleObject.ContainsKey(lInstanceId.ToString()))
		{
			m_dicWisdomTempleObject[lInstanceId.ToString()].InteractionFinish();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBossMonsterCreated(PDWisdomTempleBossMonsterInstance monsterInst)
	{
		StartCoroutine(AsyncCreateMonster(monsterInst));
	}

	#region ColorMatchingObject
	bool m_bColorMatching = false;
	//----------------------------------------------------------------------------------------------------
	IEnumerator CreateColorMatchingObject(PDWisdomTempleColorMatchingObjectInstance[] aColorMatchingObjectInst, bool bMatching = false, bool bBossKill = false)
	{
		Debug.Log("CsSceneDun16_WisdomTemple.CreateColorMatchingObject()");
		if (aColorMatchingObjectInst != null)
		{
			yield return new WaitUntil(() => (m_bColorMatching == false));
			m_bColorMatching = true;

			for (int i = 0; i < aColorMatchingObjectInst.Length; i++)
			{
				StartCoroutine(CreateColorMatchingObject(aColorMatchingObjectInst[i], bMatching, bBossKill));
			}

			yield return new WaitForSeconds(0.5f);

			int nCount = 0;
			while (true)
			{
				foreach (var dicColorMatchingObject in m_dicWisdomTempleObject)
				{
					if (dicColorMatchingObject.Value.IsIdle() == false)
					{
						nCount++;
					}
				}

				if (nCount == 0)
				{
					break;
				}

				nCount = 0;
				yield return new WaitForSeconds(0.1f);
			}

			m_bColorMatching = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator CreateColorMatchingObject(PDWisdomTempleColorMatchingObjectInstance colorMatchingObjectInst, bool bMatching, bool bBossKill)
	{
		if (colorMatchingObjectInst != null)
		{
			string strKey = colorMatchingObjectInst.row.ToString() + colorMatchingObjectInst.col.ToString();

			if (m_dicWisdomTempleObject.ContainsKey(strKey) == false)
			{
				GameObject goObject = Instantiate(m_goColorMatchingObject, transform.Find("Object")) as GameObject;

				goObject.AddComponent<CapsuleCollider>();
				goObject.AddComponent<CsWisdomTempleObject>();
				goObject.SetActive(false);

				CsWisdomTempleObject csWisdomTempleObject = goObject.GetComponent<CsWisdomTempleObject>();

				m_dicWisdomTempleObject.Add(strKey, csWisdomTempleObject);
				csWisdomTempleObject.Init(colorMatchingObjectInst.instanceId,
										  colorMatchingObjectInst.objectId,
										  colorMatchingObjectInst.row,
										  colorMatchingObjectInst.col,
										  CsRplzSession.Translate(colorMatchingObjectInst.position));

				yield return new WaitForSeconds(0.5f);
				goObject.SetActive(true);
			}
			else
			{
				m_dicWisdomTempleObject[strKey].ChangeObject(colorMatchingObjectInst.instanceId, colorMatchingObjectInst.objectId, bMatching, bBossKill);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ColorMatchingObjectCheck()
	{
		Debug.Log("CsSceneDun16_WisdomTemple.ColorMatchingObjectCheck()");
		yield return new WaitUntil(() => (m_bColorMatching == false));
		yield return new WaitForSeconds(0.5f);
		m_csDungeonManager.WisdomTempleColorMatchingObjectCheck();
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator ColorMathingFinish(PDWisdomTemplePuzzleRewardObjectInstance[] pDWisdomTemplePuzzleRewardObjectInstance)
	{
		Debug.Log("CsSceneDun16_WisdomTemple.ColorMathingFinish()");
		yield return new WaitUntil(() => (m_bColorMatching == false));

		foreach (var dicColorMatchingObject in m_dicWisdomTempleObject)
		{
			dicColorMatchingObject.Value.ColorMatchingFinish();
		}

		yield return new WaitForSeconds(2f);
		ClearColorMatchingObject();

		for (int i = 0; i < pDWisdomTemplePuzzleRewardObjectInstance.Length; i++)
		{
			GameObject goObject = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/InteractionObject/" + m_csDungeonManager.WisdomTemple.PuzzleRewardObjectPrefabName), transform.Find("Object")) as GameObject;
			Destroy(goObject.GetComponent<CsInteractionObject>());

			goObject.AddComponent<CapsuleCollider>();
			goObject.AddComponent<CsWisdomTempleObject>();
			goObject.SetActive(true);

			CsWisdomTempleObject csWisdomTempleObject = goObject.GetComponent<CsWisdomTempleObject>();
			csWisdomTempleObject.Init(pDWisdomTemplePuzzleRewardObjectInstance[i].instanceId,
									  m_csDungeonManager.WisdomTemple.PuzzleRewardObjectInteractionMaxRange,
									  m_csDungeonManager.WisdomTemple.PuzzleRewardObjectScale,
									  CsRplzSession.Translate(pDWisdomTemplePuzzleRewardObjectInstance[i].position));

			m_dicWisdomTempleObject.Add(pDWisdomTemplePuzzleRewardObjectInstance[i].instanceId.ToString(), csWisdomTempleObject);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void RemoveColorMatchingObject(int nRow, int nCol)
	{
		string strKey = nRow.ToString() + nCol.ToString();

		if (m_dicWisdomTempleObject.ContainsKey(strKey))
		{
			Destroy(m_dicWisdomTempleObject[strKey].gameObject);
			m_dicWisdomTempleObject.Remove(strKey);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void ClearColorMatchingObject()
	{
		foreach (var dicColorMatchingObject in m_dicWisdomTempleObject)
		{
			GameObject.Destroy(dicColorMatchingObject.Value.gameObject);
		}
		m_dicWisdomTempleObject.Clear();
	}
	#endregion InteractionManagement

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonWisdomTemple());
	}
}
