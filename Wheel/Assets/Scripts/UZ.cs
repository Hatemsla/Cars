using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UZ : MonoBehaviour
{
    public float speed = 10f;
    public int rayCount = 100;
    public Button leftBtn;
    public Button rightBtn;
    public Button upBtn;
    public Button downBtn;

    [SerializeField] private Text distanceText;

    private Vector3 _direction;

    private void FixedUpdate()
    {
        AngleCalc(gameObject, rayCount, 75, 105);
        UZMove();
    }
    public Vector3 AngleCalc(GameObject start, int rays, float minAngle, float maxAngle)
    {
        float angleZone = 180f - minAngle - (180f - maxAngle);
        float offset = angleZone / rays;
        int count = 0;
        float averageDistance = 0;

        for (float i = minAngle; i <= maxAngle; i += offset)
        {
            Vector3 noAngle = start.transform.right;
            Quaternion spreadAngle = Quaternion.AngleAxis(i, new Vector3(0, 1, 0));
            Vector3 newVector = spreadAngle * noAngle;

            Ray ray = new Ray(start.transform.position, newVector);

            RaycastHit raycastHit = new RaycastHit();
            if (Physics.Raycast(start.transform.position, ray.direction, out raycastHit, 1000f))
            {
                Debug.DrawRay(start.transform.position, start.transform.position + ray.direction * 100, Color.green, 0.0f, false);
                count++;
                averageDistance += raycastHit.distance;
            }
            else
            {
                Debug.DrawRay(start.transform.position, start.transform.position + ray.direction * 100, Color.white, 0.0f, false);
            }
        }
        if (count != 0)
        {
            averageDistance /= count;
        }
        distanceText.text = averageDistance + "";
        return start.transform.position + start.transform.forward * 100;
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
