using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSlowingTrap : MonoBehaviour
{
    public float trapLifeDuration = 5f;

    [SerializeField] private Sprite[] cobwebSprites;
    [SerializeField] private SpriteRenderer cobwebRenderer;
    [SerializeField] private float speedModifier = 0.5f;
    
    private float _trapLifeCounter = 0f;

    public static StaticSlowingTrap Create(Vector3 position, Vector3 scale, GameObject trapPrefab, float lifeTime, float speedModifier)
    {
        GameObject web = Instantiate(trapPrefab, position, Quaternion.identity);
        
        StaticSlowingTrap trapScript = web.GetComponent<StaticSlowingTrap>();
        trapScript.SetUpTrap(scale, lifeTime, speedModifier);

        return trapScript;
    }

    private void SetUpTrap(Vector3 scale, float lifeTime, float speedModifier)
    {
        cobwebRenderer.sprite = cobwebSprites[Random.Range(0, cobwebSprites.Length)];
        transform.localScale = scale;
        trapLifeDuration = lifeTime;
        this.speedModifier = speedModifier;
    }

    private void Start()
    {
        _trapLifeCounter = trapLifeDuration;
    }

    private void Update()
    {
        if (trapLifeDuration < 0f)
            return;
        
        if(_trapLifeCounter <= 0)
            DeleteTrap();

        _trapLifeCounter -= Time.deltaTime;
    }

    public float GetModifier()
    {
        return speedModifier;
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
