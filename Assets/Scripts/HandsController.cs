using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    public so_NPCStats myBaseStats; // Used to get base damage

    [SerializeField]
    private so_Item leftItem;
    [SerializeField]
    private so_Item rightItem;

    void Update()
    {
        Aiming();
    }

    // Handles equiping an item which also involves unequiping the old item
    // TODO: unequip old item
    public void EquipItem(so_Item item, bool inLeft = true)
    {
        if(inLeft)
        {
            leftItem = item;
        }
        else
        {
            rightItem = item;
        }
    }

    private void Aiming()
    {
        Vector3 mousePos = GetMousePosition();
        Vector3 aimDirection = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle + 90f);
    }

    // TODO: Move this function to some helper script as it is also used in PlayerController and might be useful in future
    private Vector3 GetMousePosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        return temp;
    }
}
