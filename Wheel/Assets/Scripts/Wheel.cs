using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Text Revs;
    [SerializeField] private Text Distance;
    [SerializeField] private InputField InputSpeed;
    [SerializeField] private InputField InputTime;
    [SerializeField] private GameObject PanelSettings;
    [SerializeField] private GameObject StartBtn;
    [SerializeField] private GameObject StopBtn;
    [SerializeField] private GameObject OpenBtn;
    [SerializeField] private Transform Disk;
    [SerializeField] private Transform Rubber;

    private float _radius = 0.3f;
    private float _timeCurrent;
    private float _timeStop;
    private float _speed;
    private float _revsInSec; //обороты в секунду
    private float _wheelLength;
    private float _distance; //растояние которое проходит колесо
    private bool _moveWheelState = false;

    private void Start()
    {
        _wheelLength = 2 * Mathf.PI * _radius;
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
            _timeStop = Convert.ToSingle(InputTime.text);
            _speed = Convert.ToSingle(InputSpeed.text);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

        StartBtn.SetActive(false);
        StopBtn.SetActive(true);

        _moveWheelState = true;
    }

    private void MoveWheel() 
    {
        if (_timeCurrent <= _timeStop)
        {
            _distance += _speed * Time.deltaTime;
            _revsInSec = _distance / _wheelLength;
            Distance.text = $"Дистанция: {_distance:f2} м";
            Revs.text = $"Обороты: {_revsInSec:f2}";
            Disk.Rotate(Vector3.right * _speed * Time.deltaTime * 10f);
            Rubber.Rotate(Vector3.right * _speed * Time.deltaTime * 10f);
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        _timeCurrent += Time.deltaTime;
    }

    public void StopButton()
    {
        _moveWheelState = false;
        StopBtn.SetActive(false);
        StartBtn.SetActive(true);
    }

    public void CloseButton()
    {
        PanelSettings.SetActive(false);
        OpenBtn.SetActive(true);
    }

    public void OpenButton()
    {
        PanelSettings.SetActive(true);
        OpenBtn.SetActive(false);
    }
}
