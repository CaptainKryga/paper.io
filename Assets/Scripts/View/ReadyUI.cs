using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class ReadyUI : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;

		[SerializeField] private Button btnReady;
		
		[SerializeField] private GameObject panelBlock;
		[SerializeField] private TMP_Text timer;

		private bool isReady;
		public bool IsReady => isReady;
		
		public void SetVisiblePanelBlock(bool visible)
		{
			isReady = visible;
			panelBlock.SetActive(visible);
			btnReady.image.color = visible ? Color.green : Color.white;
		}

		public void UpdateTimer(string time)
		{
			timer.text = time;
		}
	}
}