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

    protected virtual void Start()
    {
        currentHp = myStats.maxHp;
        SetTarget(PlayerManager.instance.player.transform);
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
    }

    public virtual void TakeDamage(int damageAmount)
    {
        //Debug.Log("Damage taken!");
        TextPopup.Create(transform.position, damageAmount);
        currentHp -= damageAmount;

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
