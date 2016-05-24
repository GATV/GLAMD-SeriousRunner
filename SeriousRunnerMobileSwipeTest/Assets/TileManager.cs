using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;

    private Transform playerTransform;

    private float spawnZ = -10.0f;
    private float tileLength = 20.0f;
    private int amountTilesOnScreen = 7;
    private float safeZone = 15.0f;

    private List<GameObject> activeTiles;

    // Use this for initialization
    private void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amountTilesOnScreen; i++)
        {
            SpawnTile();
        }

    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amountTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();

        }
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject gameobj;

        gameobj = Instantiate(tilePrefabs[0]) as GameObject;
        gameobj.transform.SetParent(transform);
        gameobj.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(gameobj);
    }


}

