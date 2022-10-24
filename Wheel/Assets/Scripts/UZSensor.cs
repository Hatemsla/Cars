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
        }
    }

    public float Convert(float value, float From1, float From2, float To1, float To2)
    {
        return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
    }
}
