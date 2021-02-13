using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: if enough time try to seperate HandsController into 2 seperate scripts one for player and one for enemies
public class HandsController : MonoBehaviour
{
    public so_NPCStats myBaseStats; // Used to get and alter character stats when using items i.e. for damage and defense
    //public LayerMask myEnemyLayers;
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
    private bool[] usingItem = { false, false };

    private float currentLeftCooldown = 0f;
    private float currentRightCooldown = 0f;

    // if seperating this script these would be in their correseponding scripts
    private Enemy enemyScript = null;
    private PlayerController playerScript = null;

    private void Start()
    {
        if(followMouse)
        {
            playerScript = transform.parent.gameObject.GetComponent<PlayerController>();
        }
        else
        {
            enemyScript = transform.parent.gameObject.GetComponent<Enemy>();
        }

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

    public float GetHighestCooldown()
    {
        return leftItem.useCooldown > rightItem.useCooldown ? leftItem.useCooldown : rightItem.useCooldown;
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
        float waitTime = 0.5f;
        currentLeftCooldown = leftItem.useCooldown;
        leftSprite.enabled = true;
        usingItem[0] = true;

        if (leftItem.itemType == ItemType.Shield)
        {
            AudioManager.instance.PlayOneShotSound("UseShield");

            if (enemyScript != null)
                enemyScript.UpdateArmor(leftItem.modifierValue);
            
            if (playerScript != null)
                playerScript.UpdateArmor(leftItem.modifierValue);

            waitTime += .5f;
        }
        else
        {
            leftCollider.enabled = true;
            switch(leftItem.itemType)
            {
                case ItemType.Spear:
                    AudioManager.instance.PlayOneShotSound("Swing2");
                    break;
                case ItemType.Dagger:
                    AudioManager.instance.PlayOneShotSound("Swing3");
                    break;
                default:
                    AudioManager.instance.PlayOneShotSound("Swing1");
                    break;
            }
        }

        myLeftAnimator.SetTrigger("Use" + leftItem.itemType.ToString());

        yield return new WaitForSeconds(waitTime);

        usingItem[0] = false;
        leftCollider.enabled = false;
        leftSprite.enabled = false;

        if (leftItem.itemType == ItemType.Shield)
        {
            if (enemyScript != null)
                enemyScript.UpdateArmor(-leftItem.modifierValue);

            if (playerScript != null)
                playerScript.UpdateArmor(-leftItem.modifierValue);
        }
    }

    IEnumerator UseRightItem()
    {
        float waitTime = 0.5f;
        currentRightCooldown = rightItem.useCooldown;
        rightSprite.enabled = true;
        usingItem[1] = true;

        if (rightItem.itemType == ItemType.Shield)
        {
            AudioManager.instance.PlayOneShotSound("UseShield");

            if (enemyScript != null)
                enemyScript.UpdateArmor(rightItem.modifierValue);

            if (playerScript != null)
                playerScript.UpdateArmor(rightItem.modifierValue);

            waitTime += .5f;
        }
        else
        {
            rightCollider.enabled = true;
            switch (rightItem.itemType)
            {
                case ItemType.Spear:
                    AudioManager.instance.PlayOneShotSound("Swing2");
                    break;
                case ItemType.Dagger:
                    AudioManager.instance.PlayOneShotSound("Swing3");
                    break;
                default:
                    AudioManager.instance.PlayOneShotSound("Swing1");
                    break;
            }
        }

        myRightAnimator.SetTrigger("Use" + rightItem.itemType.ToString());

        yield return new WaitForSeconds(waitTime);

        usingItem[1] = false;
        rightCollider.enabled = false;
        rightSprite.enabled = false;

        if (rightItem.itemType == ItemType.Shield)
        {
            if (enemyScript != null)
                enemyScript.UpdateArmor(-rightItem.modifierValue);

            if (playerScript != null)
                playerScript.UpdateArmor(-rightItem.modifierValue);
        }
    }

    private void Aiming()
    {
        if (usingItem[0] || usingItem[1])
            return;

        if (followMouse)
        {
            Vector3 mousePos = GetMousePosition();
            Vector3 aimDirection = (mousePos - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle + 90f);
        }
        else
        {
            if (followTarget == null)
                return;
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
