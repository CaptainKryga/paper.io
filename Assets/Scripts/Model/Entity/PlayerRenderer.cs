using Model.TileMap;
using UnityEngine;

namespace Model.Entity
{
	public class PlayerRenderer : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] private TileDataBase tileDataBase;

		public void Init(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		public void Rotate(Vector3 vector)
		{

		}
	}
}