using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Monster : MovingObject
{
    private Transform target;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float distance;
    [SerializeField] private GameObject _animation;
    private int horizontal;
    private int vertical;

    private MonsterStatManager statManager;
    private Animator _animator;

    protected override void Start()
    {
        statManager = GetComponent<MonsterStatManager>();
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

        AttemptMove<Player>(horizontal, vertical);
            
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
            float _distance = Vector2.Distance(target.transform.position, gameObject.transform.position);
            //Debug.Log(distance);
            if (_distance > 1.5f)
            {
                CalDist();
            }
            else if (GameManager.instance.enemysTurn)
            {
                int dmg = statManager.curStat.Dmg;
                GameManager.Sound.Play("Sounds/HitPlayer001");
                _animator.SetTrigger("isAttack");
                target.gameObject.GetComponent<Player>().curState.HP -= dmg;
                Debug.Log("적 공격");

            }
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
        Player other = component as Player;
        Debug.Log("몬스터가 공격해야함");
        if(other != null)
        {
            int dmg = statManager.curStat.Dmg;

            GameManager.Sound.Play("Sounds/HitPlayer001");
            _animator.SetTrigger("isAttack");
            GameObject _target = other.gameObject;

            _target.GetComponent<PlayerStatManager>().GetDamaged(dmg);
            Debug.Log("적 공격");
        }
    }
    protected override void OnCanMove<T>(T component)
    {
    
    }
}
