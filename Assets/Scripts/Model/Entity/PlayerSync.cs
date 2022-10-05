using Photon.Pun;
using UnityEngine;

namespace Model.Entity
{
    public class PlayerSync : MonoBehaviour, IPunObservable
    {
        [SerializeField] private PhotonView _photonView;
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else if (stream.IsReading)
            {
                transform.position = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
