using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsController : MonoBehaviour
{
    public Detail[] details;

    private void Start()
    {
        details = GetComponentsInChildren<Detail>();
    }

    public void Expand()
    {
        for(int i = 0; i < details.Length; i++)
        {
            StartCoroutine(details[i].MoveFromBeginToEnd());
        }
    }

    public void Compress()
    {
        for (int i = 0; i < details.Length; i++)
        {
            StartCoroutine(details[i].MoveFromEndToBegin());
        }
    }
}
