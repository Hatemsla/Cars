using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Vector3 position;

    private void Start()
    {
        position = transform.position;
    }
}
