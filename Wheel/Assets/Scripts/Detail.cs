using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detail : MonoBehaviour
{
    public Point[] points;

    private void Start()
    {
        points = GetComponentsInChildren<Point>(); 
    }

    public IEnumerator MoveFromBeginToEnd()
    {
        float offset = 0;
        Vector3 originPosition = transform.TransformDirection(transform.position);
        while (transform.position != points[1].position)
        {
            transform.position = Vector3.Lerp(originPosition, points[1].position, offset);
            offset += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    public IEnumerator MoveFromEndToBegin()
    {
        float offset = 0;
        Vector3 originPosition = transform.position;
        while (transform.position != points[0].position)
        {
            transform.position = Vector3.Lerp(originPosition, points[0].position, offset);
            offset += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
