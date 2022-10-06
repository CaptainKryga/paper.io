using Model.TileMap;
using UnityEngine;

namespace Model.Entity
{
	public class PlayerRenderer : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] private TileDataBase tileDataBase;

		public void UpdatePlayer(string playerName, int playerId)
		{
			if (playerId > 0 && playerId < tileDataBase.sprites.Length)
			{
				spriteRenderer.sprite = tileDataBase.sprites[playerId];
			}
		}

		public void Rotate(Vector3 vector)
		{

		}
	}
}