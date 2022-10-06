using System;
using Model.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Model
{
	//Master-client
	public class BattleController : MonoBehaviour
	{
		[SerializeField] private ReadyController readyController;

		[SerializeField] private CustomRaiseEvents customRaiseEvents;

		private int countPlayersBattle;
		private bool isBattle;
		private int minPlayers = 1;

		private void OnEnable()
		{
			customRaiseEvents.SendBattleUpdatePlayer_Action += PlayerUpdateBattle;
			customRaiseEvents.SendReadyUpdatePlayer_Action += PlayerUpdateReady;
			customRaiseEvents.ReadyUpdateAllPlayers_Action += ReadyUpdateAllPlayers;

			customRaiseEvents.EndBattle_Action += GameOver;
		}

		private void OnDisable()
		{
			customRaiseEvents.SendBattleUpdatePlayer_Action -= PlayerUpdateBattle;
			customRaiseEvents.SendReadyUpdatePlayer_Action -= PlayerUpdateReady;
			customRaiseEvents.ReadyUpdateAllPlayers_Action -= ReadyUpdateAllPlayers;
			
			customRaiseEvents.EndBattle_Action -= GameOver;
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
			Debug.Log("countPlayersReady: " + countPlayersReady + "| battle ? " + isBattle);

			if (isBattle)
				return;
			
			ReadyUpdateAllPlayers(countPlayersReady >= minPlayers);
			customRaiseEvents.Request_ReadyUpdateAllPlayers(countPlayersReady >= minPlayers);
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

			if (countPlayerBattle < minPlayers)
			{
				//Game Over All Battle
				foreach (var player in players)
				{
					if ((bool) player.CustomProperties["isBattle"])
					{
						//gameover or win?
						customRaiseEvents.Request_GameOverLastPlayer(player.ActorNumber);
						//end game
						customRaiseEvents.Request_EndBattle();
					}
				}

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
					customRaiseEvents.Request_StartBattle(player.ActorNumber, new Vector3(5, 5, 0));
				}
			}

			isBattle = true;
		}

		public void GameOver()
		{
			isBattle = false;
		}

		private void ReadyUpdateAllPlayers(bool isReady)
		{
			readyController.StartDelay(isReady);
		}
	}
}