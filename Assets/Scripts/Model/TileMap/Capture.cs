using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Capture : PTilemap
	{
		public void UpdateCapture(Vector3Int[] arr)
		{
			if (arr == null)
				return;

			//крайние точки по x and y
			Vector2Int min = (Vector2Int) arr[0];
			Vector2Int max = min;
			
			for (int x = 0; x < arr.Length; x++)
			{
				if (arr[x].x > max.x) max.x = arr[x].x;
				if (arr[x].y > max.y) max.y = arr[x].y;
				if (arr[x].x < min.x) min.x = arr[x].x;
				if (arr[x].y < min.y) min.y = arr[x].y;
			}
			Debug.Log("arr: " + arr.Length + " |" + max + " | " + min);

			List<Vector3Int> temp = new List<Vector3Int>();
			for (int x = 0; x < Math.Abs(max.x - min.x); x++)
			{
				for (int y = 0; y < Math.Abs(max.y - min.y); y++)
				{
					IsCapture(temp, arr,new Vector3Int(min.x + x, min.y + y));
				}
			}

			Debug.Log("temp: " + temp.Count);

			for (int x = 0; x < arr.Length; x++)
			{
				capture.SetTile(arr[x], tile);
				capture.SetTileFlags(arr[x], TileFlags.None);
				capture.SetColor(arr[x], colorCapture);
				ghost.SetTile(arr[x], null);
			}
			
			temp = CheckFromTree(temp);
			
			for (int x = 0; x < temp.Count; x++)
			{
				capture.SetTile(temp[x], tile);
				capture.SetTileFlags(temp[x], TileFlags.None);
				capture.SetColor(temp[x], colorCapture);
			}
			

			// for (int x = 0; x < arr.Length; x++)
			// {
			// tilemap.SetTile(arr[x], tile);
			// tilemap.SetTileFlags(arr[x], TileFlags.None);
			// tilemap.SetColor(arr[x], color * new Color(1f,1f,1f, .24f));
			// }
		}

		public void Init(Vector3Int pos, Color colorGhost, Color colorCapture)
		{
			this.colorCapture = colorCapture;
			this.colorGhost = colorGhost;
			
			capture.SetTile(pos, tile);
			capture.SetTileFlags(pos, TileFlags.None);
			capture.SetColor(pos, this.colorCapture);
		}
		
		private void IsCapture(List<Vector3Int> save, Vector3Int[] arr, Vector3Int pos)
		{
			if (ghost.GetColor(pos) != colorCapture && !arr.Contains(pos))
				save.Add(pos);
		}

		private List<Vector3Int> CheckFromTree(List<Vector3Int> list)
		{
			for (int x = 0; x < list.Count; x++)
			{
				if (CheckSides(list, list[x]))
				{
					DeleteSides(list, list[x]);
					
					x = 0;
				}
			}
			
			return list;
		}

		private bool CheckSides(List<Vector3Int> list, Vector3Int pos)
		{
			if (CheckSide(pos, list, pos + Vector3Int.up) ||
				CheckSide(pos, list, pos - Vector3Int.up) ||
				CheckSide(pos, list, pos + Vector3Int.right) ||
				CheckSide(pos, list, pos - Vector3Int.right))
				return true;
			return false;
		}

		private bool CheckSide(Vector3Int pos2, List<Vector3Int> list, Vector3Int pos)
		{
			if (!list.Contains(pos))
			{
				if (capture.GetColor(pos) != colorCapture &&
					ghost.GetColor(pos) != colorGhost)
				{
					Debug.Log("CheckSide: " + pos2 + "|" + pos + capture.GetColor(pos) + "|" + colorCapture);
					Debug.Log("CheckSide: " + pos2 + "|" + pos + ghost.GetColor(pos) + "|" + colorGhost);
					return true;
				}
			}
			return false;
		}

		private void DeleteSides(List<Vector3Int> list, Vector3Int pos)
		{
			List<Vector3Int> del = new List<Vector3Int>();
			List<Vector3Int> save = new List<Vector3Int>();

			del.Add(pos);
			while (true)
			{
				for (int x = 0; x < del.Count; x++)
				{
					if (list.Contains(del[x] + Vector3Int.up))
						save.Add(del[x] + Vector3Int.up);
					if (list.Contains(del[x] - Vector3Int.up))
						save.Add(del[x] - Vector3Int.up);
					if (list.Contains(del[x] + Vector3Int.right))
						save.Add(del[x] + Vector3Int.right);
					if (list.Contains(del[x] - Vector3Int.right))
						save.Add(del[x] - Vector3Int.right);
				}

				for (int x = 0; x < del.Count; x++)
				{
					list.Remove(del[x]);
				}
				del.Clear();
				del = save;
				save = new List<Vector3Int>();
				
				if (del.Count == 0)
					break;
			}
		}
	}

	public class Tree
	{
		public Vector3Int pos;
		public Tree north, south, west, east;
		public bool isFail;
	}
}
