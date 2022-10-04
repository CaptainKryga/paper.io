using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	public class Lemin : MonoBehaviour
	{
		[SerializeField] private LeminLogic leminLogic;
		
		[SerializeField] private Transform parent;
		[SerializeField] private GameObject prefab;

		[SerializeField] private int size = 10;
		[SerializeField] private int step = 5, width = 100;
		[SerializeField] private Vector3Int startPosition = new Vector3Int(100, 100, 0);

		[SerializeField] private Color cClear, cCapture, cGhost, cActual, cStart, cEnd;

		public enum ECaptured
		{
			clear,
			ghost,
			capture,
			start,
			end
		}

		private LeminCell[][] map;

		private void Start()
		{
			if (PlayerPrefs.GetInt("size") <= 0)
				CreateDatabase();
			OnClick_Load();
		}

		public void OnClick_SetClear()
		{
			cActual = cClear;
		}

		public void OnClick_SetCapture()
		{
			cActual = cCapture;
		}

		public void OnClick_SetGhost()
		{
			cActual = cGhost;
		}

		public void OnClick_SetStart()
		{
			cActual = cStart;
		}

		public void OnClick_SetEnd()
		{
			cActual = cEnd;
		}

		public void OnClick_Result()
		{
			Vector3Int[] arr = leminLogic.GetCapturedCells(GetPath(), map, this, cCapture);
			// for (int x = 0; x < map.Length; x++)
			// {
				// UpdateLeminCell(map[arr[x].x][arr[x].y], cCapture);
			// }
		}

		private Vector3Int[] GetPath()
		{
			List<Vector3Int> temp = new List<Vector3Int>();
			temp.Add(GetStart());
			
			for (int x = 0, i = 1; x < map.Length; x++)
			{
				for (int y = 0; y < map.Length; y++)
				{
					if (map[x][y].type is Lemin.ECaptured.ghost or Lemin.ECaptured.start or Lemin.ECaptured.end)
					{
						temp.Add(new Vector3Int(x, y));
						map[x][y].text.text = i.ToString();
						// UpdateLeminCell(map[x][y], Color.yellow);
						i++;
					}
				}
			}

			return temp.ToArray();
		}

		private Vector3Int GetStart()
		{
			for (int x = 0; x < map.Length; x++)
			{
				for (int y = 0; y < map.Length; y++)
				{
					if (map[x][y].type == Lemin.ECaptured.start)
					{
						map[x][y].text.text = 1.ToString();
						// UpdateLeminCell(map[x][y], Color.yellow);
						return new Vector3Int(x, y);
					}
				}
			}

			Debug.LogError("Problem start position on path lemin");
			return Vector3Int.zero;
		}

		public void OnClick_SetAll()
		{
			for (int y = 0; y < map.Length; y++)
			{
				for (int x = 0; x < map[y].Length; x++)
				{
					UpdateLeminCell(map[y][x]);
				}
			}
		}

		public void UpdateLeminCell(LeminCell cell)
		{
			cell.img.color = cActual;
			cell.type = GetTypeFromColor(cActual);
		}

		public void UpdateLeminCell(LeminCell cell, Color color)
		{
			cell.img.color = color;
			cell.type = GetTypeFromColor(color);
		}

		private ECaptured GetTypeFromColor(Color color)
		{
			if (color == cCapture)
				return ECaptured.capture;
			else if (color == cGhost)
				return ECaptured.ghost;
			else if (color == cStart)
				return ECaptured.start;
			else if (color == cEnd)
				return ECaptured.end;
			return ECaptured.clear;
		}

		private Color GetColorFromType(ECaptured type)
		{
			if (type == ECaptured.capture)
				return cCapture;
			else if (type == ECaptured.ghost)
				return cGhost;
			else if (type == ECaptured.start)
				return cStart;
			else if (type == ECaptured.end)
				return cEnd;
			return cClear;
		}

		private void CreateDatabase()
		{
			PlayerPrefs.SetInt("size", size);

			map = new LeminCell[PlayerPrefs.GetInt("size")][];
			for (int y = 0; y < map.Length; y++)
			{
				map[y] = new LeminCell[PlayerPrefs.GetInt("size")];
				for (int x = 0; x < map[y].Length; x++)
				{
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int) ECaptured.clear);
				}
			}

			Debug.Log("CreateDatabase");
		}

		public void OnClick_Save()
		{
			PlayerPrefs.SetInt("size", size);
			for (int y = 0; y < map.Length; y++)
			{
				for (int x = 0; x < map[y].Length; x++)
				{
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int) map[y][x].type);
				}
			}

			Debug.Log("Save");
		}

		public void OnClick_Load()
		{
			map = new LeminCell[PlayerPrefs.GetInt("size")][];
			for (int y = 0; y < map.Length; y++)
			{
				map[y] = new LeminCell[PlayerPrefs.GetInt("size")];
				for (int x = 0; x < map[y].Length; x++)
				{
					map[y][x] = Instantiate(prefab, parent).GetComponent<LeminCell>();
					map[y][x].GetComponent<RectTransform>().position = startPosition +
																		new Vector3Int(y * width + y * step,
																			x * width + x * step, 0);
					map[y][x].Init(this, new Vector2(y, x));

					ECaptured type = (ECaptured) PlayerPrefs.GetInt("test:" + y + "|" + x);
					map[y][x].type = type;
					map[y][x].img.color = GetColorFromType(type);
				}
			}

			Debug.Log("Load");
		}
	}
}
