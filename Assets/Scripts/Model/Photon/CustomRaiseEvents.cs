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
		
		public Action<Vector3Int> ReceiveStartBattle_Action;
		public Action ReceiveGameOverLastPlayer_Action;
		
		public Action<int, bool> SendBattleUpdatePlayer_Action;
		public Action<int, bool> SendReadyUpdatePlayer_Action;
		
		//menu 0 - 100
		//send data from player
		private const byte send_DataGame = 0;
		//getData from master
		private const byte receive_DataGame = 1;
		
		//send other player's
		private const byte send_LocalUpdateFlagPlayer = 2;
		
		//game 100 - 255
		private const byte send_StartBattle = 100;
		private const byte receive_GameOverLastPlayer = 101;
		
		private const byte send_BattleUpdatePlayer = 102;
		private const byte send_ReadyUpdatePlayer = 103;

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
		
		public void Send_StartBattle(int actorId, Vector3Int position)
		{
			object[] content = { actorId, position };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(send_StartBattle, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Send_GameOverLastPlayer(int actorId)
		{
			object[] content = { actorId };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(send_LocalUpdateFlagPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Send_BattleUpdatePlayer(int actorId, bool isBattle)
		{
			object[] content = { actorId, isBattle };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(send_LocalUpdateFlagPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Send_ReadyUpdatePlayer(int actorId, bool isReady)
		{
			object[] content = { actorId, isReady };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(send_LocalUpdateFlagPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void OnEvent(EventData photonEvent)
		{
			byte eventCode = photonEvent.Code;

			//send old data
			if (eventCode == send_DataGame)
			{
				SendDataGame_Action?.Invoke();
			}
			//get old data
			else if (eventCode == receive_DataGame)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveDataGame_Action?.Invoke();
			}
			//update player change flag in game
			else if (eventCode == send_LocalUpdateFlagPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveLocalUpdateFlagPlayer_Action?.Invoke((int)data[0], (int)data[1]);
			}
			//send start battle event and startposition's
			else if (eventCode == send_StartBattle)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				Vector3Int pos = (Vector3Int) data[1];
				
				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					ReceiveStartBattle_Action?.Invoke(pos);
			}
			//last player on battle map
			else if (eventCode == receive_GameOverLastPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				
				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					ReceiveGameOverLastPlayer_Action?.Invoke();
			}
			//update battle count player's
			else if (eventCode == send_BattleUpdatePlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				bool isBattle = (bool) data[0];

				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					SendBattleUpdatePlayer_Action?.Invoke(actorId, isBattle);
			}
			//update ready count player's
			else if (eventCode == send_ReadyUpdatePlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				bool isReady = (bool) data[0];
				
				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					SendReadyUpdatePlayer_Action?.Invoke(actorId, isReady);
			}
		}
	}
}