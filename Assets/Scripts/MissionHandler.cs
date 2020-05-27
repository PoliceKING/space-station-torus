using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MissionHandler : MonoBehaviour
{
    public int currentScore;

    string filePath;
    const string FILE_NAME = "SaveGame.json";

    public float timerMins = 8;
    public float timerSecs = 0;
    public bool timeOut = false;

    public GameObject pickupPrefab;
    public GameObject dropoffPrefab;

    public GameObject currentPickup = null;
    public GameObject currentDropoff = null;

    public GameScore gameScore;
    public bool onMission = false;
    public bool gameOver = false;
    
    SpawnPointsGenerator spawns;

    private void Start()
    {
        spawns = GetComponent<SpawnPointsGenerator>();

        filePath = Application.persistentDataPath;
        gameScore = new GameScore();
        Debug.Log(filePath);
        LoadScore();
    }

    private void Update()
    {
        Countdown();
    }

    void Countdown()
    {
        if(timerSecs <= 0 && timerMins <= 0 && !timeOut)
        {
            timeOut = true;
        }
        else if (timerSecs < 0)
        {
            timerSecs = 60f;
            timerMins -= 1f;
        }
        else if (timerSecs > 0 || timerMins > 0 && !timeOut)
        {
            timerSecs -= Time.deltaTime;
        }
    }

    public void GeneratePickUp()
    {
        if (!timeOut)
        {
            if (!onMission)
            {
                int randomSpawn = UnityEngine.Random.Range(0, spawns.spawnTransformArray.Length);
                Instantiate(pickupPrefab, spawns.spawnTransformArray[randomSpawn].position + (spawns.spawnTransformArray[randomSpawn].up * 1.5f), spawns.spawnTransformArray[randomSpawn].rotation);

                currentPickup = GameObject.Find("PickUp(Clone)");
                onMission = true;
            }
        }
        else
        {
            return;
        }
    }

    public void GenerateDropOff()
    {
        int randomSpawn = UnityEngine.Random.Range(0, spawns.spawnTransformArray.Length);
        Instantiate(dropoffPrefab, spawns.spawnTransformArray[randomSpawn].position + (spawns.spawnTransformArray[randomSpawn].up * 1.5f), spawns.spawnTransformArray[randomSpawn].rotation);
        currentDropoff = GameObject.Find("DropOff(Clone)");
    }

    void SaveScore()
    {
        string gameScoreJson = JsonUtility.ToJson(gameScore);
        File.WriteAllText(filePath + "/" + FILE_NAME, gameScoreJson);
        Debug.Log("File created and saved");
    }

    void LoadScore()
    {
        if(File.Exists(filePath + "/" + FILE_NAME))
        {
            string loadedJson = File.ReadAllText(filePath + "/" + FILE_NAME);
            gameScore = JsonUtility.FromJson<GameScore>(loadedJson);
            Debug.Log("File loaded successfully");
        }
        else
        {
            gameScore.score = 100;
            Debug.Log("File not found");
        }
    }

    private void OnApplicationQuit()
    {
        if(!gameOver && timeOut && currentScore > gameScore.score)
        {
            gameScore.score = currentScore;
            SaveScore();
        }
    }

    [Serializable]
    public struct GameScore
    {
        public int score;
    }
}
