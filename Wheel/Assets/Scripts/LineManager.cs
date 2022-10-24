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
    public InputField PInput;
    public InputField IInput;
    public InputField DInput;
    public InputField PowerInput;
    public Text PowerLText;
    public Text PowerRText;
    public Text ErrorText;
    public Text PText;
    public Text IText;
    public Text DText;
    public PlatformMoveOnLine platform;
    public GameObject inputPanel;
    public GameObject outputPanel;

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

    private void Update()
    {
        PowerLText.text = platform.powerL.ToString();
        PowerRText.text = platform.powerR.ToString();
        ErrorText.text = platform.error.ToString();
        PText.text = platform.proportionalGain.ToString();
        IText.text = platform.integralGain.ToString();
        DText.text = platform.derivativeGain.ToString();
    }

    public void ChangeLineMap(int value)
    {
        mapMaterial.material = _lineLoad.materials[value];
    }

    public void StartRide()
    {
        platformCamera.gameObject.transform.position = platformCameraPosition.transform.position;
        platformCamera.gameObject.transform.rotation = platformCameraPosition.transform.rotation;

        inputPanel.SetActive(false);
        outputPanel.SetActive(true);
        platformCamera.enabled = true;

        float.TryParse(PInput.text, out platform.proportionalGain);
        float.TryParse(IInput.text, out platform.integralGain);
        float.TryParse(DInput.text, out platform.derivativeGain);
        float.TryParse(PowerInput.text, out platform.maxMotorTorque);

        platform.isBrake = false;
    }

    public void StopRide() 
    {
        inputPanel.SetActive(true);
        outputPanel.SetActive(false);

        platform.isBrake = true;
    }
}
