using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class VController : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;

		[SerializeField] private GameObject panelPlayerInit;

		[SerializeField] private Button btnStartBattle;
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

		public void OnClick_StartBattle()
		{
			controller.Restart(playerName, playerId);
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
			controller.Restart("", playerId);
		}

		public void OnInputField_SetName()
		{
			playerName = inputFieldPlayerName.text;
		}
	}
}