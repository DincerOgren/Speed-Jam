using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FireSpirit : MonoBehaviour
{
    public GameObject explodeParticle;
    public float hoverForce = 5f; // Constant upward force
    public float moveSpeed = 5f;  // Movement speed
    public float attackSpeed = 10f;
    public float explodeRange = 5f;
    public float oscillationAmplitude = 0.5f; // How much it moves up and down
    public float oscillationFrequency = 2f;   // Speed of the oscillation
    public Transform target;

    [Header("AttackSettings")]
    public float chaseRange;
    public float attackRange;

    private bool isExploding = false;

    public GameObject player;

    private Rigidbody rb;
    private Vector3 hoverDirection;

    private bool chaseIt, attackIt;

    private float journeyLength;

    [Header("Gravity")]
    public float maxHeight = 50f;
    public float graivtyForce = 10f;

    public float distance;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
    }

    void FixedUpdate()
    {
        SimulateGravity();
        //SimulateFlight();
        //FollowTarget();
        distance = ((player.transform.position - transform.position).magnitude);
        
        if (distance <= explodeRange)
        {
            ExplodeIt();
        }
        else
        {
            if (distance > chaseRange && distance > attackRange)
            {
                chaseIt = false;
                attackIt = false;
            }
            if (distance < chaseRange && distance > attackRange)
            {
                chaseIt = true;
                attackIt = false;
            }

            if (distance < chaseRange && distance < attackRange)
            {
                attackIt = true;
                chaseIt = false;
            }

            if (chaseIt) Chase();
            if (attackIt) Attack();
        }
    }

    private void ExplodeIt()
    {
        transform.position = transform.position;
        rb.velocity = Vector3.zero;

        if (!isExploding)
        {
            isExploding = true;
            print(rb.velocity);

            foreach (Transform trans in transform)
            {
                trans.gameObject.SetActive(false);
            }

            GameObject expld = (GameObject)Instantiate(explodeParticle);
            expld.transform.position = transform.position;
            
            Destroy(gameObject, 0.5f);
        }
    }
    void SimulateGravity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground")))
        {
            float height = hitInfo.point.y + maxHeight;
            float dist = (height - transform.position.y);
            
            //if (dist < maxHeight)
            //{
            //    rb.AddForce(Vector3.up * hoverForce, ForceMode.VelocityChange);
            //}
            //else
            //    rb.AddForce(Vector3.down * hoverForce, ForceMode.VelocityChange);
            //if (dist>maxHeight)
            //{
            //    return;
            //}
            float amountToLift;
            amountToLift = dist * hoverForce - GetPlayerVerticalVelocity().y;
            
            Vector3 liftForce = new(0, amountToLift, 0);
            rb.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private void Chase()
    {
        transform.LookAt(player.transform.position);

        Vector3 velocity = transform.forward * moveSpeed;

        // Rigidbody'ye doðru bir hýz uygula
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void Attack()
    {
        transform.LookAt(player.transform.position);

        Vector3 velocity = transform.forward * attackSpeed;

        // Rigidbody'ye doðru bir hýz uygula
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        player.transform.GetComponent<Health>().TakeDamage(100);
    }


    private Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0, rb.velocity.y, 0);
    }
    private void SimulateFlight()
    {
        // Apply constant upward force for hovering
        rb.AddForce(Vector3.up * hoverForce, ForceMode.Force);

        // Add oscillating movement (up and down)
        float oscillation = Mathf.Sin(Time.time * oscillationFrequency) * oscillationAmplitude;
        hoverDirection = new Vector3(0, oscillation, 0);

        // Apply hover oscillation
        rb.velocity += hoverDirection;
    }

    private void FollowTarget()
    {
        if (target == null) return;

        // Calculate direction to the target
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0; // Keep movement horizontal only

        // Apply forward force toward the target
        rb.AddForce(directionToTarget * moveSpeed, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 100);

        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
    }
}
