using System;
using ExitGames.Client.Photon;
using Model.Photon;
using Photon.Pun;
using UnityEngine;

namespace Model.Entity
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private EntityController entityController;
		[SerializeField] private PlayerRenderer playerRenderer;
		private PlayerSync playerSync;

		[SerializeField] private CustomRaiseEvents customRaiseEvents;
		
		private int playerId;
		[SerializeField] private Transform body;
		[SerializeField] private float moveSpeed = 5;
		[SerializeField] private Transform movePoint;
		private Vector3 newVec;
		private bool isMove;
		
		[SerializeField] private LayerMask layerCollider;
		
		public int PlayerId => playerId;

		private void OnEnable()
		{
			customRaiseEvents.ReceiveStartBattle_Action += StartBattle;
			customRaiseEvents.ReceiveGameOverLastPlayer_Action += GameOver;
		}

		private void OnDisable()
		{
			customRaiseEvents.ReceiveStartBattle_Action -= StartBattle;
			customRaiseEvents.ReceiveGameOverLastPlayer_Action -= GameOver;
		}

		private void Start()
		{
			body.transform.position = Vector3.up * 1000;
			
			isMove = false;
			
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", false);
			hash.Add("isReady", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}

		public void Init(PlayerSync playerSync)
		{
			movePoint.parent = null;
			this.playerSync = playerSync;
		}

		public void UpdatePlayer(string playerName, int playerId)
		{
			playerRenderer.UpdatePlayer(playerName, playerId);
			playerSync.UpdatePlayer(playerName, playerId);
			this.playerId = playerId;
		}

		private void Update()
		{
			if (!isMove)
				return;
			
			body.transform.position = Vector3.MoveTowards(body.transform.position, movePoint.position,
				moveSpeed * Time.deltaTime);
			
			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
				newVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
			else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
				newVec = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

			if (Physics2D.OverlapCircle(movePoint.position + newVec, .2f, layerCollider))
				GameOver();
			
			if (movePoint.position == body.transform.position && newVec != Vector3.zero)
			{
				entityController.UpdatePosition(Vector3Int.FloorToInt(body.transform.position));
				movePoint.position += newVec;
				playerRenderer.Rotate(newVec);
			}
		}

		public void StartBattle(Vector3Int position)
		{
			movePoint.position = position;
			body.position = movePoint.position;
			
			isMove = true;
			
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", true);
			hash.Add("isReady", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			// customRaiseEvents.Send_BattleUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, false);
		}
		
		private void GameOver()
		{
			movePoint.position = Vector3.down * 100;
			body.position = movePoint.position;
			isMove = false;
			entityController.GameOver();
			
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			customRaiseEvents.Request_BattleUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, false);
		}

		private void OnDestroy()
		{
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", false);
			hash.Add("isReady", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			customRaiseEvents.Request_BattleUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, false);
		}
	}
}