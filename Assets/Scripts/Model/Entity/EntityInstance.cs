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

		[SerializeField] private TileDataBase tileDataBase;

		[SerializeField] private EntityController entityController;
		[SerializeField] private PlayerMove playerMove;
		
		//restart game after death or win
		public void Restart(string playerName, int playerId)
		{
			playerMove.Init(entityController, photonConnectRoom.CreatePlayer(playerName));
			entityController.Restart(playerMove, tileDataBase, playerId);
		}

		public void GameOver(PlayerMove player)
		{
			photonConnectRoom.DestroyPlayer(player.gameObject);
			mController.GameOver();
		}
	}
}