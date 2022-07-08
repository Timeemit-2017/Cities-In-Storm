using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
/// </summary>
public class ButtonChangeSprite : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;

    private Image image;
    private RectTransform rectTransform;

    private bool lastHoverState = false;
    private bool thisHoverState = false;



    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        thisHoverState = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
        if (thisHoverState != lastHoverState)
        {
            if (thisHoverState)
            {
                image.sprite = hoverSprite;
            }
            else
            {
                image.sprite = defaultSprite;
            }
        }

        lastHoverState = thisHoverState;
    }
}
