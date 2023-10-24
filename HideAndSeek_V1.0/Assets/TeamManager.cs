using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class TeamManager : NetworkBehaviour
{
    private List<PlayerRef> hiderList = new List<PlayerRef>();
    private List<PlayerRef> seekerList = new List<PlayerRef>();

    //Select a Team
    public bool SelectTeam(PlayerRef player)
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
    }
}