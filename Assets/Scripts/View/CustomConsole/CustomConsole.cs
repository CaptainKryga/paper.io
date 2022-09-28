using System;
using TMPro;
using UnityEngine;

namespace View.CustomConsole
{
	public class CustomConsole : MonoBehaviour
	{
		[SerializeField] private VController _vController;
		
		[SerializeField] private GameObject console;
		[SerializeField] private TMP_InputField inputField;
		[SerializeField] private Transform content;
		[SerializeField] private GameObject prefab;
		
		private Console.CustomConsole customConsole;

		private void Start()
		{
			customConsole = new Console.CustomConsole(inputField, prefab, content, console);
			
			customConsole.IsVisible = false;
			customConsole.UpdateVisibleConsole();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.P))
			{
				customConsole.IsVisible = !customConsole.IsVisible;
				customConsole.UpdateVisibleConsole();
			}
		}

		public void OnClick_SendMessage()
		{
			_vController.ConsoleCommand(customConsole.GetNewCommand());
		}
	}
}
