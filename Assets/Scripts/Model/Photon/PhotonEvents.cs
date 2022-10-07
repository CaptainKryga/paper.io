using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Model.Photon
{
    public class PhotonEvents : MonoBehaviourPunCallbacks
    {
        public static PhotonEvents Singleton { private set; get; }

        public Action PlayerLeftRoom_Action;

        private void Awake()
        {
            Singleton = this;
        }

        public override void OnConnected()
        {
            Debug.Log("PHOTON: OnConnected");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("PHOTON: OnLeftRoom");
        }

        public override void OnMasterClientSwitched(global::Photon.Realtime.Player newMasterClient)
        {
            Debug.Log("PHOTON: OnMasterClientSwitched " + newMasterClient.NickName);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("PHOTON: OnCreateRoomFailed " + returnCode + " _ " + message);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PHOTON: OnJoinRoomFailed " + returnCode + " _ " + message);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("PHOTON: OnCreatedRoom");
        }

        public override void  OnJoinedLobby()
        {
            Debug.Log("PHOTON: OnJoinedLobby");
        }

        public override void  OnLeftLobby()
        {
            Debug.Log("PHOTON: OnLeftLobby");
        }

        public override void  OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("PHOTON: OnDisconnected " + cause.ToString());
        }

        public override void  OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("PHOTON: OnRegionListReceived " + regionHandler.ToString());
        }

        public override void  OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("PHOTON: OnRoomListUpdate " + roomList.Count);
        }

        public override void  OnJoinedRoom()
        {
            Debug.Log("PHOTON: OnJoinedRoom");
            
            //update start data
            PlayerLeftRoom_Action?.Invoke();
        }

        public override void  OnPlayerEnteredRoom(global::Photon.Realtime.Player newPlayer)
        {
            Debug.Log("PHOTON: OnPlayerEnteredRoom " + newPlayer.NickName);
        }

        public override void  OnPlayerLeftRoom(global::Photon.Realtime.Player otherPlayer)
        {
            Debug.Log("PHOTON: OnPlayerLeftRoom " + otherPlayer.NickName);
            PlayerLeftRoom_Action?.Invoke();
        }

        public override void  OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PHOTON: OnJoinRandomFailed " + returnCode + " _ " + message);
        }

        public override void  OnConnectedToMaster()
        {
            Debug.Log("PHOTON: OnConnectedToMaster");
        }

        public override void  OnFriendListUpdate(List<FriendInfo> friendList)
        {
            Debug.Log("PHOTON: OnFriendListUpdate " + friendList.Count);
        }

        public override void  OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.Log("PHOTON: OnCustomAuthenticationResponse " + data.Count);
        }

        public override void  OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.Log("PHOTON: OnCustomAuthenticationFailed " + debugMessage);
        }

        public override void  OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Debug.Log("PHOTON: OnLobbyStatisticsUpdate " + lobbyStatistics.Count);
        }

        public override void  OnErrorInfo(ErrorInfo errorInfo)
        {
            Debug.Log("PHOTON: OnErrorInfo " + errorInfo.ToString());
        }
    }
}