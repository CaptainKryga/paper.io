using System.Collections.Generic;
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
			
			Debug.Log("arr: " + arr.Length);
			
			for (int x = 0; x < arr.Length; x++)
			{
				tilemap.SetTile(arr[x], tile);
				tilemap.SetTileFlags(arr[x], TileFlags.None);
				tilemap.SetColor(arr[x], color);
			}
		}

		public void Init(Vector3Int pos, Color capture)
		{
			color = capture;
			
			tilemap.SetTile(pos, tile);
			tilemap.SetTileFlags(pos, TileFlags.None);
			tilemap.SetColor(pos, this.color);
		}
		
		
		//static
		List<Vector3Int> list = new List<Vector3Int>();
		private Vector3Int[] GetCapturedArray(Vector3Int pos)
		{
			//now
			List<Vector3Int> check = new List<Vector3Int>();
			//next
			List<Vector3Int> save = new List<Vector3Int>();

			list.Add(pos);
			check.Add(pos);
			
			while (true)
			{
				for (int x = 0; x < check.Count; x++)
				{
					IsCapture(save, pos, pos + Vector3Int.right);
					IsCapture(save, pos, pos + -Vector3Int.right);
					IsCapture(save, pos, pos + Vector3Int.up);
					IsCapture(save, pos, pos + -Vector3Int.up);
				}

				check.Clear();
				for (int x = 0; x < save.Count; x++)
				{
					check.Add(save[x]);
					list.Add(save[x]);
				}
				
				if (check.Count == 0)
					break;
			}

			return list.ToArray();
		}

		private void IsCapture(List<Vector3Int> save, Vector3Int pos, Vector3Int newPos)
		{
			if (!list.Contains(newPos) && tilemap.GetColor(pos) == color)
				save.Add(newPos);
		}
	}
}
