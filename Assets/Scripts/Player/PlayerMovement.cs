using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap _tilemap;

    public Vector3Int player_LocalToCellPos;
    public Vector3Int player_WorldToCellPos;

    private Vector3 aim_pos;
    private float speed = 5f;

    void Start()
    {
        SetPos();
        aim_pos = transform.position; 
    }

    void Update()
    {
        Keyboard_Movement();
        MovePlayer();
    }

    void SetPos()
    {
        player_LocalToCellPos = _tilemap.LocalToCell(transform.position);
        transform.position = player_LocalToCellPos;
    }

    void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, aim_pos, speed * Time.deltaTime);
        if (transform.position == aim_pos)
        {
            SetPos();
        }
    }
    void Keyboard_Movement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            aim_pos += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            aim_pos += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            aim_pos += new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            aim_pos += new Vector3(0, -1, 0);
        }
    }
}
