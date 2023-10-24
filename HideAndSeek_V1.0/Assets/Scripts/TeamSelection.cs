using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class TeamSelection : NetworkBehaviour
{
    [SerializeField] private PlayerRef PlayerRef; // Reference to the PlayerInfo network object.

    public void SelectTeam(int newTeamId)
    {
        if (networkObject.IsServer)
        {
            PlayerRef.TeamId = newTeamId;
            networkObject.SendEvent(new TeamSelectedEvent { TeamId = newTeamId });
        }
    }
}

public class TeamSelectedEvent : NetworkEvent
{
    public int TeamId;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(TeamId);
    }

    public override void Deserialize(NetworkReader reader)
    {
        TeamId = reader.ReadInt32();
    }

    /*[SerializeField]
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
    }*/
}
