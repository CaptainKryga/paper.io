using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Test
{
	public class Lemin : MonoBehaviour
	{
		[SerializeField] private LeminLogic leminLogic;
		
		[SerializeField] private Transform parent;
		[SerializeField] private TMP_Text prefabDebug;

		[SerializeField] private int size = 10;
		[SerializeField] private int step = 5, width = 100;
		[SerializeField] private Vector3Int startPosition = new Vector3Int(100, 100, 0);

		// [SerializeField] private Color cClear, cCapture, cGhost, cActual, cStart, cEnd;

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

		private LeminCell[][] cells;

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
				
				UpdateLeminCell(point);
			}
		}

		public void OnClick_SetClear()
		{
			tbActual = tbClear;
		}

		public void OnClick_SetCapture()
		{
			tbActual = tbCapture;
		}

		public void OnClick_SetGhost()
		{
			tbActual = tbGhost;
		}

		public void OnClick_SetStart()
		{
			tbActual = tbStart;
		}

		public void OnClick_SetEnd()
		{
			tbActual = tbEnd;
		}

		public void OnClick_Result()
		{
			Vector3Int[] path = GetPath();
			Vector3Int[] arr = leminLogic.GetCapturedCells(path, cells);
			for (int x = 0; x < path.Length; x++)
			{
				UpdateLeminCell(path[x], tbCapture);
				// cells[path[x].x][path[x].y].debug.text = "c";
			}
			for (int x = 0; x < arr.Length; x++)
			{
				UpdateLeminCell(arr[x], tbCapture);
				// cells[arr[x].x][arr[x].y].debug.text = "c";
			}
		}

		private Vector3Int[] GetPath()
		{
			List<Vector3Int> temp = new List<Vector3Int>();
			// temp.Add(GetStart());
			
			for (int x = 0, i = 1; x < cells.Length; x++)
			{
				for (int y = 0; y < cells.Length; y++)
				{
					if (cells[x][y].type is Lemin.ECaptured.ghost or Lemin.ECaptured.end)
					{
						temp.Add(new Vector3Int(x, y));
						Debug.Log(new Vector3Int(x, y));
						i++;
					}
					else if (cells[x][y].type is Lemin.ECaptured.start)
					{
						temp.Insert(0, new Vector3Int(x, y));
						i++;
					}
				}
			}

			return temp.ToArray();
		}

		public void OnClick_SetAll()
		{
			for (int y = 0; y < cells.Length; y++)
			{
				for (int x = 0; x < cells[y].Length; x++)
				{
					UpdateLeminCell(new Vector3Int(x, y));
				}
			}
		}

		public void UpdateLeminCell(Vector3Int vec)
		{
			cells[vec.x][vec.y].type = GetTypeFromTileBase(tbActual);
			tilemap.SetTile(vec, tbActual);
		}
		
		public void UpdateLeminCell(Vector3Int vec, TileBase tb)
		{
			cells[vec.x][vec.y].type = GetTypeFromTileBase(tb);
			tilemap.SetTile(vec, tb);
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
			cells = new LeminCell[size][];
			tilemap.size = new Vector3Int(size, size);
			for (int y = 0; y < cells.Length; y++)
			{
				cells[y] = new LeminCell[size];
				for (int x = 0; x < cells[y].Length; x++)
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
			for (int y = 0; y < cells.Length; y++)
			{
				for (int x = 0; x < cells[y].Length; x++)
				{
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int) cells[y][x].type);
				}
			}

			Debug.Log("Save");
		}

		public void OnClick_Load()
		{
			size = PlayerPrefs.GetInt("size");
			cells = new LeminCell[size][];
			tilemap.size = new Vector3Int(size, size);
			for (int x = 0; x < cells.Length; x++)
			{
				cells[x] = new LeminCell[size];
				for (int y = 0; y < cells[x].Length; y++)
				{
					cells[x][y] = new LeminCell(
						Instantiate(prefabDebug, new Vector3(x + .5f, y + .5f), Quaternion.identity, parent));

					ECaptured type = (ECaptured) PlayerPrefs.GetInt("test:" + x + "|" + y);
					cells[x][y].type = type;
					
					tilemap.SetTile(new Vector3Int(x, y), GetTileBaseFromType(type));
				}
			}

			Debug.Log("Load");
		}
	}
}
