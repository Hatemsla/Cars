using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using WebSocketSharp;
using Assets.Scripts;

public class DataController : MonoBehaviour
{
    public PlatformMoveOnLine platformMoveOnLine;
    public PlatformMoveInMaze platformMoveInMaze;

    private WebSocket _ws;
    private MessageReciver _messageReciver;
    private string _message;
    private bool _isOpen;
    private bool _isMessage;

    private void Start()
    {
        _ws = new WebSocket("ws://localhost:8080");
        _ws.OnMessage += (sender, e) =>
        {
            _message = e.Data;
            _isMessage = true;
        };

        _ws.OnOpen += (sender, e) =>
        {
            _isOpen = true;
        };

        _ws.OnClose += (sender, e) =>
        {
            _isOpen = false;
        };

        _ws.Connect();
    }

    private void FixedUpdate()
    {
        if (_isOpen)
        {
            MessageSender sender = new MessageSender(platformMoveOnLine.IKsensors[0].grayScale, platformMoveOnLine.IKsensors[1].grayScale,
                platformMoveInMaze.uzLeft.distance, platformMoveInMaze.uzSideLeft.distance, platformMoveInMaze.uzForward.distance,
                platformMoveInMaze.uzSideRight.distance, platformMoveInMaze.uzRight.distance, platformMoveInMaze.isStart);

            string jsonSender = JsonConvert.SerializeObject(sender);

            _ws.Send(jsonSender);
        }

        if(_isMessage)
        {
            _isMessage = false;
            _messageReciver = JsonConvert.DeserializeObject<MessageReciver>(_message);
            platformMoveOnLine.powerL = (float)_messageReciver.PowerL;
            platformMoveOnLine.powerR = (float)_messageReciver.PowerR;
            platformMoveInMaze.powerL = (float)_messageReciver.PowerL;
            platformMoveInMaze.powerR = (float)_messageReciver.PowerR;
            platformMoveInMaze.isBrake = _messageReciver.IsBrake;
            if (_messageReciver.IsBrake)
                Debug.Log(_messageReciver.IsBrake);

            //PlatformMoveOnLine.powerL = float.Parse(_message.Split()[0].Replace('.', ','));
            //PlatformMoveOnLine.powerR = float.Parse(_message.Split()[1].Replace('.', ','));
            //PlatformMoveInMaze.powerL = float.Parse(_message.Split()[0].Replace('.', ','));
            //PlatformMoveInMaze.powerR = float.Parse(_message.Split()[1].Replace('.', ','));
            //PlatformMoveInMaze.isBrake = bool.Parse(_message.Split()[2]);
        }
    }
}
