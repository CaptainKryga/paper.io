using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		protected Color color;
		[SerializeField] protected Tilemap tilemap;
		[SerializeField] protected TileBase tile;
	}
}