using UnityEngine;
using UnityEngine.UI;
using PhotonNetwork;

public class teamSelection : MonoBehaviour
{
    public GameObject SeekerList;
    public GameObject HiderList;
    
    public void StartGame()
    {
        // Load the game map scene.
        SceneManager.LoadScene("New_Multiplayer");
    }
    //Adds to each of the team lists so players can see how many players are in each team.
    private void UpdateTeamLists()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            string playerName = player.NickName;
            string team = (string)player.CustomProperties["Team"];
            if (team == "Seekers")
            {
                AddPlayerToList(playerName, SeekerList);
            }
            else if (team == "HiderList")
            {
                AddPlayerToList(playerName, HiderList);
            }
        }
    }

    private void AddPlayerToList(string playerName, GameObject teamList)
    {
        // Create a new Text element to represent the player.
        GameObject playerText = new GameObject("PlayerText");
        Text text = playerText.AddComponent<Text>();
        text.text = playerName;
        text.transform.SetParent(teamList.transform);
    }
}
