﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlatformMoveOnLine : MonoBehaviour
{
    public float maxMotorTorque = 250f;
    public float maxBrakeTorque = 20000f;
    public float currentSpeed;
    public float maxSpeed = 30f;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public IKSensor[] _IKsensors;

    private float _rightSteer;
    private float _leftSteer;
    private float _maxRightSteer = 100;
    private float _minRightSteer = 0;


    private void FixedUpdate()
    {
        Drive();
        ApplySteer();
        UpdateAllWheelPose();
    }

    private void ApplySteer()
    {
        if (_IKsensors[0].grayScale < _IKsensors[1].grayScale)
        {
            _rightSteer = _maxRightSteer * _IKsensors[0].grayScale;
            _leftSteer = _minRightSteer;
        }
        else if(_IKsensors[0].grayScale > _IKsensors[1].grayScale)
        {
            _rightSteer = _minRightSteer;
            _leftSteer = _maxRightSteer * _IKsensors[1].grayScale;
        }
    }

    /// <summary>
    /// Обновление вращения всех колес
    /// </summary>
    void UpdateAllWheelPose()
    {
        UpdateWheelPose(rightWheelsCollider[0], rightWheelsTransform[0]);
        UpdateWheelPose(rightWheelsCollider[1], rightWheelsTransform[1]);
        UpdateWheelPose(leftWheelsCollider[0], leftWheelsTransform[0]);
        UpdateWheelPose(leftWheelsCollider[1], leftWheelsTransform[1]);
    }

    /// <summary>
    /// Вращение колеса
    /// </summary>
    /// <param name="wheelCollider"></param>
    /// <param name="wheelTransform"></param>
    void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;

        if(currentSpeed < maxSpeed)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * _rightSteer;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * _leftSteer;
            }
        }
        else
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.motorTorque = 0;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.motorTorque = 0;
            }
        }
    }
}
