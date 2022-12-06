using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wheels : MonoBehaviour
{
    [SerializeField] private GameObject _robot;

    private float _timeStop;
    private float _speed;
    private float _currentTime;
    private WheelCollider _frontRightWheel;
    private WheelCollider _frontLeftWheel;
    private WheelCollider _rearRightWheel;
    private WheelCollider _rearLeftWheel;
    

    [Obsolete]
    public void moveRightWheels(object[] items)
    {
        // 0 - скорость, 1 - время, 2 - вперед/назад, 3 - левые/правые, 4 - платформа
        bool[] temp = (bool[])items[3];
        _speed = float.Parse(items[0].ToString());
        _timeStop = float.Parse(items[1].ToString());
        _currentTime = 0;
        string dir = items[2].ToString();
        if (temp[1])
        {
            _frontRightWheel = ((GameObject)items[4]).transform.Find("RightWheels(Clone)/FrontCol").GetComponent<WheelCollider>();
            _rearRightWheel = ((GameObject)items[4]).transform.Find("RightWheels(Clone)/BackCol").GetComponent<WheelCollider>();
            if (dir == "Forward")
            {
                StartCoroutine(MoveForward(_currentTime, _timeStop, _frontRightWheel, _rearRightWheel, ((GameObject)items[4]).GetComponent<Rigidbody>()));
            }
            else
            {
                StartCoroutine(MoveBackward(_currentTime, _timeStop, _frontRightWheel, _rearRightWheel, ((GameObject)items[4]).GetComponent<Rigidbody>()));
            }
        }
    }

    [Obsolete]
    public void moveLeftWheels(object[] items)
    {
        bool[] temp = (bool[])items[3];
        _speed = float.Parse(items[0].ToString());
        _timeStop = float.Parse(items[1].ToString());
        string dir = items[2].ToString();
        if (temp[0])
        {
            _frontLeftWheel = ((GameObject)items[4]).transform.Find("LeftWheels(Clone)/FrontCol").GetComponent<WheelCollider>();
            _rearLeftWheel = ((GameObject)items[4]).transform.Find("LeftWheels(Clone)/BackCol").GetComponent<WheelCollider>();
            if (dir == "Forward")
            {
                StartCoroutine(MoveForward(_currentTime, _timeStop, _frontLeftWheel, _rearLeftWheel, ((GameObject)items[4]).GetComponent<Rigidbody>()));
            }
            else
            {
                StartCoroutine(MoveBackward(_currentTime, _timeStop, _frontLeftWheel, _rearLeftWheel, ((GameObject)items[4]).GetComponent<Rigidbody>()));
            }
        }
    }
    private IEnumerator MoveForward(float currentTime, float timeStop, WheelCollider frontWheel, WheelCollider rearWheel, Rigidbody rb)
    {
        rb.drag = 0.05f;
        while (currentTime <= timeStop)
        {
            frontWheel.motorTorque = _speed * 10;
            rearWheel.motorTorque = _speed * 10;
            yield return new WaitForSeconds(Time.deltaTime);
            currentTime += Time.deltaTime;
            Debug.Log(currentTime);
        }
        frontWheel.motorTorque = 0;
        rearWheel.motorTorque = 0;
        rb.drag = 20;
    }
    private IEnumerator MoveBackward(float currentTime, float timeStop, WheelCollider frontWheel, WheelCollider rearWheel, Rigidbody rb)
    {
        rb.drag = 0.05f;
        while (currentTime <= timeStop)
        {
            frontWheel.motorTorque = -_speed * 10;
            rearWheel.motorTorque = -_speed * 10;
            yield return new WaitForSeconds(Time.deltaTime);
            currentTime += Time.deltaTime;
            Debug.Log(currentTime);
        }
        frontWheel.motorTorque = 0;
        rearWheel.motorTorque = 0;
        rb.drag = 20;
    }
}
