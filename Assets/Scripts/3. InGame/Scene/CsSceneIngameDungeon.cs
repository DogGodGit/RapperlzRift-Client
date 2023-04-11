using ClientCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneIngameDungeon : CsSceneIngame
{
	protected bool m_bClearDiretion = false;

	protected CsDungeonManager m_csDungeonManager;
	protected List<CsBuffBoxArea> m_listBuffArea = new List<CsBuffBoxArea>();

	Dictionary<string, GameObject> m_dicDungeonEffect = new Dictionary<string, GameObject>();

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator DirectingByDungeonEnter() // 던전 입장 연출.( 1차완료 테스트 필요).
	{
		CsIngameData.Instance.Directing = true;
		Debug.Log("DirectingByDungeonEnter     ===Start===");
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		yield return new WaitForSeconds(0.5f);

		Vector3 vtOffestPos = m_csPlayer.transform.forward * 7;
		Vector3 vtHeroStartPos = m_csPlayer.transform.position;
		m_csPlayer.transform.position = m_csPlayer.transform.position - vtOffestPos;
		m_csPlayer.LookAtPosition(vtHeroStartPos);
		CsGameEventToUI.Instance.OnEventHideMainUI(true);
		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.EnterAction1);
		Debug.Log("DirectingByDungeonEnter     ===Run===");
		m_csPlayer.MoveByDirecting(vtHeroStartPos, 0.2f, true);
		yield return new WaitForSeconds(1f);
	
		m_csPlayer.MoveByDirecting(vtHeroStartPos, 0.2f, false);
		Debug.Log("DirectingByDungeonEnter     ===Walk===");
		yield return new WaitForSeconds(0.5f);

		m_csPlayer.ChangeState(CsHero.EnState.Idle);
		m_csPlayer.NavMeshSetting(); // 강제 이동후 초기화 해줘야 동작됨.
		Debug.Log("DirectingByDungeonEnter     ===IDLE===");
		yield return new WaitForSeconds(0.3f);

		m_csPlayer.NetEventBattleModeStart();
		yield return new WaitForSeconds(0.6f);

		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.EnterAction2);
		yield return new WaitForSeconds(0.8f);

		CsGameEventToUI.Instance.OnEventHideMainUI(false);
		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);

		CsIngameData.Instance.Directing = false;
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator BossAppearanceDirection(CsMonster csMonster) // 보스 등장.
	{
		if (csMonster != null)
		{
			Debug.Log("1. ****************               BossAppearanceDirection           **********************");
			CsIngameData.Instance.Directing = true;
			CsGameEventToUI.Instance.OnEventHideMainUI(true);
			m_csPlayer.SetAutoPlay(null, false);
			m_csPlayer.ResetBattleMode();                       // 자동전투 종료.
			yield return new WaitForSeconds(0.3f);

			m_csPlayer.ChangeState(CsHero.EnState.Idle);
			CsGameEventToUI.Instance.OnEventJoystickReset();
			yield return new WaitForSeconds(0.1f);
			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);

			Debug.Log("2. ****************               BossAppearanceDirection           **********************");
			CsGameEventToUI.Instance.OnEventBossAppear(csMonster.MonsterInfo.Name, true);
			m_csPlayer.SelectTarget(csMonster.transform, true);
			Vector3 vtPos = csMonster.transform.position + (csMonster.transform.forward * 11);
			m_csPlayer.transform.position = vtPos;
			yield return new WaitForSeconds(0.1f);

			m_csPlayer.transform.position = vtPos;
			m_csPlayer.transform.LookAt(csMonster.transform);
			yield return new WaitForSeconds(0.1f);

			csMonster.BossStateSetting(true);
			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.BossAppear);
			yield return new WaitForSeconds(1f);

			csMonster.BossMotionSetting(CsMonster.EnAnimStatus.Skill01);
			yield return new WaitForSeconds(2f);

			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
			CsGameEventToUI.Instance.OnEventBossAppear(csMonster.MonsterInfo.Name, false);
			yield return new WaitForSeconds(0.6f);

			m_csPlayer.ChangeState(CsHero.EnState.Idle);
			csMonster.BossMotionSetting(CsMonster.EnAnimStatus.Idle);
			yield return new WaitForSeconds(1.7f);

			CsGameEventToUI.Instance.OnEventHideMainUI(false);
			csMonster.BossStateSetting(false);

			CsIngameData.Instance.Directing = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator DungeonClearDirection(Vector3 vtPos, float flRotationY, float flDelayTime = 1.0f)
	{
		if (m_csPlayer.Dead)
		{
			ClearHero();
			CsGameEventToUI.Instance.OnEventClearDirectionFinish(CsDungeonManager.Instance.DungeonPlay);
		}
		else
		{
			CsIngameData.Instance.Directing = true;
			m_csPlayer.SetAutoPlay(null, true);
			m_csPlayer.MyHeroNavMeshAgent.enabled = false;
			CsGameEventToUI.Instance.OnEventJoystickReset();
			m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
			yield return new WaitForSeconds(flDelayTime);
			yield return new WaitForSeconds(0.5f);
		
			CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Clear);
			m_csPlayer.transform.position = vtPos;
			m_csPlayer.ChangeEulerAngles(flRotationY);
			ClearHero();
			yield return new WaitForSeconds(0.8f);

			m_csPlayer.MyHeroNavMeshAgent.enabled = true;
			m_csPlayer.DungeonClear(); // 추후 변경 필요	
			yield return new WaitForSeconds(0.8f);

			CsGameEventToUI.Instance.OnEventClearDirectionFinish(CsDungeonManager.Instance.DungeonPlay);
			CsIngameData.Instance.Directing = false;
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void CreateBuffObject(PDInfiniteWarBuffBoxInstance pDInfiniteWarBuffBoxInstance)
	{
		CsInfiniteWarBuffBox csInfiniteWarBuffBox;
		Transform trArea = transform.Find("Area");

		csInfiniteWarBuffBox = m_csDungeonManager.InfiniteWar.GetInfiniteWarBuffBox(pDInfiniteWarBuffBoxInstance.id);

		if (csInfiniteWarBuffBox != null)
		{
			GameObject goBuffArea = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Area/" + csInfiniteWarBuffBox.PrefabName), trArea) as GameObject;
			Debug.Log("CreateBuffObject      goBuffArea = " + goBuffArea);
			goBuffArea.transform.position = CsRplzSession.Translate(pDInfiniteWarBuffBoxInstance.position);
			goBuffArea.name = csInfiniteWarBuffBox.PrefabName;

			CsBuffBoxArea csBuffBoxArea = goBuffArea.GetComponent<CsBuffBoxArea>();
			csBuffBoxArea.Init(pDInfiniteWarBuffBoxInstance.instanceId, csInfiniteWarBuffBox);
			m_listBuffArea.Add(csBuffBoxArea);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void CreateBuffObject(PDProofOfValorBuffBoxInstance pDProofOfValorBuffBoxInstance)
	{
		CsProofOfValorBuffBox csProofOfValorBuffBox;
		CsProofOfValorBuffBoxArrange csProofOfValorBuffBoxArrange;
		Transform trArea = transform.Find("Area");

		for (int i = 0; i < CsGameData.Instance.ProofOfValor.ProofOfValorBuffBoxList.Count; i++)
		{
			csProofOfValorBuffBox = CsGameData.Instance.ProofOfValor.ProofOfValorBuffBoxList[i];
			csProofOfValorBuffBoxArrange = csProofOfValorBuffBox.GetProofOfValorBuffBoxArrange(pDProofOfValorBuffBoxInstance.arrangeId);
			if (csProofOfValorBuffBoxArrange != null)
			{
				GameObject goBuffArea = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Area/" + csProofOfValorBuffBox.PrefabName), CsRplzSession.Translate(pDProofOfValorBuffBoxInstance.position),
													Quaternion.Euler(new Vector3(0f, pDProofOfValorBuffBoxInstance.rotationY, 0)), trArea) as GameObject;

				CsBuffBoxArea csBuffBoxArea = goBuffArea.GetComponent<CsBuffBoxArea>();
				csBuffBoxArea.Init(pDProofOfValorBuffBoxInstance.instanceId, csProofOfValorBuffBox, csProofOfValorBuffBoxArrange);
				m_listBuffArea.Add(csBuffBoxArea);
				break;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveBuffObject(long lInstanceId)
	{
		CsBuffBoxArea csBuffBoxArea = m_listBuffArea.Find(a => a.InstanceId == lInstanceId);
		if (csBuffBoxArea != null)
		{
			if (m_listBuffArea.Contains(csBuffBoxArea))
			{
				m_listBuffArea.Remove(csBuffBoxArea);
			}

			csBuffBoxArea.Destroy();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearBuffObject()
	{
		for (int i = 0; i < m_listBuffArea.Count; i++)
		{
			m_listBuffArea[i].Destroy();
		}
		m_listBuffArea.Clear();
	}

	//------------------------------------------------------------------------------------------
	protected IEnumerator StartLoadEffect(string[] astr)
	{
		GameObject goEffect = new GameObject();
		goEffect.transform.name = "Effect";

		for (int i = 0; i < astr.Length; i++)
		{
			ResourceRequest reqPrefab = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Effect/" + astr[i]);
			yield return reqPrefab;

			GameObject go = GameObject.Instantiate((GameObject)reqPrefab.asset, Vector3.zero, Quaternion.identity, goEffect.transform) as GameObject;
			go.transform.name = go.transform.name.Replace("(Clone)", "");
			go.transform.SetParent(gameObject.transform);
			go.SetActive(false);

			if (m_dicDungeonEffect.ContainsKey(go.transform.name) == false)
			{
				m_dicDungeonEffect.Add(go.transform.name, go);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator NormalEffect(string strEffectName, Transform trOwner, Vector3 vtPosition, Quaternion qtnRotation, float flSec)
	{
		if (m_dicDungeonEffect.ContainsKey(strEffectName))
		{
			GameObject goEffect = Instantiate(m_dicDungeonEffect[strEffectName].gameObject, vtPosition, qtnRotation, trOwner);

			goEffect.gameObject.SetActive(true);

			yield return new WaitForSeconds(flSec);

			if (goEffect != null)
			{
				Destroy(goEffect);
			}
		}
	}
}