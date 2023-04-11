using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupSmallAmountCharging : MonoBehaviour 
{
	CsCashProduct m_csCashProduct = null;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		InitializeUI();

		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsCashManager.Instance.EventCashProductPurchaseCancel += OnEventCashProductPurchaseCancel;
		CsCashManager.Instance.EventCashProductPurchaseComplete += OnEventCashProductPurchaseComplete;
		CsCashManager.Instance.EventCashProductPurchaseFail += OnEventCashProductPurchaseFail;
		CsCashManager.Instance.EventDisableDimImage += OnEventDisableDimImage;
	}


	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsCashManager.Instance.EventCashProductPurchaseCancel -= OnEventCashProductPurchaseCancel;
		CsCashManager.Instance.EventCashProductPurchaseComplete -= OnEventCashProductPurchaseComplete;
		CsCashManager.Instance.EventCashProductPurchaseFail -= OnEventCashProductPurchaseFail;
		CsCashManager.Instance.EventDisableDimImage -= OnEventDisableDimImage;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Transform trImageBackground = transform.Find("ImageBackground");

		CsUIData.Instance.SetText(trImageBackground.Find("TextTitle"), "A125_TXT_00022", true);
		CsUIData.Instance.SetButton(trImageBackground.Find("ButtonClose"), PopupClose);

		Transform trFrameVip = trImageBackground.Find("FrameVip");

		CsUIData.Instance.SetButton(trFrameVip.Find("ButtonVip"), OnClickButtonVip);
		CsUIData.Instance.SetText(trFrameVip.Find("ButtonVip/TextVip"), "A125_TXT_00024", true);

		UpdateVipInfo();

		Transform trContent = trImageBackground.Find("ImageFrameContent/Scroll View/Viewport/Content");
		GameObject goSmallAmountChargingItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/SmallAmountChargingItem");

		foreach (CsCashProduct csCashProduct in CsGameData.Instance.CashProductList)
		{
			if (csCashProduct.Type == 1)
				continue;

			Transform trChargingItem = Instantiate(goSmallAmountChargingItem, trContent).transform;
			trChargingItem.name = csCashProduct.InAppProduct.InAppProductKey.ToString();

			CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == csCashProduct.ProductId);

			bool bFirstBuy = csCashProductPurchaseCount == null || csCashProductPurchaseCount.Count <= 0;

			trChargingItem.Find("TextFirstBuy").gameObject.SetActive(false);
			trChargingItem.Find("ImageIconFirstBuy").gameObject.SetActive(false);

			if (bFirstBuy)
			{
				CsUIData.Instance.SetText(trChargingItem.Find("TextFirstBuy"), "A125_TXT_00023", true);
			}

			CsUIData.Instance.SetImage(trChargingItem.Find("ImageIcon"), "GUI/PopupCharging/" + csCashProduct.ImageName);
			CsUIData.Instance.SetText(trChargingItem.Find("TextValueName"), csCashProduct.Name, false);
			CsUIData.Instance.SetButton(trChargingItem.Find("ButtonBuy"), () => OnClickButtonBuy(csCashProduct));

			CsInAppProductPrice csInAppProductPrice = csCashProduct.InAppProduct.GetInAppProductPrice();
			
			if (csInAppProductPrice == null)
			{
				CsUIData.Instance.SetText(trChargingItem.Find("ButtonBuy/TextBuy"), "", false);
			}
			else
			{
				CsUIData.Instance.SetText(trChargingItem.Find("ButtonBuy/TextBuy"), csInAppProductPrice.DisplayPrice + " " + csInAppProductPrice.CurrencyCode, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateVipInfo()
	{
		Transform trImageBackground = transform.Find("ImageBackground");
		Transform trFrameVip = trImageBackground.Find("FrameVip");

		CsUIData.Instance.SetText(trFrameVip.Find("TextVip"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_00025"), CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel), false);

		int nHeroVipLevel = CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel;
		int nHeroVipPoint = CsGameData.Instance.MyHeroInfo.VipPoint;

		CsVipLevel csVipLevelNext = CsGameData.Instance.GetVipLevel(nHeroVipLevel + 1);
		int nHeroVipNextPoint = 0;

		if (csVipLevelNext == null)
		{
			nHeroVipNextPoint = CsGameData.Instance.GetVipLevel(nHeroVipLevel).RequiredAccVipPoint;
		}
		else
		{
			nHeroVipNextPoint = csVipLevelNext.RequiredAccVipPoint;
		}

		if (nHeroVipNextPoint < nHeroVipPoint)
		{
			nHeroVipPoint = nHeroVipNextPoint;
		}

		Slider slider = trFrameVip.Find("Slider").GetComponent<Slider>();
		slider.value = (float)nHeroVipPoint / (float)nHeroVipNextPoint;

		CsUIData.Instance.SetText(trFrameVip.Find("TextValueVipPoint"), string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroVipPoint, nHeroVipNextPoint), false);

		trFrameVip.Find("ImageIconDia").gameObject.SetActive(csVipLevelNext != null);
		trFrameVip.Find("TextDescription").gameObject.SetActive(csVipLevelNext != null);

		CsUIData.Instance.SetText(trFrameVip.Find("TextDescription"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_00026"), (nHeroVipNextPoint - nHeroVipPoint), (nHeroVipLevel + 1)), false);
	}

	#region event
	//---------------------------------------------------------------------------------------------------
	void OnClickButtonVip()
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Vip, EnSubMenu.VipInfo);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonBuy(CsCashProduct csCashProduct)
	{
		transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
		transform.Find("ImageDim").gameObject.SetActive(true);

		m_csCashProduct = csCashProduct;

		CsCashManager.Instance.SendCashProductPurchaseStart(csCashProduct.ProductId);

		//Transform trPopupConfirm = transform.Find("ImageBackground/PopupConfirm");

		//Transform trImageBackground = trPopupConfirm.Find("ImageBackground");

		//CsUIData.Instance.SetText(trImageBackground.Find("TextTitle"), "A125_NAME_00002", true);
		//CsUIData.Instance.SetButton(trImageBackground.Find("ButtonClose"), OnClickButtonClosePopupConfirm);
		//CsUIData.Instance.SetImage(trImageBackground.Find("ImageIcon"), csCashProduct.ImageName);
		//CsUIData.Instance.SetText(trImageBackground.Find("TextPrice"), "A125_TXT_00013", true);

		//CsInAppProductPrice csInAppProductPrice = csCashProduct.InAppProduct.GetInAppProductPrice();

		//if (csInAppProductPrice == null)
		//{
		//    CsUIData.Instance.SetText(trImageBackground.Find("ButtonCharging/TextValue"), "", false);
		//}
		//else
		//{
		//    CsUIData.Instance.SetText(trImageBackground.Find("TextValuePrice"), csInAppProductPrice.DisplayPrice + " " + csInAppProductPrice.CurrencyCode, false);
		//}

		//CsUIData.Instance.SetText(trImageBackground.Find("TextAcquisition"), "A125_TXT_00014", true);

		//int nUnOwnDia = csCashProduct.UnOwnDia;

		//CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == csCashProduct.ProductId);

		//if (csCashProductPurchaseCount != null && csCashProductPurchaseCount.Count > 0)
		//{
		//    nUnOwnDia += csCashProduct.FirstPurchaseBonusUnOwnDia;
		//}

		//CsUIData.Instance.SetText(trImageBackground.Find("ImageFrameAcquiredDia/TextDia"), nUnOwnDia.ToString("#,##0"), false);

		//CsUIData.Instance.SetButton(trImageBackground.Find("ButtonCancel"), OnClickButtonCancelPopupConfirm);
		//CsUIData.Instance.SetText(trImageBackground.Find("ButtonCancel/TextCancel"), "A125_BTN_00015", true);
		//CsUIData.Instance.SetButton(trImageBackground.Find("ButtonBuy"), () => OnClickButtonBuyPopupConfirm(csCashProduct));
		//CsUIData.Instance.SetText(trImageBackground.Find("ButtonBuy/TextBuy"), "A125_BTN_00016", true);

		//trPopupConfirm.gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	//void OnClickButtonClosePopupConfirm()
	//{
	//    transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
	//}

	//---------------------------------------------------------------------------------------------------
	//void OnClickButtonCancelPopupConfirm()
	//{
	//    transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
	//}

	//---------------------------------------------------------------------------------------------------
	//void OnClickButtonBuyPopupConfirm(CsCashProduct csCashProduct)
	//{
	//    transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
	//    transform.Find("ImageDim").gameObject.SetActive(true);

	//    m_csCashProduct = csCashProduct;

	//    CsCashManager.Instance.SendCashProductPurchaseStart(csCashProduct.ProductId);
	//}

	//---------------------------------------------------------------------------------------------------
	void OnEventCashProductPurchaseCancel()
	{
		CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00020"));

		transform.Find("ImageDim").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCashProductPurchaseComplete()
	{
		CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00019"));

		transform.Find("ImageDim").gameObject.SetActive(false);

		UpdateVipInfo();

		//Transform trImageFrameContent = transform.Find("ImageBackground/ImageFrameContent");
		//Transform trContent = trImageFrameContent.Find("Scroll View/Viewport/Content");

		//Transform trChargingItem = trContent.Find(m_csCashProduct.InAppProduct.InAppProductKey.ToString());

		//if (trChargingItem != null)
		//{
		//    CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == m_csCashProduct.ProductId);

		//    bool bFirstBuy = csCashProductPurchaseCount == null || csCashProductPurchaseCount.Count <= 0;

		//    trChargingItem.Find("TextFirstBuy").gameObject.SetActive(bFirstBuy);
		//    trChargingItem.Find("ImageIconFirstBuy").gameObject.SetActive(bFirstBuy);
		//}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCashProductPurchaseFail()
	{
		CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00021"));

		transform.Find("ImageDim").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDisableDimImage()
	{
		transform.Find("ImageDim").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	#endregion event
}
