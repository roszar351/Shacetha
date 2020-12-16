using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Image healthSlider;
    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerController>().OnHealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = healthPercent;
        hpText.SetText(currentHealth.ToString());
    }
}