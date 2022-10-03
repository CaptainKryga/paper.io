using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : MonoBehaviour
	{
		[SerializeField] private Tilemap ghost;
		[SerializeField] private TileBase tile;
		[SerializeField] private Color color;

		public void UpdateTile(Vector3Int pos)
		{
			ghost.SetTile(pos, tile);
		}

		public void UpdateStart(Vector3Int pos, Color color)
		{
			ghost.SetColor(pos, color);
			
			ghost.SetTile(pos, tile);
		}
	}
}
