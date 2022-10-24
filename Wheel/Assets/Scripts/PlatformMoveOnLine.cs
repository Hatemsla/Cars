using UnityEngine;

public class PlatformMoveOnLine : MonoBehaviour
{
    public float maxMotorTorque = 250f;
    public float maxBrakeTorque = 20000f;
    public float currentSpeed;
    public float maxSpeed = 30f;
    public float error;
    public float errorOld;
    public float proportionalGain = 1;
    public float integralGain = 1;
    public float derivativeGain = 1;
    public float powerR, powerL;
    public float steer;
    public WheelCollider[] rightWheelsCollider;
    public WheelCollider[] leftWheelsCollider;
    public Transform[] rightWheelsTransform;
    public Transform[] leftWheelsTransform;
    public IKSensor[] _IKsensors;

    [HideInInspector] public bool isBrake;

    private void Start()
    {
        isBrake = true;
    }

    private void Update()
    {
        UpdateAllWheelPose();
    }

    private void FixedUpdate()
    {
        if (!isBrake)
        {
            Drive();
            ApplySteer();
        }

        Brake();
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

    private void ApplySteer()
    {
        error = (_IKsensors[0].grayScale - _IKsensors[1].grayScale);
        steer += error;
        steer = steer < -0.1f ? -0.1f : 0.1f;

        powerL = (maxMotorTorque - (proportionalGain * error + derivativeGain * (error - errorOld) + integralGain * steer));
        powerR = (maxMotorTorque + (proportionalGain * error + derivativeGain * (error - errorOld) + integralGain * steer));

        errorOld = error;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * rightWheelsCollider[0].radius * rightWheelsCollider[0].rpm * 60 / 1000;

        if (currentSpeed < maxSpeed)
        {
            Debug.Log(Time.deltaTime);
            foreach (WheelCollider wheel in rightWheelsCollider)
            {
                wheel.motorTorque = powerR;
            }
            foreach (WheelCollider wheel in leftWheelsCollider)
            {
                wheel.motorTorque = powerL;
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