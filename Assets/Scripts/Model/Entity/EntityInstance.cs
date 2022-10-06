using System;
using Model.Entity;
using Model.Photon;
using Model.TileMap;
using UnityEngine;

namespace Model
{
	public class EntityInstance : MonoBehaviour
	{
		[SerializeField] private MController mController;
		[SerializeField] private PhotonConnectRoom photonConnectRoom;
		[SerializeField] private CustomRaiseEvents customRaiseEvents;  

		[SerializeField] private TileDataBase tileDataBase;

		[SerializeField] private EntityController entityController;
		[SerializeField] private PlayerMove player;

		public TileDataBase TileDataBase => tileDataBase;

		private void OnEnable()
		{
			customRaiseEvents.ReceiveStartBattle_Action += StartBattle;
		}

		private void OnDisable()
		{
			customRaiseEvents.ReceiveStartBattle_Action -= StartBattle;
		}

		public void InitPlayer()
		{
			player.Init(photonConnectRoom.CreatePlayer("player"));
			entityController.InitPlayer(player);
		}
		
		//restart game after death or win
		public void UpdatePlayer(string playerName, int playerId)
		{
			photonConnectRoom.UpdatePlayer(playerName);
			player.UpdatePlayer(playerName, playerId);
		}
		
		public void StartBattle(Vector3Int position)
		{
			entityController.StartBattle(position, tileDataBase, player.PlayerId);
		}

		public void GameOver(PlayerMove player, bool isWin)
		{
			// photonConnectRoom.DestroyPlayer(player.gameObject);
			mController.GameOver(isWin);
		}
	}
}