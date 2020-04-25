using UnityEngine;


namespace CITY
{
    public class NavTowerBlock : MonoBehaviour
    {
        public BuildingProfile myProfile;
        public Transform basePrefab;
        public int recursionLevel = 0;

        private int maxLevel = 3;
        private Renderer myRenderer;
        private MeshFilter myMeshFilter;
        private Mesh myMesh;
        private Material myMaterial;

        public void SetProfile(BuildingProfile profile)
        {
            myProfile = profile;
            maxLevel = myProfile.maxHeight;
        }

        public void Initialize(int recLevel, Material mat, Mesh mesh)
        {
            recursionLevel = recLevel;
            myMesh = mesh;
            myMaterial = mat;
            maxLevel = myProfile.maxHeight;
        }

        private void Awake()
        {
            myRenderer = GetComponent<MeshRenderer>();
            myMeshFilter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            Transform child;
            if (recursionLevel == 0)
            {
                int meshNum = myProfile.groundBlocks.Length;
                int matNum = myProfile.groundMaterials.Length;
                myMesh = myProfile.groundBlocks[Random.Range(0, meshNum)];
                myMaterial = myProfile.groundMaterials[Random.Range(0, matNum)];
            }

            myMeshFilter.mesh = myMesh;
            myRenderer.material = myMaterial;

            if (recursionLevel < maxLevel)
            {
                if (recursionLevel == maxLevel - 1)
                {
                    child = Instantiate(basePrefab, transform.position + transform.up, transform.rotation, this.transform);
                    int MeshNum = myProfile.roofBlocks.Length;
                    int matNum = myProfile.roofMaterials.Length;
                    child.GetComponent<NavTowerBlock>().Initialize(recursionLevel + 1, myProfile.roofMaterials[Random.Range(0, matNum)], myProfile.roofBlocks[Random.Range(0, MeshNum)]);
                }
                else
                {
                    child = Instantiate(basePrefab, transform.position + transform.up, transform.rotation, this.transform);
                    int meshNum = myProfile.mainBlocks.Length;
                    int matNum = myProfile.mainMaterials.Length;
                    child.GetComponent<NavTowerBlock>().Initialize(recursionLevel + 1, myProfile.mainMaterials[Random.Range(0, matNum)], myProfile.mainBlocks[Random.Range(0, meshNum)]);

                }
            }
        }
    }
}
