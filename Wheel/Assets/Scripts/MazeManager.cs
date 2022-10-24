using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    public InputField PInput;
    public InputField IInput;
    public InputField DInput;
    public InputField SpeedInput;
    public Text PowerLText;
    public Text PowerRText;
    public Text ErrorText;
    public Text PText;
    public Text IText;
    public Text DText;
    public PlatformMoveInMaze platform;
    public GameObject inputPanel;
    public GameObject outputPanel;

    private void Start()
    {
        platform.isBrake = true;
        PInput.text = 60.ToString();
        IInput.text = 50.ToString();
        DInput.text = 50.ToString();
        SpeedInput.text = 300.ToString();
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

    public void StartRide()
    {
        inputPanel.SetActive(false);
        outputPanel.SetActive(true);

        float.TryParse(PInput.text, out platform.proportionalGain);
        float.TryParse(IInput.text, out platform.integralGain);
        float.TryParse(DInput.text, out platform.derivativeGain);
        float.TryParse(SpeedInput.text, out platform.maxMotorTorque);

        platform.isBrake = false;
    }

    public void StopRide()
    {
        inputPanel.SetActive(true);
        outputPanel.SetActive(false);

        platform.isBrake = true;
    }
}
