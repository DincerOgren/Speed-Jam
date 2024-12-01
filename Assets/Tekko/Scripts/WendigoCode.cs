using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WendigoCode : MonoBehaviour
{
    public GameObject player;

    private bool isAttacking , isChasing , debounce;
    public int damage;

    Animator animator;

    Rigidbody rb;

    public float moveSpeed = 5f;

    private int currentAttack;

    [Header("RANGE")]
    public float attackRange;
    public float chaseRange;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        float distance = (player.transform.position - transform.position).magnitude;

        animator.SetFloat("speed", rb.velocity.magnitude);
        print(rb.velocity.magnitude);

        if(distance < chaseRange && distance > attackRange)
        {
            ChasePlayer();
        }
        if(distance < chaseRange && distance < attackRange)
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        if(!isAttacking)
        {
            transform.LookAt(player.transform.position);

            Vector3 velocity = transform.forward * moveSpeed;


            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        
    }

    private void AttackPlayer()
    {
        print("attack");
        transform.LookAt(player.transform.position);

        if(!isAttacking)
        {
            isAttacking = true;
            if(currentAttack >=2)
            {
                currentAttack = 0;
            }

            animator.SetTrigger(currentAttack.ToString());
            currentAttack++;

            Invoke("WaitDebounce", 1.5f);
        }
    }

    private void WaitDebounce()
    {
        isAttacking = false;
        debounce = false;
    }

}
