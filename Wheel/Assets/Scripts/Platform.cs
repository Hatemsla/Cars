using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float maxMotorTorque;
    public float currentSpeed;
    public float maxSpeed;
    public WheelCollider[] leftWheelsCollider;
    public WheelCollider[] rightWheelsCollider;
    public Transform[] leftWheelsTransform;
    public Transform[] rightWheelsTransform;

    private float _verticalInput;
    private float _horizontalInput;
    private float _rightSteer;
    private float _leftSteer;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        Drive();
        UpdateAllWheelPoses();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;

        if(_verticalInput > 0 && _horizontalInput == 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
        }
        else if(_verticalInput < 0 && _horizontalInput == 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
        }
        else if(_verticalInput > 0 && _horizontalInput > 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
        }
        else if(_verticalInput > 0 && _horizontalInput < 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
        }
        else if (_verticalInput > 0 && _horizontalInput > 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
        }
        else if (_verticalInput > 0 && _horizontalInput < 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
        }

        if (_horizontalInput == 0 && _verticalInput == 0)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = 20000f;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = 20000f;
            }
        }
        else
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.brakeTorque = 0;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.brakeTorque = 0;
        }
    }

    private void UpdateAllWheelPoses()
    {
        UpdateWheelPose(rightWheelsCollider[0], rightWheelsTransform[0]);
        UpdateWheelPose(rightWheelsCollider[1], rightWheelsTransform[1]);
        UpdateWheelPose(leftWheelsCollider[0], leftWheelsTransform[0]);
        UpdateWheelPose(leftWheelsCollider[1], leftWheelsTransform[1]);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
