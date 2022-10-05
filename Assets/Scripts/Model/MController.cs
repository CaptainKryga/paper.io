using Model.Entity;
using Model.Photon;
using Model.TileMap;
using UnityEngine;
using View;

namespace Model
{
	public class MController : MonoBehaviour
	{
		[SerializeField] private PhotonConnectRoom photonConnectRoom;
		[SerializeField] private VController vController;

		[SerializeField] private TilemapInstance tilemapInstance;
		[SerializeField] private EntityInstance entityInstance;

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
			vController.InitPlayer();
		}

		//restart game after death or win
		public void Restart()
		{
			entityInstance.Restart();
		}

		public void GameOver()
		{
			vController.GameOver();
		}
	}
}
