using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		[SerializeField] protected Tilemap local;
		[SerializeField] protected Tilemap remote;
		[SerializeField] protected TileBase localTile;
	}
}