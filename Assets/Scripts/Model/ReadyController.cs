using UnityEngine;
using View;

namespace Model
{
	public class ReadyController : MonoBehaviour
	{
		[SerializeField] private MController mController;
		[SerializeField] private ReadyUI readyUI;
		private float delay;
		private bool isStart;

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