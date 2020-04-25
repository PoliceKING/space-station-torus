using UnityEngine;

namespace CITY
{
    public class CircleSpawnerScript : MonoBehaviour
    {
        public Transform geometryParent;
        public GameObject buildingPrefab;
        public BuildingProfile[] profileArray;

        public void BuildingRing(int objectCount, float ringRadius, float objectChance)
        {
            for (int i = 0; i <= objectCount; i++)
            {
                geometryParent = GameObject.Find("LevelGeometry").transform;
                int random = Random.Range(0, profileArray.Length);
                float angle = i * Mathf.PI * 2 / objectCount;
                Vector3 pos = transform.position + new Vector3(Mathf.Cos(angle) * ringRadius, Mathf.Sin(angle) * ringRadius, 0);
                if (Random.Range(0, 100) < objectChance)
                {
                    Instantiate(buildingPrefab, pos, Quaternion.LookRotation(transform.forward, -pos), geometryParent).GetComponent<NavTowerBlock>().SetProfile(profileArray[random]);
                }
                i++;
            }
        }
    }
}
