using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootingTrap : MonoBehaviour
{
    public Transform projectileTarget;
    public Transform rotationPoint;
    public GameObject projectile;
    public Animator animator;
    [Tooltip("Should be higher than 0.5f as it waits that long between the tell and actually shooting")]
    public float cooldown = 3f;

    [SerializeField]
    private GameObject particles;

    private float _currentShotCooldown = 0f;
    private Projectile _currentProjectile;
    
    private static readonly int TrapTell = Animator.StringToHash("TrapTell");

    private void Start()
    {
        if (particles != null)
            particles.SetActive(false);

        rotationPoint.localEulerAngles = new Vector3(0f, 0f, Random.Range(0, 360));
    }

    private void Update()
    {
        if (_currentShotCooldown <= 0)
        {
            _currentShotCooldown = cooldown;
            ShootProjectile();
        }
        else
        {
            _currentShotCooldown -= Time.deltaTime;
        }
    }

    public virtual void ShootProjectile()
    {
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        animator.SetTrigger(TrapTell);
        if (particles != null)
            particles.SetActive(true);

        yield return new WaitForSeconds(.5f);

        AudioManager.instance.PlayOneShotSound("Spell1");

        if (particles != null)
            particles.SetActive(false);

        _currentProjectile = Projectile.Create(transform.position, projectileTarget.position, projectile, 1, 1, 1, -45);
        rotationPoint.localEulerAngles = new Vector3(0f, 0f, rotationPoint.localEulerAngles.z + 15f);
    }
}
