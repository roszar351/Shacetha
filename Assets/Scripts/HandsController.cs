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
    private Transform leftAttackPoint = null;
    [SerializeField]
    private Transform rightAttackPoint = null;

    private SpriteRenderer leftSprite;
    private SpriteRenderer rightSprite;
    private Transform followTarget;

    private float currentLeftCooldown = 0f;
    private float currentRightCooldown = 0f;

    private void Start()
    {
        leftSprite = leftHand.GetComponent<SpriteRenderer>();
        rightSprite = rightHand.GetComponent<SpriteRenderer>();
        so_Item tempItem = leftItem;
        leftItem = null;
        EquipItem(tempItem, true);
        tempItem = rightItem;
        rightItem = null;
        EquipItem(tempItem, false);
        leftSprite.enabled = false;
        rightSprite.enabled = false;
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
            }

            if(leftItem.itemType == ItemType.Shield)
            {
                myLeftAnimator.enabled = false;
                leftHand.transform.localPosition = new Vector3(0.25f, -0.15f, 0);
                leftSprite.flipY = false;
                leftAttackPoint.transform.localPosition = new Vector3(0f, -.2f, 0f) - new Vector3(0f, leftItem.itemRange, 0f);
            }
            else
            {
                myLeftAnimator.enabled = true;
                leftHand.transform.localPosition = new Vector3(0.4f, -0.3f, 0);
                leftSprite.flipY = true;
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
            }

            if (rightItem.itemType == ItemType.Shield)
            {
                myRightAnimator.enabled = false;
                rightHand.transform.localPosition = new Vector3(-0.25f, -0.15f, 0);
                rightSprite.flipY = false;
            }
            else
            {
                myRightAnimator.enabled = true;
                rightHand.transform.localPosition = new Vector3(-0.4f, -0.3f, 0);
                rightSprite.flipY = true;
                rightAttackPoint.transform.localPosition = new Vector3(0f, -.2f, 0f) - new Vector3(0f, rightItem.itemRange, 0f);
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
        }
    }

    public void UseRightHand()
    {
        if (rightItem == null)
            return;

        if (currentRightCooldown <= 0)
        {
            StartCoroutine("UseRightItem");

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
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(rightItem != null)
            Gizmos.DrawWireSphere(rightAttackPoint.position, rightItem.damageRadius);
        Gizmos.color = new Color(255 / 255, 192 / 255, 203 / 255);
        if(leftItem != null)
            Gizmos.DrawWireSphere(leftAttackPoint.position, leftItem.damageRadius);
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
            myLeftAnimator.SetBool("Attack", true);
        }

        yield return new WaitForSeconds(leftItem.duration);

        myLeftAnimator.SetBool("Attack", false);
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
            myRightAnimator.SetBool("Attack", true);
        }


        yield return new WaitForSeconds(rightItem.duration);

        myRightAnimator.SetBool("Attack", false);
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
