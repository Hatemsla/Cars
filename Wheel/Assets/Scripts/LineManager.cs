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
    public InputField proportionalInput;
    public InputField integralInput;
    public InputField derivativeInput;
    public InputField powerInput;
    public Text powerLText;
    public Text powerRText;
    public Text errorText;
    public Text proportionalText;
    public Text integralText;
    public Text derivativeText;
    public PlatformMoveOnLine platform;
    public GameObject inputPanel;
    public GameObject outputPanel;

    [SerializeField] private LineLoad _lineLoad;

    private void Start()
    {
        platformCamera.gameObject.transform.parent = topCameraPosition.parent;
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

    private void Update()
    {
        powerLText.text = platform.powerL.ToString();
        powerRText.text = platform.powerR.ToString();
        errorText.text = platform.error.ToString();
        proportionalText.text = platform.proportionalGain.ToString();
        integralText.text = platform.integralGain.ToString();
        derivativeText.text = platform.derivativeGain.ToString();
    }

    public void ChangeLineMap(int value)
    {
        mapMaterial.material = _lineLoad.materials[value];
    }

    public void StartRide()
    {
        platformCamera.gameObject.transform.parent = platformCameraPosition.parent;
        platformCamera.gameObject.transform.position = platformCameraPosition.transform.position;
        platformCamera.gameObject.transform.rotation = platformCameraPosition.transform.rotation;

        inputPanel.SetActive(false);
        outputPanel.SetActive(true);
        platformCamera.enabled = true;

        float.TryParse(proportionalInput.text, out platform.proportionalGain);
        float.TryParse(integralInput.text, out platform.integralGain);
        float.TryParse(derivativeInput.text, out platform.derivativeGain);
        float.TryParse(powerInput.text, out platform.maxMotorTorque);

        platform.maxMotorTorque *= 100;

        platform.isBrake = false;
    }

    public void StopRide() 
    {
        inputPanel.SetActive(true);
        outputPanel.SetActive(false);

        platform.isBrake = true;
    }
}
