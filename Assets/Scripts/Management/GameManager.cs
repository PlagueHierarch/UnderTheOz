using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 1f;
    public static GameManager instance = null;
    public BoardManager boardScript;

    private int level = 3;

    [HideInInspector] public bool playersTurn = true;
    void Awake()
    {
        if(instance == null)
        {
             instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScence(level); 
    }

    public void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
        if(playersTurn) //���� ���(����)�� ���� ������ �ٲ�� ������ ���� ���Ͱ� �����Ƿ� �ٷ� �÷��̾� ������ �ٲ�
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        yield return new WaitForSeconds(turnDelay);
        playersTurn = true; 
    }
}
