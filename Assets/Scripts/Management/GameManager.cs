using DungeonGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay;
    /*[HideInInspector] */public bool playersTurn = true;
    public static GameManager instance = null;

    public int stage = 0;

    [SerializeField] private GameObject boardManager;

    private float playerTimer = 0f;
    private float enemiesTimer = 0f;


    void Awake()
    {
        stage = 0;
        if(instance == null)
        {
             instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    void InitGame()
    {

    }

    public void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
        Timer();
        if (playersTurn) //���� ���(����)�� ���� ������ �ٲ�� ������ ���� ���Ͱ� �����Ƿ� �ٷ� �÷��̾� ������ �ٲ�
        {
            return;
        }
        else if(!playersTurn)
        {
            EnemiesTurn();
        }
    }

    private void Timer()
    {
        enemiesTimer += Time.deltaTime;
        playerTimer += Time.deltaTime;
    }
    private void EnemiesTurn()
    {
        Debug.Log("��� ��");
        if (enemiesTimer >= turnDelay)
        {
            MoveEnemies();
            PlayerTurn();
            enemiesTimer = 0f;
        }
    }
    private void PlayerTurn()
    {
        if (playerTimer >= turnDelay)
        {
            playersTurn = true;
            playerTimer = 0f;
        }
    }
    private void MoveEnemies()
    {
        Debug.Log("��� ������");
    }

}
