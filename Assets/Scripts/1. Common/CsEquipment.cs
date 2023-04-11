using System.Collections;
using UnityEngine;

public enum EnEquipType
{
	Weapon = 1,
	Armor = 2,
	Face = 3,
	Hair = 4
}

public class CsEquipment : MonoBehaviour
{
	CsHeroCustomData m_csHeroCustomData;

	GameObject m_goCape = null;
	GameObject m_goArmor = null;
	GameObject m_goWeapon = null;
	GameObject m_goArrow = null;
	GameObject m_goFace = null;
	GameObject m_goHair = null;
	GameObject m_goFishStaff = null;
	GameObject m_goWing = null;
	GameObject m_goCostumeEffect = null;
	GameObject m_goArtifact = null;

	bool m_bHelmet = false;
	bool m_bMid = false;
	bool m_bInGame = false;

	float m_flLegsLengthOffset = 0;

	public float LegsLengthOffset { get { return m_flLegsLengthOffset; } }

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		RemoveEquipments();
		m_csHeroCustomData = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void LowChangEquipments(CsHeroCustomData csHeroCustomData)
	{
		//Debug.Log("LowChangEquipments");
		m_csHeroCustomData = csHeroCustomData;
		m_bMid = false;
		m_bInGame = true;
		CsCustomizingManager.Instance.SetCustomData(m_csHeroCustomData);

		RemoveEquipment(EnEquipType.Weapon);
		ChangeWeapon(m_csHeroCustomData.JobId, csHeroCustomData.WeaponId, 0);

		RemoveCostumeEffect();
		RemoveEquipment(EnEquipType.Armor);

		CsCostume csCostume = CsGameData.Instance.GetCostume(csHeroCustomData.CostumeId);
		if (csCostume != null)
		{
			ChangeCostum(csCostume, CsGameData.Instance.GetCostumeEffect(csHeroCustomData.CostumeEffectId));
		}
		else
		{
			ChangeArmor(m_csHeroCustomData.JobId, csHeroCustomData.ArmorId, csHeroCustomData.CustomPresetHair, 0);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void MidChangeEquipments(CsHeroCustomData csHeroCustomData, bool bInGame = true)
	{
		Debug.Log("MidChangeEquipments  "+ bInGame);
		m_csHeroCustomData = csHeroCustomData;
		m_bMid = true;
		m_bInGame = bInGame;
		CsCustomizingManager.Instance.SetCustomData(m_csHeroCustomData);

		RemoveEquipment(EnEquipType.Weapon);
		ChangeWeapon(m_csHeroCustomData.JobId, csHeroCustomData.WeaponId, 0);

		RemoveCostumeEffect();
		RemoveEquipment(EnEquipType.Armor);

		CsCostume csCostume = CsGameData.Instance.GetCostume(csHeroCustomData.CostumeId);
		if (csCostume != null)
		{
			ChangeCostum(csCostume, CsGameData.Instance.GetCostumeEffect(csHeroCustomData.CostumeEffectId));
		}
		else
		{
			ChangeArmor(m_csHeroCustomData.JobId, csHeroCustomData.ArmorId, csHeroCustomData.CustomPresetHair, 0);
		}
	}


	#region EquipmentsManagement

	//---------------------------------------------------------------------------------------------------
	void ChangeWeapon(int nJobId, int nWeaponId, int nEnchantLevel)
	{
		string strPrefabName = GetPrefabName(EnEquipType.Weapon, nJobId, nWeaponId);
		
		if (m_goWeapon == null || m_goWeapon.name != strPrefabName)
		{
			if (m_bMid)
			{
				GameObject goWeapon = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroWeapon/" + strPrefabName);
				//GameObject goWeapon = Resources.Load("Prefab/SelectHero/HeroWeapon/" + strPrefabName) as GameObject;
				CreateWeapon(goWeapon, strPrefabName);
			}
			else
			{
				StartCoroutine(AsyncChangeWeapon(nJobId, nWeaponId, nEnchantLevel, strPrefabName));
			}
		}
		else
		{
			m_goWeapon.SetActive(true);
			if (m_goArrow != null)
			{
				m_goArrow.SetActive(true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator AsyncChangeWeapon(int nJobId, int nWeaponId, int nEnchantLevel, string strPrefabName)
	{
		GameObject goWeapon = null;

		if (CsIngameData.Instance.Weapon.ContainsKey(strPrefabName))
		{
			goWeapon = CsIngameData.Instance.Weapon[strPrefabName];
		}
		else
		{
			ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroWeapon/" + strPrefabName);
			yield return req;
			goWeapon = (GameObject)req.asset;
		}

		CreateWeapon(goWeapon, strPrefabName);
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeArmor(int nJobId, int nArmorId, int nHairId, int nEnchantLevel)
	{
		//Debug.Log("ChangeArmor  : " + nArmorId);
		string strPrefabName = GetPrefabName(EnEquipType.Armor, nJobId, nArmorId);
		string strKey = nJobId.ToString() + strPrefabName;

		if (m_goArmor == null || m_goArmor.name != strPrefabName)
		{
			if (m_bMid)
			{
				GameObject goArmor = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroArmor/" + nJobId.ToString() + "/" + strPrefabName);				
				CreateArmor(goArmor, strPrefabName);

				RemoveEquipment(EnEquipType.Face);
				ChangeFace(nJobId, strPrefabName, m_bMid); // 얼굴 생성.

				RemoveEquipment(EnEquipType.Hair);

				if ((EnJob)nJobId == EnJob.Witch)
				{
					if (strPrefabName == "104" || strPrefabName == "105" || strPrefabName == "106" || strPrefabName == "107")
					{
						nHairId = 1;
					}
				}

				ChangeHair(nJobId, nHairId, m_bMid);
				CsCustomizingManager.Instance.SetMyHeroCustom(m_csHeroCustomData, transform, m_bMid);
			}
			else
			{
				StartCoroutine(AsyncChangeArmor(nJobId, nArmorId, nHairId, nEnchantLevel, strPrefabName, strKey));
			}
		}
		else
		{
			m_goArmor.SetActive(true);

			if (m_goCape != null)
			{
				m_goCape.SetActive(true);
			}

			if (m_goFace != null)
			{
				m_goFace.SetActive(true);
			}

			if (m_goHair != null)
			{
				m_goHair.SetActive(true);
			}
			CsCustomizingManager.Instance.SetMyHeroCustom(m_csHeroCustomData, transform, m_bMid);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator AsyncChangeArmor(int nJobId, int nArmorId, int nHairId, int nEnchantLevel, string strPrefabName, string strKey)
	{
		GameObject goArmor = null;
		if (CsIngameData.Instance.Armor.ContainsKey(strKey))
		{
			goArmor = CsIngameData.Instance.Armor[strKey];
		}
		else
		{
			ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroArmor/" + nJobId.ToString() + "/" + strPrefabName);			
			yield return req;
			goArmor = (GameObject)req.asset;
		}

		CreateArmor(goArmor, strPrefabName);

		RemoveEquipment(EnEquipType.Face);
		ChangeFace(nJobId, strPrefabName, m_bMid); // 얼굴 생성.

		RemoveEquipment(EnEquipType.Hair);
		if ((EnJob)nJobId == EnJob.Witch)
		{
			if (strPrefabName == "104" || strPrefabName == "105" || strPrefabName == "106" || strPrefabName == "107")
			{
				nHairId = 1;
			}
		}

		ChangeHair(nJobId, nHairId, m_bMid);
		CsCustomizingManager.Instance.SetMyHeroCustom(m_csHeroCustomData, transform, m_bMid);
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeCostum(CsCostume csCostume, CsCostumeEffect csCostumeEffect)
	{
		Debug.Log("ChangeCostum             JobId = " + m_csHeroCustomData.JobId);

		if (m_bMid)
		{
			GameObject goCostume = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroArmor/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostume.PrefabName);
			CreateArmor(goCostume, csCostume.PrefabName);

			CsCostumeDisplay csCostumeDisplay = csCostume.CostumeDisplayList.Find(a => a.JobId == m_csHeroCustomData.JobId);

			RemoveEquipment(EnEquipType.Face);
			RemoveEquipment(EnEquipType.Hair);

			if (csCostumeDisplay != null)
			{
				GameObject goFace = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroFace/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostumeDisplay.FacePrefabName);
				CreateFace(goFace, csCostumeDisplay.FacePrefabName);

				m_bHelmet = false;
				GameObject goHair = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroHair/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostumeDisplay.HairPrefabName);
				CreateHair(goHair, csCostumeDisplay.HairPrefabName);
			}
			else
			{
				Debug.Log("ChangeCostum    csCostumeDisplay == null");
			}

			CreateCostumeEffect(csCostumeEffect);
			CsCustomizingManager.Instance.SetMyHeroCustom(m_csHeroCustomData, transform, true);
		}
		else
		{
			StartCoroutine(AsyncChangeCostum(csCostume, csCostumeEffect));
		}		
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator AsyncChangeCostum(CsCostume csCostume, CsCostumeEffect csCostumeEffect)
	{
		ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroArmor/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostume.PrefabName);
		yield return req;

		CreateArmor((GameObject)req.asset, csCostume.PrefabName);

		CsCostumeDisplay csCostumeDisplay = csCostume.CostumeDisplayList.Find(a => a.JobId == m_csHeroCustomData.JobId);
		RemoveEquipment(EnEquipType.Face);
		RemoveEquipment(EnEquipType.Hair);

		if (csCostumeDisplay != null)
		{
			GameObject goFace = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/HeroFace/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostumeDisplay.FacePrefabName);
			CreateFace(goFace, csCostumeDisplay.FacePrefabName);

			m_bHelmet = false;
			GameObject goHair = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/HeroHair/" + m_csHeroCustomData.JobId.ToString() + "/" + csCostumeDisplay.HairPrefabName);
			CreateHair(goHair, csCostumeDisplay.HairPrefabName);
		}
		else
		{
			Debug.Log("AsyncChangeCostum    ChangeCostum    csCostumeDisplay == null");
		}

		CreateCostumeEffect(csCostumeEffect);
		CsCustomizingManager.Instance.SetMyHeroCustom(m_csHeroCustomData, transform, false);
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeFace(int nJobId, string strArmorPrefabName, bool bMid = false)
	{
		int nArmorPrefab = int.Parse(strArmorPrefabName);
		int nFaceId = 1;
		GameObject goFace = null;

		m_bHelmet = false;

		switch ((EnJob)nJobId)
		{
			case EnJob.Gaia:
				if (107 == nArmorPrefab)
				{
					nFaceId = 2;
					m_bHelmet = true;
				}
				else if (108 == nArmorPrefab)
				{
					nFaceId = 3;
					m_bHelmet = true;
				}
				break;
			case EnJob.Asura:
				if (106 == nArmorPrefab)
				{
					m_bHelmet = true;
				}
				else if(107 == nArmorPrefab)
				{
					nFaceId = 2;
					m_bHelmet = true;
				}
				else if (108 == nArmorPrefab)
				{
					nFaceId = 3;
				}
				break;
			case EnJob.Deva:
				if (107 == nArmorPrefab)
				{
					nFaceId = 2;
					m_bHelmet = true;
				}
				else if (108 == nArmorPrefab)
				{
					nFaceId = 3;
					m_bHelmet = true;
				}
				break;
			case EnJob.Witch:
				if (107 == nArmorPrefab)
				{
					nFaceId = 2;
				}
				else if (108 == nArmorPrefab)
				{
					nFaceId = 3;
				}
				break;
		}

		Debug.Log("ChangeFace   : " + nFaceId + " , " + m_bHelmet);

		if (bMid)
		{
			goFace = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroFace/" + nJobId.ToString() + "/" + nFaceId.ToString());
		}
		else
		{
			string strKey = nJobId.ToString() + nFaceId.ToString();

			if (CsIngameData.Instance.Face.ContainsKey(strKey))
			{
				goFace = CsIngameData.Instance.Face[strKey];
			}
			else
			{
				goFace = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/HeroFace/" + nJobId.ToString() + "/" + nFaceId.ToString());
			}
		}

		CreateFace(goFace, nFaceId.ToString());
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeHair(int nJobId, int nHairId, bool bMid = false)
	{
		if (m_bHelmet)
		{
			Debug.Log("ChangeHair    No Hair  >>  Equipted a Helmet : " + m_bHelmet);
		}
		else
		{
			GameObject goHair = null;
			
			string strPrefabName = GetPrefabName(EnEquipType.Hair, nJobId, nHairId);
			Debug.Log("ChangeHair    : " + nHairId + " , " + nHairId + " , " + strPrefabName);
			if (m_goHair == null || m_goHair.name != strPrefabName)
			{
				if (bMid)
				{
					goHair = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/SelectHero/HeroHair/" + nJobId.ToString() + "/" + strPrefabName);					
				}
				else
				{
					string strKey = nJobId.ToString() + strPrefabName;

					if (CsIngameData.Instance.Hair.ContainsKey(strKey))
					{						
						goHair = CsIngameData.Instance.Hair[strKey];
					}
					else
					{
						goHair = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/HeroHair/" + nJobId.ToString() + "/" + strPrefabName);						
					}
				}

				CreateHair(goHair, strPrefabName);
			}
			else
			{
				m_goHair.SetActive(true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateWeapon(GameObject goWeapon, string strPrefabName)
	{
		if (goWeapon != null)
		{
			SkinnedMeshRenderer[] skinrenderer = goWeapon.GetComponentsInChildren<SkinnedMeshRenderer>();

			if (skinrenderer != null)
			{
				m_goWeapon = ProcessBonedObject(skinrenderer[0]);
				m_goWeapon.name = strPrefabName;
				
				if (skinrenderer.Length > 1)
				{
					m_goArrow = ProcessBonedObject(skinrenderer[1]);
					m_goArrow.name = strPrefabName;
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateArmor(GameObject goArmor, string strPrefabName)
	{
		if (goArmor != null)
		{
			SkinnedMeshRenderer[] skinrenderer = goArmor.GetComponentsInChildren<SkinnedMeshRenderer>();

			if (skinrenderer != null)
			{
				m_goArmor = ProcessBonedObject(skinrenderer[0]);
				m_goArmor.name = strPrefabName;

				if (skinrenderer.Length > 1)
				{
					m_goCape = ProcessBonedObject(skinrenderer[1]);
					m_goCape.name = strPrefabName;
				}			
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateFace(GameObject goFace, string strPrefabName)
	{
		if (goFace != null)
		{
			SkinnedMeshRenderer skinrenderer = goFace.GetComponentInChildren<SkinnedMeshRenderer>();

			if (skinrenderer != null)
			{
				m_goFace = ProcessBonedObject(skinrenderer, goFace);
				m_goFace.name = strPrefabName;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateHair(GameObject goHair, string strPrefabName)
	{
		if (goHair != null)
		{
			SkinnedMeshRenderer skinrenderer = goHair.GetComponentInChildren<SkinnedMeshRenderer>();

			if (skinrenderer != null)
			{
				m_goHair = ProcessBonedObject(skinrenderer);
				m_goHair.name = strPrefabName;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	GameObject ProcessBonedObject(SkinnedMeshRenderer rendererResource, GameObject goFace = null)
	{
		if (rendererResource == null)
		{
			Debug.Log("NULL : " + gameObject.name);
		}

		GameObject goCreateEquip = new GameObject(rendererResource.gameObject.name);
		goCreateEquip.transform.SetParent(transform);
		SkinnedMeshRenderer rendererNew = goCreateEquip.AddComponent<SkinnedMeshRenderer>();

		Transform trRoot = transform;
		Transform[] atrBones = new Transform[rendererResource.bones.Length];

		for (int i = 0; i < rendererResource.bones.Length; i++)
		{
			atrBones[i] = FindChildByName(rendererResource.bones[i].name, trRoot);
		}

		rendererNew.bones = atrBones;
		rendererNew.sharedMesh = rendererResource.sharedMesh;
		rendererNew.sharedMaterials = rendererResource.sharedMaterials;
		rendererNew.transform.position = rendererResource.transform.position;
		rendererNew.localBounds = rendererResource.localBounds;
		rendererNew.rootBone = rendererResource.rootBone;
		rendererNew.updateWhenOffscreen = true;

		if (m_bInGame)
		{
			rendererNew.receiveShadows = false;
		}
		else
		{
			rendererNew.receiveShadows = true;
		}

		goCreateEquip.layer = gameObject.layer;

		if (goFace != null)
		{
			goFace = goFace.transform.GetChild(1).gameObject;

			
		}

		return goCreateEquip;
	}

	//---------------------------------------------------------------------------------------------------
	Transform FindChildByName(string strBoneName, Transform trFindObject)
	{
		Transform trReturnBone;
		if (trFindObject.name.Equals(strBoneName)) return trFindObject.transform;

		for (int i = 0; i < trFindObject.childCount; i++)
		{
			trReturnBone = FindChildByName(strBoneName, trFindObject.GetChild(i));

			if (trReturnBone != null)
			{
				return trReturnBone;
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	string GetPrefabName(EnEquipType enEquipType, int nJobId, int nGearId)
	{
		string strPrefabName = "";

		if (enEquipType == EnEquipType.Weapon)
		{
			if (nGearId == 0)
			{
				strPrefabName = ((1000 * nJobId) + 100).ToString();
			}
			else if (nGearId == -1)
			{
				strPrefabName = ((1000 * nJobId) + 108).ToString();
			}
			else
			{
				strPrefabName = CsGameData.Instance.GetMainGear(nGearId).PrefabName;
			}
		}
		else if (enEquipType == EnEquipType.Armor)
		{
			if (nGearId == 0)
			{
				strPrefabName = "100";
			}
			else if (nGearId == -1)
			{
				strPrefabName = "108";
			}
			else
			{
				strPrefabName = CsGameData.Instance.GetMainGear(nGearId).PrefabName;
			}
		}
		else if (enEquipType == EnEquipType.Hair)
		{
			if (nGearId == -1)
			{
				strPrefabName = "3";
			}
			else
			{
				nGearId++;
				strPrefabName = nGearId.ToString();
			}
		}

		return strPrefabName;
	}

	#endregion EquipmentsManagement

	#region ObjectManagement

	//---------------------------------------------------------------------------------------------------
	public void CreateFishStaff(int nJobId)
	{
		RemoveFishStaff();

		GameObject goFishStaff = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/FishStaff/" + nJobId.ToString());
		SkinnedMeshRenderer skinrenderer = goFishStaff.GetComponentInChildren<SkinnedMeshRenderer>();
		if (skinrenderer == null) return;
		m_goFishStaff = ProcessBonedObject(skinrenderer);
		m_goFishStaff.layer = transform.gameObject.layer;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveFishStaff()
	{
		if (m_goFishStaff != null)
		{
			Destroy(m_goFishStaff);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CreateWing(CsHeroCustomData csHeroCustomData, Transform trPovot, bool bUICharacter = false)
	{
		RemoveWing();
		CsWing csWing = CsGameData.Instance.GetWing(csHeroCustomData.WingId);

		if (csWing == null) return;

		if (m_goWing != null)
		{
			Destroy(m_goWing);
		}

		if (trPovot == null)
		{
			trPovot = transform.Find("Bip001");
		}

		float flScale = 1.5f;

		if (csHeroCustomData.JobId == (int)EnJob.Gaia)
		{
			flScale = 2.0f;
		}
		else if (csHeroCustomData.JobId == (int)EnJob.Asura)
		{
			flScale = 1.6f;
			trPovot = transform.Find("Bip01");
		}
		else if (csHeroCustomData.JobId == (int)EnJob.Deva)
		{
			flScale = 1.7f;
		}

		string str = trPovot.name;
		string strName = str + " Pelvis/" + str + " Spine/" + str + " Spine1";
		Transform trParent = trPovot.Find(strName);

		if (trParent != null)
		{
			m_goWing = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/WingObject/" + csWing.PrefabName), trParent);
			m_goWing.transform.localScale = new Vector3(flScale, flScale, flScale);
			m_goWing.layer = transform.gameObject.layer;
			m_goWing.tag = transform.tag;
			m_goWing.name = csWing.PrefabName;

			int nLayer = transform.gameObject.layer;

			if (bUICharacter)
			{
				nLayer = LayerMask.NameToLayer("UIChar");
			}

			Transform[] atrWing = m_goWing.GetComponentsInChildren<Transform>();
			for (int i = 0; i < atrWing.Length; ++i)
			{
				atrWing[i].gameObject.layer = nLayer;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveWing()
	{
		if (m_goWing != null)
		{
			Destroy(m_goWing);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CreateCostumeEffect(CsCostumeEffect csCostumeEffect)
	{
		RemoveCostumeEffect();
		if (csCostumeEffect == null) return;

		m_goCostumeEffect = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/CostumeEffect/" + csCostumeEffect.PrefabName), transform);
		m_goCostumeEffect.name = csCostumeEffect.PrefabName;

		float flCenter = CsIngameData.Instance.HeroCenter == 0 ? 1 : CsIngameData.Instance.HeroCenter;
		m_goCostumeEffect.transform.position = new Vector3(transform.position.x, transform.position.y + flCenter, transform.position.z);

		Transform[] atrCostumeEffect = m_goCostumeEffect.GetComponentsInChildren<Transform>();
		int nLayer = transform.gameObject.layer;

		for (int i = 0; i < atrCostumeEffect.Length; ++i)
		{
			atrCostumeEffect[i].gameObject.layer = nLayer;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveCostumeEffect()
	{
		if (m_goCostumeEffect != null)
		{
			Destroy(m_goCostumeEffect);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CreateArtifact(CsHeroCustomData csHeroCustomData, int nArtifactNo)
	{
		Debug.Log("CreateArtifact " + nArtifactNo);
		RemoveArtifact();
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(nArtifactNo);
		m_csHeroCustomData = csHeroCustomData;

		if (csArtifact == null) return;

		float flScale = 1f;

		if (m_csHeroCustomData.JobId == (int)EnJob.Gaia)
		{
			flScale = 1.3f;
		}
		else if (m_csHeroCustomData.JobId == (int)EnJob.Deva)
		{
			flScale = 1.1f;
		}

		m_goArtifact = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/ArteffectObject/" + csArtifact.PrefabName), transform);
		m_goArtifact.name = csArtifact.PrefabName;
		m_goArtifact.transform.localScale = new Vector3(flScale, 1, flScale);

		Transform[] atrArtifact = m_goArtifact.GetComponentsInChildren<Transform>();
		int nLayer = transform.gameObject.layer;

		for (int i = 0; i < atrArtifact.Length; ++i)
		{
			atrArtifact[i].gameObject.layer = nLayer;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveArtifact()
	{
		if (m_goArtifact != null)
		{
			Destroy(m_goArtifact);
		}
	}

	#endregion ObjectManagement

	#region Setting

	//---------------------------------------------------------------------------------------------------
	public void UpdateFreeSet(EnEquipType enType, int nJobId, int nValue)
	{
		if (enType == EnEquipType.Hair) // 프리셋은 머리만 존재.
		{
			RemoveEquipment(enType);
			ChangeHair(nJobId, nValue, m_bMid);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateFaceSize(Transform trHero, EnCustomFace enCustomFace, float flValue)
	{
		
	}
	float  GetValue(float flMax, float flMin, float flValue)
	{
		if (flValue > 0)		// 양수
		{
			return flValue * flMax;
		}
		else if(flValue < 0)	// 음수
		{
			return flValue * flMin;
		}

		return flValue;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateBodySize(Transform trHero, EnCustomBody enCustomBody, float flValue)
	{
		if (trHero == null || m_csHeroCustomData == null) return;

		Transform tr1 = null;
		Transform tr2 = null;

		flValue = flValue - 100;

		if ((EnJob)m_csHeroCustomData.JobId == EnJob.Gaia)
		{
			switch (enCustomBody)
			{
				case EnCustomBody.HeadSize:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 Head");
					tr1.localScale = new Vector3(1 + flValue, 1 + flValue, 1 + flValue);
					break;

				case EnCustomBody.ArmsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);

					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand");
					tr1.localScale = new Vector3(1 - flValue, tr1.localScale.y, tr1.localScale.z);
					tr2.localScale = new Vector3(1 - flValue, tr2.localScale.y, tr2.localScale.z);
					break;

				case EnCustomBody.ArmsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, 1 + flValue);
					tr2.localScale = new Vector3(tr2.localScale.x, 1 + flValue, 1 + flValue);

					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand");
					tr1.localScale = new Vector3(tr1.localScale.x, 1 - flValue, 1 - flValue);
					tr2.localScale = new Vector3(tr2.localScale.x, 1 - flValue, 1 - flValue);
					break;

				case EnCustomBody.ChestSize: // 가슴크기(x, y) - 10 %/ +25 %
					flValue = GetValue(0.0025f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/B_ Spine1");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(0.59279f + flValue, 0.59279f + flValue, tr1.localScale.z); // 높이는 조정 안함.
					}
					break;

				case EnCustomBody.WaistWidth: // 허리두께(x, y) - 15 %/ +40 %
					flValue = GetValue(0.004f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/B_ Spine");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(0.59279f + flValue, 0.59279f + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.PelvisWidth: // 골반두께(Z축) - 20 %/ +30 %
					flValue = GetValue(0.003f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, tr1.localScale.y, 0.59279f + flValue);
					}
					break;

				case EnCustomBody.HipsSize:	// 엉덩이크기(Y축) - 20 %/ +30 %
					flValue = GetValue(0.003f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, 0.59279f + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.LegsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 L Thigh");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 R Thigh");
					tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);

					// 키 정보 변경 되면 네비메시 OffSet 정보 or 포지션 값을 같이 올려 줘야 함.
					if (transform.GetComponent<CsHero>() == null)
					{
						if (CsGameData.Instance.MyHeroTransform == null)	// Intro인 경우
						{
							transform.position = new Vector3(transform.position.x, (flValue / 2) - 0.27f, transform.position.z);
						}
					}
					else
					{
						transform.GetComponent<CsHero>().MyHeroNavMeshAgent.baseOffset = flValue;
					}

					m_flLegsLengthOffset = flValue;
					break;

				case EnCustomBody.LegsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					Transform trLeftLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 L Thigh");
					Transform trRightLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 R Thigh");
					trLeftLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					trRightLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					break;
			}
		}
		else if ((EnJob)m_csHeroCustomData.JobId == EnJob.Asura)
		{
			switch (enCustomBody)
			{
				case EnCustomBody.HeadSize:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
					tr1.localScale = new Vector3(1 + flValue, 1 + flValue, 1 + flValue);
					break;

				case EnCustomBody.ArmsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm");
					tr2 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm");
					tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);
					break;

				case EnCustomBody.ArmsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm");
					tr2 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm");
					tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, 1 + flValue);
					tr2.localScale = new Vector3(tr2.localScale.x, 1 + flValue, 1 + flValue);
					break;

				case EnCustomBody.ChestSize: // 가슴크기(x,y) -10% / +20%
					flValue = GetValue(0.002f, 0.001f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1//B_ Spine1");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.WaistWidth: // 허리두께(x,y)  -15% / +30% 
					flValue = GetValue(0.003f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/B_ Spine");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.PelvisWidth: // 골반두께(Y축)  -20% /+40%
					flValue = GetValue(0.004f, 0.002f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/B_ Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.HipsSize: // 엉덩이크기(x축) -15% /+30%
					flValue = GetValue(0.003f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/B_ Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					}
					break;

				case EnCustomBody.LegsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh");
					tr2 = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh");
					tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);

					// 키 정보 변경 되면 네비메시 OffSet 정보 or 포지션 값을 같이 올려 줘야 함.
					if (transform.GetComponent<CsHero>() == null)
					{
						if (CsGameData.Instance.MyHeroTransform == null)
						{
							transform.position = new Vector3(transform.position.x, (flValue / 2) - 0.27f, transform.position.z);
						}
					}
					else
					{
						transform.GetComponent<CsHero>().MyHeroNavMeshAgent.baseOffset = flValue;
					}
					m_flLegsLengthOffset = flValue;
					break;

				case EnCustomBody.LegsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					Transform trLeftLower = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 L Thigh");
					Transform trRightLower = trHero.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 R Thigh");
					trLeftLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					trRightLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					break;
			}
		}
		else if ((EnJob)m_csHeroCustomData.JobId == EnJob.Deva)
		{
			switch (enCustomBody)
			{
				case EnCustomBody.HeadSize:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 Head");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, 1 + flValue);
					}
					break;

				case EnCustomBody.ArmsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
						tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);
					}
					break;

				case EnCustomBody.ArmsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, 1 + flValue);
						tr2.localScale = new Vector3(tr2.localScale.x, 1 + flValue, 1 + flValue);
					}
					break;

				case EnCustomBody.ChestSize: // 가슴크기(x,y) -20% /+30% 
					flValue = GetValue(0.003f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/B_Spine1");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z); // 높이는 조정 안함.
					}
					break;

				case EnCustomBody.WaistWidth: // 허리두께(x,y)  -10% /+25%
					flValue = GetValue(0.0025f, 0.001f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/B_Spine");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.PelvisWidth: // 골반두께(X축) -10%/+40%
					flValue = GetValue(0.004f, 0.001f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, 1 + flValue);
					}
					break;

				case EnCustomBody.HipsSize: // 엉덩이크기(Z축) -15%/+35%
					flValue = GetValue(0.0035f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.y, tr1.localScale.z, 1 + flValue);
					}
					break;

				case EnCustomBody.LegsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 L Thigh");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 R Thigh");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
						tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);
					}

					// 키 정보 변경 되면 네비메시 OffSet 정보 or 포지션 값을 같이 올려 줘야 함.
					if (transform.GetComponent<CsHero>() == null)
					{
						if (CsGameData.Instance.MyHeroTransform == null)
						{
							transform.position = new Vector3(transform.position.x, (flValue / 2) - 0.27f, transform.position.z);
						}
					}
					else
					{
						transform.GetComponent<CsHero>().MyHeroNavMeshAgent.baseOffset = flValue;
					}
					m_flLegsLengthOffset = flValue;
					break;

				case EnCustomBody.LegsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					Transform trLeftLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 L Thigh");
					Transform trRightLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 R Thigh");
					trLeftLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					trRightLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					break;
			}
		}
		else if ((EnJob)m_csHeroCustomData.JobId == EnJob.Witch)
		{
			switch (enCustomBody)
			{
				case EnCustomBody.HeadSize:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 Head");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, 1 + flValue);
					}
					break;

				case EnCustomBody.ArmsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
						tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);
					}
					break;

				case EnCustomBody.ArmsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, 1 + flValue, 1 + flValue);
						tr2.localScale = new Vector3(tr2.localScale.x, 1 + flValue, 1 + flValue);
					}
					break;

				case EnCustomBody.ChestSize: // 가슴크기(x,y) -15%/+40%  
					flValue = GetValue(0.004f, 0.0015f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/B_Spine1");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.WaistWidth: // 허리두께(x,y) -30%/+30%
					flValue = GetValue(0.003f, 0.003f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/B_Spine");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, 1 + flValue, tr1.localScale.z);
					}
					break;

				case EnCustomBody.PelvisWidth: // 골반두께(z축) -30%/+40%
					flValue = GetValue(0.004f, 0.003f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(tr1.localScale.x, tr1.localScale.y, 1 + flValue);
					}
					break;

				case EnCustomBody.HipsSize: // 엉덩이크기(x축) -30%/+40%
					flValue = GetValue(0.004f, 0.003f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/B_Pelvis");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
					}
					break;

				case EnCustomBody.LegsLength:
					flValue = GetValue(0.002f, 0.002f, flValue);
					tr1 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Thigh");
					tr2 = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Thigh");
					if (tr1 != null)
					{
						tr1.localScale = new Vector3(1 + flValue, tr1.localScale.y, tr1.localScale.z);
						tr2.localScale = new Vector3(1 + flValue, tr2.localScale.y, tr2.localScale.z);
					}

					// 키 정보 변경 되면 네비메시 OffSet 정보 or 포지션 값을 같이 올려 줘야 함.					
					if (transform.GetComponent<CsHero>() == null)
					{
						if (CsGameData.Instance.MyHeroTransform == null)
						{
							transform.position = new Vector3(transform.position.x, (flValue / 2) - 0.27f, transform.position.z);
						}
					}
					else
					{
						transform.GetComponent<CsHero>().MyHeroNavMeshAgent.baseOffset = flValue;
					}
					m_flLegsLengthOffset = flValue;
					break;

				case EnCustomBody.LegsWidth:
					flValue = GetValue(0.002f, 0.002f, flValue);
					Transform trLeftLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Thigh");
					Transform trRightLower = trHero.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Thigh");
					if (trLeftLower != null)
					{
						trLeftLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
						trRightLower.localScale = new Vector3(trLeftLower.localScale.x, 1 + flValue, 1 + flValue);
					}
					break;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateColor(EnCustomColor enCustomColor, Color32 setcolor)
	{
		SkinnedMeshRenderer rendererNew;
		SkinnedMeshRenderer rendererNew2;
		//Debug.Log("색상을 교체합니다.  enCustomColor = " + enCustomColor + " setcolor =" + setcolor);

		//_Color1 = R	피부
		//_Color2 = G	머리카락 + 수염.
		//_Color3 = B	눈썹
		//_Color4 = A	눈동자

		//Debug.Log("UpdateColor : " + m_bHelmet + " , " + setcolor);
		if (m_bHelmet)
		{
			setcolor = new Color32(255, 255, 255, 255);
		}

		switch (enCustomColor)
		{
			case EnCustomColor.Skin:    // 피부
				if (m_goFace != null)
				{
					rendererNew = m_goFace.GetComponent<SkinnedMeshRenderer>();
					rendererNew.materials[0].SetColor("_Color1", setcolor);
				}

				if (m_goArmor != null)
				{
					rendererNew2 = m_goArmor.GetComponent<SkinnedMeshRenderer>();
					if (rendererNew2.materials[0].GetTexture("_MaskTex") != null)
					{
						rendererNew2.materials[0].SetColor("_Color1", setcolor);
					}
				}
				break;
			case EnCustomColor.Hair:    // 머리카락
				if (m_goHair != null)
				{
					//Debug.Log("UpdateColor    EnCustomColor.Hair : " + setcolor + " , " + m_bHelmet);
					rendererNew = m_goHair.GetComponent<SkinnedMeshRenderer>();
					rendererNew.materials[0].SetColor("_Color2", setcolor);
				}
				break;
			case EnCustomColor.EyeBrowAndLips: // 눈썹, 입술
				if (m_goFace != null)
				{
					rendererNew = m_goFace.GetComponent<SkinnedMeshRenderer>();
					rendererNew.materials[0].SetColor("_Color3", setcolor);
				}
				break;
			case EnCustomColor.Eyes:    // 눈동자
				if (m_goFace != null)
				{
					rendererNew = m_goFace.GetComponent<SkinnedMeshRenderer>();
					rendererNew.materials[0].SetColor("_Color4", setcolor);
				}
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveFaceCustomAsset(bool bRemove)
	{
		
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveEquipments() //  모든 장비 삭제.
	{
		RemoveArtifact();
		RemoveWing();
		RemoveFishStaff();
		RemoveCostumeEffect();
		RemoveEquipment(EnEquipType.Weapon);
		RemoveEquipment(EnEquipType.Armor);
		RemoveEquipment(EnEquipType.Face);
		RemoveEquipment(EnEquipType.Hair);
	}

	//---------------------------------------------------------------------------------------------------
	public void ViewEquipments()
	{
		if (m_goWing != null)
		{
			m_goWing.SetActive(true);
		}

		if (m_goArtifact != null)
		{
			m_goArtifact.SetActive(true);
		}

		if (m_goArmor != null)
		{
			m_goArmor.SetActive(true);
		}

		if (m_goCostumeEffect != null)
		{
			m_goCostumeEffect.SetActive(true);
		}

		if (m_goCape != null)
		{
			m_goCape.SetActive(true);
		}

		if (m_goWeapon != null)
		{
			m_goWeapon.SetActive(true);
		}

		if (m_goArrow != null)
		{
			m_goArrow.SetActive(true);
		}

		if (m_goArmor != null)
		{
			m_goArmor.SetActive(true);
		}

		if (m_goFace != null)
		{
			m_goFace.SetActive(true);
		}

		if (m_goHair != null)
		{
			m_goHair.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void HideEquipments()
	{
		if (m_goWing != null)
		{
			m_goWing.SetActive(false);
		}

		if (m_goArtifact != null)
		{
			m_goArtifact.SetActive(false);
		}

		if (m_goArmor != null)
		{
			m_goArmor.SetActive(false);
		}

		if (m_goCostumeEffect != null)
		{
			m_goCostumeEffect.SetActive(false);
		}

		if (m_goCape != null)
		{
			m_goCape.SetActive(false);
		}

		if (m_goWeapon != null)
		{
			m_goWeapon.SetActive(false);
		}

		if (m_goArrow != null)
		{
			m_goArrow.SetActive(false);
		}

		if (m_goArmor != null)
		{
			m_goArmor.SetActive(false);
		}
		
		if (m_goFace != null)
		{
			m_goFace.SetActive(false);
		}

		if (m_goHair != null)
		{
			m_goHair.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveEquipment(EnEquipType enEquipType)
	{
		if (enEquipType == EnEquipType.Weapon && m_goWeapon != null)
		{
			Destroy(m_goWeapon);
			m_goWeapon = null;

			if (m_goArrow != null)
			{
				Destroy(m_goArrow);
				m_goArrow = null;
			}
		}
		else if (enEquipType == EnEquipType.Armor && m_goArmor != null)
		{
			Destroy(m_goArmor);
			m_goArmor = null;

			if (m_goCape != null)
			{
				Destroy(m_goCape);
				m_goCape = null;
			}
		}
		else if (enEquipType == EnEquipType.Face && m_goFace != null)
		{
			Destroy(m_goFace);
			m_goFace = null;
		}
		else if (enEquipType == EnEquipType.Hair && m_goHair != null)
		{
			Destroy(m_goHair);
			m_goHair = null;
		}
	}

	#endregion Setting

}


