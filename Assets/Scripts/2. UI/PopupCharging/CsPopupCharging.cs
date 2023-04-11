using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupCharging : CsUpdateableMonoBehaviour 
{
	CsCashProduct m_csCashProduct = null;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;

		CsCashManager.Instance.EventCashProductPurchaseCancel += OnEventCashProductPurchaseCancel;
		CsCashManager.Instance.EventCashProductPurchaseComplete += OnEventCashProductPurchaseComplete;
		CsCashManager.Instance.EventCashProductPurchaseFail += OnEventCashProductPurchaseFail;
		CsCashManager.Instance.EventDisableDimImage += OnEventDisableDimImage;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
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
		Button buttonPopup = transform.GetComponent<Button>();
		buttonPopup.onClick.RemoveAllListeners();
		buttonPopup.onClick.AddListener(PopupClose);

		Transform trImageBackground = transform.Find("ImageBackground");

		CsUIData.Instance.SetText(trImageBackground.Find("TextTitle"), "A125_TXT_00007", true);
		CsUIData.Instance.SetText(trImageBackground.Find("TextDescription"), "A125_TXT_00012", true);

		GameObject goChargingItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/ChargingItem");

		// 충전 아이템
		Transform trContent = trImageBackground.Find("Scroll View/Viewport/Content");

		// 소액 충전
		Transform trSmallChargingItem = Instantiate(goChargingItem, trContent).transform;
		trSmallChargingItem.name = "Small Charging";

		trSmallChargingItem.Find("TextFirstBuy").gameObject.SetActive(false);
		trSmallChargingItem.Find("ImageIconFirstBuy").gameObject.SetActive(false);
		CsUIData.Instance.SetImage(trSmallChargingItem.Find("ImageItem"), "GUI/PopupCharging/ico_diamond_1");
		CsUIData.Instance.SetText(trSmallChargingItem.Find("TextValueDia"), "A125_TXT_00008", true);
		CsUIData.Instance.SetButton(trSmallChargingItem.Find("ButtonCharging"), OnClickButtonSmallCharging);
		CsUIData.Instance.SetText(trSmallChargingItem.Find("ButtonCharging/TextValue"), "A125_BTN_00011", true);

		// 다이아
		foreach (CsCashProduct csCashProduct in CsGameData.Instance.CashProductList)
		{
			if (csCashProduct.Type != 1)
				continue;

			Transform trChargingItem = Instantiate(goChargingItem, trContent).transform;
			trChargingItem.name = csCashProduct.InAppProduct.InAppProductKey.ToString();

			CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == csCashProduct.ProductId);

			bool bFirstBuy = csCashProductPurchaseCount == null || csCashProductPurchaseCount.Count <= 0;

			trChargingItem.Find("TextFirstBuy").gameObject.SetActive(bFirstBuy);
			trChargingItem.Find("ImageIconFirstBuy").gameObject.SetActive(bFirstBuy);

			if (bFirstBuy)
			{
				CsUIData.Instance.SetText(trChargingItem.Find("TextFirstBuy"), "A125_TXT_00023", true);
			}

			CsUIData.Instance.SetImage(trChargingItem.Find("ImageItem"), "GUI/PopupCharging/" + csCashProduct.ImageName);
			CsUIData.Instance.SetText(trChargingItem.Find("TextValueDia"), string.Format(csCashProduct.Name, csCashProduct.UnOwnDia.ToString("#,##0")), false);
			CsUIData.Instance.SetButton(trChargingItem.Find("ButtonCharging"), () => OnClickButtonCharging(csCashProduct));

			CsInAppProductPrice csInAppProductPrice = csCashProduct.InAppProduct.GetInAppProductPrice();

			if (csInAppProductPrice == null)
			{
				CsUIData.Instance.SetText(trChargingItem.Find("ButtonCharging/TextValue"), "", false);
			}
			else
			{
				CsUIData.Instance.SetText(trChargingItem.Find("ButtonCharging/TextValue"), csInAppProductPrice.DisplayPrice + " " + csInAppProductPrice.CurrencyCode, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	#region event

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSmallCharging()
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventOpenPopupSmallAmountCharging();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCharging(CsCashProduct csCashProduct)
	{
		Transform trPopupConfirm = transform.Find("ImageBackground/PopupConfirm");

		Transform trImageBackground = trPopupConfirm.Find("ImageBackground");

		CsUIData.Instance.SetText(trImageBackground.Find("TextTitle"), "A125_NAME_00002", true);
		CsUIData.Instance.SetButton(trImageBackground.Find("ButtonClose"), OnClickButtonClosePopupConfirm);
		CsUIData.Instance.SetImage(trImageBackground.Find("ImageIcon"), "GUI/PopupCharging/" + csCashProduct.ImageName);
		CsUIData.Instance.SetText(trImageBackground.Find("TextPrice"), "A125_TXT_00013", true);

		CsInAppProductPrice csInAppProductPrice = csCashProduct.InAppProduct.GetInAppProductPrice();

		if (csInAppProductPrice == null)
		{
			CsUIData.Instance.SetText(trImageBackground.Find("ButtonCharging/TextValue"), "", false);
		}
		else
		{
			CsUIData.Instance.SetText(trImageBackground.Find("TextValuePrice"), csInAppProductPrice.DisplayPrice + " " + csInAppProductPrice.CurrencyCode, false);
		}

		CsUIData.Instance.SetText(trImageBackground.Find("TextAcquisition"), "A125_TXT_00014", true);

		int nUnOwnDia = csCashProduct.UnOwnDia;

		CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == csCashProduct.ProductId);

		if (csCashProductPurchaseCount == null || csCashProductPurchaseCount.Count <= 0)
		{
			nUnOwnDia += csCashProduct.FirstPurchaseBonusUnOwnDia;
		}

		CsUIData.Instance.SetText(trImageBackground.Find("ImageFrameAcquiredDia/TextDia"), nUnOwnDia.ToString("#,##0"), false);

		CsUIData.Instance.SetButton(trImageBackground.Find("ButtonCancel"), OnClickButtonCancelPopupConfirm);
		CsUIData.Instance.SetText(trImageBackground.Find("ButtonCancel/TextCancel"), "A125_BTN_00015", true);
		CsUIData.Instance.SetButton(trImageBackground.Find("ButtonBuy"), () => OnClickButtonBuyPopupConfirm(csCashProduct));
		CsUIData.Instance.SetText(trImageBackground.Find("ButtonBuy/TextBuy"), "A125_BTN_00016", true);

		trPopupConfirm.gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonClosePopupConfirm()
	{
		transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCancelPopupConfirm()
	{
		transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonBuyPopupConfirm(CsCashProduct csCashProduct)
	{
		transform.Find("ImageBackground/PopupConfirm").gameObject.SetActive(false);
		transform.Find("ImageDim").gameObject.SetActive(true);

		m_csCashProduct = csCashProduct;

		CsCashManager.Instance.SendCashProductPurchaseStart(csCashProduct.ProductId);
	}

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

		Transform trImageBackground = transform.Find("ImageBackground");
		Transform trContent = trImageBackground.Find("Scroll View/Viewport/Content");

		Transform trChargingItem = trContent.Find(m_csCashProduct.InAppProduct.InAppProductKey.ToString());

		if (trChargingItem != null)
		{
			CsCashProductPurchaseCount csCashProductPurchaseCount = CsCashManager.Instance.CashProductPurchaseCountList.Find(purchaseCount => purchaseCount.ProductId == m_csCashProduct.ProductId);

			bool bFirstBuy = csCashProductPurchaseCount == null || csCashProductPurchaseCount.Count <= 0;

			trChargingItem.Find("TextFirstBuy").gameObject.SetActive(bFirstBuy);
			trChargingItem.Find("ImageIconFirstBuy").gameObject.SetActive(bFirstBuy);
		}
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
