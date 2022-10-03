using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		protected Color colorCapture;
		protected Color colorGhost;
		[SerializeField] protected Tilemap capture;
		[SerializeField] protected Tilemap ghost;
		[SerializeField] protected TileBase tile;
	}
}