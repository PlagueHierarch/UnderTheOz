using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject boardManager;

    void Awake()
    {
        //if (GameManager.instance == null)
        //{
        //    //Instantiate(gameManager);
        //}
        if (DungeonGenerator.DungeonGenerator.dungeonInstance == null)
        {
            Instantiate(boardManager);
        }
    }

}
