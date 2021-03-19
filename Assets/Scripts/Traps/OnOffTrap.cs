using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffTrap : MonoBehaviour
{
    public float trapLifeDuration = 5f;
    
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private int damage = 10;

    private float _trapDurationCounter = 0f;
    private bool _trapEnabled;
    private bool _playerInTrap = false;
    
    private void Start()
    {
        _trapDurationCounter = trapLifeDuration;
        DisableTrap();
    }

    private void Update()
    {
        if (trapLifeDuration < 0f)
            return;
        
        if(_trapDurationCounter <= 0)
            DisableTrap();

        _trapDurationCounter -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (_playerInTrap && _trapEnabled)
        {
            PlayerManager.instance.DealDamageToPlayer(damage);
        }
    }

    public void EnableTrap()
    {
        if(_trapEnabled)
            return;

        myRenderer.sprite = onSprite;
        _trapDurationCounter = trapLifeDuration;
        _trapEnabled = true;
    }

    public void DisableTrap()
    {
        myRenderer.sprite = offSprite;
        _trapEnabled = false;
    }

    public void DeleteTrap()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 19)
        {
            _playerInTrap = true;
            if (!_trapEnabled)
            {
                Invoke(nameof(EnableTrap), .6f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 19)
        {
            _playerInTrap = false;
        }
    }
}
