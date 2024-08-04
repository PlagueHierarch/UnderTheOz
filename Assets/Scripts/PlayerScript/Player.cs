using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MovingObject
{
    private Animator animator;
    private GameManager _gameManager;

    public GameObject _camera;

    protected override void Start()
    {
        _camera = GameObject.Find("Main Camera");
        animator = GetComponent<Animator>();
        _gameManager = GetComponent<GameManager>();
        base.Start();
    }

    void Update()
    {
        CameraMove();


        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }
        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Obstacle>(horizontal, vertical);
        }
    }
    protected override void AttemptMove<T> (int xDir,  int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        //RaycastHit2D hit;

        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected override void OnCantMove<T>(T component)
    {
    }

    void CameraMove()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, new Vector3(transform.position.x, transform.position.y, -10f), 0.7f);
    }
}
