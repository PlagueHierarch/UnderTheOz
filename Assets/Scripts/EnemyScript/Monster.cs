using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MovingObject
{
    private Transform target;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float distance;
    [SerializeField] private GameObject _animation;

    private Animator _animator;

    protected override void Start()
    {
        _animator = _animation.GetComponent<Animator>();
        base.Start();
    }

    private void Update()
    { 
        if (!GameManager.instance.monstersEnd.Contains(gameObject) && GameManager.instance.enemysTurn)
        {
            //Debug.Log("not moving");
            UpdateTarget();
        }
    }

    private void CalDist()
    {
        int horizontal;
        int vertical;
        if (target.transform.position.y > gameObject.transform.position.y) vertical = 1;
        else if (target.transform.position.y < gameObject.transform.position.y) vertical = -1;
        else vertical = 0;

        if (target.transform.position.x > gameObject.transform.position.x) horizontal = 1;
        else if (target.transform.position.x < gameObject.transform.position.x) horizontal = -1;
        else horizontal = 0;

        if(horizontal > 0)
        {
            _animation.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            _animation.GetComponent<SpriteRenderer>().flipX=true;
        }

        _animator.SetTrigger("isMove");

        AttemptMove<Obstacle>(horizontal, vertical);
            
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        //Debug.Log("moved");
    }

    private void UpdateTarget()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, distance, playerLayer);
        //Debug.Log(col);

        if (col != null)
        {
            target = col.transform;
            distance = Vector2.Distance(target.transform.position, gameObject.transform.position);
            //Debug.Log(distance);
            if (distance > 1.5f)

            CalDist();
            GameManager.instance.monstersEnd.Add(gameObject);
        }

        else
        {
            target = null;
            GameManager.instance.monstersEnd.Add(gameObject);
        }
        
    }
    /* private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !GameManager.instance.monstersEnd.Contains(gameObject))
        {
            target = collision.gameObject.transform;
            distance = Vector2.Distance(target.transform.position, gameObject.transform.position);
            //Debug.Log(Vector2.Distance(target.transform.position, gameObject.transform.position));
            if(distance > 1.5f)
                CalDist();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target = null; 
    } */

    protected override void OnCantMove<T>(T component)
    {
    }
    protected override void OnCanMove<T>(T component)
    {
    
    }
}
