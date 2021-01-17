using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public so_NPCStats myStats;
    public HandsController myHands;

    protected int currentHp;
    protected Transform target;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        currentHp = myStats.maxHp;
        SetTarget(PlayerManager.instance.player.transform);
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Move()
    {
        Debug.LogError("Implement Move Method!");
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

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
        myHands.SetFollowTarget(target);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        //Debug.Log("Damage taken!");
        damageAmount = (int)(damageAmount * (100f / (100f + myStats.totalArmor)));
        currentHp -= damageAmount;
        TextPopup.Create(transform.position, damageAmount);

        if (currentHp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            Invoke("Die", 0.5f);
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
