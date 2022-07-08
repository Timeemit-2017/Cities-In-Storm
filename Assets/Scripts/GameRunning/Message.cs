using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class Message : MonoBehaviour
{
    public GameObject messagePrefab;
    public List<TextMeshProUGUI> messages;
    public float originY;

    public void Start()
    {
        originY = 0;
    }

    public void SummonMessage(string from, string content)
    {
        GameObject result = Object.Instantiate(messagePrefab, transform);
        TextMeshProUGUI tmp = result.GetComponent<TextMeshProUGUI>();
        RectTransform rect = result.GetComponent<RectTransform>();

        tmp.text = $"<{from}> {content}";
    }

    public void SummonMessage(string content)
    {
        SummonMessage("Unknown", content);
    }
}
