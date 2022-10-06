using Test;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Capture : PTilemap
	{
		[SerializeField] private TilemapInstance tilemapInstance;

		[SerializeField] private LeminLogic leminLogic;

		private int[][] test;

		private LeminCell[][] cells;

		public void UpdateCapture(Vector3Int[] path)
		{
			if (path == null)
				return;
			
			
			for (int x = 0; x < cells.Length; x++)
			{
				for (int y = 0; y < cells.Length; y++)
				{
					cells[x][y].Debug = cells[x][y].type == Lemin.ECaptured.clear ? "-" :
						cells[x][y].type.ToString().Substring(0, 2);
				}
			}

			Vector3Int[] captured = leminLogic.GetCapturedCells(path, cells);
			
			for (int x = 0; x < path.Length; x++)
			{
				local.SetTile(path[x], localTile);
				// local.SetTileFlags(path[x], TileFlags.None);
				// local.SetColor(path[x], colorCapture);
				cells[path[x].x][path[x].y].type = Lemin.ECaptured.capture;
			}
			
			for (int x = 0; x < captured.Length; x++)
			{
				local.SetTile(captured[x], localTile);
				// local.SetTileFlags(captured[x], TileFlags.None);
				// local.SetColor(captured[x], colorCapture);
				cells[captured[x].x][captured[x].y].type = Lemin.ECaptured.capture;
			}
			
		}

		public void Init(Vector3Int pos, TileBase tile, Color colorGhost, Color colorCapture)
		{
			this.localTile = tile;
			this.colorCapture = colorCapture;
			this.colorGhost = colorGhost;

			this.cells = tilemapInstance.GetCells;

			InitCell(pos);
		}

		private void InitCell(Vector3Int pos)
		{
			local.SetTile(pos, localTile);
			// local.SetTileFlags(pos, TileFlags.None);
			// local.SetColor(pos, this.colorCapture);
			cells[pos.x][pos.y].type = Lemin.ECaptured.capture;
		}

	}
}
