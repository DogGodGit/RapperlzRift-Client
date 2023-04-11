using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ClientCommon;

public abstract class CsPhotonSession : IPhotonPeerListener
{
	protected enum EnStatus { Closed, Connecting, Connected }
	protected PhotonPeer m_photonPeer;
	public PhotonPeer PhotonPeer { get { return m_photonPeer; } }
	protected EnStatus m_enStatus = EnStatus.Closed;

	//----------------------------------------------------------------------------------------------------
	public void Init(string strIpAddress, string strApplicationName, int nMaximumTransferUnit = 8192, int nWarningSize = 400)
	{
		if (m_photonPeer != null)
		{
			Disconnect();
		}
#if UNITY_EDITOR
        Debug.Log("CsPhotonSession.Init()  strIpAddress = " + strIpAddress + " // strApplicationName = " + strApplicationName);
#endif
        m_photonPeer = new PhotonPeer(this, ConnectionProtocol.Tcp);
		m_photonPeer.MaximumTransferUnit = nMaximumTransferUnit;
		m_photonPeer.WarningSize = nWarningSize;
        m_photonPeer.DisconnectTimeout = 30000;
		m_photonPeer.Connect(strIpAddress, strApplicationName);
		m_enStatus = EnStatus.Connecting;
	}

	//----------------------------------------------------------------------------------------------------
	public void Disconnect()
	{
		if (m_photonPeer != null)
		{
			m_photonPeer.Disconnect();
			m_photonPeer = null;
		}
		m_enStatus = EnStatus.Closed;
	}

	//----------------------------------------------------------------------------------------------------
	public void Service()
	{
		if (m_photonPeer != null)
		{
			m_photonPeer.Service();
		}
	}
	
	//----------------------------------------------------------------------------------------------------
	public bool IsConnected() { return EnStatus.Connected == m_enStatus; }
	public bool IsConnecting() { return EnStatus.Connecting == m_enStatus; }
	public bool HasPeer() { return m_photonPeer != null; }

	//----------------------------------------------------------------------------------------------------
	public void Send(OperationRequest request, bool sendReliable, byte channelId, bool encrypt)
	{
		if (m_photonPeer == null) return;
		m_photonPeer.OpCustom(request, sendReliable, channelId, encrypt);
	}

	//----------------------------------------------------------------------------------------------------
	public void Send(byte byCustomOpCode, Dictionary<byte, object> dicParameters, bool bSendReliable, byte byChannelId, bool bEncrypt)
	{
		if (m_photonPeer == null) return;
		m_photonPeer.OpCustom(byCustomOpCode, dicParameters, bSendReliable, byChannelId, bEncrypt);
	}

	#region Implementation of IPhotonPeerListener
	//----------------------------------------------------------------------------------------------------
	public void DebugReturn(DebugLevel level, string message)
	{
	}

	//----------------------------------------------------------------------------------------------------
	public abstract void OnEvent(EventData eventData);
	public abstract void OnOperationResponse(OperationResponse operationResponse);

	//----------------------------------------------------------------------------------------------------
	public void OnStatusChanged(StatusCode enStatusCode)
	{
		switch (enStatusCode)
		{
			case StatusCode.Connect:
				m_photonPeer.EstablishEncryption();
				break;

			case StatusCode.Disconnect:
			case StatusCode.DisconnectByServer:
			case StatusCode.DisconnectByServerLogic:
			case StatusCode.DisconnectByServerUserLimit:
			case StatusCode.Exception:
			case StatusCode.ExceptionOnConnect:
			case StatusCode.TimeoutDisconnect:
				if (IsConnected() || IsConnecting())
				{
					m_enStatus = EnStatus.Closed;
					m_photonPeer.Disconnect();
					m_photonPeer = null;
					Debug.Log(enStatusCode.ToString());
					OnDisconnected(enStatusCode);
				}
				break;

			case StatusCode.EncryptionEstablished:
				m_enStatus = EnStatus.Connected;
				Debug.Log("OnConnected");
				OnConnected();
				break;

			case StatusCode.QueueIncomingReliableWarning:
				Debug.Log("QueueIncomingReliableWarningQueueIncomingReliableWarningQueueIncomingReliableWarning");
				break;

			case StatusCode.QueueIncomingUnreliableWarning:
				Debug.Log("QueueIncomingUnreliableWarningQueueIncomingUnreliableWarningQueueIncomingUnreliableWarning");
				break;

			default:
				Disconnect();
				OnDisconnected(enStatusCode);
				break;
		}
	}

	#endregion

	//----------------------------------------------------------------------------------------------------
	protected abstract void OnConnected();
	protected abstract void OnDisconnected(StatusCode enStatusCode);
}
