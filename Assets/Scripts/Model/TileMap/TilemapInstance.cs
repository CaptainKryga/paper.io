using Test;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class TilemapInstance : MonoBehaviour
	{
		[SerializeField] private TileDataBase tileDataBase;
		
		[SerializeField] private Tilemap back;
		[SerializeField] private TileBase tbClear, tbCapture, tbGhost, tbCollider;

		private LeminCell[][] cells;
		
		public Tilemap GetTileMap => back;
		public LeminCell[][] GetCells => cells;

		public void InitTileMap()
		{
			cells = new LeminCell[tileDataBase.sizeMap][];
			back.size = new Vector3Int(tileDataBase.sizeMap, tileDataBase.sizeMap);
			for (int x = 0; x < cells.Length; x++)
			{
				cells[x] = new LeminCell[tileDataBase.sizeMap];
				for (int y = 0; y < cells[x].Length; y++)
				{
					cells[x][y] = new LeminCell();
					// cells[x][y] = new LeminCell(
						// Instantiate(prefabDebug, new Vector3(x + .5f, y + .5f), Quaternion.identity, parent));
		 			cells[x][y].type = Lemin.ECaptured.clear;
					back.SetTile(new Vector3Int(x, y), GetTileBaseFromType(cells[x][y].type));
				}
			}
		}

		private TileBase GetTileBaseFromType(Lemin.ECaptured type)
		{
			if (type == Lemin.ECaptured.capture)
				return tbCapture;
			else if (type == Lemin.ECaptured.ghost)
				return tbGhost;
			else if (type == Lemin.ECaptured.collider)
				return tbCollider;
			return tbClear;
		}


		public int GetTileId(TileBase tile)
		{
			for (int x = 0; x < tileDataBase.tiles.Length; x++)
			{
				if (tileDataBase.tiles[x] == tile)
					return x;
			}

			return 0;
		}
	}
}