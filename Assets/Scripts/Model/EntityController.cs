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

		[SerializeField] private Player player;

		public void Restart(Player player)
		{
			this.player = player;
			player.Init(this);
		}
		
		public void UpdatePosition(Vector3Int pos)
		{
			ghost.UpdateTile(pos);
		}

		public void UpdateStart(Vector3Int pos, Color color)
		{
			ghost.UpdateStart(pos, color);
		}

		public void GameOver()
		{
			entityInstance.GameOver(player);
		}
	}
}