using System;
using UnityEngine;

namespace View
{
	public class VController : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;

		[SerializeField] private GameObject panelPlayerInit;
		[SerializeField] private GameObject panelGameOver;

		private void Start()
		{
			panelPlayerInit.SetActive(false);
			panelGameOver.SetActive(false);
		}

		public void ConsoleCommand(string command)
		{
			controller.ConsoleCommand(command);
		}

		public void InitPlayer()
		{
			panelPlayerInit.SetActive(true);
		}

		public void OnClick_StartBattle()
		{
			panelPlayerInit.SetActive(false);
			panelGameOver.SetActive(false);
			controller.Restart();
		}

		public void GameOver()
		{
			panelGameOver.SetActive(true);
		}
	}
}