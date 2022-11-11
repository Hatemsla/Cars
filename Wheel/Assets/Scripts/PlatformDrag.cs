using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrag : MonoBehaviour
{
    public Camera mainCamera;
    private float _mZCoord;
    private Vector3 _offset;

    private void Update()
    {
        transform.Rotate(new Vector3(0, Input.mouseScrollDelta.y * 2, 0));
    }

    private void OnMouseDown()
    {
        _mZCoord = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;

        _offset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = _mZCoord;

        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + _offset;
    }
}
