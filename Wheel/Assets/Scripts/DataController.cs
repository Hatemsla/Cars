using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using WebSocketSharp;
using Assets.Scripts;

public class DataController : MonoBehaviour
{
    public PlatformMoveOnLine PlatformMoveOnLine;
    public PlatformMoveInMaze PlatformMoveInMaze;

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
            MessageSender sender = new MessageSender(PlatformMoveOnLine.IKsensors[0].grayScale, PlatformMoveOnLine.IKsensors[1].grayScale,
                PlatformMoveInMaze.UZLeft.distance, PlatformMoveInMaze.UZSideLeft.distance, PlatformMoveInMaze.UZForward.distance,
                PlatformMoveInMaze.UZSideRight.distance, PlatformMoveInMaze.UZRight.distance, PlatformMoveInMaze.isStart);

            string jsonSender = JsonConvert.SerializeObject(sender);

            _ws.Send(jsonSender);
        }

        if(_isMessage)
        {
            _isMessage = false;
            _messageReciver = JsonConvert.DeserializeObject<MessageReciver>(_message);
            PlatformMoveOnLine.powerL = (float)_messageReciver.PowerL;
            PlatformMoveOnLine.powerR = (float)_messageReciver.PowerR;
            PlatformMoveInMaze.powerL = (float)_messageReciver.PowerL;
            PlatformMoveInMaze.powerR = (float)_messageReciver.PowerR;
            PlatformMoveInMaze.isBrake = _messageReciver.IsBrake;
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
