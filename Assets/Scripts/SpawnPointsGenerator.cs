using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsGenerator : MonoBehaviour
{
    public ArrayList spawnList = new ArrayList();
    public GameObject spawnTester;

    public int spawnPointMaxCount;
    public GameObject playerPrefab;
    public bool playerIsSpawned = false;
    public bool showSpawnPoints = false;

    public Transform[] spawnTransformArray;

    public void GenerateSpawnPoint(Transform blockTransform)
    {
        spawnList.Add(blockTransform);
        if(showSpawnPoints)
        {
            Instantiate(spawnTester, blockTransform.position + (blockTransform.up * 2.0f), blockTransform.rotation);
        }
        if(spawnList.Count >= spawnPointMaxCount)
        {
            spawnTransformArray = new Transform[spawnList.Count];
            spawnList.CopyTo(spawnTransformArray);
            if(!playerIsSpawned)
            {
                int randomSpawn = Random.Range(0, spawnTransformArray.Length);
                Instantiate(playerPrefab, spawnTransformArray[randomSpawn].position + (spawnTransformArray[randomSpawn].up * 2.0f), spawnTransformArray[randomSpawn].rotation);
                playerIsSpawned = true;
            }
        }
    }
}
