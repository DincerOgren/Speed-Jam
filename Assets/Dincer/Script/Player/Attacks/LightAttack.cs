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

    public void PerformLightAttack(Vector3 dir)
    {
        if (attackTimer >= timeBetweenAttacks)
        {
            attackTimer = 0;
            ShootProjectile(dir);
        }

    }

    private void ShootProjectile(Vector3 shootDir)
    {
        var proj = Instantiate(projectilePrefab, projectileExitPoint.position, Quaternion.identity);

        Vector3 dir = shootDir;

        proj.GetComponent<Rigidbody>().velocity = projectileSpeed * dir;
        
        
    }
}
