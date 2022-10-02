using System;
using UnityEngine;

namespace Model.Entity
{
	public class Entity : MonoBehaviour
	{
		public float moveSpeed = 5;
		public Transform movePoint;

		private void Start()
		{
			movePoint.parent = null;
		}

		private void Update()
		{
			transform.position = Vector3.MoveTowards(transform.position, movePoint.position,
				moveSpeed * Time.deltaTime);

			if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
			{
				if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
				{
					movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
				}
			
				if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
				{
					movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
				}
			}
		}
	}
}
