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
		[SerializeField] private PlayerChecker playerChecker;

		[SerializeField] private CustomRaiseEvents customRaiseEvents;
		
		[SerializeField] private int playerId;
		[SerializeField] private Transform body;
		[SerializeField] private float moveSpeed = 5;
		[SerializeField] private Transform movePoint;
		private Vector3 newVec;
		private bool isMove;
		private int sizeMap;
		
		public int PlayerId => playerId;
		public Transform Body => body;

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

		public void Init(PlayerSync playerSync, int sizeMap)
		{
			movePoint.parent = null;
			this.playerSync = playerSync;
			playerSync.Init(body);
			playerChecker.InitPlayer(body);

			this.sizeMap = sizeMap;
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

			if (movePoint.position.x >= sizeMap || movePoint.position.x < 0 ||
			    movePoint.position.y >= sizeMap || movePoint.position.y < 0)
			{
				Debug.Log("movePoint: " + movePoint.position);
				GameOver(false);
				return;
			}
			
			if (movePoint.position == body.transform.position && newVec != Vector3.zero)
			{
				entityController.UpdatePosition(Vector3Int.FloorToInt(body.transform.position));
				movePoint.position += newVec;
				playerRenderer.Rotate(newVec);
				playerChecker.CheckAttack(movePoint);
			}
		}

		public void StartBattle(Vector3Int position)
		{
			movePoint.position = position + new Vector3(.5f, .5f);
			body.position = movePoint.position;

			isMove = true;
			// playerSync.IsMove = true;
			
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", true);
			hash.Add("isReady", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			// customRaiseEvents.Send_BattleUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, false);
			
			playerChecker.StartBattle();
		}
		
		public void GameOver(bool isWin)
		{
			movePoint.position = Vector3.down * 100;
			body.position = movePoint.position;
			
			isMove = false;
			
			entityController.GameOver(isWin);
			
			Hashtable hash = new Hashtable();
			hash.Add("isBattle", false);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
			customRaiseEvents.Request_BattleUpdatePlayer(PhotonNetwork.LocalPlayer.ActorNumber, false);
			
			playerChecker.EndBattle();
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