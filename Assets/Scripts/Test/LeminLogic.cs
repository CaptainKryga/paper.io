using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
	public class LeminLogic : MonoBehaviour
	{
		private LeminCell[][] cells;
		
		//path => player path on map
		//path[0] => start
		//path[^1] => end

		//map => tilemap LeminCells
		public Vector3Int[] GetCapturedCells(Vector3Int[] path, LeminCell[][] cells)
		{
			this.cells = cells;
			
			List<Vector3Int> temp = path.ToList();
			Debug.Log(temp.Count);

			// if (path[0] != path[^1])
			// {
				deep = 0;
				List<Vector3Int> recursiveStartEnd = GetListFromTree(FindWidthStarA(path[0], null, path.Length));
				temp.AddRange(recursiveStartEnd);
			// }
	
			//TEMP
			for (int x = 0; x < temp.Count; x++)
			{
				cells[temp[x].x][temp[x].y].Debug = "t1";
			}
			
			//if ghost element's <= 0 return
			if (temp.Count <= 0)
			{
				Debug.LogError("problem lemin: need more path length");
				return null;
			}

			//find pair's from path cell's
			List<LeminSub> tempX = FindPairsPathCells(temp);

			//TEMP X
			// for (int x = 0; x < tempX.Count; x++)
			// {
			// 	cells[tempX[x].min.x][tempX[x].min.y].Debug = "min";
			// 	cells[tempX[x].max.x][tempX[x].max.y].Debug = "max";
			// }
			

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
				if (cells[temp[y].x][temp[y].y].type == Lemin.ECaptured.capture &&
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
			for (int x = 0; x < tempX.Count; x++)
			{
				for (int x2 = 1; x2 < tempX[x].max.x - tempX[x].min.x; x2++)
				{
					Vector3Int vec = tempX[x].min + Vector3Int.right * x2;
					if (cells[vec.x][vec.y].type == Lemin.ECaptured.clear && !captured.Contains(vec))
					{
						deep = 0;
						crash = false;
						RecursiveFindEmptySlots(temp, vec);
						Debug.Log("DEEEEEEEP:" + deep);
						if (!crash)
							captured.AddRange(temp);
						temp.Clear();
					}
				}
			}

			return captured.ToArray();
		}

		private bool isExit;

		private Tree FindWidthStarA(Vector3Int start, Tree parent, int pathLength)
		{
			List<Tree> queue = new List<Tree>();
			List<Vector3Int> visited = new List<Vector3Int>();
			Tree tree = new Tree(start, null);
			queue.Add(tree);

			if (pathLength == 1)
			{
				queue.Clear();
				
				//firt step from fix bug one tap
				if (CheckCellNotFinal(queue, visited, tree, Vector3Int.right)) { }
				else if (CheckCellNotFinal(queue, visited, tree, Vector3Int.left)) { }
				else if (CheckCellNotFinal(queue, visited, tree, Vector3Int.up)) { }
				else if (CheckCellNotFinal(queue, visited, tree, Vector3Int.down)) { }
			}

			int i = 0;
			while (queue.Count > 0)
			{
				for (int x = 0; x < queue.Count; x++)
				{
					if (CheckCell(queue, visited, queue[x], Vector3Int.right))
						return queue[^1];

					if (CheckCell(queue, visited, queue[x], Vector3Int.left))
						return queue[^1];

					if (CheckCell(queue, visited, queue[x], Vector3Int.up))
						return queue[^1];

					if (CheckCell(queue, visited, queue[x], Vector3Int.down))
						return queue[^1];

					queue.Remove(queue[x]);
				}
			}

			return null;
		}

		private bool CheckCellNotFinal(List<Tree> queue, List<Vector3Int> visited, Tree parent, Vector3Int vector)
		{
			Vector3Int vec = parent.pos + vector;
			if (vec.x >= cells.Length || vec.x < 0 ||
				vec.y >= cells.Length || vec.y < 0)
			{
				return false;
			}
			
			if (!visited.Contains(vec) && cells[vec.x][vec.y].type == Lemin.ECaptured.capture)
			{
				Tree newTree = new Tree(vec, parent);
				if (parent.parent != null)
					queue.Add(newTree);
				visited.Add(newTree.pos);

				if (parent.parent == null)
				{
					if (vector != Vector3Int.right)
						CheckCellNotFinal(queue, visited, newTree, Vector3Int.right);
					if (vector != Vector3Int.left)
						CheckCellNotFinal(queue, visited, newTree, Vector3Int.left);
					if (vector != Vector3Int.up)
						CheckCellNotFinal(queue, visited, newTree, Vector3Int.up);
					if (vector != Vector3Int.down)
						CheckCellNotFinal(queue, visited, newTree, Vector3Int.down);
				}
				return true;
			}

			return false;
		}
		
		private bool CheckCell(List<Tree> queue, List<Vector3Int> visited, Tree parent, Vector3Int vector)
		{
			Vector3Int vec = parent.pos + vector;
			if (vec.x >= cells.Length || vec.x < 0 ||
				vec.y >= cells.Length || vec.y < 0)
			{
				return false;
			}
			
			if (cells[vec.x][vec.y].type == Lemin.ECaptured.end)
			{
				Tree newTree = new Tree(vec, parent);
				queue.Add(newTree);
				return true;
			}
			
			if (!visited.Contains(vec) && cells[vec.x][vec.y].type == Lemin.ECaptured.capture)
			{
				Tree newTree = new Tree(vec, parent);
				queue.Add(newTree);
				visited.Add(newTree.pos);
			}
			
			return false;
		}

		private List<Vector3Int> GetListFromTree(Tree tree)
		{
			List<Vector3Int> list = new List<Vector3Int>();

			while (tree != null)
			{
				list.Add(tree.pos);
				tree = tree.parent;
			}

			return list;
		}


		private int deep;
		private bool crash;
		private void RecursiveFindEmptySlots(List<Vector3Int> captured, Vector3Int pos)
		{
			if (pos.x + 1 >= cells.Length || pos.x - 1 < 0 || pos.y + 1 >= cells.Length || pos.y - 1 < 0)
			{
				crash = true;
				Debug.LogError("Problem in lemin. Please help!!!");
				return;
			}
			deep++;
			captured.Add(pos);
			if (!captured.Contains(pos + Vector3Int.right) &&
				cells[pos.x + 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos + Vector3Int.right);
			}

			if (!captured.Contains(pos - Vector3Int.right) &&
			    cells[pos.x - 1][pos.y].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos - Vector3Int.right);
			}

			if (!captured.Contains(pos + Vector3Int.up) &&
			    cells[pos.x][pos.y + 1].type == Lemin.ECaptured.clear)
			{
				RecursiveFindEmptySlots(captured, pos + Vector3Int.up);
			}

			if (!captured.Contains(pos - Vector3Int.up) &&
				cells[pos.x][pos.y - 1].type == Lemin.ECaptured.clear)
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

	public class Tree
	{
		public Tree parent;
		public Vector3Int pos;

		public Tree(Vector3Int pos, Tree parent)
		{
			this.pos = pos;
			this.parent = parent;
		}
	}
}

