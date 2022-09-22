using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public List<AxleInfo> AxleInfos;
    public Rigidbody rb;
    public Transform platform;
    public Transform wheelRightFront;
    public Transform wheelLeftFront;
    public Transform wheelLeftBack;
    public Transform wheelRightBack;

    private Quaternion _rotation;
    private Vector3 _position;

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        if(steering == 0 && motor == 0)
        {
            rb.drag = 10;
        }
        else
        {
            rb.drag = 0;
        }
        
        if(steering != 0 && motor == 0)
        {
            platform.Rotate(platform.up, steering * Time.deltaTime * 10);
            if(steering < 0)
            {
                wheelRightFront.Rotate(Vector3.left, (steering * Time.deltaTime * 10));
                wheelLeftFront.Rotate(Vector3.left, -(steering * Time.deltaTime * 10));
                wheelRightBack.Rotate(Vector3.left, (steering * Time.deltaTime * 10));
                wheelLeftBack.Rotate(Vector3.left, -(steering * Time.deltaTime * 10));
            }
            else
            {
                wheelRightFront.Rotate(Vector3.left, -(steering * Time.deltaTime * 10));
                wheelLeftFront.Rotate(Vector3.left, (steering * Time.deltaTime * 10));
                wheelRightBack.Rotate(Vector3.left, -(steering * Time.deltaTime * 10));
                wheelLeftBack.Rotate(Vector3.left, (steering * Time.deltaTime * 10));
            }
        }
        else
        {
            foreach(AxleInfo axleInfo in AxleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;

                    axleInfo.leftWheel.GetWorldPose(out _position, out _rotation);
                    axleInfo.leftWheelVisualTransform.rotation = _rotation;
                    axleInfo.leftWheelVisualTransform.rotation *= Quaternion.Euler(0f, 180f, 0f);
                    axleInfo.rightWheel.GetWorldPose(out _position, out _rotation);
                    axleInfo.rightWheelVisualTransform.rotation = _rotation;
                    axleInfo.rightWheelVisualTransform.rotation *= Quaternion.Euler(0f, 180f, 0f);
                }
            }
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform leftWheelVisualTransform;
    public Transform rightWheelVisualTransform;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}