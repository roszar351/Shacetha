using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public so_NPCStats myStats;
    public HandsController myHands;
    public EnemyAnimations myAnimations;

    protected int currentHp;
    protected int totalArmor;
    protected Transform target;
    protected Rigidbody2D rb;
    protected bool attacking = false;
    protected float currentInvincibleTimer = 0f;

    [SerializeField]
    protected float invincibleTime = 1f;

    [SerializeField]
    private so_GameEvent onDeathEvent;

    protected virtual void Start()
    {
        currentHp = myStats.maxHp;
        SetTarget(PlayerManager.instance.player.transform);
        rb = GetComponent<Rigidbody2D>();
        totalArmor = myStats.baseArmor;
    }

    private void Update()
    {
        currentInvincibleTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(target != null)
            myAnimations.UpdateIdleAnimation(target.position - transform.position);
    }

    public void UpdateArmor(int modifierValue)
    {
        totalArmor = myStats.baseArmor + modifierValue;
    }

    public virtual void Move()
    {
        Debug.LogError("Implement Move Method!");
    }

    public virtual void MoveAway()
    {
        Debug.LogError("Implement MoveAway Method!");
    }

    public virtual void Attack()
    {
        Debug.LogError("Implement Attack Method!");
    }

    public virtual bool UseAbility()
    {
        Debug.LogError("Implement UseAbility Method!");
        return false;
    }

    public virtual void Idle()
    {
        myAnimations.PlayMovementAnimation(new Vector2(0, 0));
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;

        if(myHands != null && myHands.enabled)
            myHands.SetFollowTarget(target);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (currentInvincibleTimer > 0)
            return;

        currentInvincibleTimer = invincibleTime;
        //Debug.Log("Damage taken!");
        damageAmount = (int)(damageAmount * (100f / (100f + totalArmor)));
        currentHp -= damageAmount;
        TextPopup.Create(transform.position, damageAmount);

        if (currentHp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            Invoke("Die", 0.5f);
        }
    }

    // Can be overwritten to return the desired bool value
    public virtual bool CheckBool(int whichBool = 0)
    {
        return false;
    }

    protected virtual void Die()
    {
        myHands.gameObject.SetActive(false);
        onDeathEvent.Raise();
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            TakeDamage(collision.GetComponent<CurrentItemStats>().GetModifierValue());
        }
    }
}
