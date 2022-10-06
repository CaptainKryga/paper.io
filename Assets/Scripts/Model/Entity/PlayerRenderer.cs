using Model.TileMap;
using TMPro;
using UnityEngine;

namespace Model.Entity
{
	public class PlayerRenderer : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] private TMP_Text info;
		[SerializeField] private TileDataBase tileDataBase;

		public void UpdatePlayer(string playerName, int playerId)
		{
			if (playerId > 0 && playerId < tileDataBase.sprites.Length)
			{
				spriteRenderer.sprite = tileDataBase.sprites[playerId];
			}

			info.text = playerName;
		}

		public void Rotate(Vector3 vector)
		{

		}
	}
}