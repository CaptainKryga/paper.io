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
		[SerializeField] private PlayerMove player;

		public TileDataBase TileDataBase => tileDataBase;

		public void InitPlayer()
		{
			player.Init(photonConnectRoom.CreatePlayer("player"));
			entityController.InitPlayer(player);
		}
		
		//restart game after death or win
		public void Restart(Vector3Int position, string playerName, int playerId)
		{
			photonConnectRoom.UpdatePlayer(playerName);
			player.UpdatePlayer(playerName, playerId);
		}
		
		public void StartBattle(Vector3Int position, int playerId)
		{
			entityController.StartBattle(position, tileDataBase, playerId);
		}

		public void GameOver(PlayerMove player)
		{
			photonConnectRoom.DestroyPlayer(player.gameObject);
			mController.GameOver();
		}
	}
}