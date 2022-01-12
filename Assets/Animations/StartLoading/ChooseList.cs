using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制H
/// </summary>
public class ChooseList : MonoBehaviour
{
    private Animator animator;
    private RectTransform rectTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        animator.SetBool("IfMouseIn", RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition));
    }
}
