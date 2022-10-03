using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : PTilemap
	{
		[SerializeField] private Tilemap capture;
		private Color colorCapture;

		private bool isCapture;
		private List<Vector3Int> list = new List<Vector3Int>();

		public Vector3Int[] UpdateTile(Vector3Int pos)
		{
			//if start captured
			if (!isCapture && capture.GetColor(pos) != colorCapture)
			{
				isCapture = true;
			}

			if (isCapture)
			{
				list.Add(pos);
				
				if (capture.GetColor(pos) == colorCapture)
				{
					isCapture = false;
					Vector3Int[] temp = list.ToArray();
					list.Clear();
					return temp;
				}
				tilemap.SetTile(pos, tile);
				tilemap.SetTileFlags(pos, TileFlags.None);
				tilemap.SetColor(pos, color);
			}

			return null;
		}

		public void Init(Color ghost, Color capture)
		{
			color = ghost;
			colorCapture = capture;
		}
	}
}
