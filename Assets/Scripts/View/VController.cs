using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class VController : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;
		[SerializeField] private ReadyUI readyUI;

		[SerializeField] private Image panelPlayerInit;
		[SerializeField] private Sprite spriteWin, spriteDefeat;
		
		[SerializeField] private TMP_InputField inputFieldPlayerName;

		[SerializeField] private TMP_Text textWin;

		private string playerName;
		private int playerId;

		private void Start()
		{
			panelPlayerInit.gameObject.SetActive(false);
		}

		public void ConsoleCommand(string command)
		{
			controller.ConsoleCommand(command);
		}

		public void InitPlayer()
		{
			panelPlayerInit.gameObject.SetActive(true);
		}

		public void GameOver(bool isWin)
		{
			panelPlayerInit.gameObject.SetActive(true);

			panelPlayerInit.sprite = isWin ? spriteWin : spriteDefeat;
			textWin.text = isWin ? "YOU WINNER" : "YOU DEFEAT";
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
				panelPlayerInit.gameObject.SetActive(false);
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