using ExitGames.Client.Photon;
using Model.Photon;
using Model.TileMap;
using Photon.Pun;
using UnityEngine;
using View;

namespace Model
{
	public class MController : MonoBehaviour
	{
		[SerializeField] private PhotonConnectRoom photonConnectRoom;
		[SerializeField] private CustomRaiseEvents customRaiseEvents;
		[SerializeField] private VController vController;

		[SerializeField] private TilemapInstance tilemapInstance;
		[SerializeField] private EntityInstance entityInstance;
		[SerializeField] private BattleController battleController;

		[SerializeField] private SyncChangeFlag syncChangeFlag;

		//connect to server and init player
		public void Init()
		{
			if (!photonConnectRoom.IsConnect)
				photonConnectRoom.ConnectToServer();
			else
				InitPlayer();
		}
		public void InitPlayer()
		{
			tilemapInstance.InitTileMap();
			entityInstance.InitPlayer();
			vController.InitPlayer();
		}

		//restart game after death or win
		public void UpdatePlayer(string playerName, int playerId)
		{
			if (playerName == null || playerName.Length < 3 || playerId < 1 || playerId > 15)
			{
				syncChangeFlag.SelectedPlayerFlag(playerId);
				vController.ReceiveStartBattle(false);
				return;
			}
			else
			{
				
			}
			entityInstance.UpdatePlayer(playerName, playerId);
		}

		public void Ready(bool isReady)
		{
			Hashtable hash = new Hashtable();
			hash.Add("isReady", isReady);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			customRaiseEvents.Request_ReadyUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, isReady);
		}

		public void StartBattle()
		{
			//if all it's okey true
			vController.ReceiveStartBattle(true);
			
			if (PhotonNetwork.IsMasterClient)
				battleController.StartBattle();
			// battleController.
		}

		public void GameOver(bool isWin)
		{
			vController.GameOver(isWin);
		}
	}
}
