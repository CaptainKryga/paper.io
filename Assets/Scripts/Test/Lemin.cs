using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Test
{
	public class Lemin : MonoBehaviour
	{
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
			GetResult();
		}

		public void OnClick_SetAll()
		{
			for (int y = 0; y < map.Length; y++)
			for (int x = 0; x < map[y].Length; x++)
				UpdateLeminCell(map[y][x]);
		}

		public void UpdateLeminCell(LeminCell cell)
		{
			cell.img.color = cActual;
			cell.type = GetTypeFromColor(cActual);
		}

		private void UpdateLeminCell(LeminCell cell, Color color)
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

		private void GetResult()
		{
			//find all ghost element's
			List<Vector2Int> temp = new List<Vector2Int>();
			List<Vector2Int> recursiveStartEnd = new List<Vector2Int>();

			int i = 0;
			for (int x = 0; x < map.Length; x++)
			{
				for (int y = 0; y < map.Length; y++)
				{
					if (map[x][y].type is ECaptured.ghost or ECaptured.start or ECaptured.end)
					{
						temp.Add(new Vector2Int(x, y));
						map[x][y].text.text = i.ToString();
						// UpdateLeminCell(map[x][y], Color.yellow);
						i++;

						if (map[x][y].type == ECaptured.start)
						{
							isExit = false;
							RecursiveFindStartEnd(new Vector2Int(x, y), recursiveStartEnd);
							Debug.Log("RecursiveFindStartEnd: " + recursiveStartEnd.Count);
						}

						// Debug.Log("Type: " + map[x][y].type);
					}
				}
			}

			for (int x = 0; x < recursiveStartEnd.Count; x++, i++)
			{
				map[recursiveStartEnd[x].x][recursiveStartEnd[x].y].text.text = i.ToString();
				temp.Add(recursiveStartEnd[x]);
				// UpdateLeminCell(map[recursiveStartEnd[x].x][recursiveStartEnd[x].y], Color.yellow);
			}


			//if ghost element's <= 0 return
			if (temp.Count <= 0)
				return;

			//find pair element's from custom lemin
			Vector2Int[] captured = temp.ToArray();

			List<LeminSub> tempX = new List<LeminSub>();
			List<LeminSub> tempY = new List<LeminSub>();

			List<Vector2Int> temp2 = captured.ToList();

			//get all once level cell's
			//get min max
			int min = temp2[0].y, max = min;
			for (int x = 0; x < temp2.Count; x++)
			{
				if (temp2[x].y > max) max = temp2[x].y;
				if (temp2[x].y < min) min = temp2[x].y;
			}

			//find level item's
			// List<Vector2Int> list = new List<Vector2Int>();
			temp2 = temp2.OrderBy(l => l.x).ToList();
			temp2 = temp2.OrderBy(l => l.y).ToList();
			Debug.Log("count: " + temp2.Count);
			temp2 = temp2.Distinct().ToList();
			Debug.Log("count: " + temp2.Count);
			for (int x = 0; x < temp2.Count; x++)
			{
				Debug.Log("sort: " + temp2[x]);
			}

			//pair's
			for (int y = 0; y < temp2.Count;)
			{
				if (y + 1 >= temp2.Count)
					break;

				//check capture or not
				if (map[temp2[y].x][temp2[y].y].type == ECaptured.capture &&
					temp2[y] + Vector2Int.right == temp2[y + 1])
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp2[y].x][temp2[y].y].text.text = "capt\n" + temp2[y].y;
					y++;
					continue;
				}

				//bad var *---\n
				if (temp2[y].y != temp2[y + 1].y)
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp2[y].x][temp2[y].y].text.text = "step\n" + temp2[y].y;
					y++;
					continue;
				}

				//nice var *----i
				if (temp2[y] + Vector2Int.right != temp2[y + 1])
				{
					tempX.Add(new LeminSub() {min = temp2[y], max = temp2[y + 1]});
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.red);
					map[temp2[y].x][temp2[y].y].text.text = "min\n" + temp2[y].y;
					// UpdateLeminCell(map[temp2[y + 1].x][temp2[y + 1].y], Color.grey);
					map[temp2[y + 1].x][temp2[y + 1].y].text.text = "max\n" + temp2[y].y;
					y += 2;
				}
				//bad var *i?????
				else if (temp2[y] + Vector2Int.right == temp2[y + 1])
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp2[y].x][temp2[y].y].text.text = "2\n" + temp2[y].y;
					// UpdateLeminCell(map[temp2[y + 1].x][temp2[y + 1].y], Color.yellow);
					map[temp2[y + 1].x][temp2[y + 1].y].text.text = "2\n" + temp2[y + 1].y;
					y += 2;
				}
			}

			// return;

			//find new captured cell's from width search(custom lemin)
			deep = 0;
			for (int x = 0; x < tempX.Count; x++)
			{
				for (int x2 = 1; x2 < tempX[x].max.x - tempX[x].min.x; x2++)
				{
					Vector2Int vec = tempX[x].min + Vector2Int.right * x2;
					if (map[vec.x][vec.y].type == ECaptured.clear)
					{
						RecursiveFindEmptySlots(vec);
					}
				}
			}

			//check not pair's element's
			//

		}

		private bool isExit = false;

		private void RecursiveFindStartEnd(Vector2Int start, List<Vector2Int> list)
		{
			list.Add(start);
			if (!isExit && map[start.x][start.y].type == ECaptured.end)
			{
				isExit = true;
				return;
			}

			if (start.x + 1 < map.Length && !isExit && !list.Contains(start + Vector2Int.right) &&
				map[start.x + 1][start.y].type is ECaptured.capture or ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector2Int.right, list);
				// if (isExit) list.Add(start + Vector2Int.right);
			}

			if (start.x - 1 >= 0 && !isExit && !list.Contains(start - Vector2Int.right) &&
				map[start.x - 1][start.y].type is ECaptured.capture or ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector2Int.right, list);
				// if (isExit) list.Add(start - Vector2Int.right);
			}

			if (start.y + 1 < map.Length && !isExit && !list.Contains(start + Vector2Int.up) &&
				map[start.x][start.y + 1].type is ECaptured.capture or ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector2Int.up, list);
				// if (isExit) list.Add(start + Vector2Int.up);
			}

			if (start.y - 1 >= 0 && !isExit && !list.Contains(start - Vector2Int.up) &&
				map[start.x][start.y - 1].type is ECaptured.capture or ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector2Int.up, list);
				// if (isExit) list.Add(start - Vector2Int.up);
			}

			if (!isExit)
				list.Remove(start);
		}

		private int deep = 0;

		private void RecursiveFindEmptySlots(Vector2Int pos)
		{
			UpdateLeminCell(map[pos.x][pos.y], cCapture);
			map[pos.x][pos.y].text.text = pos + "\n" + deep++.ToString();
			if (map[pos.x + 1][pos.y].type == ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector2Int.right);
			}

			if (map[pos.x - 1][pos.y].type == ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos - Vector2Int.right);
			}

			if (map[pos.x][pos.y + 1].type == ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector2Int.up);
			}

			if (map[pos.x][pos.y - 1].type == ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos - Vector2Int.up);
			}
		}
	}

	public struct LeminSub
	{
		public Vector2Int max;
		public Vector2Int min;
	}
}
