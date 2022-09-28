using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UZSensor : MonoBehaviour
{
    public float distance;

    private RaycastHit _hit;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, 1000f))
        {
            distance = _hit.distance;
            Debug.DrawRay(transform.position, transform.forward * 1000f);
            //Debug.Log(_hit.transform.gameObject);
        }
    }
}
