using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    public InputField proportionalInput;
    public InputField integralInput;
    public InputField derivativeInput;
    public InputField speedInput;
    public Text powerLText;
    public Text powerRText;
    public Text errorText;
    public Text proportionalText;
    public Text integralText;
    public Text derivativeText;
    public PlatformMoveInMaze platform;
    public GameObject inputPanel;
    public GameObject outputPanel;

    private void Start()
    {
        platform.isBrake = true;
        proportionalInput.text = 60.ToString();
        integralInput.text = 50.ToString();
        derivativeInput.text = 50.ToString();
        speedInput.text = 300.ToString();
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

    public void StartRide()
    {
        inputPanel.SetActive(false);
        outputPanel.SetActive(true);

        float.TryParse(proportionalInput.text, out platform.proportionalGain);
        float.TryParse(integralInput.text, out platform.integralGain);
        float.TryParse(derivativeInput.text, out platform.derivativeGain);
        float.TryParse(speedInput.text, out platform.maxMotorTorque);

        platform.isStart = true;
        platform.isBrake = false;
    }

    public void StopRide()
    {
        inputPanel.SetActive(true);
        outputPanel.SetActive(false);

        platform.isBrake = true;
    }
}
