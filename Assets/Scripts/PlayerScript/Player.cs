using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    [SerializeField] private GameObject _gameManagerObject;
    [SerializeField] private GameObject _playerAnimation;
    [SerializeField] private GameObject _camera;

    private Animator _animator;
    private GameManager _gameManager;
    private SpriteRenderer _animationSpriteRenderer;


    protected override void Start()
    {
        _camera = GameObject.Find("Main Camera");
        _animator = _playerAnimation.GetComponent<Animator>();
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
        _animationSpriteRenderer = _playerAnimation.GetComponent<SpriteRenderer>();
        
        base.Start();
    }

    void Update()
    {
        _animator.SetBool("isMoving", false);

        CameraMove();
        if (!GameManager.instance.playersTurn)
        {
            InputKey();
        }
        else if (GameManager.instance.playersTurn)
        {
            return;
        }
    }

    protected override void AttemptMove<T> (int xDir,  int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        GameManager.instance.playersTurn = true;

    }

    private void InputKey() //8키 방향 이동 및 공격 함수
    {
        int horizontal = 0;
        int vertical = 0;

        if(Input.GetKey(KeyCode.Space))
        {
            _animator.SetTrigger("isAttack");
            GameManager.instance.playersTurn = true;
        }

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
            _animator.SetBool("isMoving", true);
            if (horizontal < 0)
            {
                _animator.SetInteger("Direction", -1); //음수가 왼쪽 양수가 오른쪽
            }
            else if(horizontal > 0)
            {
                _animator.SetInteger("Direction", 1);
            }
            AttemptMove<Obstacle>(horizontal, vertical);
        }
        if(horizontal == 0 &&  vertical == 0)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask _layer = other.gameObject.layer;
        if(_layer == 7)
        {
            _gameManager.stage += 1;
            if(_gameManager.stage == 1)
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
            }
            else if (_gameManager.stage > 5)
            {
                SceneManager.LoadScene("Stage Boss");
            }
        }
    }

    protected override void OnCantMove<T>(T component)
    {

    }

    void CameraMove()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(transform.position.x, transform.position.y, -10f), 0.7f);
    }
}
