using Common.Extensions;
using Fusion;
using Fusion.Sockets;
using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using Managers.Core.Player;
using Services.Abstraction;
using Services.Data.Netowrk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Core
{
    public class RealtimeManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private const string PHOTON_APP_ID = "d2fb9136-f41a-4bc8-98ba-127deb7d28ef";
        private const string SESSION_NAME = "MainRoom";

        [SerializeField]
        private GameMode _gameMode = GameMode.AutoHostOrClient;

        private NetworkRunner _networkRunner;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters;

        private IRealTimeService _realTimeService;
        private IVariableDatabase _variableDatabase;
        private IEnumerator _waitPhotonService;
        private bool quit;

        private void Initialization(IEvent @event, IRealTimeService realTimeService, IVariableDatabase variableDatabase)
        {
            _waitPhotonService = new WaitForSecondsRealtime(0.033f);
            _realTimeService = realTimeService;
            _variableDatabase = variableDatabase;
            _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
            @event.ListenToInitializeEvent(StartClient);
            @event.ListenToEvent(EEventType.OnApplicationQuit, ApplicationQuit);
        }

        private void ApplicationQuit()
        {
            quit = true;
            _networkRunner.Shutdown();
        }

        public void StartClient(IServiceProvider serviceProvider)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
            _networkRunner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = _gameMode,
                SessionName = "MainRoom",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });



            //_realTimeService.StartConnection(PHOTON_APP_ID);
            //this.quit = false;
            //ITaskRunner taskRunner = serviceProvider.GetService<ITaskRunner>();
            //taskRunner.StartCoroutineTask(Loop());

        }

        private IEnumerator Loop()
        {
            while (!this.quit)
            {
                _realTimeService.UpdateRealTime();
                yield return _waitPhotonService;
                //Thread.Sleep(33);
            }
        }


        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("OnPlayerJoined");

            if (runner.IsServer)
            {
                NetworkObject playerPrefab = _variableDatabase.GetDataObject<NetworkObject>("FirstGirl");
                Debug.Log(playerPrefab);
                // Create a unique position for the player
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
                networkPlayerObject.GetComponent<PlayerMover>().Initilization();
                // Keep track of the player avatars so we can remove it when they disconnect
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            // Find and remove the players avatar
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            if (Input.GetKey(KeyCode.W))
                data.direction += Vector3.forward;

            if (Input.GetKey(KeyCode.S))
                data.direction += Vector3.back;

            if (Input.GetKey(KeyCode.A))
                data.direction += Vector3.left;

            if (Input.GetKey(KeyCode.D))
                data.direction += Vector3.right;

            input.Set(data);

        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }
        public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer"); }
        void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed : " + reason); }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}