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

		[SerializeField] private Color clear, capture, ghost, actual;

		public enum ECaptured
		{
			none,
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
			actual = clear;
		}
		
		public void OnClick_SetCapture()
		{
			actual = capture;
		}
		
		public void OnClick_SetGhost()
		{
			actual = ghost;
		}
		
		public void OnClick_Result()
		{
			
		}

		public void OnClick_SetAll()
		{
			for (int y = 0; y < map.Length; y++)
				for (int x = 0; x < map[y].Length; x++)
					UpdateLeminImg(map[y][x]);
		}

		public void UpdateLeminImg(LeminCell cell)
		{
			cell.img.color = actual;
			cell.type = actual == capture ? ECaptured.capture : actual == ghost ? ECaptured.ghost : ECaptured.none;
			
			Debug.Log(cell.type);
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
					PlayerPrefs.SetInt("test:" + y + "|" + x, (int)ECaptured.none);
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
					map[y][x].img.color = cap == ECaptured.capture ? capture : cap == ECaptured.ghost ? ghost : Color.white;
				}
			}

			Debug.Log("Load");
		}
	}
}
