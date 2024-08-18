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
        GameManager.instance.stage = 1;
        GameManager.Sound.Play("Sounds/Bgm001", "Bgm");
        SceneManager.LoadScene(sceneName);
    }

    public void OffGame()
    {
        Application.Quit();
    }
}
