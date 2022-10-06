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
        private int PlayerId
        {
            set
            {
                if (value > 0 && value < tileDataBase.sprites.Length)
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

        public void UpdatePlayer(string playerName, int playerId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(PhotonNetwork.LocalPlayer.NickName);
                stream.SendNext(playerId);
            }
            else if (stream.IsReading)
            {
                transform.position = (Vector3)stream.ReceiveNext();
                PlayerName = (string)stream.ReceiveNext();
                PlayerId = (int)stream.ReceiveNext();
            }
        }
    }
}
