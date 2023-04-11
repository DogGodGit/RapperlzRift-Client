using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIPopup : UIComponent
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	public const int	kStatus_Init	= 0;
	public const int	kStatus_Opened	= 1;
	public const int	kStatus_Closed	= 2;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Events

	public event EventHandler	Closed = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	protected int	m_nStatus = kStatus_Init;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public virtual int status
	{
		get { return m_nStatus; }
	}

	public virtual bool opened
	{
		get { return m_nStatus == kStatus_Opened; }
	}

	public virtual bool closed
	{
		get { return m_nStatus == kStatus_Closed; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	public virtual void Open()
	{
		if (opened)
			return;

		if (closed)
			throw new InvalidOperationException();

		transform.SetParent(GameObject.Find("RootCanvas/PopupLayer").transform, false);

		m_nStatus = kStatus_Opened;
	}

	public virtual void Close(bool bRaiseEvent)
	{
		if (!opened)
			return;

		m_nStatus = kStatus_Closed;

		Destroy(gameObject);

		if (bRaiseEvent && Closed != null)
			Closed(this, EventArgs.Empty);
	}

	public virtual void Close()
	{
		Close(true);
	}
}
