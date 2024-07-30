using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(_player.position.x, _player.position.y, -10f), 0.7f);
    }
}
