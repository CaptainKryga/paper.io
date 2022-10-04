using System.Collections.Generic;
using System.Linq;
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

		[SerializeField] private Color cClear, cCapture, cGhost, cActual;

		public enum ECaptured
		{
			clear,
			ghost,
			capture
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
			cell.type = cActual == cCapture ? ECaptured.capture : cActual == cGhost ? ECaptured.ghost : ECaptured.clear;
		}
		
		private void UpdateLeminCell(LeminCell cell, Color color)
		{
			cell.img.color = color;
			cell.type = color == cCapture ? ECaptured.capture : cActual == cGhost ? ECaptured.ghost : ECaptured.clear;
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
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int)ECaptured.clear);
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
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int)map[y][x].type);
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
						new Vector3Int(y * width + y * step, x * width + x * step, 0);
					map[y][x].Init(this, new Vector2(y, x));

					Debug.Log(PlayerPrefs.GetInt("test:" + y + "|" + x));
					ECaptured cap = (ECaptured)PlayerPrefs.GetInt("test:" + y + "|" + x);
					map[y][x].type = cap;
					map[y][x].img.color = cap == ECaptured.capture ? cCapture : cap == ECaptured.ghost ? cGhost : Color.white;
				}
			}

			Debug.Log("Load");
		}

		private void GetResult()
		{
			//find all ghost element's
			List<Vector2Int> temp = new List<Vector2Int>();
			for (int x = 0; x < map.Length; x++)
			{
				for (int y = 0; y < map.Length; y++)
				{
					if (map[x][y].type == ECaptured.ghost)
					{
						temp.Add(new Vector2Int(x, y));
						// UpdateLeminImg(map[x][y], Color.yellow);
					}
				}
			}
			
			//if ghost element's <= 0 return
			if (temp.Count <= 0)
				return;

			//find pair element's from custom lemin
			Vector2Int[] captured = temp.ToArray();

			List<LeminSub> tempX = new List<LeminSub>();
			List<LeminSub> tempY = new List<LeminSub>();

			List<Vector2Int> temp2 = captured.ToList();
			
			//create list add all Y
			for (int i = 0; i < temp2.Count; i++)
			{
				bool isFlag = false;
				for (int j = 0; j < tempX.Count; j++)
				{
					if (temp2[i].y == tempX[j].max.y)
					{
						if (temp2[i].x > tempX[j].max.x)
						{
							tempX[j] = new LeminSub() { min = tempX[j].min, max = temp2[i]};
						}
						else if (temp2[i].x < tempX[j].min.x)
						{
							tempX[j] = new LeminSub() { min = temp2[i], max = tempX[j].max};
						}

						isFlag = true;
					}
				}

				if (!isFlag)
				{
					tempX.Add(new LeminSub() { min = temp2[i], max = temp2[i]});
				}
			}
			//create list add all X
			for (int i = 0; i < temp2.Count; i++)
			{
				bool isFlag = false;
				for (int j = 0; j < tempY.Count; j++)
				{
					if (temp2[i].x == tempY[j].max.x)
					{
						if (temp2[i].y > tempY[j].max.y)
						{
							tempY[j] = new LeminSub() { min = tempY[j].min, max = temp2[i]};
						}
						else if (temp2[i].y < tempY[j].min.y)
						{
							tempY[j] = new LeminSub() { min = temp2[i], max = tempY[j].max};
						}

						isFlag = true;
					}
				}

				if (!isFlag)
				{
					tempY.Add(new LeminSub() { min = temp2[i], max = temp2[i]});
				}
			}
			
			
			//find new captured cell's from width search(custom lemin)
			deep = 0;
			for (int x = 0; x < tempX.Count; x++)
			{
				for (int x2 = 1; x2 < tempX[x].max.x - tempX[x].min.x; x2++)
				{
					Vector2Int vec = tempX[x].min + Vector2Int.right * x2;
					if (map[vec.x][vec.y].type == ECaptured.clear)
					{
						RecursiveUpdate(vec);
					}
				}
			}
			
			//check not pair's element's
			//
			
		}

		private int deep = 0;

		private void RecursiveUpdate(Vector2Int pos)
		{
			UpdateLeminCell(map[pos.x][pos.y], cCapture);
			map[pos.x][pos.y].text.text = pos + "\n" + deep++.ToString();
			if (map[pos.x + 1][pos.y].type == ECaptured.clear)
			{
				RecursiveUpdate(pos + Vector2Int.right);
			}
			if (map[pos.x - 1][pos.y].type == ECaptured.clear)
			{
				RecursiveUpdate(pos - Vector2Int.right);
			}
			if (map[pos.x][pos.y + 1].type == ECaptured.clear)
			{
				RecursiveUpdate(pos + Vector2Int.up);
			}
			if (map[pos.x][pos.y - 1].type == ECaptured.clear)
			{
				RecursiveUpdate(pos - Vector2Int.up);
			}
		}
	}

	public struct LeminSub
	{
		public Vector2Int max;
		public Vector2Int min;
	}
}
