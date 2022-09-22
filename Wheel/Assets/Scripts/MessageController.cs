using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public Wheels prefabRightWheels;
    public Wheels prefabLeftWheels;
    public GameObject settings;
    public GameObject settingsPanel;
    public GameObject canvas;
    public GameObject platform;

    [SerializeField] private InputField speedField;
    [SerializeField] private InputField timeField;
    [SerializeField] private ToggleGroup toggleGroupRL;
    [SerializeField] private ToggleGroup toggleGroupFB;
    [SerializeField] private Button addPanelButton;
    [SerializeField] private Toggle[] toggleRL;
    [HideInInspector] public Toggle toggleFB;

    private Transform _frontRightTransform;
    private Transform _frontLeftTransform;
    private Transform _rearRightTransfrom;
    private Transform _rearLeftTransform;
    private float _posSettingsPanelTop;
    private float _posSettingsPanelBottom;
    private float _offsetPanelTop = 320;
    private Wheels _wheels;

    [System.Obsolete]
    private void Start()
    {
        settings.SetActive(false);
        _posSettingsPanelTop = settingsPanel.transform.position.y; // запоминаие позиции панели
        _posSettingsPanelBottom = settingsPanel.transform.position.x;

        prefabRightWheels = Instantiate(prefabRightWheels);
        prefabRightWheels.transform.SetParent(platform.transform);
        prefabRightWheels.transform.localPosition = new Vector3(6.7f, 3, -0.5f); // расположение правых колес относительно робота

        prefabLeftWheels = Instantiate(prefabLeftWheels);
        prefabLeftWheels.transform.SetParent(platform.transform);
        prefabLeftWheels.transform.localPosition = new Vector3(-6, 3, -0.7f); // расположение левых колес относительно робота

        _frontRightTransform = platform.transform.FindChild("RightWheels(Clone)").FindChild("FrontCol").GetComponent<Transform>();
        _frontLeftTransform = platform.transform.FindChild("RightWheels(Clone)").FindChild("BackCol").GetComponent<Transform>();
        _frontRightTransform = platform.transform.FindChild("LeftWheels(Clone)").FindChild("FrontCol").GetComponent<Transform>();
        _rearLeftTransform = platform.transform.FindChild("LeftWheels(Clone)").FindChild("BackCol").GetComponent<Transform>();
    }

    public void Submit()
    {
        var RightWheels = prefabRightWheels.GetComponent(typeof(Wheels));
        var LeftWheels = prefabLeftWheels.GetComponent(typeof(Wheels));
        var toggleRLIsOn = new bool[toggleRL.Length];
        toggleFB = toggleGroupFB.ActiveToggles().FirstOrDefault();
        for(int i = 0; i < toggleRL.Length; i++)
        {
            toggleRLIsOn[i] = toggleRL[i].isOn;
        }
        RightWheels.SendMessage("moveRightWheels", new object[] { speedField.text, timeField.text, toggleFB.GetComponentInChildren<Text>().text, toggleRLIsOn, platform }, SendMessageOptions.DontRequireReceiver);
        LeftWheels.SendMessage("moveLeftWheels", new object[] { speedField.text, timeField.text, toggleFB.GetComponentInChildren<Text>().text, toggleRLIsOn, platform }, SendMessageOptions.DontRequireReceiver);
        //StartCoroutine(whs.moveRightWheels(new object[] { speedField.text, timeField.text, toggleFB.GetComponentInChildren<Text>().text, toggleRLIsOn, platform }));
    }

    [System.Obsolete]
    public void CreatePanel()
    {
        settingsPanel = Instantiate(settingsPanel, new Vector3(_posSettingsPanelBottom, _posSettingsPanelTop - _offsetPanelTop, 0), Quaternion.identity, canvas.transform);
        settingsPanel.transform.FindChild("Settings").gameObject.SetActive(false);
        settings = settingsPanel.transform.FindChild("Settings").gameObject;
        _posSettingsPanelTop = settingsPanel.transform.position.y;
        _posSettingsPanelBottom = settingsPanel.transform.position.x;
    }

    public void AddSettingsPanel()
    {
        settings.SetActive(true);
    }
}

