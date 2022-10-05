using Model.Entity;
using Model.TileMap;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
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

        public PlayerSync CreatePlayer(string playerName)
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            return PhotonNetwork.Instantiate("PlayerSync", 
                Vector3.down * 100, Quaternion.identity).GetComponent<PlayerSync>();
        }
        
        public void DestroyPlayer(GameObject player)
        {
            PhotonNetwork.Destroy(player);
        }
    }
}
