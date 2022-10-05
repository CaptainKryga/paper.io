using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Model.Entity
{
    public class Player : Entity, IPunObservable
    {
        [SerializeField] private TMP_Text textPlayerName;
        private string nickName;
        private int localId;

        public void InitPlayer(string playerName)
        {
            nickName = playerName;
            textPlayerName.text = playerName;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(nickName);
            }
            else if (stream.IsReading)
            {
                InitPlayer((string)stream.ReceiveNext());
            }
        }
    }
}
