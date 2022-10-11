using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlatformMoveInMaze : MonoBehaviour
{
    public float maxMotorTorque = 250f;
    public float maxBrakeTorque = 20000f;
    public float currentSpeed;
    public float maxSpeed = 30f;
    public bool isBrake;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public UZSensor UZForward;
    public UZSensor UZSideRight;
    public UZSensor UZSideLeft;
    public UZSensor UZRight;
    public UZSensor UZLeft;

    private const float MaxForwardDistance = 40f;
    private const float MinSideDistance = 15f;
    private float _steerMultiply = 100;
    private float _moveMultiplay = 10;

    private void Update()
    {
        UpdateAllWheelPose();
    }

    private void FixedUpdate()
    {
        Drive();
        Brake();
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;
        float speed = maxMotorTorque * Time.deltaTime * _steerMultiply;

        if (currentSpeed < maxSpeed)
        {
            if (UZForward.distance >= MaxForwardDistance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * _moveMultiplay;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = maxMotorTorque * Time.deltaTime * _moveMultiplay;
                }
            }
            else if (UZRight.distance > UZLeft.distance)
            {
                if (UZSideLeft.distance < MinSideDistance)
                {
                    foreach (WheelCollider wheel in rightWheelsCollider)
                    {
                        wheel.motorTorque = -speed * 10;
                    }
                    foreach (WheelCollider wheel in leftWheelsCollider)
                    {
                        wheel.motorTorque = speed * 10;
                    }
                }
                else
                {
                    foreach (WheelCollider wheel in rightWheelsCollider)
                    {
                        wheel.motorTorque = -speed;
                    }
                    foreach (WheelCollider wheel in leftWheelsCollider)
                    {
                        wheel.motorTorque = speed;
                    }
                }
            }
            else if (UZLeft.distance > UZRight.distance)
            {
                if (UZSideLeft.distance < MinSideDistance)
                {
                    foreach (WheelCollider wheel in rightWheelsCollider)
                    {
                        wheel.motorTorque = speed * 10;
                    }
                    foreach (WheelCollider wheel in leftWheelsCollider)
                    {
                        wheel.motorTorque = -speed * 10;
                    }
                }
                else
                {
                    foreach (WheelCollider wheel in rightWheelsCollider)
                    {
                        wheel.motorTorque = speed;
                    }
                    foreach (WheelCollider wheel in leftWheelsCollider)
                    {
                        wheel.motorTorque = -speed;
                    }
                }
            }
            else
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = -speed;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = speed;
                }
            }

            if (UZSideLeft.distance < MinSideDistance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = -speed;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = speed;
                }
            }
            else if (UZSideRight.distance < MinSideDistance)
            {
                foreach (WheelCollider wheel in rightWheelsCollider)
                {
                    wheel.motorTorque = speed;
                }
                foreach (WheelCollider wheel in leftWheelsCollider)
                {
                    wheel.motorTorque = -speed;
                }
            }
        }
    }

    private void Brake()
    {
        if (isBrake)
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
