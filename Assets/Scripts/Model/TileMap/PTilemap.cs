using Model.Photon;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		[SerializeField] protected Tilemap tilemap;
		protected TileBase localTile;
		
		[SerializeField] protected CustomRaiseEvents customRaiseEvents;
	}
}