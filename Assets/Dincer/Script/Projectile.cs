using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime;
    float timer;
    float damage;
    private void Update()
    {

        if (lifeTime<timer)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            print("Hit Enemy");
            collision.collider.GetComponent<Health>().TakeDamage(damage);
            // play impact
            Destroy(gameObject);
        }
    }

    public void SetProjectile(float t,float dmg)
    {
        lifeTime = t;
        damage = dmg;
    }
}
