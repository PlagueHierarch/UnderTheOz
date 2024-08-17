using DungeonGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    public static GameManager s_instance;
    public static GameManager instance { get { Init(); return s_instance; } }

    public SoundManager _sound = new SoundManager();
    public ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return instance._resource; } }
    public static SoundManager Sound { get { return instance._sound; } }


    public float turnDelay;
    /*[HideInInspector] */public bool playersTurn = false;
    public bool enemysTurn = false;
    private bool playerTurnEnd = false;
    private bool enemyTurnEnd = false;

    public List<GameObject> monsters;
    public List<GameObject> monstersEnd;

    public int stage = 0;

    [SerializeField] private GameObject boardManager;
    [SerializeField] GameObject turnIndicator;

    private float playerTimer = 0f;
    private float enemiesTimer = 0f;


    void Awake()
    {
        stage = 0;
    }
    private void Start()
    {
        StartCoroutine(PlayerTurn());
        Debug.Log("C" + monsters.Count);
        //turnIndicator = GameObject.Find("turnIndicator");
        Init();
    }
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject obj = Resources.Load<GameObject>($"Prefab/GameManager"); 
            if (obj == null)
            {
                obj = new GameObject { name = "GameManager" };
                obj.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(obj);
            s_instance = obj.GetComponent<GameManager>();

            s_instance._sound.Init();
        }
    }


    public void GameOver()
    {
        enabled = false;
    }

    void Update()
    {
        Timer();
        //Debug.Log("C" + monsters.Count);
        //Debug.Log("AC" + monstersEnd.Count);
        if (monsters.Count == monstersEnd.Count && enemysTurn && enemyTurnEnd)
        {
            turnIndicator.SetActive(false);
            Debug.Log("플레이어 턴");
            StartCoroutine(PlayerTurn());
        }
        else if (playerTurnEnd && playersTurn) 
        {
            turnIndicator.SetActive(true);
            //Debug.Log(GameManager.instance.playersTurn);
            Debug.Log("상대 턴");
            StartCoroutine(EnemiesTurn());
            
        }
    }

    private void Timer()
    {
        enemiesTimer += Time.deltaTime;
        playerTimer += Time.deltaTime;
    }
    private IEnumerator EnemiesTurn()
    {
        enemysTurn = true;
        playersTurn = false;
        playerTurnEnd = false;
        /*if (enemiesTimer >= turnDelay)
        {
            Debug.Log(enemiesTimer);
            Debug.Log("상대 턴");
            enemyTurnEnd = true;
            enemiesTimer = 0f;
        }*/
        yield return new WaitForSeconds(turnDelay);
        enemyTurnEnd = true;
    }
    private IEnumerator PlayerTurn()
    {
        enemysTurn = false;
        enemyTurnEnd = false;
        monstersEnd.Clear();
        yield return new WaitForSeconds(turnDelay);
        /*if (playerTimer >= turnDelay)
        {
            Debug.Log(playerTimer);
            playerTurnEnd = true;
            Debug.Log("플레이어 턴");
            playerTimer = 0f;
        }*/
        yield return new WaitUntil(() => playersTurn == true);
        playerTurnEnd = true;
        
    }
    private void MoveEnemies()
    {
        //Debug.Log("상대 움직임");
    }

}
