using ClientCommon;
using System;
using System.Collections.Generic;
using UnityEngine;


public enum EnCustomFreeSet 
{ 
	Hair = 1, 
	Face = 2, 
	Body = 3,
}

public enum EnCustomFace
{
	JawHeight = 1,				// 턱 상하
	JawWidth = 2,				// 턱 넓이
	JawEndHeight = 3,			// 턱끝 상하
	FaceWidth = 4,				// 광대 넓이

	EyebrowHeight = 5,			// 눈썹 상하
	EyebrowRotation = 6,		// 눈썹 회전

	EyesWidth = 7,				// 눈 좌우

	NoseHeight = 8,				// 코 상하
	NoseWidth = 9,				// 코 넓이

	MouthHeight = 10,			// 입 상하
	MouthWidth = 11				// 입 좌우
}

public enum EnCustomBody
{
	HeadSize = 1,				// 머리 크기
	ArmsLength = 2,				// 팔 길이
	ArmsWidth = 3,				// 팔 넓이
	ChestSize = 4,				// 가슴 크기
	WaistWidth = 5,				// 허리 두께

	HipsSize = 6,				// 엉덩이 크기
	PelvisWidth = 7,			// 골반 넓이
	LegsLength = 8,				// 다리 길이
	LegsWidth = 9,				// 다리 넓이
}

public enum EnCustomColor
{
	Skin = 1,					// 피부
	Hair = 2,					// 머리카락, 수염
	EyeBrowAndLips = 3,			// 눈썹,입술
	Eyes = 4,					// 눈
}

public class CsCustomizingManager : MonoBehaviour 
{
	CsEquipment m_csEquipment;
	CsCustomBodyObject m_csDefaultCustom;	
	CsCustomBodyObject m_csTempCustom;
	Transform m_trHero;

	//List<Color32> m_listColor = new List<Color32>();

	List<Color32> m_listSkinColor = new List<Color32>();
	List<Color32> m_listFaceColor = new List<Color32>();

	//public List<Color32> listColor { get { return m_listColor;} }
	public List<Color32> listSkinColor { get { return m_listSkinColor; } }
	public List<Color32> listFaceColor { get { return m_listFaceColor; } }
	public CsEquipment Equipment { get { return m_csEquipment; } set { m_csEquipment = value; } }
	public CsCustomBodyObject DefaultCustom { get { return m_csDefaultCustom; } }
	public CsCustomBodyObject TempCustom { get { return m_csTempCustom; } }

	//---------------------------------------------------------------------------------------------------
	public static CsCustomizingManager Instance
	{
		get { return CsSingleton<CsCustomizingManager>.GetInstance(); }
	}

	//----------------------------------------------------------------------------------------------------
	public void Init()
	{
		Debug.Log("CsCustomizingManager.Init() ");
		SetCustomObject(1);		// 임시값 생성.
		SetCsutomColor();		// 컬러값 설정.
	}

	//----------------------------------------------------------------------------------------------------
	public void InitCustom(int nJobId, Transform trHero, bool bCreate = false) //  캐릭터 생성 or 커스텀기본 캐릭터.
	{
		m_trHero = trHero;
		m_csEquipment = trHero.GetComponent<CsEquipment>();
		m_csEquipment.MidChangeEquipments(new CsHeroCustomData(nJobId, bCreate), false);
	}

