using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private int index;
    
    private void Start()
    {
        List<Material> mats = new List<Material>();
        GetComponent<MeshRenderer>().GetSharedMaterials(mats);
            
        mats[index] = materials[Random.Range(0, materials.Length)];
        
        GetComponent<MeshRenderer>().SetSharedMaterials(mats);
    }
}