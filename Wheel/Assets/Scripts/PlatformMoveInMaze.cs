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
    public float forwardUzDistance, rightSideUzDistance, leftSideUzDistance,
        rightUzDistance, leftUzDistance;
    public float newError;
    public bool isBrake;
    public bool isStop;
    public bool isStart;
    public bool isReversing;
    public bool isMoving;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public UZSensor uzForward;
    public UZSensor uzSideRight;
    public UZSensor uzSideLeft;
    public UZSensor uzRight;
    public UZSensor uzLeft;

    private const float MinForwardDistance = 50f;
    private const float MinSideDistance = 20f;
    private const float timeToStop = 4f;
    private float _currentStopTime = 0;
    private float _reversCounter = 0;
    private const float reversFor = 2f;
    private float _movingCounter = 0;
    private const float movingFor = 3f;

    private void Update()
    {
        UpdateAllWheelPose();

        forwardUzDistance = uzForward.distance;
        leftSideUzDistance = uzSideLeft.distance;
        rightSideUzDistance = uzSideRight.distance;
        leftUzDistance = uzLeft.distance;
        rightUzDistance = uzRight.distance;
    }

    private void FixedUpdate()
    {
        //Drive();
        RobotMove();
        Brake();

        if(_currentStopTime <= timeToStop)
            _currentStopTime += Time.deltaTime;
        else
            isStop = false;

        if (isMoving)
        {
            if(_movingCounter <= movingFor)
                _movingCounter += Time.deltaTime;
            else
            {
                isMoving = false;
                _movingCounter = 0;
            }
        }

        if (!isStop && isReversing)
        {
            _reversCounter += Time.deltaTime;
            if(_reversCounter >= reversFor)
            {
                _reversCounter = 0;
                isReversing = false;
            }
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
            if (uzForward.distance < 5 && !isReversing)
            {
                isReversing = true;
                powerL = -700;
                powerR = -700;
            }
            else if (!isReversing && (uzForward.distance < 15 || uzSideLeft.distance < 10 || 
                uzSideRight.distance < 10 || uzLeft.distance < 5 || uzRight.distance < 5))
            {
                if (!isStop && !isMoving)
                {
                    isStop = true;
                    StartCoroutine(Stop(1.5f));
                }
            }

            if (!isReversing)
            {
                if (uzForward.distance > MinForwardDistance)
                {
                    if (uzSideLeft.distance < MinSideDistance || uzSideRight.distance < MinSideDistance)
                        CalcPID(uzSideLeft.distance, uzSideRight.distance);
                    else
                        CalcPID(0, 0);
                }
                else if (uzLeft.distance > uzRight.distance || uzLeft.distance < uzRight.distance)
                {
                    if (uzSideLeft.distance < MinSideDistance || uzSideRight.distance < MinSideDistance)
                        CalcPID(uzSideLeft.distance, uzSideRight.distance);
                    else
                        CalcPID(uzLeft.distance, uzRight.distance);
                }
                else
                    CalcPID(uzLeft.distance, uzRight.distance);

                if (uzSideLeft.distance < MinSideDistance || uzSideRight.distance < MinSideDistance)
                    CalcPID(uzSideLeft.distance, uzSideRight.distance);

                RobotMove();
            }
        }
    }

    private IEnumerator Stop(float stopTime)
    {
        _currentStopTime = 0;
        isBrake = true;
        yield return new WaitForSeconds(stopTime);
        isBrake = false;
        isMoving = true;
    }

    private void RobotMove()
    {
        foreach (WheelCollider wheel in rightWheelsCollider)
            wheel.motorTorque = powerR;
        foreach (WheelCollider wheel in leftWheelsCollider)
            wheel.motorTorque = powerL;
    }

    private void Brake()
    {
        if (isBrake)
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.brakeTorque = maxBrakeTorque;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.brakeTorque = maxBrakeTorque;
        }
        else
        {
            foreach (WheelCollider wheel in rightWheelsCollider)
                wheel.brakeTorque = 0;
            foreach (WheelCollider wheel in leftWheelsCollider)
                wheel.brakeTorque = 0;
        }
    }

    /// <summary>
    /// Обновление вращения всех колес
    /// </summary>
    private void UpdateAllWheelPose()
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
    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
