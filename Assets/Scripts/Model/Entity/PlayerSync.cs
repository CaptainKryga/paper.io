using Model.TileMap;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Model.Entity
{
    public class PlayerSync : Entity, IPunObservable
    {
        private TileDataBase tileDataBase;
        [SerializeField] private TMP_Text textPlayerName;
        private string nickName;
        private int localId;

        private string NickName
        {
            set
            {
                nickName = value;
                textPlayerName.text = nickName;
            }
        }
        
        private int LocalId
        {
            set
            {
                localId = value;
                if (tileDataBase)
                    spriteRenderer.sprite = tileDataBase.sprites[localId];
            }
        }

        public void UpdatePlayer(TileDataBase tileDataBase, string playerName, int playerId)
        {
            nickName = playerName;
            textPlayerName.text = nickName;
            
            localId = playerId;
            this.tileDataBase = tileDataBase;
            spriteRenderer.sprite = tileDataBase.sprites[localId];
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(nickName);
                stream.SendNext(localId);
            }
            else if (stream.IsReading)
            {
                NickName = (string)stream.ReceiveNext();
                LocalId = (int)stream.ReceiveNext();
            }
        }
    }
}
