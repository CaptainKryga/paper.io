using System.Collections.Generic;
using Test;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : PTilemap
	{
		[SerializeField] private TilemapInstance tilemapInstance;
		private Color colorCapture;

		private bool isCapture;
		private List<Vector3Int> list = new List<Vector3Int>();
		
		private LeminCell[][] cells;

		public Vector3Int[] UpdateTile(Vector3Int pos)
		{
			//if start captured
			if (!isCapture && local.GetTile(pos) != localTile)
			{
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.start;
				
				local.SetTile(pos, localTile);
				// remote.SetTileFlags(pos, TileFlags.None);
				// remote.SetColor(pos, colorGhost);
				isCapture = true;
				return null;
			}

			if (isCapture)
			{
				if (local.GetTile(pos) == localTile)
				{
					cells[list[^1].x][list[^1].y].type = Lemin.ECaptured.end;
					isCapture = false;
					Vector3Int[] temp = list.ToArray();
					list.Clear();
					return temp;
				}
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.ghost;

				remote.SetTile(pos, localTile);
				// remote.SetTileFlags(pos, TileFlags.None);
				// remote.SetColor(pos, colorGhost);
			}

			return null;
		}

		public void Init(TileBase tile, Color colorGhost, Color colorCapture)
		{
			this.localTile = tile;
			this.colorGhost = colorGhost;
			this.colorCapture = colorCapture;
			this.cells = tilemapInstance.GetCells;
		}
	}
}
