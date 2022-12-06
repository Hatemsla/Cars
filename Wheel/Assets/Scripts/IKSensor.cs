using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IKSensor : MonoBehaviour
{
    public float grayScale; 

    private RaycastHit _hit;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, 100f))
        {
            Texture2D texture = (Texture2D)_hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
            Color color = texture.GetPixelBilinear(_hit.textureCoord2.x, _hit.textureCoord2.y);
            grayScale = Convert(color.grayscale, 1, 0, 0, 4095);
        }
    }

    public float Convert(float value, float from1, float from2, float to1, float to2)
    {
        return (value - from1) / (from2 - from1) * (to2 - to1) + to1;
    }
}
