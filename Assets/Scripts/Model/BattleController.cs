using System;
using Model.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Model
{
	//Master-client
	public class BattleController : MonoBehaviour
	{
		[SerializeField] private ReadyController readyController;

		[SerializeField] private CustomRaiseEvents customRaiseEvents;

		private int countPlayersBattle;
		private bool isBattle;

		private void OnEnable()
		{
			customRaiseEvents.SendBattleUpdatePlayer_Action += PlayerUpdateBattle;
			customRaiseEvents.SendReadyUpdatePlayer_Action += PlayerUpdateReady;
			customRaiseEvents.ReadyUpdateAllPlayers_Action += ReadyUpdateAllPlayers;
		}

		private void OnDisable()
		{
			customRaiseEvents.SendBattleUpdatePlayer_Action -= PlayerUpdateBattle;
			customRaiseEvents.SendReadyUpdatePlayer_Action -= PlayerUpdateReady;
			customRaiseEvents.ReadyUpdateAllPlayers_Action -= ReadyUpdateAllPlayers;
		}

		public void PlayerUpdateReady(int playerActor, bool isReady)
		{
			Player[] players = PhotonNetwork.PlayerList;
			int countPlayersReady = 0;
			foreach (var player in players)
			{
				if ((bool) player.CustomProperties["isReady"])
				{
					countPlayersReady++;
				}
			}
			countPlayersBattle = countPlayersReady;

			if (countPlayersReady > 1 && !isBattle)
			{
				ReadyUpdateAllPlayers(true);
				customRaiseEvents.Request_ReadyUpdateAllPlayers(true);
			}
			else if (countPlayersReady <= 1 && !isBattle)
			{
				ReadyUpdateAllPlayers(false);
				customRaiseEvents.Request_ReadyUpdateAllPlayers(false);
			}
			Debug.Log("countPlayersReady: " + countPlayersReady);
		}

		public void PlayerUpdateBattle(int playerActor, bool isBattle)
		{
			Player[] players = PhotonNetwork.PlayerList;
			int countPlayerBattle = 0;
			foreach (var player in players)
			{
				if ((bool) player.CustomProperties["isBattle"])
				{
					countPlayerBattle++;
				}
			}

			if (countPlayerBattle <= 1)
			{
				//Game Over All Battle
				foreach (var player in players)
				{
					if ((bool) player.CustomProperties["isBattle"])
					{
						//gameover or win?
						customRaiseEvents.Request_GameOverLastPlayer(player.ActorNumber);
					}
				}

				isBattle = false;
				
				Debug.Log("countPlayerBattle: " + countPlayerBattle);
			}
		}

		public void StartBattle()
		{
			Player[] players = PhotonNetwork.PlayerList;
			foreach (var player in players)
			{
				if ((bool) player.CustomProperties["isReady"])
				{
					customRaiseEvents.Request_StartBattle(player.ActorNumber, new Vector3Int(5, 5, 0));
				}
			}

			isBattle = true;
		}

		private void ReadyUpdateAllPlayers(bool isReady)
		{
			readyController.StartDelay(isReady);
		}
	}
}