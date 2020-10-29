using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    public so_NPCStats myBaseStats; // Used to get base damage
    public LayerMask myEnemyLayers;
    public Animator myAnimator;

    [SerializeField]
    private so_Item leftItem = null;
    [SerializeField]
    private so_Item rightItem = null;
    [SerializeField]
    private GameObject leftHand = null;
    [SerializeField]
    private GameObject rightHand = null;
    [SerializeField]
    private Transform leftAttackPoint = null;
    [SerializeField]
    private Transform rightAttackPoint = null;

    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;

    private float currentLeftCooldown = 0f;
    private float currentRightCooldown = 0f;

    private void Start()
    {
        leftSprite = leftHand.GetComponent<SpriteRenderer>();
        rightSprite = rightHand.GetComponent<SpriteRenderer>();
        EquipItem(leftItem, true);
        EquipItem(rightItem, false);
        leftSprite.enabled = false;
        rightSprite.enabled = false;
    }

    void Update()
    {
        currentLeftCooldown -= Time.deltaTime;
        currentRightCooldown -= Time.deltaTime;

        Aiming();
    }

    // Handles equiping an item
    public void EquipItem(so_Item item, bool inLeft = true)
    {
        if(inLeft)
        {
            leftItem = item;
            leftSprite.sprite = leftItem.weaponSprite;
            if(leftItem.itemType == ItemType.Weapon)
            {
                leftHand.transform.localPosition = new Vector3(0.4f, -0.3f, 0);
                leftSprite.flipY = true;
            }
            else
            {
                leftHand.transform.localPosition = new Vector3(0.25f, -0.15f, 0);
                leftSprite.flipY = false;
            }
        }
        else
        {
            rightItem = item;
            rightSprite.sprite = rightItem.weaponSprite;
            if (rightItem.itemType == ItemType.Weapon)
            {
                rightHand.transform.localPosition = new Vector3(-0.4f, -0.3f, 0);
                rightSprite.flipY = true;
            }
            else
            {
                // TODO: Shield position doesn't set properly(might be related to animator)
                rightHand.transform.localPosition = new Vector3(-0.25f, -0.15f, 0);
                rightSprite.flipY = false;
            }
        }
    }

    public void UseLeftHand()
    {
        if (currentLeftCooldown <= 0)
        {
            StartCoroutine("UseLeftItem");

            if (leftItem.itemType == ItemType.Weapon)
            {
                Collider2D[] damageArea = Physics2D.OverlapCircleAll(leftAttackPoint.position, leftItem.itemRange, myEnemyLayers);
                for (int i = 0; i < damageArea.Length; i++)
                {
                    damageArea[i].GetComponent<Enemy>().TakeDamage(myBaseStats.baseDamage + leftItem.modifierValue);
                }
            }
        }
    }

    public void UseRightHand()
    {
        if (currentRightCooldown <= 0)
        {
            StartCoroutine("UseRightItem");

            if (rightItem.itemType == ItemType.Weapon)
            {
                Collider2D[] damageArea = Physics2D.OverlapCircleAll(rightAttackPoint.position, rightItem.itemRange, myEnemyLayers);
                for (int i = 0; i < damageArea.Length; i++)
                {
                    damageArea[i].GetComponent<Enemy>().TakeDamage(myBaseStats.baseDamage + rightItem.modifierValue);
                }
            }
        }
    }

    IEnumerator UseLeftItem()
    {
        currentLeftCooldown = leftItem.useCooldown;
        leftSprite.enabled = true;
        if(leftItem.itemType == ItemType.Weapon)
        {
            myAnimator.SetBool("LeftAttack", true);
        }
        else
        {
            myBaseStats.totalArmor = myBaseStats.baseArmor + leftItem.modifierValue;
        }

        yield return new WaitForSeconds(leftItem.duration);

        myAnimator.SetBool("LeftAttack", false);
        leftSprite.enabled = false;
        myBaseStats.totalArmor = myBaseStats.baseArmor;
    }

    IEnumerator UseRightItem()
    {
        currentRightCooldown = rightItem.useCooldown;
        rightSprite.enabled = true;
        if (rightItem.itemType == ItemType.Weapon)
        {
            myAnimator.SetBool("RightAttack", true);
        }
        else
        {
            myBaseStats.totalArmor = myBaseStats.baseArmor + rightItem.modifierValue;
        }


        yield return new WaitForSeconds(rightItem.duration);

        myAnimator.SetBool("RightAttack", false);
        rightSprite.enabled = false;
        myBaseStats.totalArmor = myBaseStats.baseArmor;
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
