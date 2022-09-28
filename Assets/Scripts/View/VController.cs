using UnityEngine;

namespace View
{
	public class VController : MonoBehaviour
	{
		[SerializeField] private Controller.Controller controller;
		
		public void ConsoleCommand(string command)
		{
			controller.ConsoleCommand(command);
		}
	}
}