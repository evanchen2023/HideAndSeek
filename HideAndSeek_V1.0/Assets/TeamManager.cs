using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class TeamManager : NetworkBehaviour
{
    private List<PlayerRef> hiders = new List<PlayerRef>();
    private List<PlayerRef> seekers = new List<PlayerRef>();

    // Define spawn points for Hiders and Seekers
    public Transform[] hiderSpawnPoints;
    public Transform[] seekerSpawnPoints;

    public void OnPlayerJoined(PlayerRef player)
    {
        // When a player joins, add them to their respective team and handle initialization.
        players.Add(player);

        // Determine the player's team, e.g., based on the TeamId.
        if (player.TeamId == 1)
        {
            hiders.Add(player);
            HandleHiderInitialization(player);
        }
        else if (player.TeamId == 2)
        {
            seekers.Add(player);
            HandleSeekerInitialization(player);
        }
    }

    public void OnTeamSelected(int teamId, PlayerRef player)
    {
        // Handle team selection logic, such as assigning a team and spawning players.
        // This method is called when a player selects a team using the TeamSelection script.

        // Example: When a player selects a team, call this method with the chosen team ID.
        player.TeamId = teamId;

        if (teamId == 1)
        {
            // The player chose to be a Hider.
            HandleHiderInitialization(player);
        }
        else if (teamId == 2)
        {
            // The player chose to be a Seeker.
            HandleSeekerInitialization(player);
        }
    }

    private void HandleHiderInitialization(PlayerRef hider)
    {
        // Implement Hider-specific initialization logic.
        // Assign a spawn point for Hiders.

        if (hiderSpawnPoints.Length > 0)
        {
            // Randomly select a spawn point for the Hider.
            int randomSpawnIndex = Random.Range(0, hiderSpawnPoints.Length);
            hider.transform.position = hiderSpawnPoints[randomSpawnIndex].position;
        }
    }

    private void HandleSeekerInitialization(PlayerRef seeker)
    {
        // Implement Seeker-specific initialization logic.
        // Assign a spawn point for Seekers.

        if (seekerSpawnPoints.Length > 0)
        {
            // Randomly select a spawn point for the Seeker.
            int randomSpawnIndex = Random.Range(0, seekerSpawnPoints.Length);
            seeker.transform.position = seekerSpawnPoints[randomSpawnIndex].position;
        }
    }

    //Select a Team
    /*public bool SelectTeam(PlayerRef player)
    {
        bool isSeeker = false;
        int seekerCount = seekerList.Count;
        int hiderCount = hiderList.Count;
        int minHiders = seekerCount * 3;
            
        //First Player Joined Should always be a Seeker
        if (seekerList.Count == 0)
        {
            isSeeker = true;
        }

        //Can only be a Seeker if less Seekers than hiders, and if hiderCount reaches minimum required count
        if (seekerCount < hiderCount)
        {
            if (hiderCount >= minHiders)
            {
                isSeeker = true;
            }
        }
        
        //Apply Team Changes
        ApplyTeamChange(isSeeker, player);
        
        return isSeeker;
    }

    private void ApplyTeamChange(bool isSeeker, PlayerRef player)
    {
        if (isSeeker)
        {
            AddSeeker(player);
        }
        else
        {
            AddHider(player);
        }
    }
    
    //Add Hider
    private void AddHider(PlayerRef player)
    {
        hiderList.Add(player);
    }
    
    //Add Seeker
    private void AddSeeker(PlayerRef player)
    {
        seekerList.Add(player);
    }*/
}