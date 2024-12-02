using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isBoss;
    public bool isPlayer;
    public bool isWendigo;
    public bool isMothman;
    public bool isExploder;

    [SerializeField] float maxHealth;
    float currentHealth;


    bool isDead;
    private void Awake()
    {

    }

    private void Start()
    {
        currentHealth = maxHealth;
    }


    private void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;


        if (currentHealth - amount <= 0)
        {
            isDead = true;
            currentHealth = 0;
            Die();
        }
        else
        {
            currentHealth -= amount;

        }

    }


    public float GetHealthPercentage => currentHealth * 100 / maxHealth;

    void Die()
    {
        //Dead anim
        gameObject.SetActive(false);
    }



}
