using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_mouseDetect : MonoBehaviour
{
    public float offset;
    public GameObject image;
    private RectTransform image_rect;

    private void Start()
    {
        image_rect = image.GetComponent<RectTransform>();
    }
    private void OnMouseExit()
    {
       image_rect.anchoredPosition = new Vector2(0,0);
    }

    private void OnMouseEnter()
    {
        image_rect.anchoredPosition = new Vector2 (-offset, offset);
    }

    private void OnMouseDown()
    {
        image_rect.anchoredPosition = new Vector2(offset, -offset);
    }

    private void OnMouseUp()
    {
        image_rect.anchoredPosition = new Vector2(0, 0);
    }

}
