using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
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
			
			PhotonEvents.Singleton.PlayerLeftRoom_Action += PlayerLeft;
		}

		private void OnDisable()
		{
			customRaiseEvents.ReceiveLocalUpdateFlagPlayer_Action -= ReceiveSelectedPlayerFlag;
			
			PhotonEvents.Singleton.PlayerLeftRoom_Action -= PlayerLeft;
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
				
				Hashtable hash = new Hashtable();
				hash.Add("playerId", now);
				PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
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

		private void PlayerLeft()
		{
			Player[] players = PhotonNetwork.PlayerList;

			for (int x = 1; x < buttons.Length; x++)
			{
				buttons[x].interactable = true;
				buttons[x].image.color = Color.white;
			}
			
			for (int x = 0; x < players.Length; x++)
			{
				int id = (int)players[x].CustomProperties["playerId"];
				
				if (id == 0)
					continue;

				if (players[x] == PhotonNetwork.LocalPlayer)
				{
					buttons[id].interactable = true;
					buttons[id].image.color = Color.green;
				}
				else
				{
					buttons[id].interactable = false;
					buttons[id].image.color = Color.red;
				}
			}
		}
	}
}
