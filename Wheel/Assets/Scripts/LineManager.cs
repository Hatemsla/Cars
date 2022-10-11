using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineManager : MonoBehaviour
{
    public Transform platformCameraPosition;
    public Transform topCameraPosition;
    public Camera platformCamera;
    public Renderer mapMaterial;
    public Dropdown lineMaps;
    public PlatformMoveOnLine platform;

    [SerializeField] private LineLoad _lineLoad;

    private void Start()
    {
        platformCamera.gameObject.transform.position = topCameraPosition.transform.position;
        platformCamera.gameObject.transform.rotation = topCameraPosition.transform.rotation;

        _lineLoad.LoadLines();
        lineMaps.ClearOptions();
        for (int i = 0; i < _lineLoad.materials.Length; i++)
        {
            lineMaps.options.Add(new Dropdown.OptionData($"Карта №{i + 1}"));
        }
        lineMaps.value = 0;
        lineMaps.itemText = lineMaps.itemText;
    }

    public void ChangeLineMap(int value)
    {
        mapMaterial.material = _lineLoad.materials[value];
    }

    public void StartRide()
    {
        platformCamera.gameObject.transform.position = platformCameraPosition.transform.position;
        platformCamera.gameObject.transform.rotation = platformCameraPosition.transform.rotation;

        gameObject.SetActive(false);
        platformCamera.enabled = true;
        platform.isStart = true;
    }
}
