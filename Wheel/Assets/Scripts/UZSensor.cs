using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UZSensor : MonoBehaviour
{
    public float distance;
    public float radius = 5.0f;
    public int segments = 12;
    public float curveAmount = 360.0f;
    private float calcAngle;
    private List<Vector3> nodes = new List<Vector3>();

    private RaycastHit _hit;

    private void Update()
    {
        //if (Physics.Raycast(transform.position, transform.forward, out _hit, 1000f, -1, QueryTriggerInteraction.Ignore))
        //{
        //    distance = _hit.distance;
        //    Debug.DrawRay(transform.position, transform.forward * 1000f);
        //}
        AngleCalc(30, 100);
        
    }

    public void AngleCalc(float angle, int raysCount)
    {
        var rayOffset = angle / raysCount;
        int catchRaysCount = 0;
        RaycastHit hit;

        for (var i = -angle * 0.5f; i <= angle * 0.5f; i += rayOffset)
        {
            var direction = Quaternion.AngleAxis(i, transform.up) * transform.forward;
            if (Physics.Raycast(transform.position, direction, out hit, 1000f, -1, QueryTriggerInteraction.Ignore))
            {
                distance += hit.distance;
                catchRaysCount++;
                Debug.DrawLine(transform.position, transform.position + direction * hit.distance);
            }

        }

        if (catchRaysCount > 0)
            distance /= catchRaysCount;
    }
}
