using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public Image healthSlider;
    public TextMeshProUGUI hpText;
    public so_NPCStats initialStats;

    private void Start()
    {
        GetComponent<PlayerController>().OnHealthChanged += OnHealthChanged;
        OnHealthChanged(initialStats.maxHp, initialStats.maxHp);
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = healthPercent;
        hpText.SetText(currentHealth.ToString());
    }
}