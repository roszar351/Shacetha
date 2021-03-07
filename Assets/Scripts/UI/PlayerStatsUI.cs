using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public so_NPCStats playerStats;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI movementText;
    public Image leftSlot;
    public Image rightSlot;

    public InventorySlot leftItem;
    public InventorySlot rightItem;

    private void Start()
    {
        PlayerCooldownUIHelper.instance.OnEquippedItemChanged += OnEquippedItemChanged;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        //TODO: probably shouldn't update every update call
        UpdateCharacterSheetUI();
    }

    void OnEquippedItemChanged(so_Item item, bool isLeft)
    {
        if(isLeft)
        {
            leftItem.AddItem(item);
            //leftItem = item;
            //leftSlot.enabled = true;
            //leftSlot.sprite = item.weaponSprite;
        }
        else
        {
            rightItem.AddItem(item);
            //rightItem = item;
            //rightSlot.enabled = true;
            //rightSlot.sprite = item.weaponSprite;
        }
    }

    void UpdateCharacterSheetUI()
    {
        hpText.SetText("Max Healthpoints: " + playerStats.maxHp);
        damageText.SetText("Attack Damage: " + playerStats.baseDamage);
        defenceText.SetText("Base Defence: " + playerStats.baseArmor);
        movementText.SetText("Movement Speed: " + playerStats.movementSpeed);
    }
}
