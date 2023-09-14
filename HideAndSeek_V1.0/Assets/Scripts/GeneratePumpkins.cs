using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePumpkins : MonoBehaviour
{
    public GameObject pumpkin;
    public int xPos;
    public int zPos;
    public int pumpkinCount;
    public Terrain terrain;
    public float yOffset = 0.5f;

    public float minDistance = 5.0f;
    List<Vector3> generatedPositions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(PumpkinDrop());
    }

    IEnumerator PumpkinDrop()
    {
        while (pumpkinCount < 10)
        {
            xPos = Random.Range(26, 76);
            zPos = Random.Range(16, 78);

            // Get Terrain Height
            float terrainHeight = terrain.SampleHeight(new Vector3(xPos, 0, zPos));

            // Set new position for pumpkins
            Vector3 pumpkinPosition = new Vector3(xPos, terrainHeight + yOffset, zPos);

            bool tooClose = false;

            foreach(var pos in generatedPositions)
            {
                if(Vector3.Distance(pumpkinPosition, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if(!tooClose)
            {
                // Instantiate pumpkins with pumpkinPosition
                Instantiate(pumpkin, pumpkinPosition, Quaternion.identity);
                generatedPositions.Add(pumpkinPosition);

                yield return new WaitForSeconds(0.2f);
                pumpkinCount += 1;
            }
            else
            {
                // start looping if too close
                yield return null;
            }
        }
    }

}