	//----------------------------------------------------------------------------------------------------
	public void SetMyHeroCustom(CsHeroCustomData csHeroCustomData, Transform trHero, bool bFaceCustom) // Intro
	{
		Debug.Log("SetMyHeroCustom   : "+ bFaceCustom);
		m_trHero = trHero;
		m_csEquipment = trHero.GetComponent<CsEquipment>();
		SetCustomData(csHeroCustomData);

		m_csDefaultCustom.JobId = m_csTempCustom.JobId = csHeroCustomData.JobId;

		if (bFaceCustom)
		{
			SetCustom();
		}
		else
		{
			SetCustomBodySize(m_csTempCustom);
			SetCustomColor(m_csTempCustom);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void Reset()
	{
		Debug.Log("CsCustomizingManager.Reset() ");
		SetCustomObject(m_csDefaultCustom.JobId);
		SetCustom();
	}

	//----------------------------------------------------------------------------------------------------
	public void ChangeCustomFreeSet(EnCustomFreeSet enCustomFreeSet, int nJobId, int nValue, bool bIntro = true)
	{
		if (enCustomFreeSet == EnCustomFreeSet.Face)
		{
			switch (nValue)
			{	
				case 0:
					m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 100);				// 턱 상하
					m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 100);				// 턱 넓이
					m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 100);			// 턱끝 상하
					m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 100);				// 광대 넓이
					m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 100);			// 눈썹 상하
					m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 100);		// 눈썹 회전
					m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);				// 눈 좌우
					m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 100);				// 코 상하
					m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 100);				// 코 넓이
					m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 100);			// 입 상하
					m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 100);				// 입 좌우
					break;
				case 1:
					switch (nJobId)
					{
						case 1:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 150);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 150);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 150);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 200);
							break;
						case 2:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 160);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 140);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 40);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 80);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 120);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 100);
							break;
						case 3:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 25);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 140);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 30);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 60);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 80);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 30);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 30);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 60);
							break;
						case 4:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 30);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 65);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 50);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 20);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 35);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 45);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 50);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 120);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 100);
							break;
					}
					break;
				case 2:
					switch (nJobId)
					{
						case 1:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 0);
							break;
						case 2:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 140);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 80);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 0);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 80);
							break;
						case 3:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 70);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 55);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 135);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 50);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 170);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 130);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 90);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 140);
							break;
						case 4:
							m_csTempCustom.SetCustomFace(EnCustomFace.JawHeight, 140);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawWidth, 130);
							m_csTempCustom.SetCustomFace(EnCustomFace.JawEndHeight, 50);
							m_csTempCustom.SetCustomFace(EnCustomFace.FaceWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowHeight, 20);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyebrowRotation, 80);
							m_csTempCustom.SetCustomFace(EnCustomFace.EyesWidth, 100);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseHeight, 200);
							m_csTempCustom.SetCustomFace(EnCustomFace.NoseWidth, 70);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthHeight, 120);
							m_csTempCustom.SetCustomFace(EnCustomFace.MouthWidth, 140);
							break;
					}
					break;
			}

			SetCustomFace(m_csTempCustom);
		}
		else if (enCustomFreeSet == EnCustomFreeSet.Hair)
		{
			m_csTempCustom.CustomPresetHair = nValue;
			m_csEquipment.UpdateFreeSet(EnEquipType.Hair, nJobId, nValue);
			SetCustomColor(m_csTempCustom);
		}
		else if (enCustomFreeSet == EnCustomFreeSet.Body) // 지정된 크기 조절 값으로 설정.
		{
			switch (nValue)
			{
				case 0:
				m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 100);
				m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 100);
				break;
				case 1:
				switch (nJobId)
				{
					case 1:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 115);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 170);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 170);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 170);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 170);
						break;
					case 2:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 115);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 170);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 70);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 170);
						break;
					case 3:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 115);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 140);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 185);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 1);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 1);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 160);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 185);
						break;
					case 4:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 90);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 200);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 200);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 50);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 200);
						break;
				}
				break;
				case 2:
				switch (nJobId)
				{
					case 1:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 125);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 120);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 40);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 120);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 40);
						break;
					case 2:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 140);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 40);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 120);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 40);
						break;
					case 3:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 70);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 40);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 40);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 10040);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 120);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 40);
						break;
					case 4:
						m_csTempCustom.SetCustomBody(EnCustomBody.HeadSize, 50);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsWidth, 50);
						m_csTempCustom.SetCustomBody(EnCustomBody.ArmsLength, 30);
						m_csTempCustom.SetCustomBody(EnCustomBody.ChestSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.PelvisWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.WaistWidth, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.HipsSize, 100);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsWidth, 30);
						m_csTempCustom.SetCustomBody(EnCustomBody.LegsLength, 30);
						break;
				}
				break;
			}
			SetCustomBodySize(m_csTempCustom);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void ChangeCustomFace(EnCustomFace enCustomFace,int nValue)
	{
		m_csTempCustom.SetCustomFace(enCustomFace, nValue);
		m_csEquipment.UpdateFaceSize(m_trHero, enCustomFace, nValue);
	}

	//----------------------------------------------------------------------------------------------------
	public void ChangeCustomBody(EnCustomBody enCustomBody, int nValue)
	{
		m_csTempCustom.SetCustomBody(enCustomBody, nValue);
		m_csEquipment.UpdateBodySize(m_trHero, enCustomBody, nValue);
	}

	//----------------------------------------------------------------------------------------------------
	public void ChangeCustomColor(EnCustomColor enCustomColor, int nValue)
	{
		m_csTempCustom.SetCustomColor(enCustomColor, nValue);
		if (enCustomColor == EnCustomColor.Skin)
		{
			m_csEquipment.UpdateColor(enCustomColor, m_listSkinColor[nValue]);	
		}
		else
		{
			m_csEquipment.UpdateColor(enCustomColor, m_listFaceColor[nValue]);	
		}
	}

	//----------------------------------------------------------------------------------------------------
	void SetCustom()
	{
		SetCustomFace(m_csTempCustom);
		SetCustomBodySize(m_csTempCustom);
		SetCustomColor(m_csTempCustom);
	}

	//---------------------------------------------------------------------------------------------------
	void SetCustomObject(int nJobId)
	{
		m_csDefaultCustom = new CsCustomBodyObject();
		m_csTempCustom = new CsCustomBodyObject();
		m_csTempCustom.JobId = m_csDefaultCustom.JobId = nJobId;

		for (int i = 1; i <= (int)EnCustomFreeSet.Body; i++)
		{
			m_csDefaultCustom.CustomPresetHair = 0;
			m_csTempCustom.CustomPresetHair = 0;
		}

		// 얼굴 사이즈 조절값.
		for (int i = 1; i <= (int)EnCustomFace.MouthWidth; i++) 
		{
			m_csDefaultCustom.CustomFace.Add((EnCustomFace)i, 100);
			m_csTempCustom.CustomFace.Add((EnCustomFace)i, 100);
		}

		// 신체 사이즈 조절값.
		for (int i = 1; i <= (int)EnCustomBody.LegsWidth; i++)  
		{
			m_csDefaultCustom.CustomBody.Add((EnCustomBody)i, 100);
			m_csTempCustom.CustomBody.Add((EnCustomBody)i, 100);
		}

		// 색상 조절값.
		int nValue = 0;
		for (int i = 1; i <= (int)EnCustomColor.Eyes; i++)		
		{
			if ((EnCustomColor)i == EnCustomColor.Skin)
			{
				nValue = 0;
			}
			else if ((EnCustomColor)i == EnCustomColor.Hair)
			{
				switch ((EnJob)nJobId)
				{
				case EnJob.Gaia:
					nValue = 2;
					break;
				case EnJob.Asura:
					nValue = 12;
					break;
				case EnJob.Deva:
					nValue = 33;
					break;
				case EnJob.Witch:
					nValue = 31;
					break;
				}
			}
			else if ((EnCustomColor)i == EnCustomColor.EyeBrowAndLips)	// 눈썹, 입술
			{
				switch ((EnJob)nJobId)
				{
				case EnJob.Gaia:
					nValue = 2;
					break;
				case EnJob.Asura:
					nValue = 4;
					break;
				case EnJob.Deva:
					nValue = 33;
					break;
				case EnJob.Witch:
					nValue = 4;
					break;
				}
			}
			else if ((EnCustomColor)i == EnCustomColor.Eyes)	// 눈동자
			{
				switch ((EnJob)nJobId)
				{
				case EnJob.Gaia:
					nValue = 4;
					break;
				case EnJob.Asura:
					nValue = 30;
					break;
				case EnJob.Deva:
					nValue = 27;
					break;
				case EnJob.Witch:
					nValue = 4;
					break;
				}
			}

			m_csDefaultCustom.CustomColor.Add((EnCustomColor)i, nValue);
			m_csTempCustom.CustomColor.Add((EnCustomColor)i, nValue);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void SetCustomFace(CsCustomBodyObject csCustomBodyObject)
	{
		foreach (KeyValuePair<EnCustomFace, int> item in csCustomBodyObject.CustomFace)
		{
			m_csEquipment.UpdateFaceSize(m_trHero, item.Key, item.Value);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void SetCustomBodySize(CsCustomBodyObject csCustomBodyObject)
	{
		foreach (KeyValuePair<EnCustomBody, int> item in csCustomBodyObject.CustomBody)
		{
			m_csEquipment.UpdateBodySize(m_trHero, item.Key, item.Value);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void SetCustomColor(CsCustomBodyObject csCustomBodyObject)
	{
		foreach (KeyValuePair<EnCustomColor, int> item in csCustomBodyObject.CustomColor)
		{
			if (item.Key == EnCustomColor.Skin)
			{
				m_csEquipment.UpdateColor(item.Key, m_listSkinColor[item.Value]);
			}
			else
			{
				m_csEquipment.UpdateColor(item.Key, m_listFaceColor[item.Value]);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetCsutomColor() // 0 ~ 33
	{
		m_listSkinColor.Clear();
		m_listSkinColor.Add(new Color32(255, 255, 255, 255));
		m_listSkinColor.Add(new Color32(239, 203, 205, 255));
		m_listSkinColor.Add(new Color32(239, 186, 189, 255));
		m_listSkinColor.Add(new Color32(255, 162, 165, 255));
		m_listSkinColor.Add(new Color32(247, 154, 155, 255));
		m_listSkinColor.Add(new Color32(231, 199, 165, 255));
		m_listSkinColor.Add(new Color32(247, 199, 148, 255));
		m_listSkinColor.Add(new Color32(247, 182, 115, 255));
		m_listSkinColor.Add(new Color32(231, 170, 99, 255));
		m_listSkinColor.Add(new Color32(222, 150, 66, 255));
		m_listSkinColor.Add(new Color32(222, 169, 221, 255));
		m_listSkinColor.Add(new Color32(206, 235, 173, 255));
		m_listSkinColor.Add(new Color32(219, 100, 73, 255));
		m_listSkinColor.Add(new Color32(177, 103, 75, 255));
		m_listSkinColor.Add(new Color32(165, 144, 188, 255));
		m_listSkinColor.Add(new Color32(151, 88, 119, 255));
		m_listSkinColor.Add(new Color32(148, 166, 189, 255));
		m_listSkinColor.Add(new Color32(115, 154, 214, 255));
		m_listSkinColor.Add(new Color32(63, 113, 208, 255));
		m_listSkinColor.Add(new Color32(53, 113, 240, 255));
		m_listSkinColor.Add(new Color32(93, 221, 109, 255));
		m_listSkinColor.Add(new Color32(166, 206, 80, 255));
		m_listSkinColor.Add(new Color32(175, 186, 89, 255));
		m_listSkinColor.Add(new Color32(184, 137, 103, 255));
		m_listSkinColor.Add(new Color32(154, 115, 106, 255));
		m_listSkinColor.Add(new Color32(139, 97, 88, 255));
		m_listSkinColor.Add(new Color32(109, 67, 58, 255));
		m_listSkinColor.Add(new Color32(247, 219, 222, 255));
		m_listSkinColor.Add(new Color32(222, 199, 189, 255));
		m_listSkinColor.Add(new Color32(247, 219, 189, 255));
		m_listSkinColor.Add(new Color32(231, 195, 222, 255));
		m_listSkinColor.Add(new Color32(165, 174, 247, 255));
		m_listSkinColor.Add(new Color32(198, 138, 123, 255));
		m_listSkinColor.Add(new Color32(69, 55, 55, 255));

		m_listFaceColor.Clear();
		m_listFaceColor.Add(new Color32(41, 20, 0, 255));
		m_listFaceColor.Add(new Color32(41, 20, 0, 255));
		m_listFaceColor.Add(new Color32(161, 44, 27, 255));
		m_listFaceColor.Add(new Color32(148, 97, 24, 255));
		m_listFaceColor.Add(new Color32(222, 109, 107, 255));
		m_listFaceColor.Add(new Color32(223, 138, 107, 255));
		m_listFaceColor.Add(new Color32(255, 142, 0, 255));
		m_listFaceColor.Add(new Color32(255, 0, 0, 255));
		m_listFaceColor.Add(new Color32(255, 0, 173, 255));
		m_listFaceColor.Add(new Color32(222, 109, 189, 255));
		m_listFaceColor.Add(new Color32(165, 0, 255, 255));
		m_listFaceColor.Add(new Color32(74, 0, 99, 255));
		m_listFaceColor.Add(new Color32(50, 9, 64, 255));
		m_listFaceColor.Add(new Color32(0, 36, 82, 255));
		m_listFaceColor.Add(new Color32(0, 81, 99, 255));
		m_listFaceColor.Add(new Color32(33, 166, 189, 255));
		m_listFaceColor.Add(new Color32(116, 109, 222, 255));
		m_listFaceColor.Add(new Color32(107, 162, 222, 255));
		m_listFaceColor.Add(new Color32(107, 211, 222, 255));
		m_listFaceColor.Add(new Color32(0, 203, 255, 255));
		m_listFaceColor.Add(new Color32(0, 255, 222, 255));
		m_listFaceColor.Add(new Color32(0, 60, 0, 255));
		m_listFaceColor.Add(new Color32(74, 65, 8, 255));
		m_listFaceColor.Add(new Color32(90, 97, 0, 255));
		m_listFaceColor.Add(new Color32(107, 158, 24, 255));
		m_listFaceColor.Add(new Color32(41, 158, 33, 255));
		m_listFaceColor.Add(new Color32(115, 219, 107, 255));
		m_listFaceColor.Add(new Color32(165, 219, 107, 255));
		m_listFaceColor.Add(new Color32(181, 255, 0, 255));
		m_listFaceColor.Add(new Color32(255, 207, 0, 255));
		m_listFaceColor.Add(new Color32(222, 207, 107, 255));
		m_listFaceColor.Add(new Color32(255, 255, 255, 255));
		m_listFaceColor.Add(new Color32(115, 117, 115, 255));
		m_listFaceColor.Add(new Color32(0, 0, 0, 255));
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCustomData(CsHeroCustomData HeroCustomData)
	{
		m_csTempCustom.JobId = HeroCustomData.JobId;
		SetCustomObject(HeroCustomData.JobId);

		m_csTempCustom.CustomPresetHair = HeroCustomData.CustomPresetHair;

		m_csTempCustom.CustomFace[EnCustomFace.JawHeight] = HeroCustomData.CustomFaceJawHeight;
		m_csTempCustom.CustomFace[EnCustomFace.JawWidth] = HeroCustomData.CustomFaceJawWidth;
		m_csTempCustom.CustomFace[EnCustomFace.JawEndHeight] = HeroCustomData.CustomFaceJawEndHeight;
		m_csTempCustom.CustomFace[EnCustomFace.FaceWidth] = HeroCustomData.CustomFaceWidth;
		m_csTempCustom.CustomFace[EnCustomFace.EyebrowHeight] = HeroCustomData.CustomFaceEyebrowHeight;
		m_csTempCustom.CustomFace[EnCustomFace.EyebrowRotation] = HeroCustomData.CustomFaceEyebrowRotation;
		m_csTempCustom.CustomFace[EnCustomFace.EyesWidth] = HeroCustomData.CustomFaceEyesWidth;
		m_csTempCustom.CustomFace[EnCustomFace.NoseHeight] = HeroCustomData.CustomFaceNoseHeight;
		m_csTempCustom.CustomFace[EnCustomFace.NoseWidth] = HeroCustomData.CustomFaceNoseWidth;
		m_csTempCustom.CustomFace[EnCustomFace.MouthHeight] = HeroCustomData.CustomFaceMouthHeight;
		m_csTempCustom.CustomFace[EnCustomFace.MouthWidth] = HeroCustomData.CustomFaceMouthWidth;

		m_csTempCustom.CustomBody[EnCustomBody.HeadSize] = HeroCustomData.CustomBodyHeadSize;
		m_csTempCustom.CustomBody[EnCustomBody.ArmsLength] = HeroCustomData.CustomBodyArmsLength;
		m_csTempCustom.CustomBody[EnCustomBody.ArmsWidth] = HeroCustomData.CustomBodyArmsWidth;
		m_csTempCustom.CustomBody[EnCustomBody.ChestSize] = HeroCustomData.CustomBodyChestSize;
		m_csTempCustom.CustomBody[EnCustomBody.WaistWidth] = HeroCustomData.CustomBodyWaistWidth;
		m_csTempCustom.CustomBody[EnCustomBody.HipsSize] = HeroCustomData.CustomBodyHipsSize;
		m_csTempCustom.CustomBody[EnCustomBody.PelvisWidth] = HeroCustomData.CustomBodyPelvisWidth;
		m_csTempCustom.CustomBody[EnCustomBody.LegsLength] = HeroCustomData.CustomBodyLegsLength;
		m_csTempCustom.CustomBody[EnCustomBody.LegsWidth] = HeroCustomData.CustomBodyLegsWidth;

		m_csTempCustom.CustomColor[EnCustomColor.Skin] = HeroCustomData.CustomColorSkin;
		m_csTempCustom.CustomColor[EnCustomColor.Eyes] = HeroCustomData.CustomColorEyes;
		m_csTempCustom.CustomColor[EnCustomColor.EyeBrowAndLips] = HeroCustomData.CustomColorBeardAndEyebrow;
		m_csTempCustom.CustomColor[EnCustomColor.Hair] = HeroCustomData.CustomColorHair;
	}

	//---------------------------------------------------------------------------------------------------
	public CsLobbyHero SaveObject(CsLobbyHero csLobbyHero)
	{
		csLobbyHero.CustomPresetHair = m_csTempCustom.CustomPresetHair == 0 ? 0 : m_csTempCustom.CustomPresetHair - 1;

		csLobbyHero.CustomFaceJawHeight = m_csTempCustom.CustomFace[EnCustomFace.JawHeight];
		csLobbyHero.CustomFaceJawWidth = m_csTempCustom.CustomFace[EnCustomFace.JawWidth];
		csLobbyHero.CustomFaceJawEndHeight = m_csTempCustom.CustomFace[EnCustomFace.JawEndHeight];
		csLobbyHero.CustomFaceWidth = m_csTempCustom.CustomFace[EnCustomFace.FaceWidth];
		csLobbyHero.CustomFaceEyebrowHeight = m_csTempCustom.CustomFace[EnCustomFace.EyebrowHeight];
		csLobbyHero.CustomFaceEyebrowRotation = m_csTempCustom.CustomFace[EnCustomFace.EyebrowRotation];
		csLobbyHero.CustomFaceEyesWidth = m_csTempCustom.CustomFace[EnCustomFace.EyesWidth];
		csLobbyHero.CustomFaceNoseHeight = m_csTempCustom.CustomFace[EnCustomFace.NoseHeight];
		csLobbyHero.CustomFaceNoseWidth = m_csTempCustom.CustomFace[EnCustomFace.NoseWidth];
		csLobbyHero.CustomFaceMouthHeight = m_csTempCustom.CustomFace[EnCustomFace.MouthHeight];
		csLobbyHero.CustomFaceMouthWidth = m_csTempCustom.CustomFace[EnCustomFace.MouthWidth];

		csLobbyHero.CustomBodyHeadSize = m_csTempCustom.CustomBody[EnCustomBody.HeadSize];
		csLobbyHero.CustomBodyArmsLength = m_csTempCustom.CustomBody[EnCustomBody.ArmsLength];
		csLobbyHero.CustomBodyArmsWidth = m_csTempCustom.CustomBody[EnCustomBody.ArmsWidth];
		csLobbyHero.CustomBodyChestSize = m_csTempCustom.CustomBody[EnCustomBody.ChestSize];
		csLobbyHero.CustomBodyWaistWidth = m_csTempCustom.CustomBody[EnCustomBody.WaistWidth];
		csLobbyHero.CustomBodyHipsSize = m_csTempCustom.CustomBody[EnCustomBody.HipsSize];
		csLobbyHero.CustomBodyPelvisWidth = m_csTempCustom.CustomBody[EnCustomBody.PelvisWidth];
		csLobbyHero.CustomBodyLegsLength = m_csTempCustom.CustomBody[EnCustomBody.LegsLength];
		csLobbyHero.CustomBodyLegsWidth = m_csTempCustom.CustomBody[EnCustomBody.LegsWidth];

		csLobbyHero.CustomColorSkin = m_csTempCustom.CustomColor[EnCustomColor.Skin];
		csLobbyHero.CustomColorEyes = m_csTempCustom.CustomColor[EnCustomColor.Eyes];
		csLobbyHero.CustomColorBeardAndEyebrow = m_csTempCustom.CustomColor[EnCustomColor.EyeBrowAndLips];
		csLobbyHero.CustomColorHair = m_csTempCustom.CustomColor[EnCustomColor.Hair];
		return csLobbyHero;
	}
}

public class CsCustomBodyObject
{
	int m_nJobId = 1;
	int m_nFreeSetHair = 0;
	Dictionary<EnCustomFace, int> m_dicCustomFace = new Dictionary<EnCustomFace, int>();
	Dictionary<EnCustomBody, int> m_dicCustomBody = new Dictionary<EnCustomBody, int>();
	Dictionary<EnCustomColor, int> m_dicCustomColor = new Dictionary<EnCustomColor, int>();

	public int JobId { get { return m_nJobId; } set { m_nJobId = value; } }
	public int CustomPresetHair { get { return m_nFreeSetHair; } set { m_nFreeSetHair = value; } }
	public Dictionary<EnCustomFace, int> CustomFace { get { return m_dicCustomFace; } }
	public Dictionary<EnCustomBody, int> CustomBody { get { return m_dicCustomBody; } }
	public Dictionary<EnCustomColor, int> CustomColor { get { return m_dicCustomColor; } }

	//---------------------------------------------------------------------------------------------------
	public float GetCustomFace(EnCustomFace enCustomFace)
	{
		return m_dicCustomFace[enCustomFace];
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCustomFace(EnCustomFace enCustomFace, int nValue)
	{
		m_dicCustomFace[enCustomFace] = nValue;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetCustomBody(EnCustomBody enCustomBody)
	{
		return m_dicCustomBody[enCustomBody];
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCustomBody(EnCustomBody enCustomBody, int nValue)
	{
		m_dicCustomBody[enCustomBody] = nValue;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetCustomColor(EnCustomColor enColor)
	{
		return m_dicCustomColor[enColor];
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCustomColor(EnCustomColor enColor, int nColorIndex)
	{
		m_dicCustomColor[enColor] = nColorIndex;
	}
}

public class CsHeroCustomData
{
	int m_nJobId = 1;
	int m_nWingId = 0;

	Guid m_guidWeapon = Guid.Empty;
	int m_nWeaponId = 0;
	int m_nWeaponEnchantLevel = 0;

	Guid m_guidArmor = Guid.Empty;
	int m_nArmorId = 0;
	int m_nArmorEnchantLevel = 0;

	int m_nCostumeId = 0;
	int m_nCostumeEffectId = 0;

	int m_nArtifactNo = 0;

	int m_nCustomPresetHair;
	int m_nCustomFaceJawHeight;
	int m_nCustomFaceJawWidth;
	int m_nCustomFaceJawEndHeight;
	int m_nCustomFaceWidth;
	int m_nCustomFaceEyebrowHeight;
	int m_nCustomFaceEyebrowRotation;
	int m_nCustomFaceEyesWidth;
	int m_nCustomFaceNoseHeight;
	int m_nCustomFaceNoseWidth;
	int m_nCustomFaceMouthHeight;
	int m_nCustomFaceMouthWidth;
	int m_nCustomBodyHeadSize;
	int m_nCustomBodyArmsLength;
	int m_nCustomBodyArmsWidth;
	int m_nCustomBodyChestSize;
	int m_nCustomBodyHipsSize;
	int m_nCustomBodyWaistWidth;
	int m_nCustomBodyPelvisWidth;
	int m_nCustomBodyLegsLength;
	int m_nCustomBodyLegsWidth;
	int m_nCustomColorSkin;
	int m_nCustomColorEyes;
	int m_nCustomColorBeardAndEyebrow;
	int m_nCustomColorHair;

	public int JobId { get { return m_nJobId; } }
	public int WingId { get { return m_nWingId; } set { m_nWingId = value; } }

	public Guid Weapon { get { return m_guidWeapon; } }
	public int WeaponId { get { return m_nWeaponId; } }
	public int WeaponEnchantLevel { get { return m_nWeaponEnchantLevel; } }

	public Guid Armor { get { return m_guidArmor; } }
	public int ArmorId { get { return m_nArmorId; } }
	public int ArmorEnchantLevel { get { return m_nArmorEnchantLevel; } }

	public int CostumeId { get { return m_nCostumeId; } }
	public int CostumeEffectId { get { return m_nCostumeEffectId; } }

	public int ArtifactNo { get { return m_nArtifactNo; } }
	
	public int CustomPresetHair { get { return m_nCustomPresetHair; } }
	public int CustomFaceJawHeight { get { return m_nCustomFaceJawHeight; } }
	public int CustomFaceJawWidth { get { return m_nCustomFaceJawWidth; } }
	public int CustomFaceJawEndHeight { get { return m_nCustomFaceJawEndHeight; } }
	public int CustomFaceWidth { get { return m_nCustomFaceWidth; } }
	public int CustomFaceEyebrowHeight { get { return m_nCustomFaceEyebrowHeight; } }
	public int CustomFaceEyebrowRotation { get { return m_nCustomFaceEyebrowRotation; } }
	public int CustomFaceEyesWidth { get { return m_nCustomFaceEyesWidth; } }
	public int CustomFaceNoseHeight { get { return m_nCustomFaceNoseHeight; } }
	public int CustomFaceNoseWidth { get { return m_nCustomFaceNoseWidth; } }
	public int CustomFaceMouthHeight { get { return m_nCustomFaceMouthHeight; } }
	public int CustomFaceMouthWidth { get { return m_nCustomFaceMouthWidth; } }
	public int CustomBodyHeadSize { get { return m_nCustomBodyHeadSize; } }
	public int CustomBodyArmsLength { get { return m_nCustomBodyArmsLength; } }
	public int CustomBodyArmsWidth { get { return m_nCustomBodyArmsWidth; } }
	public int CustomBodyChestSize { get { return m_nCustomBodyChestSize; } }
	public int CustomBodyWaistWidth { get { return m_nCustomBodyWaistWidth; } }
	public int CustomBodyHipsSize { get { return m_nCustomBodyHipsSize; } }
	public int CustomBodyPelvisWidth { get { return m_nCustomBodyPelvisWidth; } }
	public int CustomBodyLegsLength { get { return m_nCustomBodyLegsLength; } }
	public int CustomBodyLegsWidth { get { return m_nCustomBodyLegsWidth; } }
	public int CustomColorSkin { get { return m_nCustomColorSkin; } }
	public int CustomColorEyes { get { return m_nCustomColorEyes; } }
	public int CustomColorBeardAndEyebrow { get { return m_nCustomColorBeardAndEyebrow; } }
	public int CustomColorHair { get { return m_nCustomColorHair; } }

	public CsHeroCustomData(PDHero pDHero)
	{
		CsJob csjob = CsGameData.Instance.GetJob(pDHero.jobId);
		m_nJobId = csjob.ParentJobId == 0 ? csjob.JobId : csjob.ParentJobId;
		m_nWingId = pDHero.equippedWingId;

		if (pDHero.equippedWeapon != null)
		{
			m_guidWeapon = pDHero.equippedWeapon.id;
			m_nWeaponId = pDHero.equippedWeapon.mainGearId;
			m_nWeaponEnchantLevel = pDHero.equippedWeapon.enchantLevel;
		}

		if (pDHero.equippedArmor != null)
		{
			m_guidArmor = pDHero.equippedArmor.id;
			m_nArmorId = pDHero.equippedArmor.mainGearId;
			m_nArmorEnchantLevel = pDHero.equippedArmor.enchantLevel;
		}

		m_nCostumeId = pDHero.equippedCostumeId;
		m_nCostumeEffectId = pDHero.appliedCostumeEffectId;

		m_nArtifactNo = pDHero.equippedArtifactNo;

		m_nCustomPresetHair = pDHero.customPresetHair + 1;
		m_nCustomFaceJawHeight = pDHero.customFaceJawHeight;
		m_nCustomFaceWidth = pDHero.customFaceJawWidth;
		m_nCustomFaceJawEndHeight = pDHero.customFaceJawEndHeight;
		m_nCustomFaceWidth = pDHero.customFaceWidth;
		m_nCustomFaceEyebrowHeight = pDHero.customFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = pDHero.customFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = pDHero.customFaceEyesWidth;
		m_nCustomFaceNoseHeight = pDHero.customFaceNoseHeight;
		m_nCustomFaceNoseWidth = pDHero.customFaceNoseWidth;
		m_nCustomFaceMouthHeight = pDHero.customFaceMouthHeight;
		m_nCustomFaceMouthWidth = pDHero.customFaceMouthWidth;

		m_nCustomBodyHeadSize = pDHero.customBodyHeadSize;
		m_nCustomBodyArmsLength = pDHero.customBodyArmsLength;
		m_nCustomBodyArmsWidth = pDHero.customBodyArmsWidth;
		m_nCustomBodyChestSize = pDHero.customBodyChestSize;
		m_nCustomBodyWaistWidth = pDHero.customBodyWaistWidth;
		m_nCustomBodyHipsSize = pDHero.customBodyHipsSize;
		m_nCustomBodyPelvisWidth = pDHero.customBodyPelvisWidth;
		m_nCustomBodyLegsLength = pDHero.customBodyLegsLength;
		m_nCustomBodyLegsWidth = pDHero.customBodyLegsWidth;

		m_nCustomColorSkin = pDHero.customColorSkin;
		m_nCustomColorEyes = pDHero.customColorEyes;
		m_nCustomColorBeardAndEyebrow = pDHero.customColorBeardAndEyebrow;
		m_nCustomColorHair = pDHero.customColorHair;
	}

	public CsHeroCustomData(CsHeroInfo csHeroInfo)
	{
		m_nJobId = csHeroInfo.Job.ParentJobId == 0 ? csHeroInfo.Job.JobId : csHeroInfo.Job.ParentJobId;
		m_nWingId = csHeroInfo.EquippedWingId;

		if (csHeroInfo.HeroMainGearEquippedWeapon != null)
		{
			m_guidWeapon = csHeroInfo.HeroMainGearEquippedWeapon.Id;
			m_nWeaponId = csHeroInfo.HeroMainGearEquippedWeapon.MainGear.MainGearId;
			m_nWeaponEnchantLevel = csHeroInfo.HeroMainGearEquippedWeapon.EnchantLevel;
		}

		if (csHeroInfo.HeroMainGearEquippedArmor != null)
		{
			m_guidArmor = csHeroInfo.HeroMainGearEquippedArmor.Id;
			m_nArmorId = csHeroInfo.HeroMainGearEquippedArmor.MainGear.MainGearId;
			m_nArmorEnchantLevel = csHeroInfo.HeroMainGearEquippedArmor.EnchantLevel;
		}
		
		m_nCostumeId = csHeroInfo.EquippedCostumeId;
		m_nCostumeEffectId = csHeroInfo.AppliedCostumeEffectId;

		//m_nArtifactNo = csHeroInfo.equippedArtifactNo;

		m_nCustomPresetHair = csHeroInfo.CustomPresetHair + 1;
		m_nCustomFaceJawHeight = csHeroInfo.CustomFaceJawHeight;
		m_nCustomFaceWidth = csHeroInfo.CustomFaceJawWidth;
		m_nCustomFaceJawEndHeight = csHeroInfo.CustomFaceJawEndHeight;
		m_nCustomFaceWidth = csHeroInfo.CustomFaceWidth;
		m_nCustomFaceEyebrowHeight = csHeroInfo.CustomFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = csHeroInfo.CustomFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = csHeroInfo.CustomFaceEyesWidth;
		m_nCustomFaceNoseHeight = csHeroInfo.CustomFaceNoseHeight;
		m_nCustomFaceNoseWidth = csHeroInfo.CustomFaceNoseWidth;
		m_nCustomFaceMouthHeight = csHeroInfo.CustomFaceMouthHeight;
		m_nCustomFaceMouthWidth = csHeroInfo.CustomFaceMouthWidth;

		m_nCustomBodyHeadSize = csHeroInfo.CustomBodyHeadSize;
		m_nCustomBodyArmsLength = csHeroInfo.CustomBodyArmsLength;
		m_nCustomBodyArmsWidth = csHeroInfo.CustomBodyArmsWidth;
		m_nCustomBodyChestSize = csHeroInfo.CustomBodyChestSize;
		m_nCustomBodyWaistWidth = csHeroInfo.CustomBodyWaistWidth;
		m_nCustomBodyHipsSize = csHeroInfo.CustomBodyHipsSize;
		m_nCustomBodyPelvisWidth = csHeroInfo.CustomBodyPelvisWidth;
		m_nCustomBodyLegsLength = csHeroInfo.CustomBodyLegsLength;
		m_nCustomBodyLegsWidth = csHeroInfo.CustomBodyLegsWidth;

		m_nCustomColorSkin = csHeroInfo.CustomColorSkin;
		m_nCustomColorEyes = csHeroInfo.CustomColorEyes;
		m_nCustomColorBeardAndEyebrow = csHeroInfo.CustomColorBeardAndEyebrow;
		m_nCustomColorHair = csHeroInfo.CustomColorHair;
	}

	public CsHeroCustomData(CsMyHeroInfo csMyHeroInfo)
	{
		m_nJobId = csMyHeroInfo.Job.ParentJobId == 0 ? csMyHeroInfo.Job.JobId : csMyHeroInfo.Job.ParentJobId;
		m_nWingId = csMyHeroInfo.EquippedWingId;

		for (int i = 0; i < csMyHeroInfo.HeroMainGearEquippedList.Count; i++)
		{
			CsHeroMainGear csHeroMainGear = csMyHeroInfo.HeroMainGearEquippedList[i];
			if (csHeroMainGear != null)
			{
				if (csHeroMainGear.MainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Weapon)
				{
					m_guidWeapon = csHeroMainGear.Id;
					m_nWeaponId = csHeroMainGear.MainGear.MainGearId;
					m_nWeaponEnchantLevel = csHeroMainGear.EnchantLevel;
				}
				else if (csHeroMainGear.MainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Armor)
				{
					m_guidArmor = csHeroMainGear.Id;
					m_nArmorId = csHeroMainGear.MainGear.MainGearId;
					m_nArmorEnchantLevel = csHeroMainGear.EnchantLevel;
				}
			}
		}

		m_nCostumeId = CsCostumeManager.Instance.GetMyHeroCostumeId();
		m_nCostumeEffectId = CsCostumeManager.Instance.GetMyHeroCostumeEffectId();

		//m_nArtifactNo = csHeroInfo.equippedArtifactNo;

		m_nCustomPresetHair = csMyHeroInfo.CustomPresetHair + 1;
		m_nCustomFaceJawHeight = csMyHeroInfo.CustomFaceJawHeight;
		m_nCustomFaceWidth = csMyHeroInfo.CustomFaceJawWidth;
		m_nCustomFaceJawEndHeight = csMyHeroInfo.CustomFaceJawEndHeight;
		m_nCustomFaceWidth = csMyHeroInfo.CustomFaceWidth;
		m_nCustomFaceEyebrowHeight = csMyHeroInfo.CustomFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = csMyHeroInfo.CustomFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = csMyHeroInfo.CustomFaceEyesWidth;
		m_nCustomFaceNoseHeight = csMyHeroInfo.CustomFaceNoseHeight;
		m_nCustomFaceNoseWidth = csMyHeroInfo.CustomFaceNoseWidth;
		m_nCustomFaceMouthHeight = csMyHeroInfo.CustomFaceMouthHeight;
		m_nCustomFaceMouthWidth = csMyHeroInfo.CustomFaceMouthWidth;

		m_nCustomBodyHeadSize = csMyHeroInfo.CustomBodyHeadSize;
		m_nCustomBodyArmsLength = csMyHeroInfo.CustomBodyArmsLength;
		m_nCustomBodyArmsWidth = csMyHeroInfo.CustomBodyArmsWidth;
		m_nCustomBodyChestSize = csMyHeroInfo.CustomBodyChestSize;
		m_nCustomBodyWaistWidth = csMyHeroInfo.CustomBodyWaistWidth;
		m_nCustomBodyHipsSize = csMyHeroInfo.CustomBodyHipsSize;
		m_nCustomBodyPelvisWidth = csMyHeroInfo.CustomBodyPelvisWidth;
		m_nCustomBodyLegsLength = csMyHeroInfo.CustomBodyLegsLength;
		m_nCustomBodyLegsWidth = csMyHeroInfo.CustomBodyLegsWidth;

		m_nCustomColorSkin = csMyHeroInfo.CustomColorSkin;
		m_nCustomColorEyes = csMyHeroInfo.CustomColorEyes;
		m_nCustomColorBeardAndEyebrow = csMyHeroInfo.CustomColorBeardAndEyebrow;
		m_nCustomColorHair = csMyHeroInfo.CustomColorHair;
	}

	public CsHeroCustomData(CsLobbyHero csLobbyHero)
	{
		CsJob csjob = CsGameData.Instance.GetJob(csLobbyHero.JobId);
		m_nJobId = csjob.ParentJobId == 0 ? csjob.JobId : csjob.ParentJobId;
		m_nWingId = csLobbyHero.EquippedWingId;
		
		m_nWeaponId = csLobbyHero.EquippedWeaponGearId;
		m_nWeaponEnchantLevel = csLobbyHero.EquippedWeaponEnchantLevel;
		m_nArmorId = csLobbyHero.EquippedArmorGearId;
		m_nArmorEnchantLevel = csLobbyHero.EquippedArmorEnchantLevel;

		m_nCostumeId = csLobbyHero.EquippedCostumeId;
		m_nCostumeEffectId = csLobbyHero.AppliedCostumeEffectId;

		//m_nArtifactNo = csLobbyHero.equippedArtifactNo;

		m_nCustomPresetHair = csLobbyHero.CustomPresetHair + 1;
		m_nCustomFaceJawHeight = csLobbyHero.CustomFaceJawHeight;
		m_nCustomFaceWidth = csLobbyHero.CustomFaceJawWidth;
		m_nCustomFaceJawEndHeight = csLobbyHero.CustomFaceJawEndHeight;
		m_nCustomFaceWidth = csLobbyHero.CustomFaceWidth;
		m_nCustomFaceEyebrowHeight = csLobbyHero.CustomFaceEyebrowHeight;
		m_nCustomFaceEyebrowRotation = csLobbyHero.CustomFaceEyebrowRotation;
		m_nCustomFaceEyesWidth = csLobbyHero.CustomFaceEyesWidth;
		m_nCustomFaceNoseHeight = csLobbyHero.CustomFaceNoseHeight;
		m_nCustomFaceNoseWidth = csLobbyHero.CustomFaceNoseWidth;
		m_nCustomFaceMouthHeight = csLobbyHero.CustomFaceMouthHeight;
		m_nCustomFaceMouthWidth = csLobbyHero.CustomFaceMouthWidth;

		m_nCustomBodyHeadSize = csLobbyHero.CustomBodyHeadSize;
		m_nCustomBodyArmsLength = csLobbyHero.CustomBodyArmsLength;
		m_nCustomBodyArmsWidth = csLobbyHero.CustomBodyArmsWidth;
		m_nCustomBodyChestSize = csLobbyHero.CustomBodyChestSize;
		m_nCustomBodyWaistWidth = csLobbyHero.CustomBodyWaistWidth;
		m_nCustomBodyHipsSize = csLobbyHero.CustomBodyHipsSize;
		m_nCustomBodyPelvisWidth = csLobbyHero.CustomBodyPelvisWidth;
		m_nCustomBodyLegsLength = csLobbyHero.CustomBodyLegsLength;
		m_nCustomBodyLegsWidth = csLobbyHero.CustomBodyLegsWidth;

		m_nCustomColorSkin = csLobbyHero.CustomColorSkin;
		m_nCustomColorEyes = csLobbyHero.CustomColorEyes;
		m_nCustomColorBeardAndEyebrow = csLobbyHero.CustomColorBeardAndEyebrow;
		m_nCustomColorHair = csLobbyHero.CustomColorHair;
	}

	public CsHeroCustomData(int nJobId, bool Create = false)
	{
		CsJob csjob = CsGameData.Instance.GetJob(nJobId);
		m_nJobId = csjob.ParentJobId == 0 ? csjob.JobId : csjob.ParentJobId;
		m_nCustomPresetHair = 0;

		if (Create)
		{
			m_nWeaponId = -1;
			m_nArmorId = -1;
		}
		else
		{
			m_nWeaponId = 0;
			m_nArmorId = 0;
		}

		m_nCostumeId = 0;
		m_nCostumeEffectId = 0;

		m_nArtifactNo = 0;

		m_nWeaponEnchantLevel = 0;
		m_nArmorEnchantLevel = 0;
		m_nCustomFaceJawHeight = 100;
		m_nCustomFaceWidth = 100;
		m_nCustomFaceJawEndHeight = 100;
		m_nCustomFaceWidth = 100;
		m_nCustomFaceEyebrowHeight = 100;
		m_nCustomFaceEyebrowRotation = 100;
		m_nCustomFaceEyesWidth = 100;
		m_nCustomFaceNoseHeight = 100;
		m_nCustomFaceNoseWidth = 100;
		m_nCustomFaceMouthHeight = 100;
		m_nCustomFaceMouthWidth = 100;

		m_nCustomBodyHeadSize = 100;
		m_nCustomBodyArmsLength = 100;
		m_nCustomBodyArmsWidth = 100;
		m_nCustomBodyChestSize = 100;
		m_nCustomBodyWaistWidth = 100;
		m_nCustomBodyHipsSize = 100;
		m_nCustomBodyPelvisWidth = 100;
		m_nCustomBodyLegsLength = 100;
		m_nCustomBodyLegsWidth = 100;

		// 색상 조절값.
		int nValue = 0;
		for (int i = 1; i <= (int)EnCustomColor.Eyes; i++)
		{
			if ((EnCustomColor)i == EnCustomColor.Skin)
			{
				m_nCustomColorSkin = 0;
			}
			else if ((EnCustomColor)i == EnCustomColor.Hair)
			{
				switch ((EnJob)nJobId)
				{
					case EnJob.Gaia:
						nValue = 2;
						break;
					case EnJob.Asura:
						nValue = 12;
						break;
					case EnJob.Deva:
						nValue = 33;
						break;
					case EnJob.Witch:
						nValue = 31;
						break;
				}
				m_nCustomColorHair = nValue;
			}
			else if ((EnCustomColor)i == EnCustomColor.EyeBrowAndLips)  // 눈썹, 입술
			{
				switch ((EnJob)nJobId)
				{
					case EnJob.Gaia:
						nValue = 2;
						break;
					case EnJob.Asura:
						nValue = 4;
						break;
					case EnJob.Deva:
						nValue = 33;
						break;
					case EnJob.Witch:
						nValue = 4;
						break;
				}
				m_nCustomColorBeardAndEyebrow = nValue;
			}
			else if ((EnCustomColor)i == EnCustomColor.Eyes)    // 눈동자
			{
				switch ((EnJob)nJobId)
				{
					case EnJob.Gaia:
						nValue = 4;
						break;
					case EnJob.Asura:
						nValue = 30;
						break;
					case EnJob.Deva:
						nValue = 27;
						break;
					case EnJob.Witch:
						nValue = 4;
						break;
				}
				m_nCustomColorEyes = nValue;
			}
		}
	}

	public void SetWeapon(Guid guidWeapon, int nWeaponId, int nWeaponEnchantLevel)
	{
		m_guidWeapon = guidWeapon;
		m_nWeaponId = nWeaponId;
		m_nWeaponEnchantLevel = nWeaponEnchantLevel;
	}

	public void SetArmor(Guid guidArmor, int nArmorId, int nArmorEnchantLevel)
	{
		m_guidArmor = guidArmor;
		m_nArmorId = nArmorId;
		m_nArmorEnchantLevel = nArmorEnchantLevel;
	}

	public void SetCostum(int nCostumeId, int nCostumeEffectId)
	{
		m_nCostumeId = nCostumeId;
		m_nCostumeEffectId = nCostumeEffectId;
	}
}
