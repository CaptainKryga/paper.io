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

		private PlayerMove _player;

		public void InitPlayer(PlayerMove playerMove)
		{
			_player = playerMove;
		}
		
		public void StartBattle(Vector3Int pos, TileDataBase tileDataBase, int playerId)
		{
			ghost.Init(tileDataBase.tiles[playerId], tileDataBase.koofGhost, tileDataBase.koofCapture);
			capture.Init(pos, tileDataBase.tiles[playerId], tileDataBase.koofGhost, tileDataBase.koofCapture);
		}
		
		public void UpdatePosition(Vector3Int pos)
		{
			capture.UpdateCapture(ghost.UpdateTile(pos));
		}

		public void GameOver()
		{
			entityInstance.GameOver(_player);
		}
	}
}