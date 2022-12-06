using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UZ : MonoBehaviour
{
    public float speed = 10f;
    public float angle = 30f;
    public int rayCount = 100;
    public Button leftBtn;
    public Button rightBtn;
    public Button upBtn;
    public Button downBtn;

    [SerializeField] private Text _distanceText;

    private Vector3 _direction;

    private void Update()
    {
        AngleCalc(angle, rayCount);
        UZMove();
    }

    public void AngleCalc(float angle, int raysCount)
    {
        var rayOffset = angle / raysCount;
        int catchRaysCount = 0;
        float distance = 0;
        RaycastHit hit;

        for (var i = -angle * 0.5f; i <= angle * 0.5f; i += rayOffset)
        {
            var direction = transform.TransformDirection(Quaternion.AngleAxis(i, transform.up) * transform.forward);
            if (Physics.Raycast(transform.position, direction, out hit, 40f, -1, QueryTriggerInteraction.Ignore))
            {
                distance += hit.distance;
                catchRaysCount++;
                Debug.DrawLine(transform.position, transform.position + direction * hit.distance, Color.green);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction * 40, Color.white);
            }
        }

        if (catchRaysCount > 0)
            distance /= catchRaysCount;

        _distanceText.text = distance.ToString();
    }

    private void UZMove()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    public void ForwardMove()
    {
        _direction = Vector3.back;
    }

    public void BackMove()
    {
        _direction = Vector3.forward;
    }

    public void LeftMove()
    {
        _direction = Vector3.right;
    }

    public void RightMove()
    {
        _direction = Vector3.left;
    }

    public void ButtonUp()
    {
        _direction = Vector3.zero;
    }
}
