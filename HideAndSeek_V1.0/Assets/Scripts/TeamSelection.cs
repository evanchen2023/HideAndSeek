using UnityEngine;
using UnityEngine.UI;
using Photon.fusion;

public class TeamSelection : MonoBehaviour
{
    [SerializeField]
    private Button join1; // 

    [SerializeField]
    private Button join2; // 

    private void Start()
    {
        // Add a listener to the "Join Hiders" button
        join1.onClick.AddListener(JoinHidersTeam);

        // Add a listener to the "Join Seekers" button
        join2.onClick.AddListener(JoinSeekersTeam);
    }

    private void JoinHidersTeam()
    {
        JoinRoom("Hiders"); // Join the "Hiders" room
    }

    private void JoinSeekersTeam()
    {
        JoinRoom("Seekers"); // Join the "Seekers" room
    }

    private void JoinRoom(string roomName)
    {
       
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
        }
        else
        {
            Debug.LogWarning("Not connected to the Photon network.");
        }
    }
}
