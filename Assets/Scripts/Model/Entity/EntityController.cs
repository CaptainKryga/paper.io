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
			ghost.Init(tileDataBase.tiles[playerId]);
			capture.Init(pos, tileDataBase.tiles[playerId]);
		}
		
		public void UpdatePosition(Vector3Int pos)
		{
			capture.UpdateCapture(ghost.UpdateTile(pos));
		}

		public void GameOver(bool isWin)
		{
			entityInstance.GameOver(_player, isWin);
			ghost.GameOver();
			capture.GameOver();
		}
	}
}