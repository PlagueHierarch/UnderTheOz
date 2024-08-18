using DungeonGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private GameObject _boomEffect;

    public LayerMask blockingLayer;
    private GameManager _gameManager;
    private DungeonGenerator.DungeonGenerator _dungeonGenerator;
    private BoardManager _boardManager;
    private SpriteRenderer _boomSprite;

    public PlayerSkills instance;

    private GameObject target;

    [SerializeField] private List<TMP_Text> _text;
    [SerializeField] private List<int> _scrollNum;
    [SerializeField] private List<Image> _scrollImage;

    private void Awake()
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
    }
    void Start()
    {
        _gameManager = GameManager.instance;
        _boardManager = GameObject.Find("BoardManager(Clone)").GetComponent<BoardManager>();
        _dungeonGenerator = GameObject.Find("BoardManager(Clone)").GetComponent<DungeonGenerator.DungeonGenerator>();
        _boomSprite = _boomEffect.GetComponent<SpriteRenderer>();
        UpdateNum();
    }

    void Update()
    {
        if (!_gameManager.playersTurn)
        {
            InputKeyDown();
        }
        else if (_gameManager.playersTurn)
        {
            return;
        }
    }

    private void UpdateNum()
    {
        for(int i = 0; i < 4; i++)
        {
            if (_scrollNum[i] == 0)
            {
                _scrollImage[i].color = new Color(1, 1, 1, 0.5f);
            }

            else _scrollImage[i].color = new Color(1, 1, 1, 1);
        }

        _text[0].text = _scrollNum[0].ToString();
        _text[1].text = _scrollNum[1].ToString();
        _text[2].text = _scrollNum[2].ToString();
        _text[3].text = _scrollNum[3].ToString();
    }

    private void InputKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RandomTeleport();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Bomb();
        }
    }

    public void RandomTeleport() //텔레포트 스크롤 스크립트
    {
        if (_scrollNum[0] > 0)
        {
            target = GameObject.FindWithTag("Player");
            Vector3Int randomPosisiton = _boardManager.RandomPosition(_dungeonGenerator.FloorPosition);
            target.transform.position = randomPosisiton;
            _scrollNum[0]--;
            UpdateNum();
        }

    }

    public void Bomb() //반경 5타일 내에 20 데미지
    {
        target = GameObject.FindWithTag("Player");
        transform.position = target.transform.position;
        Vector2 playerPosition = target.transform.position;
        Vector2 rayPosition;

        if (_scrollNum[2] > 0)
        {
            for (int x = -4; x <= 4; x++)
            {
                for (int y = -4; y <= 4; y++)
                {
                    RaycastHit2D hit;
                    rayPosition = playerPosition + new Vector2(x, y);
                    hit = Physics2D.Linecast(rayPosition, rayPosition, blockingLayer);
                    if (hit.collider == null)
                    {
                        continue;
                    }
                    else if (hit.collider.gameObject.CompareTag("Monster"))
                    {
                        _boomEffect.GetComponent<Animator>().SetTrigger("isBoom");
                        GameObject target = hit.collider.gameObject;
                        //target.GetComponent<MonsterStatManager>().curStat.HP -= 20;
                        Debug.Log(target.gameObject.name + target.gameObject.transform.position + "폭탄 맞음");

                    }
                }
            }

            _scrollNum[2]--;
            UpdateNum();
        }

            
    }
}
