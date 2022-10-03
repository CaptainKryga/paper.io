using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : MonoBehaviour
	{
		[SerializeField] private Tilemap ghost;
		[SerializeField] private TileBase tile;

		public void UpdateTile(Vector3Int pos)
		{
			ghost.SetTile(pos, tile);
		}
	}
}
