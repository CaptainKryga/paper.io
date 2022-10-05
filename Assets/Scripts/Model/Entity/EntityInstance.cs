using Model.Entity;
using Model.Photon;
using Model.TileMap;
using Photon.Pun;
using UnityEngine;

namespace Model
{
	public class EntityInstance : MonoBehaviour
	{
		[SerializeField] private MController mController;
		[SerializeField] private PhotonConnectRoom photonConnectRoom;

		[SerializeField] private TileDataBase tileDataBase;

		[SerializeField] private EntityController entityController;
		
		//restart game after death or win
		public void Restart(string playerName, int playerId)
		{
			//setPosition
			Vector3 startPosition = new Vector3(10.5f, 10.5f, 0);

			Debug.Log("playerId: " + playerId);
			
			entityController.Restart(photonConnectRoom.CreatePlayer(playerName, startPosition),
				tileDataBase.tiles[playerId], tileDataBase.sprites[playerId]);
		}

		public void GameOver(Player player)
		{
			photonConnectRoom.DestroyPlayer(player.gameObject);
			mController.GameOver();
		}
	}
}