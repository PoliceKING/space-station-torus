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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateSpawnPoint(Transform blockTransform)
    {
        spawnList.Add(blockTransform);
        Instantiate(spawnTester, blockTransform.position + (blockTransform.up * 2.0f), blockTransform.rotation);
        if(spawnList.Count >= spawnPointMaxCount)
        {
            Transform[] spawnArray = new Transform[spawnList.Count];
            spawnList.CopyTo(spawnArray);
            if(!playerIsSpawned)
            {
                int randomSpawn = Random.Range(0, spawnArray.Length);
                Instantiate(playerPrefab, spawnArray[randomSpawn].position + (spawnArray[randomSpawn].up * 2.0f), spawnArray[randomSpawn].rotation);
                playerIsSpawned = true;
            }
        }
        Debug.Log(spawnList.Count);
    }
}
