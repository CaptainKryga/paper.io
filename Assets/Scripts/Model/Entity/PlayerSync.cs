using System;
using System.Collections.Generic;
using Model.TileMap;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Model.Entity
{
    public class PlayerSync : MonoBehaviour, IPunObservable
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TMP_Text info;
        private Transform parent;

        private TileDataBase tileDataBase;

        private string playerName;
        private int playerId;

        private float lastTime;
        // private bool isMove;

        // public bool IsMove
        // {
        //     set => isMove = value;
        // }

        private string PlayerName
        {
            set
            {
                playerName = value;
                info.text = playerName;
            }
        }
        private int PlayerId
        {
            set
            {
                
                if (value < 0 || value > tileDataBase.sprites.Length)
                    return;
                
                playerId = value;
                spriteRenderer.sprite = tileDataBase.sprites[playerId];
            }
        }
        
        private void Awake()
        {
            tileDataBase = FindObjectOfType<EntityInstance>().TileDataBase;
            if (photonView.IsMine)
            {
                spriteRenderer.enabled = false;
                info.enabled = false;
            }
        }

        public void Init(Transform parent)
        {
            this.parent = parent;
        }

        public void UpdatePlayer(string playerName, int playerId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PhotonNetwork.LocalPlayer.NickName);
                stream.SendNext(playerId);
                
                // stream.SendNext(isMove);
                // stream.SendNext(parent.position);
            }
            else if (stream.IsReading)
            {
                PlayerName = (string)stream.ReceiveNext();
                PlayerId = (int)stream.ReceiveNext();
                
                // isMove = (bool)stream.ReceiveNext();
                // queue.Add(new SyncPosition() 
                // { 
                    // nextPosition = (Vector3)stream.ReceiveNext(),
                    // time = lastTime == 0 ? 0 : lastTime - Time.deltaTime 
                // });
            }
        }

        private void Update()
        {
            transform.position = parent.position;
        }

        // private List<SyncPosition> queue = new List<SyncPosition>();
        // public void FixedUpdate()
        // {
        //     if (queue.Count >= 2)
        //     {
        //         if (transform.position == queue[0].nextPosition)
        //             queue.Remove(queue[0]);
        //         
        //         transform.position = Vector3.MoveTowards(transform.position, 
        //             queue[0].nextPosition, queue[0].time);
        //     }
        // }
    }

    // public class SyncPosition
    // {
    //     public Vector3 nextPosition;
    //     public float time;
    // }
}
