using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IK : MonoBehaviour
{
    public float speed = 10f;
    public GameObject obj;
    public Image Image;
    public Text Shade;
    public Button leftButton;
    public Button rightButton;

    private Vector3 _IKMoveDirection;
    private bool _isLeftButtonDown;
    private bool _isRightButtonDown;
    private const float MAX_LEFT_POSITION = -2;
    private const float MAX_RIGHT_POSITION = 5;

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
        CheckMove();
        Image.color = color;
        Shade.text = Convert(color.grayscale, 0, 1, 4095, 0).ToString();
    }

    public float Convert(float value, float From1, float From2, float To1, float To2)
    {
        return (value - From1) / (From2 - From1) * (To2 - To1) + To1;
    }

    private void IKMove()
    {
        transform.Translate(_IKMoveDirection * speed * Time.deltaTime);
    }

    public void RightButtonDown()
    {
        _IKMoveDirection = Vector3.forward;
        _isLeftButtonDown = false;
        _isRightButtonDown = true;
    }

    public void LeftButtonDown()
    {
        _IKMoveDirection = Vector3.back;
        _isLeftButtonDown = true;
        _isRightButtonDown = false;
    }

    public void ButtonUp()
    {
        _IKMoveDirection = Vector3.zero;
    }

    private void CheckMove()
    {
        if (transform.position.x < MAX_LEFT_POSITION)
        {
            if (_isRightButtonDown)
            {
                IKMove();
            }
        }
        else if (transform.position.x >= MAX_RIGHT_POSITION)
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
