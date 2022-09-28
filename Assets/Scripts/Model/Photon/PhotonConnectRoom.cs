using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Model.Photon
{
    public class PhotonConnectRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private MController mController;
        
        public bool IsConnect
        {
            get => PhotonNetwork.InRoom;
        }
        
        public void ConnectToServer()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("room", 
                new RoomOptions() { MaxPlayers = 20, IsOpen = true }, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            mController.InitPlayer();
        }
    }
}
