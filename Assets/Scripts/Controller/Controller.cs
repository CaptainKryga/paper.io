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
	}
}
