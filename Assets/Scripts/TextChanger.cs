using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public Slider mySlider;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateText(mySlider.value);
    }

    public void UpdateText(float s)
    {
        int low1 = -80;
        int low2 = 0;
        int high1 = 20;
        int high2 = 100;
        if (text == null)
            return;

        text.SetText(((int)(low2 + (s - low1) * (high2 - low2) / (high1 - low1))).ToString());
    }
}
