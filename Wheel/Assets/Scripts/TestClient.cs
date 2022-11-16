using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Assets.Scripts;

public class TestClient : MonoBehaviour
{
    public string message;

    WebSocket ws;
    private bool _isOpen;
    public bool _isMessage;

    private void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log($"Message received from {((WebSocket)sender).Url}, Data: {e.Data}");
            message = e.Data;
            _isMessage = true;
        };

        ws.OnOpen += (sender, e) =>
        {
            _isOpen = true;
        };

        ws.OnClose += (sender, e) =>
        {
            _isOpen = false;
        };

        ws.Connect();
    }

    private void Update()
    {
        if (_isOpen)
        {
            ws.Send($"{transform.position.x} {transform.position.y} {transform.position.z}");
        }
    }

    private void FixedUpdate()
    {
        if (_isMessage)
        {
            _isMessage = false;
        }
    }

    public void Some(string data)
    {
        MessageReciver m = JsonConvert.DeserializeObject<MessageReciver>(data);

    }

    //public IEnumerator Move(string data)
    //{
    //    MessageReciver m = JsonConvert.DeserializeObject<MessageReciver>(data);

    //    float offset = 0;
    //    Vector3 originPosition = transform.TransformDirection(transform.position);
    //    Vector3 newPosition = new Vector3(m.P, m.I, m.D);
    //    while (transform.position != newPosition)
    //    {
    //        transform.position = Vector3.Lerp(originPosition, newPosition, offset);
    //        offset += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
