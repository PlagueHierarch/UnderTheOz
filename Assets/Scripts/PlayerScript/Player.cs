using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using DungeonGenerator;
using Unity.VisualScripting;

public class Player : MovingObject
{

    [SerializeField] private GameObject _gameManagerObject;
    [SerializeField] private GameObject _playerAnimation;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _boomEffect;

    private Animator _animator;
    private GameManager _gameManager;
    private SpriteRenderer _animationSpriteRenderer;
    private DungeonGenerator.DungeonGenerator _dungeonGenerator;
    private BoardManager _boardManager;
    private SpriteRenderer _boomSprite;
    private int horizontal = 0;
    private int vertical = 0;
    [SerializeField] private PlayerStat _playerStat;
    public PlayerStat curState { get { return _playerStat; }  set { _playerStat = value; } }

    private void ReferenceComponent()
    {
        _boomSprite = _boomEffect.GetComponent<SpriteRenderer>();
        _boardManager = GameObject.Find("BoardManager(Clone)").GetComponent<BoardManager>();
        _dungeonGenerator = GameObject.Find("BoardManager(Clone)").GetComponent<DungeonGenerator.DungeonGenerator>();
        _camera = GameObject.Find("Main Camera");
        _animator = _playerAnimation.GetComponent<Animator>();
        _gameManager = GameManager.instance;
        _animationSpriteRenderer = _playerAnimation.GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        
    }

    protected override void Start()
    {
        ReferenceComponent();
        base.Start();
    }

    void Update()
    {
        _animator.SetBool("isMoving", false);

        CameraMove();
        if (!_gameManager.playersTurn)
        {
            InputKey();
            InputKeyDown();
        }
        else if (_gameManager.playersTurn)
        {
            return;
        }
    }

    protected override void AttemptMove<T> (int xDir,  int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        _gameManager.playersTurn = true;

    }

    private void InputKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RandomTeleport();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Bomb();
        }
    }
    private void InputKey() //8Ű ���� �̵� �� ���� �Լ�
    {
        horizontal = 0;
        vertical = 0;

        if (Input.GetKey(KeyCode.Keypad8))
        {
            horizontal = 0;
            vertical = 1;
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            horizontal = 1;
            vertical = 0;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            horizontal = 0;
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            horizontal = -1;
            vertical = 0;
        }
        else if (Input.GetKey(KeyCode.Keypad9))
        {
            horizontal = 1;
            vertical = 1;
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            horizontal = 1;
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.Keypad1))
        {
            horizontal = -1;
            vertical = -1;
        }
        else if (Input.GetKey(KeyCode.Keypad7))
        {
            horizontal = -1;
            vertical = 1;
        }

        if (horizontal != 0 || vertical != 0)
        {
            GameManager.instance.playersTurn = false;
            if (horizontal < 0)
            {
                _animator.SetInteger("Direction", -1); //������ ���� ����� ������
            }
            else if(horizontal > 0)
            {
                _animator.SetInteger("Direction", 1);
            }
            AttemptMove<Monster>(horizontal, vertical);
        }
        if(horizontal == 0 &&  vertical == 0)
        {

        }
    }

    public void OnTriggerEnter2D(Collider2D other) //Ż�ⱸ�� ���� �� ȣ��
    {
        if(other.gameObject.layer == 8)
        {

            Debug.Log("���");
            _gameManager.stage += 1;
            /*if(_gameManager.stage == 1)
            {
                SceneManager.LoadScene("Stage 1");
            }
            else if (_gameManager.stage == 2)
            {
                SceneManager.LoadScene("Stage 2");
            }
            else if (_gameManager.stage == 3)
            {
                SceneManager.LoadScene("Stage 3");
            }
            else if (_gameManager.stage == 4)
            {
                SceneManager.LoadScene("Stage 4");
            }
            else if (_gameManager.stage == 5)
            {
                SceneManager.LoadScene("Stage 5");
            }*/
            if (_gameManager.stage == 6)
            {
                SceneManager.LoadScene("Stage Boss");
            }
            else if(_gameManager.stage == 7)
            {
                GameManager.Sound.Play("Sounds/BigDoor001");
            }
            else
            {
                SceneManager.LoadScene("Stage " + _gameManager.stage);
            }

        }
    }

    private void RandomTeleport() //�ڷ���Ʈ ��ũ�� ��ũ��Ʈ
    {
        GameManager.Sound.Play("Sounds/Teleport001");
        Vector3Int randomPosisiton = _boardManager.RandomPosition(_dungeonGenerator.FloorPosition);
        transform.position = randomPosisiton;
    }

    private void Bomb() //�ݰ� 5Ÿ�� ���� 20 ������
    {
        GameManager.Sound.Play("Sounds/Bomb001");

        Vector2 playerPosition = transform.position;
        Vector2 rayPosition;

        for (int x = -5; x <= 5; x++)
        {
            for (int y = -5; y <= 5; y++)
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
                    Debug.Log(target.gameObject.name + target.gameObject.transform.position + "��ź ����");

                }
            }
        }
    }

    protected override void OnCantMove<T>(T component) //���̳� �ٸ� ��ü�� �������� ȣ��
    {
        Monster other = component as Monster;
        Debug.Log("���� ����");
        
        if(other != null)
        {
            int dmg = PlayerStatManager.instance.curStats.Dmg;

            GameManager.Sound.Play("Sounds/HitMonster001");
            _animator.SetTrigger("isAttack");
            GameObject target = other.gameObject;

            target.GetComponent<MonsterStatManager>().GetDamaged(dmg);
            Debug.Log("�÷��̾� ����");
        }
        else
        {

        }
        //Vector2 playerPosition = transform.position;
        //Vector2 rayPosition;
        //RaycastHit2D hit;
        //rayPosition = playerPosition + new Vector2(horizontal, vertical);
        //hit = Physics2D.Linecast(playerPosition, rayPosition, blockingLayer);
        //if (hit.collider == null)
        //{
        //    return;
        //}
        //else if (hit.collider.gameObject.CompareTag("Monster"))
        //{
        //    int dmg = PlayerStatManager.instance.curStats.Dmg;

        //    GameManager.Sound.Play("Sounds/HitMonster001");
        //    _animator.SetTrigger("isAttack");
        //    GameObject target = hit.collider.gameObject;

        //    target.GetComponent<MonsterStatManager>().GetDamaged(dmg);
        //    Debug.Log("�÷��̾� ����");
        //}
    }
    protected override void OnCanMove<T>(T component)
    {
        _animator.SetBool("isMoving", true);
        GameManager.Sound.Play("PlayerStep001");

    }

    void CameraMove()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(transform.position.x, transform.position.y, -10f), 0.01f);
    }
}
