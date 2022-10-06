using Photon.Pun;
using UnityEngine;

namespace Model.Entity
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private PlayerRenderer playerRenderer;
		[SerializeField] private PlayerSync playerSync;

		[SerializeField] private Transform body;
		[SerializeField] private float moveSpeed = 5;
		[SerializeField] private Transform movePoint;
		private Vector3 newVec;
		private bool isMove;

		[SerializeField] private LayerMask layerCollider;

		private EntityController entityController;
		[SerializeField] private PhotonView pView;

		private void Start()
		{
			body.transform.position = Vector3.up * 1000;
			
			//test mode
			isMove = true;
		}

		public void Init(EntityController entityController, PlayerSync playerSync)
		{
			movePoint.parent = null;
			this.entityController = entityController;
			this.playerSync = playerSync;
		}

		public void UpdatePlayer(string playerName, int playerId)
		{
			playerRenderer.UpdatePlayer(playerName, playerId);
			playerSync.UpdatePlayer(playerName, playerId);
		}

		public void UpdateAccessMove(bool isMove)
		{
			this.isMove = isMove;
		}

		// private Vector3 GetRandomStartVector()
		// {
		// 	int rnd = Random.Range(0, 4);
		// 	return rnd == 0 ? Vector3.right : rnd == 1 ? Vector3.left : rnd == 2 ? Vector3.up : Vector3.down;
		// }
		
		private void Update()
		{
			if (!isMove || !pView.IsMine)
				return;
			
			body.transform.position = Vector3.MoveTowards(body.transform.position, movePoint.position,
				moveSpeed * Time.deltaTime);
			
			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
				newVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
			else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
				newVec = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

			if (Physics2D.OverlapCircle(movePoint.position + newVec, .2f, layerCollider))
				entityController.GameOver();
			
			if (movePoint.position == body.transform.position && newVec != Vector3.zero)
			{
				entityController.UpdatePosition(Vector3Int.FloorToInt(body.transform.position));
				movePoint.position += newVec;
				playerRenderer.Rotate(newVec);
			}
		}
	}
}