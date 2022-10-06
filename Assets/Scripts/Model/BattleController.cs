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
				readyController.StartDelay(true);
			}
			else if (countPlayersReady <= 1 && !isBattle)
			{
				readyController.StartDelay(false);
			}
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
						customRaiseEvents.Send_GameOverLastPlayer(player.ActorNumber);
					}
				}

				isBattle = false;
			}
		}

		public void StartBattle()
		{
			Player[] players = PhotonNetwork.PlayerList;
			foreach (var player in players)
			{
				if ((bool) player.CustomProperties["isReady"])
				{
					customRaiseEvents.Send_StartBattle(player.ActorNumber, new Vector3Int(5, 5, 0));
				}
			}

			isBattle = true;
		}
	}
}