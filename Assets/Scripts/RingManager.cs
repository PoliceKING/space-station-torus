using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CITY
{
    public class RingManager : MonoBehaviour
    {
        public Transform ringParent;
        public float stationRadius = 20f;
        public int buildingAmount = 100;
        public int stationLength = 5;
        [Range(1, 5)]
        public float ringOffset = 1;
        [Range(0, 100)]
        public float buildingChance;
        public GameObject spawnerObject;

        GameObject[] ringArray;
        CircleSpawnerScript circleSpawner;
        // Start is called before the first frame update
        void Start()
        {

            ringParent = GameObject.Find("RingSpawners").transform;
            ringArray = new GameObject[stationLength];
            GenerateRings();
        }

        void GenerateRings()
        {
            for (int i = 0; i < ringArray.Length; i++)
            {
                ringArray[i] = Instantiate(spawnerObject, transform.position + new Vector3(0, 0, i * ringOffset), Quaternion.identity, ringParent);
                circleSpawner = ringArray[i].GetComponent<CircleSpawnerScript>();
                circleSpawner.BuildingRing(buildingAmount, stationRadius, buildingChance);
            }
        }
    }
}
