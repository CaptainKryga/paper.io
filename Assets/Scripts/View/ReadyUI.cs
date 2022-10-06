using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class ReadyUI : MonoBehaviour
	{
		[SerializeField] private ReadyController readyController;
		
		[SerializeField] private Button btnReady;
		
		[SerializeField] private GameObject panelBlock;
		[SerializeField] private TMP_Text timer;

		public bool IsReady => readyController.IsReady;

		public void SetVisiblePanelBlock(bool visible)
		{
			readyController.IsReady = visible;
			panelBlock.SetActive(visible);
			btnReady.image.color = visible ? Color.green : Color.white;
		}

		public void UpdateTimer(string time)
		{
			timer.text = time;
		}
	}
}