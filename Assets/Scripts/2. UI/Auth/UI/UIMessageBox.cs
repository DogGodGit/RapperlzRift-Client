using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIMessageBox : UIPopup
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private ScrollRect	m_scrollRect = null;
	private Text		m_messageLabel = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public ScrollRect scrollRect
	{
		get { return m_scrollRect; }
	}

	public Text messageLabel
	{
		get { return m_messageLabel; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	private void Awake()
	{
		m_scrollRect = transform.Find("Frame/Body/Scroll View").GetComponent<ScrollRect>();
		m_messageLabel = transform.Find("Frame/Body/Scroll View/Viewport/Content/Text").GetComponent<Text>();

		UIButton okButton = transform.Find("Frame/Body/OKButton").GetComponent<UIButton>();
		okButton.Clicked += OnOKButton_Clicked;
	}

	private void OnOKButton_Clicked(object sender, EventArgs e)
	{
		Close();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Static member functions

	public static UIMessageBox Create()
	{
		return Util.Instantiate<UIMessageBox>(ResourceUtil.LoadPrefab("UIMessageBox"));
	}

	public static UIMessageBox Show(string sContent, EventHandler closedHandler)
	{
		UIMessageBox msgBox = Create();
		msgBox.Closed += closedHandler;
		msgBox.messageLabel.text = sContent;
		msgBox.Open();

		return msgBox;
	}
}
