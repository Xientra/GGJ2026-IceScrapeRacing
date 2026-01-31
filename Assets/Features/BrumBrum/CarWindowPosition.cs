using System;
using UnityEngine;

public class CarWindowPositon : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private void Update()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }
}
