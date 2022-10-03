using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.TileMap
{
	public class Ghost : PTilemap
	{
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
		}
	}
}
