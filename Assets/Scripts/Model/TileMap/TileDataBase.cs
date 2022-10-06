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
		
		public int GetTileId(TileBase tile)
		{
			for (int x = 0; x < tiles.Length; x++)
			{
				if (tiles[x] == tile)
					return x;
			}

			return 0;
		}
	}
}