using Model.Photon;
using UnityEngine;

namespace Model
{
	public class MController : MonoBehaviour
	{
		[SerializeField] private PhotonConnectRoom photonConnectRoom;

		public void Init()
		{
			if (!photonConnectRoom.IsConnect)
			{
				Debug.Log("no connect");
			}
		}
	}
}
