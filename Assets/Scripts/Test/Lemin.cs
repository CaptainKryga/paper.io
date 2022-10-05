using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

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

		[SerializeField] private Tilemap tilemap;
		[SerializeField] private TileBase tbClear, tbCapture, tbGhost, tbActual, tbStart, tbEnd;

		public enum ECaptured
		{
			clear,
			ghost,
			capture,
			start,
			end,
			collider
		}

		private LeminCell[][] map;

		private void Start()
		{
			if (PlayerPrefs.GetInt("size") <= 0)
				CreateDatabase();
			OnClick_Load();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
			{
				Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3Int point = tilemap.WorldToCell(mouseWorldPos);
				
				if (point.x >= size || point.x < 0 || point.y >= size || point.y < 0)
					return;
				
				
				UpdateLeminCell(map[point.x][point.y], point);
			}
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
			Vector3Int[] arr = leminLogic.GetCapturedCells(GetPath(), map);
			for (int x = 0; x < arr.Length; x++)
			{
				UpdateLeminCell(arr[x], Color.black);
			}
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
						// map[x][y].text.text = i.ToString();
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
						// map[x][y].text.text = 1.ToString();
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
			// cell.img.color = cActual;
			cell.type = GetTypeFromColor(cActual);
		}
		
		public void UpdateLeminCell(LeminCell cell, Vector3Int vec)
		{
			// cell.img.color = cActual;
			cell.type = GetTypeFromColor(cActual);
			
			tilemap.SetTileFlags(vec, TileFlags.None);
			tilemap.SetColor(vec, cActual);
		}

		public void UpdateLeminCell(LeminCell cell, Color color)
		{
			// cell.img.color = color;
			cell.type = GetTypeFromColor(color);
		}

		public void UpdateLeminCell(Vector3Int vec, Color color)
		{
			// cell.img.color = color;
			map[vec.x][vec.y].type = GetTypeFromColor(color);
			
			tilemap.SetTileFlags(vec, TileFlags.None);
			tilemap.SetColor(vec, cActual);
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

		private ECaptured GetTypeFromTileBase(TileBase tb)
		{
			if (tb == tbCapture)
				return ECaptured.capture;
			else if (tb == tbGhost)
				return ECaptured.ghost;
			else if (tb == tbStart)
				return ECaptured.start;
			else if (tb == tbEnd)
				return ECaptured.end;
			return ECaptured.clear;
		}
		
		private TileBase GetTileBaseFromType(ECaptured type)
		{
			if (type == ECaptured.capture)
				return tbCapture;
			else if (type == ECaptured.ghost)
				return tbGhost;
			else if (type == ECaptured.start)
				return tbStart;
			else if (type == ECaptured.end)
				return tbEnd;
			return tbClear;
		}

		private void CreateDatabase()
		{
			PlayerPrefs.SetInt("size", size);

			size = PlayerPrefs.GetInt("size");
			map = new LeminCell[size][];
			tilemap.size = new Vector3Int(size, size);
			for (int y = 0; y < map.Length; y++)
			{
				map[y] = new LeminCell[size];
				for (int x = 0; x < map[y].Length; x++)
				{
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int) ECaptured.clear);
					tilemap.SetTile(new Vector3Int(y, x), GetTileBaseFromType(ECaptured.clear));
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
			size = PlayerPrefs.GetInt("size");
			map = new LeminCell[size][];
			tilemap.size = new Vector3Int(size, size);
			for (int y = 0; y < map.Length; y++)
			{
				map[y] = new LeminCell[size];
				for (int x = 0; x < map[y].Length; x++)
				{
					map[y][x] = new LeminCell();

					ECaptured type = (ECaptured) PlayerPrefs.GetInt("test:" + y + "|" + x);
					map[y][x].type = type;
					
					tilemap.SetTile(new Vector3Int(y, x), GetTileBaseFromType(type));
				}
			}

			Debug.Log("Load");
		}
	}
}
