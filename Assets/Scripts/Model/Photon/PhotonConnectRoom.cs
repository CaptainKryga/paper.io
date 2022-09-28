using Photon.Pun;

namespace Model.Photon
{
    public class PhotonConnectRoom : MonoBehaviourPunCallbacks
    {
        public bool IsConnect
        {
            get => PhotonNetwork.InRoom;
        }
        
        public void ConnectToServer()
        {
            
        }
    }
}
