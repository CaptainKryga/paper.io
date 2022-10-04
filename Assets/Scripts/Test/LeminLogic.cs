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
		public Vector3Int[] GetCapturedCells(Vector3Int[] path, LeminCell[][] map, Lemin lemin, Color cCapture)
		{
			this.map = map;
			this.lemin = lemin;
			this.cCapture = cCapture;
			
			List<Vector3Int> temp = path.ToList();
			
			List<Vector3Int> recursiveStartEnd = new List<Vector3Int>();
			RecursiveFindStartEnd(path[0], recursiveStartEnd);

			int i = 0;
			
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

		private List<LeminSub> FindPairsPathCells(List<Vector3Int> temp)
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
					temp[y] + Vector3Int.right == temp[y + 1])
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
				if (temp[y] + Vector3Int.right != temp[y + 1])
				{
					tempX.Add(new LeminSub() {min = temp[y], max = temp[y + 1]});
					// UpdateLeminCell(map[temp2[y].x][temp2[y].y], Color.red);
					map[temp[y].x][temp[y].y].text.text = "min\n" + temp[y].y;
					// UpdateLeminCell(map[temp2[y + 1].x][temp2[y + 1].y], Color.grey);
					map[temp[y + 1].x][temp[y + 1].y].text.text = "max\n" + temp[y].y;
					y += 2;
				}
				//bad var *i?????
				else if (temp[y] + Vector3Int.right == temp[y + 1])
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
					Vector3Int vec = tempX[x].min + Vector3Int.right * x2;
					if (map[vec.x][vec.y].type == Lemin.ECaptured.clear)
					{
						RecursiveFindEmptySlots(vec);
					}
				}
			}
		}

		private bool isExit;
		private void RecursiveFindStartEnd(Vector3Int start, List<Vector3Int> list)
		{
			Debug.Log("RecursiveFindStartEnd");
			list.Add(start);
			if (!isExit && map[start.x][start.y].type == Lemin.ECaptured.end)
			{
				isExit = true;
				return;
			}

			if (start.x + 1 < map.Length && !isExit && !list.Contains(start + Vector3Int.right) &&
				map[start.x + 1][start.y].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector3Int.right, list);
				// if (isExit) list.Add(start + Vector2Int.right);
			}

			if (start.x - 1 >= 0 && !isExit && !list.Contains(start - Vector3Int.right) &&
				map[start.x - 1][start.y].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector3Int.right, list);
				// if (isExit) list.Add(start - Vector2Int.right);
			}

			if (start.y + 1 < map.Length && !isExit && !list.Contains(start + Vector3Int.up) &&
				map[start.x][start.y + 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector3Int.up, list);
				// if (isExit) list.Add(start + Vector2Int.up);
			}

			if (start.y - 1 >= 0 && !isExit && !list.Contains(start - Vector3Int.up) &&
				map[start.x][start.y - 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector3Int.up, list);
				// if (isExit) list.Add(start - Vector2Int.up);
			}

			if (!isExit)
				list.Remove(start);
		}

		private int deep;
		private void RecursiveFindEmptySlots(Vector3Int pos)
		{
			lemin.UpdateLeminCell(map[pos.x][pos.y], cCapture);
			Debug.Log("RecursiveFindEmptySlots");
			map[pos.x][pos.y].text.text = pos + "\n" + deep++.ToString();
			if (map[pos.x + 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector3Int.right);
			}

			if (map[pos.x - 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos - Vector3Int.right);
			}

			if (map[pos.x][pos.y + 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos + Vector3Int.up);
			}

			if (map[pos.x][pos.y - 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(pos - Vector3Int.up);
			}
		}
	}

	public struct LeminSub
	{
		public Vector3Int max;
		public Vector3Int min;
	}
}

