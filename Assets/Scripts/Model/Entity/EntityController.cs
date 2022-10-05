using Model.Entity;
using Model.TileMap;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model
{
	public class EntityController : MonoBehaviour
	{
		[SerializeField] private EntityInstance entityInstance;
		
		[SerializeField] private Ghost ghost;
		[SerializeField] private Capture capture;

		[SerializeField] private PlayerSync playerSync;

		public void Restart(PlayerSync playerSync, TileBase tile, Sprite sprite)
		{
			this.playerSync = playerSync;
			playerSync.Init(this, sprite);

			Color colorGhost = playerSync.Color * new Color(1f, 1f, 1f, .5f);
			Color colorCapture = playerSync.Color * new Color(1f, 1f, 1f, .9f);
			
			ghost.Init(tile, colorGhost, colorCapture);
			capture.Init(Vector3Int.FloorToInt(playerSync.transform.position), tile, colorGhost, colorCapture);
		}
		
		public void UpdatePosition(Vector3Int pos)
		{
			capture.UpdateCapture(ghost.UpdateTile(pos));
		}

		public void GameOver()
		{
			entityInstance.GameOver(playerSync);
		}
	}
}