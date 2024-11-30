using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;

public class LightAttack : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] Transform projectileExitPoint;
    [SerializeField]
    float projectileSpeed, projectileLifetime;
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField] float timeBetweenAttacks;
    float attackTimer = Mathf.Infinity;



    private void Update()
    {

        attackTimer += Time.deltaTime;
    }

    public void PerformLightAttack()
    {
        if (attackTimer >= timeBetweenAttacks)
        {
            attackTimer = 0;
            ShootProjectile();
        }

    }

    private void ShootProjectile()
    {
        var proj = Instantiate(projectilePrefab, projectileExitPoint.position, Quaternion.identity);

        Vector3 dir = transform.forward;

        proj.GetComponent<Rigidbody>().velocity = projectileSpeed * dir;
        
        
    }
}
