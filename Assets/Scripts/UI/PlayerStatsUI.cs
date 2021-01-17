using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public so_NPCStats playerStats;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI movementText;

    private void Update()
    {
        //TODO: probably shouldnt update every loop
        UpdateCharacterSheetUI();
    }

    void UpdateCharacterSheetUI()
    {
        hpText.SetText("Max Healthpoints: " + playerStats.maxHp);
        damageText.SetText("Attack Damage: " + playerStats.baseDamage);
        defenceText.SetText("Base Defence: " + playerStats.baseArmor);
        movementText.SetText("Movement Speed: " + playerStats.movementSpeed);
    }
}
