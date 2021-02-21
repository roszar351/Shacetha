using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public Slider mySlider;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText(mySlider.value);
    }

    public void UpdateText(float s)
    {
        int low1 = -80;
        int low2 = 0;
        int high1 = 20;
        int high2 = 100;
        if (_text == null)
            return;

        _text.SetText(((int)(low2 + (s - low1) * (high2 - low2) / (high1 - low1))).ToString());
    }
}
