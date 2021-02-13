using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileLifeTime = 5f;
    public float projectileSpeed = 5f;
    public float projectileCooldown = 1f;
    public int attackDamage = 5;
    public Vector2 projectileDirection;

    protected Rigidbody2D rb;
    protected Vector3 myTarget;

    [SerializeField]
    protected LayerMask enemyMask;

    public static Projectile Create(Vector3 position, Vector3 target, GameObject projectilePrefab, float damageMultiplier, float speedMultiplier, float lifeTimeMultiplier)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        Vector3 aimDirection = (target - position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectile.transform.eulerAngles = new Vector3(0, 0, angle);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetUpProjectile(lifeTimeMultiplier, speedMultiplier, damageMultiplier, aimDirection, target);

        return projectileScript;
    }

    public static Projectile Create(Vector3 position, Vector3 target, GameObject projectilePrefab, float damageMultiplier, float speedMultiplier, float lifeTimeMultiplier, float addToAngle)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        Vector3 aimDirection = (target - position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectile.transform.eulerAngles = new Vector3(0, 0, angle + addToAngle);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetUpProjectile(lifeTimeMultiplier, speedMultiplier, damageMultiplier, aimDirection, target);

        return projectileScript;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    public void AddMask(int layerNumber)
    {
        // use 'or' to add layer to the mask
        enemyMask |= (1 << layerNumber);
    }

    public void SetMask(LayerMask layerMask)
    {
        enemyMask = layerMask;
    }

    public void SetUpProjectile(float lifeTimeMultiplier, float speedMultiplier, float damageMultiplier, Vector3 aimDirection, Vector3 target)
    {
        myTarget = target;
        projectileLifeTime *= lifeTimeMultiplier;
        projectileSpeed *= speedMultiplier;
        attackDamage = (int)(attackDamage * damageMultiplier);
        projectileDirection = aimDirection;

        Invoke("KillProjectile", projectileLifeTime);
    }

    protected virtual void MoveProjectile()
    {
        rb.MovePosition(rb.position + projectileDirection * projectileSpeed * Time.fixedDeltaTime);
    }

    protected virtual void KillProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError("HIT: " + collision.gameObject.name + "    " + collision.gameObject.layer);
        if ((enemyMask & (1 << collision.gameObject.layer)) != 0)
        {
            if(collision.gameObject.layer == 8)
            {
                // player
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            }
            else if(collision.gameObject.layer == 9)
            {
                // enemy 
                // not used currently
            }
            KillProjectile();
        }
    }
}
