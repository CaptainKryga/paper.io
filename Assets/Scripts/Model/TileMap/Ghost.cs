using System.Collections.Generic;
using Model.Photon;
using Test;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : PTilemap
	{
		[SerializeField] private TilemapInstance tilemapInstance;

		private bool isCapture;
		private List<Vector3Int> list = new List<Vector3Int>();
		
		private LeminCell[][] cells;

		public Vector3Int[] UpdateTile(Vector3Int pos)
		{
			//if start captured
			if (!isCapture && cells[pos.x][pos.y].type != Lemin.ECaptured.capture)
			{
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.start;
				
				local.SetTile(pos, localTile);
				local.SetTileFlags(pos, TileFlags.None);
				local.SetColor(pos, Color.yellow);
				
				customRaiseEvents.Request_UpdateTileMapGhost(pos, tilemapInstance.GetTileId(localTile));
				
				isCapture = true;
				return null;
			}

			if (isCapture)
			{
				if (cells[pos.x][pos.y].type == Lemin.ECaptured.capture)
				{
					cells[list[^1].x][list[^1].y].type = Lemin.ECaptured.end;
					isCapture = false;
					Vector3Int[] temp = list.ToArray();
					list.Clear();
					return temp;
				}
				list.Add(pos);
				cells[pos.x][pos.y].type = Lemin.ECaptured.ghost;

				local.SetTile(pos, localTile);
				local.SetTileFlags(pos, TileFlags.None);
				local.SetColor(pos, Color.yellow);
				
				customRaiseEvents.Request_UpdateTileMapGhost(pos, tilemapInstance.GetTileId(localTile));
			}

			return null;
		}

		public void Init(TileBase tile)
		{
			this.localTile = tile;
			this.cells = tilemapInstance.GetCells;
		}
	}
}
