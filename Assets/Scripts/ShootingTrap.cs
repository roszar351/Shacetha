﻿using System.Collections;
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
    public float cooldown = 2f;

    [SerializeField]
    private GameObject particles;

    private float currentShotCooldown = 0f;
    private Projectile currentProjectile;

    private void Start()
    {
        if (particles != null)
            particles.SetActive(false);

        rotationPoint.localEulerAngles = new Vector3(0f, 0f, Random.Range(0, 360));
    }

    private void Update()
    {
        if (currentShotCooldown <= 0)
        {
            currentShotCooldown = cooldown;
            ShootProjectile();
        }
        else
        {
            currentShotCooldown -= Time.deltaTime;
        }
    }

    public virtual void ShootProjectile()
    {
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        animator.SetTrigger("TrapTell");
        if (particles != null)
            particles.SetActive(true);

        yield return new WaitForSeconds(.5f);

        if (particles != null)
            particles.SetActive(false);

        currentProjectile = Projectile.Create(transform.position, projectileTarget.position, projectile, 1, 1, 1, -45);
        rotationPoint.localEulerAngles = new Vector3(0f, 0f, rotationPoint.localEulerAngles.z + 15f);
    }
}
