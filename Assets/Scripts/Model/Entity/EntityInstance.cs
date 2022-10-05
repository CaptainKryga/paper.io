using Model.Entity;
using Model.Photon;
using UnityEngine;

namespace Model
{
	public class EntityInstance : MonoBehaviour
	{
		[SerializeField] private MController mController;
		[SerializeField] private PhotonConnectRoom photonConnectRoom;

		[SerializeField] private EntityController entityController;
		
		//restart game after death or win
		public void Restart()
		{
			//setPosition
			Vector3 startPosition = new Vector3(.5f, .5f, 0);

			entityController.Restart(photonConnectRoom.CreatePlayer(startPosition));
		}

		public void GameOver(Player player)
		{
			photonConnectRoom.DestroyPlayer(player.gameObject);
			mController.GameOver();
		}
	}
}