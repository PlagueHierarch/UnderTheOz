using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharSample : MonoBehaviour
{
    public float speed;
    public Rigidbody2D CR;
    Vector2 movement = new Vector2();

    public float hp;
    public float exp;
    public int level;

    public float max_exp;

    public GameObject[] items_UI;


    private void Start()
    {
        hp = 100;
        exp = 0;
        level = 1;
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        CR.velocity = movement * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy_temp")
        {
            //curStats.HP -= 5;
        }

        else if (collision.tag == "exp_temp")
        {
            //curStats.Exp += 5;
        }

        /*else if (collision.tag == "item_temp")
        {
            int itemno = collision.GetComponent<itemSample>().itemNo;
            Debug.Log(itemno);
            items_UI[itemno].GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1);
        }*/
    }
}
