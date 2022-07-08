using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class TextMeshProChildrenControl : MonoBehaviour
{
    private TextMeshPro[] children;

    public bool isActiveAtStart = true;

    public float alphaValueAtStart = 1;

    public void Init()
    {
        children = new TextMeshPro[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).GetComponent<TextMeshPro>();
        }

        gameObject.SetActive(isActiveAtStart);
        foreach (TextMeshPro item in children)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, alphaValueAtStart);
        }
    }

    public void SetTextUniformly(string text)
    {
        if(children == null)
        {
            Init();
        }
        foreach (TextMeshPro item in children)
        {
            item.text = text;
        }
    }

    /// <summary>
    /// 城市信息动画
    /// </summary>
    /// <param name="t"></param>
    /// <param name="direction">1表示逐渐显示，-1表示逐渐隐藏</param>
    /// <returns></returns>
    public IEnumerator CityHoverAnimation(int direction)
    {
        gameObject.SetActive(true);
        TextMeshPro t = children[0]; // 代表
        /*while (direction * t.color.a <= 0.49f + 0.5f * direction)
        {
            float result = Mathf.Lerp(t.color.a, 0.5f + 0.49f * direction, 0.3f);
            foreach (TextMeshPro item in children)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, result);
            }
            yield return null;
        }*/
        if(direction == 1)  // 正向
        {
            while(t.color.a <= 0.9f)
            {
                float result = Mathf.Lerp(t.color.a, 0.99f, 0.3f);
                foreach (TextMeshPro item in children)
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
                float result = Mathf.Lerp(t.color.a, 0.01f, 0.3f);
                foreach (TextMeshPro item in children)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, result);
                }
                yield return null;
            }
        }
        if(direction == -1)
        {
            gameObject.SetActive(false);
        }
        //Debug.Log("A coroutine has over.");
    }

}
