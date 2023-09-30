using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    //Network Objects
    private NetworkRunner runner;
    private INetworkSceneManager sceneManager;
    private Dictionary<PlayerRef, NetworkObject> playersList = new Dictionary<PlayerRef, NetworkObject>();

    [SerializeField] private NetworkPrefabRef playerPrefab;

    public bool connectOnAwake = false;
    //Awake
    void Awake()
    {
        if (connectOnAwake == true)
        {
            
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void StartGame(GameMode mode)
    {
        if (runner == null)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        runner.ProvideInput = true;
        if (sceneManager == null) sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            PlayerCount = 10,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = sceneManager
        }
        );
        
        Debug.Log("Started.");
        print("Server : " + runner.IsServer);
    }

    private void OnGUI()
    {
        if (runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                Debug.Log("Host Pressed");
                StartGame(GameMode.Host);
                Debug.Log("Host Proc End");
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                Debug.Log("Client Pressed");
                StartGame(GameMode.Client);
                Debug.Log("Client Proc End");
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            //Vector3 spawnPos = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers)*3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity, player);
            playersList.Add(player, networkPlayerObject);
            Debug.Log(player.PlayerId + " Joined.");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playersList.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playersList.Remove(player);
            Debug.Log(player.PlayerId + " Left.");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.direction = MovementInput();
        input.Set(data);
    }

    private Vector3 MovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        return new Vector3(horizontalInput, 0, verticalInput);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to Server.");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
