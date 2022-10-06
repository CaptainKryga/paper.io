using Model.Photon;
using Test;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class PTilemap : MonoBehaviour
	{
		[SerializeField] protected Tilemap tilemap;
		protected TileBase localTile;
		protected LeminCell[][] cells;

		[SerializeField] protected CustomRaiseEvents customRaiseEvents;
		
		public void GameOver()
		{
			for (int x = 0; x < cells.Length; x++)
			{
				for (int y = 0; y < cells.Length; y++)
				{
					cells[x][y].type = Lemin.ECaptured.clear;
					
					tilemap.SetTile(new Vector3Int(x, y), null);
					tilemap.SetTileFlags(new Vector3Int(x, y), TileFlags.None);
					tilemap.SetColor(new Vector3Int(x, y), Color.white);
				}
			}
		}
	}
}