using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using Photon.Realtime;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core.Network
{
    public class RealTimeService : IRealTimeService, IConnectionCallbacks, IMatchmakingCallbacks
    {
        private LoadBalancingClient _client;

        public RealTimeService(IEvent @event)
        {
            @event.ListenToEvent(EEventType.OnApplicationQuit, ApplicationQuit);
        }

        private void ApplicationQuit()
        {
            if (_client != null)
            {
                if (_client.IsConnected)
                {
                    _client.Disconnect();
                }
                _client.RemoveCallbackTarget(this);
            }
        }

        public void StartConnection(string appId)
        {
            _client = new LoadBalancingClient();
            _client.AddCallbackTarget(this);
            _client.StateChanged += this.OnStateChange;
            Debug.Log("StartClient");
            this._client.ConnectUsingSettings(new AppSettings() { AppIdRealtime = appId });
        }

        public void UpdateRealTime()
        {
            _client.Service();
        }

        public void JoinMainRoom()
        {
            _client.OpJoinOrCreateRoom(new EnterRoomParams() { RoomName = "default" });
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected : " + cause);
            switch (cause)
            {
            }
        }

        void IConnectionCallbacks.OnConnected()
        {
            Debug.Log("OnConnected");
        }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster Server: " + this._client.LoadBalancingPeer.ServerIpAddress);
            JoinMainRoom();
        }

        void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
        }

        void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
         
        }

        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.Log("OnCustomAuthenticationFailed");
        }

        void IMatchmakingCallbacks.OnJoinedRoom()
        {

        }

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
        {
          
        }

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
          
        }

        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
        {
          
        }

        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
        {
         
        }

        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
        {
     
        }

        void IMatchmakingCallbacks.OnLeftRoom()
        {
        
        }

        private void MyCreateRoom(string roomName, byte maxPlayers)
        {
            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomName = roomName;
            enterRoomParams.RoomOptions = new RoomOptions();
            enterRoomParams.RoomOptions.MaxPlayers = maxPlayers;
            this._client.OpCreateRoom(enterRoomParams);
        }

        private void OnStateChange(ClientState lastState, ClientState newState)
        {
            Console.WriteLine(lastState + " -> " + newState);
        }

    }
}
