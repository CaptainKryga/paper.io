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

		private EntityController entityController;
		private Color color;

		[SerializeField] private SpriteRenderer sprite;
		[SerializeField] private PhotonView pView;

		public void Init(EntityController entityController)
		{
			movePoint.parent = null;
			this.entityController = entityController;

			//rnd vec
			// newVec = GetRandomStartVector();
			//rnd color
			color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			sprite.color = color;
			entityController.UpdateStart(Vector3Int.FloorToInt(transform.position), color);
			Debug.Log("1 ");
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
				entityController.GameOver();
			
			if (movePoint.position == transform.position)
			{
				entityController.UpdatePosition(Vector3Int.FloorToInt(transform.position));
				movePoint.position += newVec;
			}
		}

		private Vector3 GetRandomStartVector()
		{
			int rnd = Random.Range(0, 4);
			return rnd == 0 ? Vector3.right : rnd == 1 ? Vector3.left : rnd == 2 ? Vector3.up : Vector3.down;
		}

		private void OnDestroy()
		{
			Destroy(movePoint.gameObject);
		}
	}
}
