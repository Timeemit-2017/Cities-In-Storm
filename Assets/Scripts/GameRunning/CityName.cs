using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class CityName : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

}
