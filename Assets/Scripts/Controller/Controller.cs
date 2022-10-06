using System;
using Model;
using UnityEngine;

namespace Controller
{
	public class Controller : MonoBehaviour
	{
		[SerializeField] private MController mController;
		
		public void Init()
		{
			mController.Init();
		}

		public void ConsoleCommand(string command)
		{
			
		}

		public void Restart(string playerName, int playerId)
		{
			mController.Restart(playerName, playerId);
		}

		public void Ready(bool isReady)
		{
			mController.Ready(isReady);
		}
	}
}
