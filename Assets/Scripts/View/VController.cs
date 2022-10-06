using TMPro;
using UnityEngine;

namespace View
{
	public class VController : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;
		[SerializeField] private ReadyUI readyUI;

		[SerializeField] private GameObject panelPlayerInit;
		[SerializeField] private TMP_InputField inputFieldPlayerName;

		private string playerName;
		private int playerId;

		private void Start()
		{
			panelPlayerInit.SetActive(false);
		}

		public void ConsoleCommand(string command)
		{
			controller.ConsoleCommand(command);
		}

		public void InitPlayer()
		{
			panelPlayerInit.SetActive(true);
		}

		public void GameOver()
		{
			panelPlayerInit.SetActive(true);
		}

		public void OnClick_Ready()
		{
			if (!readyUI.IsReady)
				controller.UpdatePlayer(playerName, playerId);
			
			readyUI.SetVisiblePanelBlock(!readyUI.IsReady);

			controller.Ready(readyUI.IsReady);
		}

		public void ReceiveStartBattle(bool isFlag)
		{
			if (isFlag)
			{
				panelPlayerInit.SetActive(false);
			}
			else
			{
				///////////////////////////////////////////////////////////////
				Debug.Log("Message BOX");
			}
		}

		public void OnClick_SetPlayerFlag(int id)
		{
			playerId = id;
			controller.UpdatePlayer("", playerId);
		}

		public void OnInputField_SetName()
		{
			playerName = inputFieldPlayerName.text;
		}
	}
}