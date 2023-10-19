using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    //Network Objects
    private NetworkRunner runner;
    private INetworkSceneManager sceneManager;
    private Dictionary<PlayerRef, NetworkObject> playersList = new Dictionary<PlayerRef, NetworkObject>();
    private Dictionary<PlayerRef, NetworkObject> cameraList = new Dictionary<PlayerRef, NetworkObject>();
    private List<NetworkObject> spawnList = new List<NetworkObject>();
    private GameObject teamManagerObject;
    private TeamManager teamManager;

    //Player Settings
    [SerializeField] private NetworkPrefabRef seekerPrefab;
    [SerializeField] private NetworkPrefabRef hiderPrefab;
    [SerializeField] private NetworkPrefabRef cameraPrefab;
    [SerializeField] private NetworkPrefabRef propPrefab;
   

    //Awake
    void Awake()
    {
        teamManagerObject = GameObject.Find("TeamManager");
        teamManager = teamManagerObject.GetComponent<TeamManager>();
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
            bool seeker = teamManager.SelectTeam(player);
            //bool seeker = false; //Always Spawn as this for Testing
            if (!seeker)
            {
                SpawnHider(runner, player);
            }
            if (seeker)
            {
                SpawnSeeker(runner, player);
            }
            Debug.Log(player.PlayerId + " Joined.");
            //Spawn Props
            PropSpawn(runner, player);
        }
        
        // //Set Cursor
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    
    //Spawn Players if they are Hider
    private void SpawnHider(NetworkRunner runner, PlayerRef player)
    {
        //Get Spawns
        spawnList = GetSpawnList();
        int spawnIndex = Random.Range(0, spawnList.Count - 1);
        Vector3 spawnPoint = spawnList[spawnIndex].transform.position;
        RemoveSpawnPoint(spawnIndex);
            
        //Add Camera
        NetworkObject networkPlayerCamera = runner.Spawn(cameraPrefab, spawnPoint, Quaternion.identity, player);
        cameraList.Add(player, networkPlayerCamera);
            
        //Add Player
        NetworkObject networkPlayerObject = runner.Spawn(hiderPrefab, spawnPoint, Quaternion.identity, player);
        playersList.Add(player, networkPlayerObject);
            
        networkPlayerCamera.GetComponent<PlayerCamera>().SetFollow(networkPlayerObject);
                
        Debug.Log(player.PlayerId + " Joining Hiders");
    }
    
    //Spawn Players if they are Seeker
    private void SpawnSeeker(NetworkRunner runner, PlayerRef player)
    {
        //Get Spawn Point
        Vector3 spawnPoint = GetSpawnPoint();
            
        //Add Camera
        NetworkObject networkPlayerCamera = runner.Spawn(cameraPrefab, spawnPoint, Quaternion.identity, player);
        cameraList.Add(player, networkPlayerCamera);
            
        //Add Player
        NetworkObject networkPlayerObject = runner.Spawn(seekerPrefab, spawnPoint, Quaternion.identity, player);
        playersList.Add(player, networkPlayerObject);
            
        networkPlayerCamera.GetComponent<PlayerCamera>().SetFollow(networkPlayerObject);
                
        Debug.Log(player.PlayerId + " Joining Seekers");
    }

    private void PropSpawn(NetworkRunner runner, PlayerRef player)
    {
        NetworkObject networkProp = runner.Spawn(propPrefab, new Vector3(0, 0, 5), Quaternion.identity, player);
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

    private Vector3 GetSpawnPoint()
    {
        //Get List of Spawn Points
        spawnList = GetSpawnList();
        //Get Random Spawn Index
        int spawnIndex = Random.Range(0, spawnList.Count - 1);
        //Set Spawn Point, Remove Index to Avoid Colluding Spawn Points
        Vector3 spawnPoint = spawnList[spawnIndex].transform.position;
        RemoveSpawnPoint(spawnIndex);

        return spawnPoint;
    }

    private List<NetworkObject> GetSpawnList()
    {
        List<NetworkObject> returnList = new List<NetworkObject>();
        var spawns = GameObject.FindGameObjectsWithTag("SeekerSpawn");

        for (int i = 0; i < spawns.Length; i++)
        {
            returnList.Add(spawns[i].GetComponent<NetworkObject>());
        }

        return returnList;
    }

    private void RemoveSpawnPoint(int spawnIndex)
    {
        if (spawnList[spawnIndex] != null)
        {
            runner.Despawn(spawnList[spawnIndex]);
            spawnList.RemoveAt(spawnIndex);
        }
    }

    //Controls
    private bool jumpButton;
    private bool sprintButton;
    private bool shootButton;
    private bool aimButton;
    
    void Update()
    {
        jumpButton = jumpButton | Input.GetKeyDown(KeyCode.Space);
        sprintButton = sprintButton | Input.GetKey(KeyCode.LeftShift);
        shootButton = shootButton | Input.GetMouseButtonDown(0);
        aimButton = aimButton | Input.GetMouseButton(1);
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

        if (shootButton)
        {
            data.shootButtons |= NetworkInputData.SHOOTBUTTON;
        }

            //Get Toggle Buttons
        data.sprintButton = sprintButton;
        data.aimButton = aimButton;
        data.shootButton = shootButton;

        //Set Controls False
        jumpButton = false;
        sprintButton = false;
        aimButton = false;
        shootButton = false;
        
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

    private Dictionary<PlayerRef, NetworkObject> GetPlayersList()
    {
        return playersList;
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
