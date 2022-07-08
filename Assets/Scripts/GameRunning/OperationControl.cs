using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CitesInStorm;

/// <summary>
///
/// </summary>
public class OperationControl : MonoBehaviour
{
    public new Camera camera;

    public CameraBehaviour cameraBehaviour;

    private RectTransform rectTransform;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void positionControl()
    {
        PieceInstante pieceInstante = GameVar.pieceControl.GetPiece(GameVar.blockBeingOperated.p);
        GetComponent<RectTransform>().position = camera.WorldToScreenPoint(pieceInstante.transform.position) + Vector3.right * transform.lossyScale.x * 32 - Vector3.up * rectTransform.sizeDelta.y / 2;
    }

    public void Update()
    {
        if (cameraBehaviour.isMoved)
        {
            positionControl();
        }
    }

    public void OnMouseOver()
    {
        Image image = GetComponent<Image>();
        Color c = image.color;
        image.color = new Color(c.r, c.g, c.b, Mathf.Lerp(image.color.a, 0, 0.9f));
    }

    /*public void OnMouseEnter()
    {
        StartCoroutine(HoverAnimation());
    }*/

    /*public void ControlHoverAnimation(bool target)
    {
        if (target)
        {
            StartCoroutine("HoverAnimation");
        }
        else
        {
            StopCoroutine("HoverAnimation");
        }
    }*/

    IEnumerator HoverAnimation()
    {
        Image image = GetComponent<Image>();
        while (image.color.a >= 0.01)
        {
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, Mathf.Lerp(image.color.a, 0, 0.3f));
            Debug.Log("HoverAnimation");
            yield return 0;
        }
    }

}
