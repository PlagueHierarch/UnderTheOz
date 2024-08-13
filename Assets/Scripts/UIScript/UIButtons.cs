using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public GameObject SettingScreen;
    public Vector2 instPos;

    public void SettingOn()
    {
        SettingScreen.SetActive(true);
    }

    public void SettingOff()
    {
        SettingScreen.SetActive(false);
    }
}
