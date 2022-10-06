using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		protected Color colorCapture;
		protected Color colorGhost;
		[SerializeField] protected Tilemap local;
		[SerializeField] protected Tilemap remote;
		[SerializeField] protected TileBase localTile;
	}
}