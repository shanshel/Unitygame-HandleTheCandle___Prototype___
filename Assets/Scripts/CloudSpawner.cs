using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public Cloud[] cloudPrefabs;
    public float maxHeight, minHieght, maxLeft, maxRight, maxSize;
    void Start()
    {
        InvokeRepeating("spawnCloud", 2f, 5f);
    }

    void spawnCloud()
    {
        int randPrefab = Random.Range(0, cloudPrefabs.Length);
        int randSide = Random.Range(0, 2);
        float randHeight = Random.Range(minHieght, maxHeight);
        Cloud cloud;
    
       
        if (randSide == 0)
        {
            cloud = Instantiate(cloudPrefabs[randPrefab], new Vector2(maxLeft, randHeight), Quaternion.identity, transform);
            cloud.xTarget = maxRight;
        }
        else
        {
            cloud = Instantiate(cloudPrefabs[randPrefab], new Vector2(maxRight, randHeight), Quaternion.identity, transform);
            cloud.xTarget = maxLeft;
        }

        
    }
}
