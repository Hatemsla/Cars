using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
    public Text revs;
    public Text distanceText;
    public InputField inputSpeed;
    public InputField inputTime;
    public GameObject panelSettings;
    public GameObject startBtn;
    public GameObject stopBtn;
    public GameObject openBtn;
    public Transform disk;
    public Transform rubber;

    private const float radius = 0.3f;
    private float _timeCurrent;
    private float _timeStop;
    private float _speed;
    private float _revsInSec; //обороты в секунду
    private float _wheelLength;
    private float _distance; //растояние которое проходит колесо
    private bool _moveWheelState = false;

    private void Start()
    {
        _wheelLength = 2 * Mathf.PI * radius;
        _timeCurrent = 0;
        _timeStop = 0;
    }

    private void FixedUpdate()
    {
        if (_moveWheelState)
        {
            MoveWheel();
        }
    }

    public void StartButton()
    {

        _timeCurrent = 0;
        _distance = 0;


        try
        {
            _timeStop = Convert.ToSingle(inputTime.text);
            _speed = Convert.ToSingle(inputSpeed.text);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

        startBtn.SetActive(false);
        stopBtn.SetActive(true);

        _moveWheelState = true;
    }

    private void MoveWheel() 
    {
        if (_timeCurrent <= _timeStop)
        {
            _distance += _speed * Time.deltaTime;
            _revsInSec = _distance / _wheelLength;
            distanceText.text = $"Дистанция: {_distance:f2} м";
            revs.text = $"Обороты: {_revsInSec:f2}";
            disk.Rotate(Vector3.right * _speed * Time.deltaTime * 10f);
            rubber.Rotate(Vector3.right * _speed * Time.deltaTime * 10f);
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        _timeCurrent += Time.deltaTime;
    }

    public void StopButton()
    {
        _moveWheelState = false;
        stopBtn.SetActive(false);
        startBtn.SetActive(true);
    }

    public void CloseButton()
    {
        panelSettings.SetActive(false);
        openBtn.SetActive(true);
    }

    public void OpenButton()
    {
        panelSettings.SetActive(true);
        openBtn.SetActive(false);
    }
}
