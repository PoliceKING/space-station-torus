using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildingProfile", order = 100)]
public class BuildingProfile : ScriptableObject
{
    public Mesh[] groundBlocks;
    public Mesh[] mainBlocks;
    public Mesh[] altBlocks;
    public Mesh[] roofBlocks;

    public Material[] groundMaterials;
    public Material[] mainMaterials;
    public Material[] roofMaterials;

    public int maxHeight;
}
