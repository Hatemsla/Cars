using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IK : MonoBehaviour
{
    public float speed = 10f;
    public GameObject obj;
    public Image image;
    public Text shade;
    public Button leftButton;
    public Button rightButton;

    private Vector3 _ikMoveDirection;
    private bool _isLeftButtonDown;
    private bool _isRightButtonDown;
    private const float maxLeftPosition = -2;
    private const float maxRightPosition = 5;

    private void FixedUpdate() 
    {
        RaycastHit hit;
        Physics.Raycast(obj.transform.position, obj.transform.forward, out hit, 10000.0f);
        Color color = Color.white;
        if (hit.collider)
        {
            Texture2D texture = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
            color = texture.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y);
            Debug.DrawLine(obj.transform.position, hit.point);
        }
        image.color = color;
        shade.text = Convert(color.grayscale, 0, 1, 4095, 0).ToString();
        CheckMove();
    }

    public float Convert(float value, float From1, float From2, float To1, float To2)
    {
        return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
    }

    private void IKMove()
    {
        transform.Translate(_ikMoveDirection * speed * Time.deltaTime);
    }

    public void RightButtonDown()
    {
        _ikMoveDirection = Vector3.forward;
        _isLeftButtonDown = false;
        _isRightButtonDown = true;
    }

    public void LeftButtonDown()
    {
        _ikMoveDirection = Vector3.back;
        _isLeftButtonDown = true;
        _isRightButtonDown = false;
    }

    public void ButtonUp()
    {
        _ikMoveDirection = Vector3.zero;
    }

    private void CheckMove()
    {
        if (transform.position.x < maxLeftPosition)
        {
            if (_isRightButtonDown)
            {
                IKMove();
            }
        }
        else if (transform.position.x >= maxRightPosition)
        {
            if (_isLeftButtonDown)
            {
                IKMove();
            }
        }
        else
        {
            IKMove();
        }
    }
}
