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
		public Action<bool> ReceiveGameOverLastPlayer_Action;
		
		public Action SendBattleUpdatePlayer_Action;
		public Action SendReadyUpdatePlayer_Action;
		
		public Action<bool> ReadyUpdateAllPlayers_Action;
		public Action EndBattle_Action;
		
		public Action<Vector3Int, int> UpdateTileMapGhost_Action;
		public Action<Vector3Int[], int> UpdateTileMapCapture_Action;
		public Action<int> AttackPlayer_Action;

		//menu 0 - 100
		//send data from player
		private const byte code_ReceiveDataGame = 0;
		//getData from master
		private const byte code_GetDataGame = 1;
		
		//send other player's
		private const byte code_LocalUpdateFlagPlayer = 2;
		
		//game 100 - 255
		private const byte send_StartBattle = 100;
		private const byte code_GameOverLastPlayer = 101;
		
		private const byte code_BattleUpdatePlayer = 102;
		private const byte code_ReadyUpdatePlayer = 103;
		private const byte code_ReadyUpdateAllPlayers = 104;
		private const byte code_EndBattle = 105;
		
		private const byte code_UpdateTileMapGhost = 106;
		private const byte code_UpdateTileMapCapture = 107;
		
		private const byte code_AttackPlayer = 108;

		private void OnEnable()
		{
			PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
		}

		private void OnDisable()
		{
			PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
		}

		public void Request_GetDataGame()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(code_GetDataGame, 0, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void Request_LocalFlagPlayer(int old, int now)
		{
			object[] content = { old, now };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			PhotonNetwork.RaiseEvent(code_LocalUpdateFlagPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_StartBattle(int actorId, Vector3 position)
		{
			object[] content = { actorId, position };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(send_StartBattle, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_GameOverLastPlayer(int actorId)
		{
			object[] content = { actorId, true };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(code_GameOverLastPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_BattleUpdatePlayer()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(code_BattleUpdatePlayer, 0, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_ReadyUpdatePlayer()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
			PhotonNetwork.RaiseEvent(code_ReadyUpdatePlayer, 0, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_ReadyUpdateAllPlayers(bool isReady)
		{
			object[] content = { isReady };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(code_ReadyUpdateAllPlayers, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_EndBattle()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
			PhotonNetwork.RaiseEvent(code_EndBattle, 0, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void Request_UpdateTileMapGhost(Vector3Int vector, int playerId)
		{
			object[] content = { (Vector3)vector, playerId };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			PhotonNetwork.RaiseEvent(code_UpdateTileMapGhost, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_UpdateTileMapCapture(Vector3Int[] vectors, int playerId)
		{
			object[] content = new object[vectors.Length + 1];
			for (int x = 0; x < vectors.Length; x++)
				content[x] = (Vector3)vectors[x];
			content[^1] = playerId;

			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			PhotonNetwork.RaiseEvent(code_UpdateTileMapCapture, content, raiseEventOptions,
				SendOptions.SendReliable);
		}
		
		public void Request_AttackPlayer(int enemyId)
		{
			object[] content = { enemyId };
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			PhotonNetwork.RaiseEvent(code_AttackPlayer, content, raiseEventOptions,
				SendOptions.SendReliable);
		}

		public void OnEvent(EventData photonEvent)
		{
			byte eventCode = photonEvent.Code;

			//send old data
			if (eventCode == code_ReceiveDataGame)
			{
				SendDataGame_Action?.Invoke();
			}
			//get old data
			else if (eventCode == code_GetDataGame)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveDataGame_Action?.Invoke();
			}
			//update player change flag in game
			else if (eventCode == code_LocalUpdateFlagPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				
				ReceiveLocalUpdateFlagPlayer_Action?.Invoke((int)data[0], (int)data[1]);
			}
			//send start battle event and startposition's
			else if (eventCode == send_StartBattle)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				Vector3 pos = (Vector3) data[1];
				
				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					ReceiveStartBattle_Action?.Invoke(Vector3Int.FloorToInt(pos));
			}
			//last player on battle map
			else if (eventCode == code_GameOverLastPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				int actorId = (int) data[0];
				bool isWin = (bool) data[1];
				
				if (PhotonNetwork.LocalPlayer.ActorNumber == actorId)
					ReceiveGameOverLastPlayer_Action?.Invoke(isWin);
			}
			//update MC battle count player's
			else if (eventCode == code_BattleUpdatePlayer)
			{
				SendBattleUpdatePlayer_Action?.Invoke();
			}
			//update MC ready count player's
			else if (eventCode == code_ReadyUpdatePlayer)
			{
				SendReadyUpdatePlayer_Action?.Invoke();
			}
			//update ready all player's
			else if (eventCode == code_ReadyUpdateAllPlayers)
			{
				object[] data = (object[])photonEvent.CustomData;
				bool isReady = (bool) data[0];
				
				ReadyUpdateAllPlayers_Action?.Invoke(isReady);
			}
			//end battle
			else if (eventCode == code_EndBattle)
			{
				EndBattle_Action?.Invoke();
			}
			//remote update ghost
			else if (eventCode == code_UpdateTileMapGhost)
			{
				object[] data = (object[])photonEvent.CustomData;

				UpdateTileMapGhost_Action?.Invoke(Vector3Int.FloorToInt((Vector3) data[0]), (int) data[1]);
			}
			//remote update capture
			else if (eventCode == code_UpdateTileMapCapture)
			{
				object[] data = (object[])photonEvent.CustomData;
				Vector3Int[] vectors = new Vector3Int[data.Length - 1];
				for (int x = 0; x < vectors.Length; x++)
					vectors[x] = Vector3Int.FloorToInt((Vector3)data[x]);
				int playerId = (int) data[^1];
	
				UpdateTileMapCapture_Action?.Invoke(vectors, playerId);
			}
			//attack player
			else if (eventCode == code_AttackPlayer)
			{
				object[] data = (object[])photonEvent.CustomData;
				int enemyId = (int) data[0];
	
				AttackPlayer_Action?.Invoke(enemyId);
			}
		}
	}
}