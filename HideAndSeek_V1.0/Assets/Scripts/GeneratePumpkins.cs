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
    List<Vector3> treePositions = new List<Vector3>();
    List<Rect> stoneAreas = new List<Rect> //stone locations and creates invisible boarder.
    {
        new Rect(31,54,7,7),
        new Rect(21,72,8,9),
        new Rect(75,62,7,7),
        new Rect(76,25,7,7)
    };

    void Start()
    {
        LoadTreePositions();
        StartCoroutine(PumpkinDrop());
    }

    void LoadTreePositions()
    {
        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] treeInstances = terrainData.treeInstances;
        foreach(var tree in treeInstances)
        {
            Vector3 pos = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;
            treePositions.Add(pos);
        }
    }
    
    IEnumerator PumpkinDrop() //Creates pumpkins on the map 
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

            //check if too close to another pumpkins
            foreach(var pos in generatedPositions)
            {
                if(Vector3.Distance(pumpkinPosition, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            //check if within forbidden stone areas
            foreach(var rect in stoneAreas)
            {
                if(rect.Contains(new Vector2(xPos, zPos)))
                {
                    tooClose = true;
                    break;
                }
            }

            foreach(var pos in treePositions) //Once player gets to the pumpkin it picks the pukin and adds to timer.
            {
                if(Vector3.Distance(pumpkinPosition, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            //check if the spot is free from any obstructions
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

