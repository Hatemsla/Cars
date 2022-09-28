using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveInMaze : MonoBehaviour
{
    public float maxMotorTorque = 250f;
    public float maxBrakeTorque = 20000f;
    public float currentSpeed;
    public float maxSpeed = 30f;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public UZSensor UZForward;
    public UZSensor UZRight;
    public UZSensor UZLeft;

    private const float MaxForwardDistance = 40f;
    private const float MinSideDistance = 15f;
    private bool _isBrake;
    private bool _isLeftSteer;
    private bool _isRightSteer;
    //private bool _isLeftSteer;

    private void Update()
    {
        UpdateAllWheelPose();
    }

    private void FixedUpdate()
    {
        if (UZForward.distance != 0)
        {
            Drive();
            //Brake();
        }
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;

        if (currentSpeed < maxSpeed)
        {
            if (UZForward.distance >= MaxForwardDistance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 10;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 10;
                }
            }
            else if (UZRight.distance > UZLeft.distance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
            }
            else if (UZLeft.distance > UZRight.distance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
            }
            else
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
            }

            if (UZLeft.distance < MinSideDistance)
            {
                Debug.Log("Steer!!!");
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
            }
            else if (UZRight.distance < MinSideDistance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * 100;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = -maxMotorTorque * Time.deltaTime * 100;
                }
            }
        }
    }

    private void Brake()
    {
        if (_isBrake)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.brakeTorque = maxBrakeTorque;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.brakeTorque = maxBrakeTorque;
            }
        }
        else
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.brakeTorque = 0;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.brakeTorque = 0;
            }
        }
    }

    /// <summary>
    /// Управление скорость колес
    /// </summary>
    /// <param name="rightWheelSpeed"></param>
    /// <param name="leftWheelSpeed"></param>
    private void SpeedIncrease(float rightWheelSpeed, float leftWheelSpeed)
    {
        foreach (WheelCollider wheel in rightWheelsCollider)
        {
            wheel.motorTorque = maxMotorTorque * Time.deltaTime * rightWheelSpeed;
        }
        foreach (WheelCollider wheel in leftWheelsCollider)
        {
            wheel.motorTorque = maxMotorTorque * Time.deltaTime * leftWheelSpeed;
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
}
