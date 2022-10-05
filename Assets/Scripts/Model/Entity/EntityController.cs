using Model.Entity;
using Model.TileMap;
using UnityEngine;

namespace Model
{
	public class EntityController : MonoBehaviour
	{
		[SerializeField] private EntityInstance entityInstance;
		
		[SerializeField] private Ghost ghost;
		[SerializeField] private Capture capture;

		[SerializeField] private PlayerMove player;

		public void Restart(PlayerMove player, TileDataBase tileDataBase, int playerId)
		{
			this.player = player;

			ghost.Init(tileDataBase.tiles[playerId], tileDataBase.koofGhost, tileDataBase.koofCapture);
			capture.Init(Vector3Int.FloorToInt(player.transform.position), tileDataBase.tiles[playerId],
				tileDataBase.koofGhost, tileDataBase.koofCapture);
		}
		
		public void UpdatePosition(Vector3Int pos)
		{
			capture.UpdateCapture(ghost.UpdateTile(pos));
		}

		public void GameOver()
		{
			entityInstance.GameOver(player);
		}
	}
}