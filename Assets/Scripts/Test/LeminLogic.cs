using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
	public class LeminLogic : MonoBehaviour
	{
		private LeminCell[][] map;
		
		//path => player path on map
		//path[0] => start
		//path[^1] => end
		
		//map => tilemap LeminCells
		public Vector3Int[] GetCapturedCells(Vector3Int[] path, LeminCell[][] map)
		{
			this.map = map;
			
			List<Vector3Int> temp = path.ToList();
			
			List<Vector3Int> recursiveStartEnd = new List<Vector3Int>();
			RecursiveFindStartEnd(path[0], recursiveStartEnd);

			temp.AddRange(recursiveStartEnd);
			
			//if ghost element's <= 0 return
			if (temp.Count <= 0)
			{
				Debug.LogError("problem lemin: need more path length");
				return null;
			}

			//find pair's from path cell's
			List<LeminSub> tempX = FindPairsPathCells(temp);

			//tempY?????
			//return all new item's from captured
			return CapturedCells(tempX);
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
					y++;
					continue;
				}

				//bad var *---\n
				if (temp[y].y != temp[y + 1].y)
				{
					y++;
					continue;
				}

				//nice var *----i
				if (temp[y] + Vector3Int.right != temp[y + 1])
				{
					tempX.Add(new LeminSub() {min = temp[y], max = temp[y + 1]});
					y += 2;
				}
				//bad var *i?????
				else if (temp[y] + Vector3Int.right == temp[y + 1])
				{
					y += 2;
				}
			}

			return tempX;
		}

		//find new captured cell's from width search(custom lemin)
		private Vector3Int[] CapturedCells(List<LeminSub> tempX)
		{
			List<Vector3Int> captured = new List<Vector3Int>();
			List<Vector3Int> temp = new List<Vector3Int>();
			deep = 0;
			for (int x = 0; x < tempX.Count; x++)
			{
				for (int x2 = 1; x2 < tempX[x].max.x - tempX[x].min.x; x2++)
				{
					Vector3Int vec = tempX[x].min + Vector3Int.right * x2;
					if (map[vec.x][vec.y].type == Lemin.ECaptured.clear && !captured.Contains(vec))
					{
						RecursiveFindEmptySlots(temp, vec);
						captured.AddRange(temp);
						temp.Clear();
					}
				}
			}

			return captured.ToArray();
		}

		private bool isExit;
		private void RecursiveFindStartEnd(Vector3Int start, List<Vector3Int> list)
		{
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
			}

			if (start.x - 1 >= 0 && !isExit && !list.Contains(start - Vector3Int.right) &&
				map[start.x - 1][start.y].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector3Int.right, list);
			}

			if (start.y + 1 < map.Length && !isExit && !list.Contains(start + Vector3Int.up) &&
				map[start.x][start.y + 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start + Vector3Int.up, list);
			}

			if (start.y - 1 >= 0 && !isExit && !list.Contains(start - Vector3Int.up) &&
				map[start.x][start.y - 1].type is Lemin.ECaptured.capture or Lemin.ECaptured.end)
			{
				RecursiveFindStartEnd(start - Vector3Int.up, list);
			}

			if (!isExit)
				list.Remove(start);
		}

		private int deep;
		private void RecursiveFindEmptySlots(List<Vector3Int> captured, Vector3Int pos)
		{
			captured.Add(pos);
			if (!captured.Contains(pos + Vector3Int.right) &&
				map[pos.x + 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos + Vector3Int.right);
			}

			if (!captured.Contains(pos - Vector3Int.right) &&
				map[pos.x - 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos - Vector3Int.right);
			}

			if (!captured.Contains(pos + Vector3Int.up) &&
				map[pos.x][pos.y + 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos + Vector3Int.up);
			}

			if (!captured.Contains(pos - Vector3Int.up) &&
				map[pos.x][pos.y - 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos - Vector3Int.up);
			}
		}
	}

	public struct LeminSub
	{
		public Vector3Int max;
		public Vector3Int min;
	}
}

