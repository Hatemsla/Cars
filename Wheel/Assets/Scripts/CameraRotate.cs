using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Camera[] allCameras;
    public float rotationStep = 2f;

    private int _k = 0;
    private float _maxRotationXAngle = 45f;
    private float _minRotationXAngle = -45f;

    private float[] _currentRotationX;

    private void Start()
    {
        _currentRotationX = new float[allCameras.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _k++;
            if (_k == allCameras.Length)
                _k = 0;

            for (int i = 0; i < allCameras.Length; i++)
            {
                if (i == _k)
                    allCameras[i].enabled = true;
                else
                    allCameras[i].enabled = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ref float currentRotation = ref _currentRotationX[_k];

            currentRotation = Mathf.Clamp(currentRotation - rotationStep, _minRotationXAngle, _maxRotationXAngle);
            SetLocalX(_k, currentRotation);
        }

        else if (Input.GetKey(KeyCode.F))
        {
            ref float currentRotation = ref _currentRotationX[_k];

            currentRotation = Mathf.Clamp(currentRotation + rotationStep, _minRotationXAngle, _maxRotationXAngle);
            SetLocalX(_k, currentRotation);
        }
    }

    private void SetLocalX(int cameraIndex, float value)
    {
        Vector3 resultLocalRot = allCameras[cameraIndex].transform.localEulerAngles;
        resultLocalRot.x = value;

        allCameras[_k].transform.localEulerAngles = resultLocalRot;
    }
}