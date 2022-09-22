using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float maxTorque = 60;
    public float speed = 10;
    public Rigidbody rb;
    public WheelCollider frontRightWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider rearRightWheel;
    public WheelCollider rearLeftWheel;
    public Transform frontRightTransform;
    public Transform frontLeftTransform;
    public Transform rearRightTransfrom;
    public Transform rearLeftTransform;
    public GameObject leftWheels;
    public GameObject rightWheels;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    private float _verticalInput;
    private float _horizontalInput;

    private void FixedUpdate()
    {
        GetInput();
        Accelerate();
        UpdateAllWheelPoses();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void Accelerate()
    {
        if (_horizontalInput != 0 && _verticalInput == 0)
        {
            if (_horizontalInput < 0) //left steer
            {
                speed = 10;
                frontLeftWheel.motorTorque = maxTorque * _horizontalInput * speed;
                rearLeftWheel.motorTorque = maxTorque * _horizontalInput * speed;
                frontRightWheel.motorTorque = -maxTorque * _horizontalInput * speed;
                rearRightWheel.motorTorque = -maxTorque * _horizontalInput * speed;
            }
            else if (_horizontalInput > 0) //right steer
            {
                speed = 10;
                frontRightWheel.motorTorque = -maxTorque * _horizontalInput * speed;
                rearRightWheel.motorTorque = -maxTorque * _horizontalInput * speed;
                frontLeftWheel.motorTorque = maxTorque * _horizontalInput * speed;
                rearLeftWheel.motorTorque = maxTorque * _horizontalInput * speed;
            }
            else
            {
                speed = 0;
                frontLeftWheel.motorTorque = maxTorque * speed;
                rearLeftWheel.motorTorque = maxTorque * speed;
                frontRightWheel.motorTorque = -maxTorque * speed;
                rearRightWheel.motorTorque = -maxTorque * speed;
            }
        }
        else if (_verticalInput != 0 && _horizontalInput == 0)
        {
            speed = 10;
            frontLeftWheel.motorTorque = maxTorque * _verticalInput * speed;
            frontRightWheel.motorTorque = maxTorque * _verticalInput * speed;
            rearLeftWheel.motorTorque = maxTorque * _verticalInput * speed;
            rearRightWheel.motorTorque = maxTorque * _verticalInput * speed;
        }
        else
        {
            speed = 0;
            frontLeftWheel.motorTorque = maxTorque * _verticalInput * speed;
            frontRightWheel.motorTorque = maxTorque * _verticalInput * speed;
            rearLeftWheel.motorTorque = maxTorque * _verticalInput * speed;
            rearRightWheel.motorTorque = maxTorque * _verticalInput * speed;
        }
    }

    private void UpdateAllWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransfrom);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    public void PrintData(float speed, float time, Vector3 direction)
    {
        Debug.Log($"Speed: {speed}, Time: {time}, Direction: {direction}");
    }
}
