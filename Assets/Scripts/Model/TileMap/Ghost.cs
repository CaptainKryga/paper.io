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
			if (!isCapture && capture.GetColor(pos) != colorCapture)
			{
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.start;
				
				ghost.SetTile(pos, tile);
				ghost.SetTileFlags(pos, TileFlags.None);
				ghost.SetColor(pos, colorGhost);
				isCapture = true;
				return null;
			}

			if (isCapture)
			{
				if (capture.GetColor(pos) == colorCapture)
				{
					cells[list[^1].x][list[^1].y].type = Lemin.ECaptured.end;
					isCapture = false;
					Vector3Int[] temp = list.ToArray();
					list.Clear();
					return temp;
				}
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.ghost;

				ghost.SetTile(pos, tile);
				ghost.SetTileFlags(pos, TileFlags.None);
				ghost.SetColor(pos, colorGhost);
			}

			return null;
		}

		public void Init(Color colorGhost, Color colorCapture)
		{
			this.colorGhost = colorGhost;
			this.colorCapture = colorCapture;
			this.cells = tilemapInstance.GetCells;
		}
	}
}
