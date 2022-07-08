using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class CardEntryControl : MonoBehaviour
{

    public Image[] children;

    public void Start()
    {
        children = new Image[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).GetComponent<Image>();
            children[i].color = new Color(children[i].color.r, children[i].color.g, children[i].color.b, 0.4f);
        }
    }

    public void EnterCardEdit()
    {
        Debug.Log("Click.");
    }

    public void StartAnimation(int direction)
    {
        StopAllCoroutines();
        StartCoroutine("HoverAnimation", direction);
    }

    public IEnumerator HoverAnimation(int direction)
    {
        Image t = children[0];
        if (direction == 1)  // 正向
        {
            while (t.color.a <= 0.9f)
            {
                float result = Mathf.Lerp(t.color.a, 0.99f, 0.3f);
                foreach (Image item in children)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, result);
                }
                yield return null;
            }
        }
        else
        {
            while (t.color.a >= 0.1f)
            {
                float result = Mathf.Lerp(t.color.a, 0.4f, 0.3f);
                foreach (Image item in children)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, result);
                }
                yield return null;
            }
        }
    }
}
