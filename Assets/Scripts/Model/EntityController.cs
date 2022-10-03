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

			Color colorGhost = player.Color * new Color(1f, 1f, 1f, .5f);
			Color colorCapture = player.Color * new Color(1f, 1f, 1f, .9f);
			
			ghost.Init(colorGhost, colorCapture);
			capture.Init(Vector3Int.FloorToInt(player.transform.position), colorCapture);
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