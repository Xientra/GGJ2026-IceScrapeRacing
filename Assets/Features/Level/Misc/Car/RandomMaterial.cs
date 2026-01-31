using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private int index;
    
    private void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterials[index] = materials[Random.Range(0, materials.Length)];
    }
}