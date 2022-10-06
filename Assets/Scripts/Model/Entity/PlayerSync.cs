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

        private string PlayerName
        {
            set
            {
                playerName = value;
                info.text = playerName;
            }
        }
        public int PlayerId
        {
            get => playerId;
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
            }
            else if (stream.IsReading)
            {
                PlayerName = (string)stream.ReceiveNext();
                PlayerId = (int)stream.ReceiveNext();
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
                transform.position = parent.position;
        }
    }
}
