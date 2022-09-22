using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //Scenes PlatformSettings - 0, Platform - 1
    public void PlatformSettings()
    {
        SceneManager.LoadScene(0);
    }
    public void Platform()
    {
        SceneManager.LoadScene(1);
    }
}
