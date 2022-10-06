using System;
using Model.Photon;
using Model.TileMap;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Model
{
	//Master-client
	public class BattleController : MonoBehaviour
	{
		[SerializeField] private ReadyController readyController;
		[SerializeField] private TileDataBase tileDataBase;

		[SerializeField] private CustomRaiseEvents customRaiseEvents;

		private int countPlayersBattle;
		private bool isBattle;
		private int minPlayers = 2;

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

		private Vector3Int[] startPoints;
		private void Start()
		{
			startPoints = new Vector3Int[16];
			int point = tileDataBase.sizeMap / 5;
			for (int x = 0, i = 0; x < tileDataBase.sizeMap; x++)
			{
				for (int y = 0; y < tileDataBase.sizeMap; y++)
				{
					if (x != 0 && y != 0 && x % point == 0 && y % point == 0)
					{
						startPoints[i++] = new Vector3Int(x, y);
					}
				}
			}
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
			RandomStartPositions();
			for (int x = 0; x < players.Length; x++)
			{
				if ((bool)players[x].CustomProperties["isReady"])
				{
					customRaiseEvents.Request_StartBattle(players[x].ActorNumber, startPoints[x]);
				}
			}

			isBattle = true;
		}

		private void RandomStartPositions()
		{
			for (int x = 0; x < startPoints.Length; x++)
			{
				int rnd = Random.Range(0, startPoints.Length);
				Vector3Int temp = startPoints[rnd];
				startPoints[rnd] = startPoints[x];
				startPoints[x] = temp;
			}
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