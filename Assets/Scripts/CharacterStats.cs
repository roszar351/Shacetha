using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHp = 100;
    public int damage = 10;
    public float movementSpeed = 1f;
    public float attackRange = 0.5f;

    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        TextPopup.Create(transform.position, damage);
        currentHp -= damage;

        if (currentHp <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
