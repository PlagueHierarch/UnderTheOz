using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject boardManager;
    [SerializeField] private GameObject _soundManager;

    private static Loader instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (DungeonGenerator.DungeonGenerator.dungeonInstance == null)
        {
            Instantiate(boardManager);
        }
        //if (SoundManager.soundInstance == null)
        //{
        //    Instantiate(_soundManager);
        //}
    }

}
