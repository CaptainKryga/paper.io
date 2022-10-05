using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Model.Photon
{
	public class CustomRaiseEvents : MonoBehaviour, IOnEventCallback
	{
		public Action SendDataGame_Action;
		public Action ReceiveDataGame_Action;
		public Action<int, int> ReceiveLocalUpdateFlagPlayer_Action;
		
		//menu 0 - 100
		//send data from player
		private const byte send_DataGame = 0;
		//getData from master
		private const byte receive_DataGame = 1;
		
		//send other player's
		private const byte send_LocalUpdateFlagPlayer = 2;
		
		//game 101 - 255

		private void OnEnable()
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
		}

		private void OnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
		}

		public void Receive_DataGame()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(receive_DataGame, 0, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void Send_LocalFlagPlayer(int old, int now)
		{
			object[] content = { old, now };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.Others };
			PhotonNetwork.RaiseEvent(send_LocalUpdateFlagPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void OnEvent(EventData photonEvent)
		{
			byte eventCode = photonEvent.Code;

			if (eventCode == send_DataGame)
			{
				SendDataGame_Action?.Invoke();
			}
			else if (eventCode == receive_DataGame)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveDataGame_Action?.Invoke();
			}
			else if (eventCode == send_LocalUpdateFlagPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveLocalUpdateFlagPlayer_Action?.Invoke((int)data[0], (int)data[1]);
			}
		}
	}
}