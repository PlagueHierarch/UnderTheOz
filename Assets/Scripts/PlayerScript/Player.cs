using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    private Animator animator;
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        base.Start();
    }

    void Update()
    {
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
}
