using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.CrashReportHandler;

public class PlatformMoveInMaze : MonoBehaviour
{
    public float maxMotorTorque = 250f;
    public float maxBrakeTorque = 20000f;
    public float currentSpeed;
    public float maxSpeed = 30f;
    public float error;
    public float errorOld;
    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;
    public float powerR, powerL;
    public float steer;
    public float forwardUZDistance, rightSideUZDistance, leftSideUZDistance,
        rightUZDistance, leftUZDistance;
    public float newError;
    public bool isBrake;
    public bool isStop;
    public bool isStart;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public UZSensor UZForward;
    public UZSensor UZSideRight;
    public UZSensor UZSideLeft;
    public UZSensor UZRight;
    public UZSensor UZLeft;

    private const float MinForwardDistance = 50f;
    private const float MinDistance = 40;
    private const float MinSideDistance = 15f;
    private float _timeToStop = 5f;
    public float _currentStopTime = 0;
    private float _steerMultiply = 100;

    private void Update()
    {
        UpdateAllWheelPose();

        forwardUZDistance = UZForward.distance;
        leftSideUZDistance = UZSideLeft.distance;
        rightSideUZDistance = UZSideRight.distance;
        leftUZDistance = UZLeft.distance;
        rightUZDistance = UZRight.distance;
    }

    private void FixedUpdate()
    {
        Drive();
        Brake();


        if(_currentStopTime <= _timeToStop)
        {
            _currentStopTime += Time.deltaTime;
        }
        else
        {
            isStop = false;
        }
    }

    private void CalcPID(float leftDisatnce, float rightDistance)
    {
        error = (leftDisatnce - rightDistance);
        steer += error;
        steer = steer < -0.1f ? -0.1f : 0.1f;

        newError = error - errorOld;

        powerL = maxMotorTorque - (proportionalGain * error + derivativeGain * (error - errorOld) + integralGain * steer);
        powerR = maxMotorTorque + (proportionalGain * error + derivativeGain * (error - errorOld) + integralGain * steer);

        errorOld = error;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;

        if (isStart)
        {
            if (UZForward.distance < 10 || UZSideLeft.distance < 7 || UZSideRight.distance < 7 ||
                UZLeft.distance < 5 || UZRight.distance < 5)
            {
                if (!isStop)
                {
                    isStop = true;
                    StartCoroutine(Stop());
                }
            }

            if (UZForward.distance > MinForwardDistance)
            {
                if (UZSideLeft.distance < MinSideDistance || UZSideRight.distance < MinSideDistance)
                {
                    CalcPID(UZSideLeft.distance, UZSideRight.distance);
                }
                else
                {
                    CalcPID(0, 0);
                }
            }
            else if (UZLeft.distance > UZRight.distance || UZLeft.distance < UZRight.distance)
            {
                if (UZSideLeft.distance < MinSideDistance || UZSideRight.distance < MinSideDistance)
                {
                    CalcPID(UZSideLeft.distance, UZSideRight.distance);
                }
                else
                {
                    CalcPID(UZLeft.distance, UZRight.distance);
                }
            }
            else
            {
                CalcPID(UZLeft.distance, UZRight.distance);
            }

            if (UZSideLeft.distance < MinSideDistance || UZSideRight.distance < MinSideDistance)
            {
                CalcPID(UZSideLeft.distance, UZSideRight.distance);
            }

            RobotMove();
        }
    }

    public IEnumerator Stop()
    {
        _currentStopTime = 0;
        isBrake = true;
        yield return new WaitForSeconds(1f);
        isBrake = false;
    }

    private void RobotMove()
    {
        foreach (WheelCollider wheel in rightWheelsCollider)
        {
            wheel.motorTorque = powerR;
        }
        foreach (WheelCollider wheel in leftWheelsCollider)
        {
            wheel.motorTorque = powerL;
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
