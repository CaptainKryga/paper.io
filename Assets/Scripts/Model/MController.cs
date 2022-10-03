using Model.Entity;
using Model.Photon;
using UnityEngine;
using View;

namespace Model
{
	public class MController : MonoBehaviour
	{
		[SerializeField] private PhotonConnectRoom photonConnectRoom;
		[SerializeField] private VController vController;

		[SerializeField] private Player player;

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
			vController.InitPlayer();
		}

		//restart game after death or win
		public void Restart()
		{
			player = photonConnectRoom.CreatePlayer();
			player.Init(this);
		}

		public void GameOver()
		{
			photonConnectRoom.DestroyPlayer(player.gameObject);
			vController.GameOver();
		}
	}
}
