using Test;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class TilemapInstance : MonoBehaviour
	{
		[SerializeField] private Tilemap back, collider;
		[SerializeField] private EdgeCollider2D edgeCollider;
		[SerializeField] private TileBase tbClear, tbCapture, tbGhost, tbCollider;

		[SerializeField] private Transform parent;
		[SerializeField] private TMP_Text prefabDebug;

		private LeminCell[][] cells;
		[SerializeField] private int size;
		
		public Tilemap GetTileMap => back;
		public LeminCell[][] GetCells => cells;

		public void InitTileMap()
		{
			cells = new LeminCell[size][];
			back.size = new Vector3Int(size, size);
			for (int x = 0; x < cells.Length; x++)
			{
				cells[x] = new LeminCell[size];
				for (int y = 0; y < cells[x].Length; y++)
				{
					cells[x][y] = new LeminCell(
						Instantiate(prefabDebug, new Vector3(x + .5f, y + .5f), Quaternion.identity, parent));
		 			cells[x][y].type = Lemin.ECaptured.clear;
					back.SetTile(new Vector3Int(x, y), GetTileBaseFromType(cells[x][y].type));
				}
			}
			InitCollider();
		}

		private void InitCollider()
		{
			for (int i = -1; i < size + 1; i++)
			{
				collider.SetTile(new Vector3Int(i, -1), GetTileBaseFromType(Lemin.ECaptured.collider));
				collider.SetTile(new Vector3Int(-1, i), GetTileBaseFromType(Lemin.ECaptured.collider));
				collider.SetTile(new Vector3Int(size - 1 - i, size), GetTileBaseFromType(Lemin.ECaptured.collider));
				collider.SetTile(new Vector3Int(size, size - 1 - i), GetTileBaseFromType(Lemin.ECaptured.collider));
			}

			edgeCollider.points = new Vector2[]
			{
				new Vector2(-2, -2),
				new Vector2(-2, size + 2),
				new Vector2(size + 2, size + 2),
				new Vector2(size + 2, -2),
				new Vector2(-2, -2),
			};
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
	}
}