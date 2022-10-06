using System.Collections.Generic;
using Test;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Capture : PTilemap
	{
		[SerializeField] private TilemapInstance tilemapInstance;
		[SerializeField] private LeminLogic leminLogic;

		public void Init(Vector3Int pos, TileBase tile)
		{
			this.localTile = tile;
			this.cells = tilemapInstance.GetCells;

			InitCell(pos);
		}

		private void InitCell(Vector3Int pos)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					Vector3Int vec = pos + new Vector3Int(x, y);
					
					if (vec.x < 0 || vec.x >= cells.Length || vec.y < 0 || vec.y >= cells.Length)
						continue;
					
					tilemap.SetTile(vec, localTile);
					tilemap.SetTileFlags(vec, TileFlags.None);
					tilemap.SetColor(vec, Color.white);
					cells[vec.x][vec.y].type = Lemin.ECaptured.capture;

					list.Add(vec);
				}
			}
			
			customRaiseEvents.Request_UpdateTileMapCapture(list.ToArray(), tilemapInstance.GetTileId(localTile));
		}
		
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
				tilemap.SetTile(path[x], localTile);
				tilemap.SetTileFlags(path[x], TileFlags.None);
				tilemap.SetColor(path[x], Color.white);
				cells[path[x].x][path[x].y].type = Lemin.ECaptured.capture;
			}
			
			for (int x = 0; x < captured.Length; x++)
			{
				tilemap.SetTile(captured[x], localTile);
				tilemap.SetTileFlags(captured[x], TileFlags.None);
				tilemap.SetColor(captured[x], Color.white);
				cells[captured[x].x][captured[x].y].type = Lemin.ECaptured.capture;
			}
			
			customRaiseEvents.Request_UpdateTileMapCapture(path, tilemapInstance.GetTileId(localTile));
			customRaiseEvents.Request_UpdateTileMapCapture(captured, tilemapInstance.GetTileId(localTile));
		}
	}
}
