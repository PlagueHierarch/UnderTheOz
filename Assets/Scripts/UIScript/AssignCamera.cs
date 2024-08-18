using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _camera;
    }



}
