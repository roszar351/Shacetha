using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTrap : MonoBehaviour
{
    public float trapLifeDuration = 5f;

    [SerializeField] private int damage = 10;
    
    private float _trapLifeCounter = 0f;

    private Vector3 changer = new Vector3(.01f, 0.01f, 0.01f);

    public static StaticTrap Create(Vector3 position, Vector3 scale, GameObject trapPrefab, float lifeTime, int damage)
    {
        GameObject slime = Instantiate(trapPrefab, position, Quaternion.identity);
        
        StaticTrap trapScript = slime.GetComponent<StaticTrap>();
        trapScript.SetUpTrap(scale, lifeTime, damage);

        return trapScript;
    }

    private void SetUpTrap(Vector3 scale, float lifeTime, int damage)
    {
        transform.localScale = scale;
        trapLifeDuration = lifeTime;
        this.damage = damage;
    }

    private void Start()
    {
        _trapLifeCounter = trapLifeDuration;
    }

    private void Update()
    {
        transform.localScale += changer;
        changer *= -1;
        
        if (trapLifeDuration < 0f)
            return;
        
        if(_trapLifeCounter <= 0)
            DeleteTrap();

        _trapLifeCounter -= Time.deltaTime;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void DisableTrap()
    {
        gameObject.SetActive(false);
    }

    public void DeleteTrap()
    {
        Destroy(gameObject);
    }
}
