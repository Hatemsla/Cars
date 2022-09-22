using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerPlatform : MonoBehaviour
{
    //public SensorsController sens;
    //public Transform UZPoint;
    //public Transform IKPoint;
    //public Transform CameraPoint;
    //public GameObject UZPrefab;
    //public GameObject IKPrefab;
    //public GameObject CameraPrefab;
    //GameObject spawnUZ;
    //GameObject spawnIK;
    //GameObject spawnCamera;
    //public GameObject SwitchCameraButton;
    //public GameObject CreateUZSens;
    //public GameObject DestroyUZSens;
    //public GameObject CreateIKSens;
    //public GameObject DestroyIKSens;
    //public GameObject CreateCam;
    //public GameObject DestroyCam;
    //public Text CountUZ;
    //public Text CountIK;
    //public Text CountCam;
    
    //public void CreateUZ()
    //{
    //    sens.AddUZ(UZPrefab, UZPoint);
    //    //CreateUZSens.SetActive(false);
    //    //DestroyUZSens.SetActive(true);
    //    CountUZ.text = sens.currentUZ.ToString();
    //}
    //public void DestroyUZ()
    //{
    //    sens.RemoveUZ(spawnUZ);
    //    //DestroyUZSens.SetActive(false);
    //    CreateUZSens.SetActive(true);
    //}
    //public void CreateIK()
    //{
    //    sens.AddIK(IKPrefab, IKPoint);
    //    //CreateIKSens.SetActive(false);
    //    //DestroyIKSens.SetActive(true);
    //    CountIK.text = sens.currentIK.ToString();
    //}
    //public void DestroyIK()
    //{
    //    sens.RemoveIK(spawnIK);
    //    DestroyIKSens.SetActive(false);
    //    CreateIKSens.SetActive(true);
    //}
    //public void CreateCamera()
    //{
    //    sens.AddCamera(CameraPrefab, CameraPoint);
    //    //SwitchCameraButton.SetActive(true);
    //    //CreateCam.SetActive(false);
    //    //DestroyCam.SetActive(true);
    //    CountCam.text = sens.currentCam.ToString();
    //}
    //public void DestroyCamera()
    //{
    //    sens.RemoveCamera(spawnCamera);
    //    SwitchCameraButton.SetActive(false);
    //    CreateCam.SetActive(true);
    //    //DestroyCam.SetActive(false);
    //}
    //public void SwitchCameras()
    //{
    //    sens.Cam.enabled = !sens.Cam.enabled;
    //    sens.mainCamera.enabled = !sens.mainCamera.enabled;
    //}
    [SerializeField] private InputField speedField;
    [SerializeField] private InputField timeField;
    [SerializeField] private ToggleGroup toggleGroupRL;
    [SerializeField] private ToggleGroup toggleGroupFB;
    [HideInInspector] public Toggle toggleRL;
    [HideInInspector] public Toggle toggleFB;
    //public void Submit()
    //{
    //    toggleRL = toggleGroupRL.ActiveToggles().FirstOrDefault();
    //    toggleFB = toggleGroupFB.ActiveToggles().FirstOrDefault();
    //    Debug.Log(toggleRL.GetComponentInChildren<Text>().text + toggleFB.GetComponentInChildren<Text>().text);
    //}
}
