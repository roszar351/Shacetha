using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public so_NPCStats myStats;

    private int currentHp;

    private void Start()
    {
        currentHp = myStats.maxHp;
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
