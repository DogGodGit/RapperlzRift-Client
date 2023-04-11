using ClientCommon;
using System;
using System.Collections;
using UnityEngine;

public class CsSceneDun07_OsirisRoom : CsSceneIngameDungeon
{
	GameObject m_goGlodItem = null;

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventOsirisRoomEnter += OnEventOsirisRoomEnter;
		m_csDungeonManager.EventOsirisRoomMonsterSpawn += OnEventOsirisRoomMonsterSpawn;
		m_csDungeonManager.EventOsirisRoomRewardGoldAcquisition += OnEventOsirisRoomRewardGoldAcquisition;
		m_csDungeonManager.EventOsirisRoomClear += OnEventOsirisRoomClear;
		m_csDungeonManager.EventOsirisRoomFail += OnEventOsirisRoomFail;
		
		m_csDungeonManager.OsirisRoomEnter();
		CreateGoldItem();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.EventOsirisRoomEnter -= OnEventOsirisRoomEnter;
		m_csDungeonManager.EventOsirisRoomMonsterSpawn -= OnEventOsirisRoomMonsterSpawn;
		m_csDungeonManager.EventOsirisRoomRewardGoldAcquisition -= OnEventOsirisRoomRewardGoldAcquisition;
		m_csDungeonManager.EventOsirisRoomClear -= OnEventOsirisRoomClear;
		m_csDungeonManager.EventOsirisRoomFail -= OnEventOsirisRoomFail;
		m_csDungeonManager.ResetDungeon();

		m_csDungeonManager = null;
		m_goGlodItem = null;
		base.OnDestroy();
	}

	#region OnEvent

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY)
	{
		Debug.Log("CsSceneDun07_OsirisRoom.OnEventOsirisRoomEnter");
		SetMyHeroLocation(m_csDungeonManager.OsirisRoom.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomMonsterSpawn(PDOsirisRoomMonsterInstance pDOsirisRoomMonsterInstance)
	{
		StartCoroutine(AsyncCreateMonster(pDOsirisRoomMonsterInstance, false, false));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomRewardGoldAcquisition()
	{
		// 골드 획득 연출
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomClear()
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
		StartCoroutine(DungeonClearDirection(new Vector3(21.07f, 0.8f, -8.38f), 0));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomFail()
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
	}

	#endregion OnEvent

	#region Setting

	//----------------------------------------------------------------------------------------------------
	void CreateGoldItem()
	{
		if (m_goGlodItem == null)
		{
			m_goGlodItem = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Effect/GoldItem"), Vector3.zero, Quaternion.identity, transform.Find("Object"));
			m_goGlodItem.name = "GlodItem";
			m_goGlodItem.SetActive(false);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void CreateGoldItem(Transform trMonster)
	{
		for (int i = 0; i < 7; i++)
		{
			Vector3 vtStartPos = UnityEngine.Random.insideUnitSphere * 3;
			vtStartPos = new Vector3(trMonster.position.x + vtStartPos.x, trMonster.position.y, trMonster.position.z + vtStartPos.z);
			StartCoroutine(MoveEffect(vtStartPos, Quaternion.identity));
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator MoveEffect(Vector3 vtStartPos, Quaternion qtnRotation)
	{
		if (m_goGlodItem != null)
		{
			GameObject goEffect = Instantiate(m_goGlodItem, vtStartPos, qtnRotation, transform.Find("Object"));
			float flTimer = Time.time;

			goEffect.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.5f);
			Vector3 vtTargetPos = m_csPlayer.transform.position;
			while (goEffect.transform.position != vtTargetPos)
			{
				if (Vector3.Distance(goEffect.transform.position, vtTargetPos) < 0.5f)
				{
					break;
				}
				else
				{
					if (Time.time - flTimer > 2)
					{
						break;
					}
				}

				goEffect.transform.position = Vector3.MoveTowards(goEffect.transform.position, vtTargetPos, Time.deltaTime * 20); // 제일 긴 거리의 0.3초 동안 속도 = 28 .
				yield return new WaitForEndOfFrame();
			}

			if (goEffect != null)
			{
				Destroy(goEffect);
			}
		}
		else
		{
			CreateGoldItem();
		}
	}

	#endregion Setting

	//----------------------------------------------------------------------------------------------------
	//public override void StartClearDiretion()
	//{
	//	//if (m_bClearDiretion) return;

	//	//Debug.Log("##############  StartClearDiretion()  override ################ ");
	//	//m_bClearDiretion = true;
	//	//StartCoroutine(DungeonClearDirection(new Vector3(21.07f, 0.8f, -8.38f), 0));
	//}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonOsirisRoom());
	}
}
