using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints_01 : MonoBehaviour
{
    public Transform GetPathpoint(int pathPointIndex)
    {
        return transform.GetChild(pathPointIndex);
    }

    public int GetNextPathpointIndex(int currentPathpointIndex)
    {
        int nextPathpointIndex = currentPathpointIndex + 1;

        if (nextPathpointIndex == transform.childCount)
        {
            nextPathpointIndex = 0;
        }

        return nextPathpointIndex;
    }
}
