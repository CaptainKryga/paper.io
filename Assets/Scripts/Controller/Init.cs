using UnityEngine;

namespace Controller
{
	public class Init : MonoBehaviour
	{
		[SerializeField] private Controller controller;

		private void Start()
		{
			controller.Init();
		}
	}
}
