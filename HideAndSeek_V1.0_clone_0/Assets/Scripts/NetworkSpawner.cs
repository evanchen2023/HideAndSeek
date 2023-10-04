using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
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
    private Dictionary<PlayerRef, NetworkObject> cameraList = new Dictionary<PlayerRef, NetworkObject>();
    
    //Player Settings
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private NetworkPrefabRef cameraPrefab;

    //Awake
    void Awake()
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
            //Add Camera
            NetworkObject networkPlayerCamera = runner.Spawn(cameraPrefab, new Vector3(0, 2, 0), Quaternion.identity, player);
            cameraList.Add(player, networkPlayerCamera);
            
            //Add Player
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity, player);
            playersList.Add(player, networkPlayerObject);
            
            networkPlayerCamera.GetComponent<PlayerCamera>().SetFollow(networkPlayerObject);
            
            Debug.Log(player.PlayerId + " Joined.");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (playersList.TryGetValue(player, out NetworkObject playerObject))
        {
            runner.Despawn(playerObject);
            playersList.Remove(player);
            Debug.Log(player.PlayerId + " Left.");
        }
        
        if (cameraList.TryGetValue(player, out NetworkObject cameraObject))
        {
            runner.Despawn(cameraObject);
            cameraList.Remove(player);
            Debug.Log(player.PlayerId + " Left.");
        }
    }

    //Controls
    private bool jumpButton;

    void Update()
    {
        jumpButton = jumpButton | Input.GetKeyDown(KeyCode.Space);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.direction = MovementInput();
        data.rotationDir = ViewInput();
        
        //Jump Controls
        if (jumpButton)
        {
            data.buttons |= NetworkInputData.JUMPBUTTON;
        }

        jumpButton = false;

        input.Set(data);
    }

    private Vector3 MovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        return new Vector3(horizontalInput, 0, verticalInput);
    }

    private Vector2 ViewInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * -1; //Invert Y Mouse

        return new Vector2(mouseX, mouseY);
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

    public Dictionary<PlayerRef, NetworkObject> GetPlayerList()
    {
        return playersList;
    }
}
