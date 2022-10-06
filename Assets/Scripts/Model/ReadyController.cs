using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using View;

namespace Model
{
	public class ReadyController : MonoBehaviour
	{
		[SerializeField] private MController mController;
		[SerializeField] private ReadyUI readyUI;
		private bool isReady;
		private float delay;
		private bool isStart;

		public bool IsReady
		{
			get => isReady;
			set
			{
				isReady = value;
				
				Hashtable hash = new Hashtable();
				hash.Add("isReady", isReady);
				PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			}
		}

		public void StartDelay(bool isStart)
		{
			delay = 5;
			this.isStart = isStart;
			readyUI.UpdateTimer(isStart ? "Start: " + (int)delay : "WAIT MORE\nPLAYERS");
		}
		
		private void Update()
		{
			if (!isStart)
				return;
			
			delay -= Time.deltaTime;
			readyUI.UpdateTimer("Start: " + (int)delay);

			if (delay < 0)
			{
				if (readyUI.IsReady)
				{
					readyUI.SetVisiblePanelBlock(false);
					mController.StartBattle();
				}
				
				isStart = false;
			}
		}
	}
}