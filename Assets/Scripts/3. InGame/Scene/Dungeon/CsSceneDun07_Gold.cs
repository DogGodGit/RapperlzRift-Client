using ClientCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CsSceneDun07_Gold : CsSceneIngameDungeon 
{
	//List<CsMonster> m_listGoldMonstersArrange = new List<CsMonster>();
 //   CsMonster m_csBossMonster;

 //   //---------------------------------------------------------------------------------------------------
 //   protected override void Start()
 //   {
 //       base.Start();
	//	m_csDungeonManager = CsDungeonManager.Instance;
	//	m_csDungeonManager.EventContinentExitForGoldDungeonEnter += OnEventContinentExitForGoldDungeonEnter;
	//	m_csDungeonManager.EventGoldDungeonEnter += OnEventGoldDungeonEnter;
	//	m_csDungeonManager.EventGoldDungeonStepStart += OnEventGoldDungeonStepStart;
	//	m_csDungeonManager.EventGoldDungeonWaveStart += OnEventGoldDungeonWaveStart;
	//	m_csDungeonManager.EventGoldDungeonWaveTimeout += OnEventGoldDungeonWaveTimeOut;
	//	m_csDungeonManager.EventGoldDungeonRevive += OnEventGoldDungeonRevive;

	//	m_csDungeonManager.EventGoldDungeonFail += OnEventGoldDungeonFail;
	//	m_csDungeonManager.EventGoldDungeonExit += OnEventGoldDungeonExit;
	//	m_csDungeonManager.EventGoldDungeonBanished += OnEventGoldDungeonBanished;
	//	m_csDungeonManager.EventGoldDungeonClear += OnEventGoldDungeonClear;

	//	m_csDungeonManager.SendGoldDungeonEnter();
	//}

 //   //---------------------------------------------------------------------------------------------------
 //   protected override void OnDestroy()
 //   {
	//	m_csDungeonManager.EventContinentExitForGoldDungeonEnter -= OnEventContinentExitForGoldDungeonEnter;
	//	m_csDungeonManager.EventGoldDungeonEnter -= OnEventGoldDungeonEnter;
	//	m_csDungeonManager.EventGoldDungeonStepStart -= OnEventGoldDungeonStepStart;
	//	m_csDungeonManager.EventGoldDungeonWaveStart -= OnEventGoldDungeonWaveStart;
	//	m_csDungeonManager.EventGoldDungeonWaveTimeout -= OnEventGoldDungeonWaveTimeOut;
	//	m_csDungeonManager.EventGoldDungeonRevive -= OnEventGoldDungeonRevive;

	//	m_csDungeonManager.EventGoldDungeonFail -= OnEventGoldDungeonFail;
	//	m_csDungeonManager.EventGoldDungeonExit -= OnEventGoldDungeonExit;
	//	m_csDungeonManager.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;
	//	m_csDungeonManager.EventGoldDungeonClear -= OnEventGoldDungeonClear;
	//	m_csDungeonManager.ResetDungeon();

	//	CsIngameData.Instance.MyHeroDead = false;
	//	m_csDungeonManager = null;
	//	base.OnDestroy();
 //   }

	//#region OnEvent

	////---------------------------------------------------------------------------------------------------
	//void OnEventContinentExitForGoldDungeonEnter() // 던전 입장. 탈것 초기화.
	//{
	//	DungeonEnter();
	//}

	////---------------------------------------------------------------------------------------------------
	//void OnEventGoldDungeonEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
	//{
	//	Debug.Log("CsSceneDun07_Gold.OnEventGoldDungeonEnter");
	//	SetMyHeroLocation(m_csDungeonManager.GoldDungeon.LocationId);
	//	SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
	//}

 //   //---------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonStepStart(PDGoldDungeonMonsterInstance[] apGoldDungeonMonsterInstance) 
 //   {
	//	Debug.Log("CsSceneDun07_Gold.OnEventGoldDungeonStepStart");
	//	for (int i = 0; i < apGoldDungeonMonsterInstance.Length; i++)
	//	{
	//		CreateGoldMonster(apGoldDungeonMonsterInstance[i]);
	//	}
		
	//	for (int i = 0; i < m_csDungeonManager.GoldDungeonStep.GoldDungeonStepMonsterArrangeList.Count; i++)
	//	{
	//		CreateGoldMonsterArrange(m_csDungeonManager.GoldDungeonStep.GoldDungeonStepMonsterArrangeList[i]);
	//	}
	//}

 //   //---------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonWaveStart(int waveNo)
 //   {
	//	for (int i = 0; i < m_dicMonsters.Count; i++)
	//	{
	//		m_dicMonsters.Values.ToList()[i].NetEventGoldDungeonWaveStart(waveNo);
	//	}

	//	for (int i = 0; i < m_listGoldMonstersArrange.Count; i++)
	//	{
	//		m_listGoldMonstersArrange[i].NetEventGoldDungeonWaveStart(waveNo);
	//	}
 //   }

 //   //---------------------------------------------------------------------------------------------------
	//void OnEventGoldDungeonWaveTimeOut() // 시간 오버 및 fail관련 처리
 //   {
 //       ClearMonster();
 //   }

 //   //---------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonRevive()
 //   {
 //       m_csPlayer.NetEventRevive();
 //   }

	////----------------------------------------------------------------------------------------------------
	//void OnEventGoldDungeonFail()
	//{
	//	ClearMonster();
	//}

 //   //----------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonExit(int nPreviousContinentId)
 //   {
 //       DungeonExit();
 //   }

 //   //----------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonBanished(int nPreviousContinentId)
 //   {
 //       DungeonExit();
 //   }

 //   //----------------------------------------------------------------------------------------------------
 //   void OnEventGoldDungeonClear(long lRewardGold)
 //   {
 //       //StartCoroutine(DungeonClearDirection());
 //   }

	//#endregion OnEvent

	//#region Setting

	//// 골드던전 몬스터 생성
	////----------------------------------------------------------------------------------------------------
	//protected void CreateGoldMonster(PDGoldDungeonMonsterInstance pdMon)
	//{
	//	if (pdMon == null) return;
	//	Debug.Log("Step" + pdMon.stepNo + "Wavevo" + pdMon.activationWaveNo);
	//	if (m_csDungeonManager.GoldDungeonStep.Step == 4)
 //       {
 //           m_csBossMonster = CreateMonster((PDMonsterInstance)pdMon);
 //           m_csBossMonster.NetEventSetGoldDungeonMonster(pdMon.stepNo, pdMon.activationWaveNo);
 //       }
 //       else
 //       {
 //           CsMonster csGoldMonster = CreateMonster((PDMonsterInstance)pdMon); // 기본정보로 몬스터 생성.
 //           csGoldMonster.NetEventSetGoldDungeonMonster(pdMon.stepNo, pdMon.activationWaveNo);
 //       }
	//}

	//// 도망치는 배경 몬스터 생성.
	////----------------------------------------------------------------------------------------------------
	//protected void CreateGoldMonsterArrange(CsGoldDungeonStepMonsterArrange pdMonArrange)
	//{
	//	if (pdMonArrange == null) return;

	//	CsMonsterInfo csMonsterInfo = CsGameData.Instance.MonsterInfoList.Find(a => a.MonsterId == pdMonArrange.MonsterArrange.MonsterId); 
	//	GameObject goMon = null;

	//	if (csMonsterInfo == null) return;

	//	GameObject goMonResources = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/MonsterObject/{0:D2}", csMonsterInfo.MonsterCharacter.PrefabName));

	//	if (goMonResources == null)
	//	{
	//		Debug.Log("CreateMonster         프리펩 네임 확인 필요         PrefabName = " + csMonsterInfo.MonsterCharacter.PrefabName);
	//		return;
	//	}
	//	goMon = Instantiate(goMonResources, transform.Find("Monster")) as GameObject; 

	//	CsMonster csMon = goMon.GetComponent<CsMonster>();

	//	csMon.GoldDungeonMonsterInit(csMonsterInfo,
	//								 MonsterInstanceType.GoldDungeonMonster,
	//								 pdMonArrange.MonsterArrange.MonsterId,
	//								 pdMonArrange.Position,
	//								 pdMonArrange.YRotation,
	//								 pdMonArrange.Step,
	//								 pdMonArrange.ActivationWaveNo,
	//								 pdMonArrange.IsFugitive);
		
	//	m_listGoldMonstersArrange.Add(csMon);
	//}

	//#endregion Setting

	////----------------------------------------------------------------------------------------------------
	//public override void InitPlayThemes()
	//{
	//	base.InitPlayThemes();
	//	AddPlayTheme(new CsPlayThemeDungeonGold());
	//}
}
