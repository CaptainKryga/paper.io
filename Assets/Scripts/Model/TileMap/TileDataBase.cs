using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "TileDataBase", menuName = "ScriptableObjects/TileDataBase", order = 1)]
	public class TileDataBase : ScriptableObject
	{
		public int sizeMap;
		
		public TileBase[] tiles;
		public Sprite[] sprites;

		public Color koofGhost, koofCapture;
	}
}