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

    private SpriteRenderer _leftSprite;
    private SpriteRenderer _rightSprite;
    private CurrentItemStats _leftStats;
    private CurrentItemStats _rightStats;
    private Transform _followTarget;
    private bool[] _usingItem = { false, false };

    private float _currentLeftCooldown = 0f;
    private float _currentRightCooldown = 0f;

    // if seperating this script these would be in their correseponding scripts
    private Enemy _enemyScript = null;
    private PlayerController _playerScript = null;

    private void Start()
    {
        if(followMouse)
        {
            _playerScript = transform.parent.gameObject.GetComponent<PlayerController>();
        }
        else
        {
            _enemyScript = transform.parent.gameObject.GetComponent<Enemy>();
        }

        _leftSprite = leftHand.GetComponent<SpriteRenderer>();
        _rightSprite = rightHand.GetComponent<SpriteRenderer>();

        _leftStats = leftHand.GetComponent<CurrentItemStats>();
        _rightStats = rightHand.GetComponent<CurrentItemStats>();

        so_Item tempItem = leftItem;
        leftItem = null;
        EquipItem(tempItem, true);

        tempItem = rightItem;
        rightItem = null;
        EquipItem(tempItem, false);

        _leftSprite.enabled = false;
        _rightSprite.enabled = false;

        leftCollider.enabled = false;
        rightCollider.enabled = false;
    }

    void Update()
    {
        _currentLeftCooldown -= Time.deltaTime;
        _currentRightCooldown -= Time.deltaTime;

        if(followMouse)
            PlayerCooldownUIHelper.instance.UpdateCooldowns(_currentLeftCooldown, _currentRightCooldown);

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
            _leftSprite.sprite = leftItem.weaponSprite;

            if (followMouse)
            {
                PlayerCooldownUIHelper.instance.ChangeImages(_leftSprite.sprite, inLeft);
                PlayerCooldownUIHelper.instance.SetMaxLeftCooldown(leftItem.useCooldown);
                PlayerCooldownUIHelper.instance.ChangeItem(item, true);
            }

            myLeftAnimator.enabled = true;
            _leftStats.SetStats(leftItem);

            if (leftItem.itemType == ItemType.Shield)
            {
                //myLeftAnimator.enabled = false;
                //leftHand.transform.localPosition = new Vector3(0.25f, -0.15f, 0);
                _leftSprite.flipY = false;
            }
            else
            {
                //myLeftAnimator.enabled = true;
                //leftHand.transform.localPosition = new Vector3(0.4f, -0.3f, 0);
                _leftSprite.flipY = true;
                leftCollider.radius = leftItem.damageRadius;
            }
        }
        else
        {
            if (followMouse && rightItem != null)
                PlayerManager.instance.playerInventory.Add(rightItem);

            rightItem = item;
            _rightSprite.sprite = rightItem.weaponSprite;

            if (followMouse)
            {
                PlayerCooldownUIHelper.instance.ChangeImages(_rightSprite.sprite, inLeft);
                PlayerCooldownUIHelper.instance.SetMaxRightCooldown(rightItem.useCooldown);
                PlayerCooldownUIHelper.instance.ChangeItem(item, false);
            }

            myRightAnimator.enabled = true;
            _rightStats.SetStats(rightItem);

            if (rightItem.itemType == ItemType.Shield)
            {
                //myRightAnimator.enabled = false;
                //rightHand.transform.localPosition = new Vector3(-0.25f, -0.15f, 0);
                _rightSprite.flipY = false;
            }
            else
            {
                //myRightAnimator.enabled = true;
                //rightHand.transform.localPosition = new Vector3(-0.4f, -0.3f, 0);
                _rightSprite.flipY = true;
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
        if (leftItem == null || !gameObject.activeSelf)
            return;

        if (_currentLeftCooldown <= 0)
        {
            StartCoroutine(nameof(UseLeftItem));
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
        if (rightItem == null || !gameObject.activeSelf)
            return;
        
        if (_currentRightCooldown <= 0)
        {
            StartCoroutine(nameof(UseRightItem));
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
        _currentLeftCooldown = leftItem.useCooldown;
        _leftSprite.enabled = true;
        _usingItem[0] = true;

        if (leftItem.itemType == ItemType.Shield)
        {
            AudioManager.instance.PlayOneShotSound("UseShield");

            if (_enemyScript != null)
                _enemyScript.UpdateArmor(leftItem.modifierValue);
            
            if (_playerScript != null)
                _playerScript.UpdateArmor(leftItem.modifierValue);

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

        _usingItem[0] = false;
        leftCollider.enabled = false;
        _leftSprite.enabled = false;

        if (leftItem.itemType == ItemType.Shield)
        {
            if (_enemyScript != null)
                _enemyScript.UpdateArmor(-leftItem.modifierValue);

            if (_playerScript != null)
                _playerScript.UpdateArmor(-leftItem.modifierValue);
        }
    }

    IEnumerator UseRightItem()
    {
        float waitTime = 0.5f;
        _currentRightCooldown = rightItem.useCooldown;
        _rightSprite.enabled = true;
        _usingItem[1] = true;

        if (rightItem.itemType == ItemType.Shield)
        {
            AudioManager.instance.PlayOneShotSound("UseShield");

            if (_enemyScript != null)
                _enemyScript.UpdateArmor(rightItem.modifierValue);

            if (_playerScript != null)
                _playerScript.UpdateArmor(rightItem.modifierValue);

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

        _usingItem[1] = false;
        rightCollider.enabled = false;
        _rightSprite.enabled = false;

        if (rightItem.itemType == ItemType.Shield)
        {
            if (_enemyScript != null)
                _enemyScript.UpdateArmor(-rightItem.modifierValue);

            if (_playerScript != null)
                _playerScript.UpdateArmor(-rightItem.modifierValue);
        }
    }

    private void Aiming()
    {
        if (_usingItem[0] || _usingItem[1])
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
            if (_followTarget == null)
                return;
            Vector3 aimDirection = (_followTarget.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle + 90f);
        }
    }

    public void SetFollowTarget(Transform target)
    {
        _followTarget = target;
    }

    // TODO: Move this function to some helper script as it is also used in PlayerController and might be useful in future
    private Vector3 GetMousePosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        return temp;
    }
}
