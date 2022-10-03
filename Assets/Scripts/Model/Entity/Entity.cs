using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.Entity
{
	public class Entity : MonoBehaviour
	{
		public float moveSpeed = 5;
		public Transform movePoint;

		public LayerMask layerCollider;

		private Vector3 newVec;
		private Vector3 save;

		private MController mController;
		private Color color;

		[SerializeField] private SpriteRenderer sprite;
		[SerializeField] private PhotonView pView;

		public void Init(MController mController)
		{
			//random position????
			//snap
			transform.position += new Vector3(.5f, .5f, 0);

			movePoint.parent = null;
			this.mController = mController;

			//random????
			newVec = Vector3.right;
			
			color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			sprite.color = color;
		}

		private void Update()
		{
			if (!pView.IsMine)
				return;
			
			transform.position = Vector3.MoveTowards(transform.position, movePoint.position,
				moveSpeed * Time.deltaTime);
			
			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
				newVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
			else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
				newVec = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

			if (Physics2D.OverlapCircle(movePoint.position + newVec, .2f, layerCollider))
				mController.GameOver();
			
			if (movePoint.position == transform.position)
			{
				movePoint.position += newVec;
			}
		}

		private void OnDestroy()
		{
			Destroy(movePoint.gameObject);
		}
	}
}
