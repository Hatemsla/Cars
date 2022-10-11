using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLoad : MonoBehaviour
{
    public Material[] materials;

    public void LoadLines()
    {
        materials = Resources.LoadAll<Material>("LineMaterials");
    }
}
