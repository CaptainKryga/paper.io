using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
	public class LeminLogic : MonoBehaviour
	{
		private LeminCell[][] map;
		private Lemin lemin;
		private Color cCapture;
		//path => player path on map
		//path[0] => start
		//path[^1] => end
		public Vector3Int[] GetResult(LeminCell[][] map, Lemin lemin, Color cCapture)
		{
			this.map = map;
			this.lemin = lemin;
			this.cCapture = cCapture;
			
			//find all ghost element's
			List<Vector2Int> temp = new List<Vector2Int>();
			List<Vector2Int> recursiveStartEnd = new List<Vector2Int>();

			int i = 0;
			for (int x = 0; x < map.Length; x++)
			{
				for (int y = 0; y < map.Length; y++)
				{
					if (map[x][y].type is Lemin.ECaptured.ghost or Lemin.ECaptured.start or Lemin.ECaptured.end)
					{
						temp.Add(new Vector2Int(x, y));
						map[x][y].text.text = i.ToString();
						// UpdateLeminCell(map[x][y], Color.yellow);
						i++;

						if (map[x][y].type == Lemin.ECaptured.start)
						{
							isExit = false;
							RecursiveFindStartEnd(new Vector2Int(x, y), recursiveStartEnd);
						}
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
				return new Vector3Int[1];

			//fpc
			List<LeminSub> tempX = FindPairsPathCells(temp);

			//tempY?????
			CapturedCells(tempX);

			//return all new item's from captured
			return new Vector3Int[1];
		}

		private List<LeminSub> FindPairsPathCells(List<Vector2Int> temp)
		{
			List<LeminSub> tempX = new List<LeminSub>();
			
			//sort items from level Y
			temp = temp.OrderBy(l => l.x).ToList();
			temp = temp.OrderBy(l => l.y).ToList();
			temp = temp.Distinct().ToList();
			
			//pair's
			for (int y = 0; y < temp.Count;)
			{
				if (y + 1 >= temp.Count)
					break;

				//check capture or not
				if (map[temp[y].x][temp[y].y].type == Lemin.ECaptured.capture &&
					temp[y] + Vector2Int.right == temp[y + 1])
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp[y].x][temp[y].y].text.text = "capt\n" + temp[y].y;
					y++;
					continue;
				}

				//bad var *---\n
				if (temp[y].y != temp[y + 1].y)
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp[y].x][temp[y].y].text.text = "step\n" + temp[y].y;
					y++;
					continue;
				}

				//nice var *----i
				if (temp[y] + Vector2Int.right != temp[y + 1])
				{
					tempX.Add(new LeminSub() {min = temp[y], max = temp[y + 1]});
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.red);
					map[temp[y].x][temp[y].y].text.text = "min\n" + temp[y].y;
					// UpdateLeminCell(map[temp2[y + 1].x][temp2[y + 1].y], Color.grey);
					map[temp[y + 1].x][temp[y + 1].y].text.text = "max\n" + temp[y].y;
					y += 2;
				}
				//bad var *i?????
				else if (temp[y] + Vector2Int.right == temp[y + 1])
				{
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.yellow);
					map[temp[y].x][temp[y].y].text.text = "2\n" + temp[y].y;
					// UpdateLeminCell(map[temp2[y + 1].x][temp2[y + 1].y], Color.yellow);
					map[temp[y + 1].x][temp[y + 1].y].text.text = "2\n" + temp[y + 1].y;
					y += 2;
				}
			}

			return tempX;
		}

		//find new captured cell's from width search(custom lemin)
		private void CapturedCells(List<LeminSub> tempX)
		{
			deep = 0;
			for (int x = 0; x < tempX.Count; x++)
			{
				for (int x2 = 1; x2 < tempX[x].max.x - tempX[x].min.x; x2++)
				{
					Vector2Int vec = tempX[x].min + Vector2Int.right * x2;
					if (map[vec.x][vec.y].type == Lemin.ECaptured.clear)
					{
						RecursiveFindEmptySlots(vec);
					}
				}
			}
		}

		private bool isExit;
		private void RecursiveFindStartEnd(Vector2Int start, List<Vector2Int> list)
		{
			list.Add(start);
			if (!isExit && map[start.x][start.y].type == Lemin.ECaptured.end)
			{
				isExit = true;
				return;
			}

			if (start.x + 1 < map.Length && !isExit && !list.Contains(start + Vector2Int.right) &&
				map[start.x + 1][start.y].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector2Int.right, list);
				// if (isExit) list.Add(start + Vector2Int.right);
			}

			if (start.x - 1 >= 0 && !isExit && !list.Contains(start - Vector2Int.right) &&
				map[start.x - 1][start.y].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector2Int.right, list);
				// if (isExit) list.Add(start - Vector2Int.right);
			}

			if (start.y + 1 < map.Length && !isExit && !list.Contains(start + Vector2Int.up) &&
				map[start.x][start.y + 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector2Int.up, list);
				// if (isExit) list.Add(start + Vector2Int.up);
			}

			if (start.y - 1 >= 0 && !isExit && !list.Contains(start - Vector2Int.up) &&
				map[start.x][start.y - 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector2Int.up, list);
				// if (isExit) list.Add(start - Vector2Int.up);
			}

			if (!isExit)
				list.Remove(start);
		}

		private int deep;
		private void RecursiveFindEmptySlots(Vector2Int pos)
		{
			lemin.UpdateLeminCell(map[pos.x][pos.y], cCapture);
			map[pos.x][pos.y].text.text = pos + "\n" + deep++.ToString();
			if (map[pos.x + 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector2Int.right);
			}

			if (map[pos.x - 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos - Vector2Int.right);
			}

			if (map[pos.x][pos.y + 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector2Int.up);
			}

			if (map[pos.x][pos.y - 1].type == Lemin.ECaptured.clear)
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

