using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIButton : UIComponent
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Events

	public event EventHandler	Clicked = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public Button button
	{
		get { return GetComponent<Button>(); }
	}

	public Text label
	{
		get { return button.GetComponentInChildren<Text>(); }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	private void Awake()
	{
		button.onClick.AddListener(OnButtonClicked);
	}

	private void OnButtonClicked()
	{
		if (Clicked != null)
			Clicked(this, EventArgs.Empty);
	}
}
