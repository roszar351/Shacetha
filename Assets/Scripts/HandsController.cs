using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    public so_NPCStats myBaseStats; // Used to get and alter character stats when using items i.e. for damage and defense
    public LayerMask myEnemyLayers;
    public Animator myLeftAnimator;
    public Animator myRightAnimator;

    [SerializeField]
    private bool followMouse = false;
    [SerializeField]
    private so_Item leftItem = null;
    [SerializeField]
    private so_Item rightItem = null;
    [SerializeField]
    private GameObject leftHand = null;
    [SerializeField]
    private GameObject rightHand = null;
    [SerializeField]
    private CircleCollider2D leftCollider = null;
    [SerializeField]
    private CircleCollider2D rightCollider = null;

    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private CurrentItemStats leftStats;
    private CurrentItemStats rightStats;
    private Transform followTarget;

    private float currentLeftCooldown = 0f;
    private float currentRightCooldown = 0f;

    private void Start()
    {
        leftSprite = leftHand.GetComponent<SpriteRenderer>();
        rightSprite = rightHand.GetComponent<SpriteRenderer>();

        leftStats = leftHand.GetComponent<CurrentItemStats>();
        rightStats = rightHand.GetComponent<CurrentItemStats>();

        so_Item tempItem = leftItem;
        leftItem = null;
        EquipItem(tempItem, true);

        tempItem = rightItem;
        rightItem = null;
        EquipItem(tempItem, false);

        leftSprite.enabled = false;
        rightSprite.enabled = false;

        leftCollider.enabled = false;
        rightCollider.enabled = false;
    }

    void Update()
    {
        currentLeftCooldown -= Time.deltaTime;
        currentRightCooldown -= Time.deltaTime;

        if(followMouse)
            PlayerCooldownUIHelper.instance.UpdateCooldowns(currentLeftCooldown, currentRightCooldown);

        Aiming();
    }

    // Handles equiping an item
    public void EquipItem(so_Item item, bool inLeft = true)
    {
        if (item == null)
            return;

        if(inLeft)
        {
            if (followMouse && leftItem != null)
                PlayerManager.instance.playerInventory.Add(leftItem);

            leftItem = item;
            leftSprite.sprite = leftItem.weaponSprite;

            if (followMouse)
            {
                PlayerCooldownUIHelper.instance.ChangeImages(leftSprite.sprite, inLeft);
                PlayerCooldownUIHelper.instance.SetMaxLeftCooldown(leftItem.useCooldown);
                PlayerCooldownUIHelper.instance.ChangeItem(item, true);
            }

            myLeftAnimator.enabled = true;
            leftStats.SetStats(leftItem);

            if (leftItem.itemType == ItemType.Shield)
            {
                //myLeftAnimator.enabled = false;
                //leftHand.transform.localPosition = new Vector3(0.25f, -0.15f, 0);
                leftSprite.flipY = false;
            }
            else
            {
                //myLeftAnimator.enabled = true;
                //leftHand.transform.localPosition = new Vector3(0.4f, -0.3f, 0);
                leftSprite.flipY = true;
                leftCollider.radius = leftItem.damageRadius;
            }
        }
        else
        {
            if (followMouse && rightItem != null)
                PlayerManager.instance.playerInventory.Add(rightItem);

            rightItem = item;
            rightSprite.sprite = rightItem.weaponSprite;

            if (followMouse)
            {
                PlayerCooldownUIHelper.instance.ChangeImages(rightSprite.sprite, inLeft);
                PlayerCooldownUIHelper.instance.SetMaxRightCooldown(rightItem.useCooldown);
                PlayerCooldownUIHelper.instance.ChangeItem(item, false);
            }

            myRightAnimator.enabled = true;
            rightStats.SetStats(rightItem);

            if (rightItem.itemType == ItemType.Shield)
            {
                //myRightAnimator.enabled = false;
                //rightHand.transform.localPosition = new Vector3(-0.25f, -0.15f, 0);
                rightSprite.flipY = false;
            }
            else
            {
                //myRightAnimator.enabled = true;
                //rightHand.transform.localPosition = new Vector3(-0.4f, -0.3f, 0);
                rightSprite.flipY = true;
                rightCollider.radius = rightItem.damageRadius;
            }
        }
    }

    public void UseLeftHand()
    {
        if (leftItem == null)
            return;

        if (currentLeftCooldown <= 0)
        {
            StartCoroutine("UseLeftItem");
            /*
            if (leftItem.itemType != ItemType.Shield)
            {
                Collider2D[] damageArea = Physics2D.OverlapCircleAll(leftAttackPoint.position, leftItem.damageRadius, myEnemyLayers);
                for (int i = 0; i < damageArea.Length; i++)
                {
                    Enemy e = damageArea[i].GetComponent<Enemy>();
                    if (e != null)
                    {
                        e.TakeDamage(myBaseStats.baseDamage + leftItem.modifierValue);
                    }
                    else
                    {
                        PlayerController p = damageArea[i].GetComponent<PlayerController>();
                        if(p != null)
                        {
                            p.TakeDamage(myBaseStats.baseDamage + leftItem.modifierValue);
                        }
                    }  
                }
            }
            */
        }
    }

    public void UseRightHand()
    {
        if (rightItem == null)
            return;
        
        if (currentRightCooldown <= 0)
        {
            StartCoroutine("UseRightItem");
            /*
            if (rightItem.itemType != ItemType.Shield)
            {
                Collider2D[] damageArea = Physics2D.OverlapCircleAll(rightAttackPoint.position, rightItem.damageRadius, myEnemyLayers);
                for (int i = 0; i < damageArea.Length; i++)
                {
                    Enemy e = damageArea[i].GetComponent<Enemy>();
                    if (e != null)
                    {
                        e.TakeDamage(myBaseStats.baseDamage + rightItem.modifierValue);
                    }
                    else
                    {
                        PlayerController p = damageArea[i].GetComponent<PlayerController>();
                        if (p != null)
                        {
                            p.TakeDamage(myBaseStats.baseDamage + rightItem.modifierValue);
                        }
                    }
                }
            }
            */
        }
    }

    IEnumerator UseLeftItem()
    {
        currentLeftCooldown = leftItem.useCooldown;
        leftSprite.enabled = true;

        if(leftItem.itemType == ItemType.Shield)
        {
            myBaseStats.totalArmor = myBaseStats.baseArmor + leftItem.modifierValue;
        }
        else
        {
            leftCollider.enabled = true;
        }

        myLeftAnimator.SetTrigger("Use" + leftItem.itemType.ToString());

        yield return new WaitForSeconds(.5f);

        leftCollider.enabled = false;
        leftSprite.enabled = false;
        myBaseStats.totalArmor = myBaseStats.baseArmor;
    }

    IEnumerator UseRightItem()
    {
        currentRightCooldown = rightItem.useCooldown;
        rightSprite.enabled = true;

        if (rightItem.itemType == ItemType.Shield)
        {
            myBaseStats.totalArmor = myBaseStats.baseArmor + rightItem.modifierValue;
        }
        else
        {
            rightCollider.enabled = true;
        }

        myRightAnimator.SetTrigger("Use" + rightItem.itemType.ToString());

        yield return new WaitForSeconds(.5f);

        rightCollider.enabled = false;
        rightSprite.enabled = false;
        myBaseStats.totalArmor = myBaseStats.baseArmor;
    }

    private void Aiming()
    {
        if (followMouse)
        {
            Vector3 mousePos = GetMousePosition();
            Vector3 aimDirection = (mousePos - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle + 90f);
        }
        else
        {
            Vector3 aimDirection = (followTarget.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle + 90f);
        }
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    // TODO: Move this function to some helper script as it is also used in PlayerController and might be useful in future
    private Vector3 GetMousePosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        return temp;
    }
}
