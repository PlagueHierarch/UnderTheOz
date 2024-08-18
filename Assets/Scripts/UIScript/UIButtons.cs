using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public GameObject SettingScreen;
    public Vector2 instPos;
    [SerializeField] private string sceneName;

    public void SettingOn()
    {
        SettingScreen.SetActive(true);
    }

    public void SettingOff()
    {
        SettingScreen.SetActive(false);
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OffGame()
    {
        Application.Quit();
    }
}
