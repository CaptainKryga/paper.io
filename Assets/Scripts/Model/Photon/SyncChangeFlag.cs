using UnityEngine;
using UnityEngine.UI;

namespace Model.Photon
{
	public class SyncChangeFlag : MonoBehaviour
	{
		[SerializeField] private CustomRaiseEvents customRaiseEvents;
		[SerializeField] private Button[] buttons;
		private int localId;

		private void OnEnable()
		{
			customRaiseEvents.ReceiveLocalUpdateFlagPlayer_Action += ReceiveSelectedPlayerFlag;
		}

		public void SelectedPlayerFlag(int now)
		{
			if (localId > 0 && localId < buttons.Length)
			{
				buttons[localId].interactable = true;
				buttons[localId].image.color = Color.white;
			}
		
			if (now > 0 && now < buttons.Length)
			{
				buttons[now].interactable = true;
				buttons[now].image.color = Color.green;

				customRaiseEvents.Request_LocalFlagPlayer(localId, now);
				localId = now;
			}
		}

		private void ReceiveSelectedPlayerFlag(int old, int now)
		{
			if (old > 0 && old < buttons.Length)
			{
				buttons[old].interactable = true;
				buttons[old].image.color = Color.white;
			}
		
			if (now > 0 && now < buttons.Length)
			{
				buttons[now].interactable = false;
				buttons[now].image.color = Color.red;
			}
		}
	}
}
