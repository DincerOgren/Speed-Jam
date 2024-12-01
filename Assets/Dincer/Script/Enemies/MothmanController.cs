using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MothmanController : MonoBehaviour
{
    //TODO: projectile scripti ekleyip, deðerleri güncelleyinec biticek; fire sprit spawn eklenecek

    


    [Header("Attack Section")]
    public float chaseRange = 25;
    public float attackRange;
    float maxAttackRange = 20;
    float minAttackRange = 15;
    public LayerMask playerLayer;
    bool shouldAttack;
    public Transform projectileExitPoint;

    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float maxProjectileLifetime;
    public float attackInterval;
    float attackTimer;

    [Header("Follow Section")]
    public float followSpeed;
    Vector3 defaultPos;
    public float waitForPlayer = 2f;
    float waitTimer;
    bool shouldFollowPlayer = false;


    [Header("Basic Fly Values")]
    public float maxHeight = 10f;
    public float hoverForce = 5f; // Constant upward force
    public float moveSpeed = 3f;  // Movement speed
    public float oscillationAmplitude = 0.5f; // How much it moves up and down
    public float oscillationFrequency = 2f;   // Speed of the oscillation
    bool isTriggeredOnce;

    [Header("Wings")]
    [SerializeField] Animator wingAnim;

    [Header("Default Ref")]
    Transform target;
    Rigidbody rb;
    Animator anim;
    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {

        attackRange = Random.Range(minAttackRange, maxAttackRange);
        defaultPos = transform.position;
        defaultPos.y = maxHeight;
        print(defaultPos);
    }

    private void Update()
    {
        UpdateAnim();
        UpdateTimers(); 
    }

    void FixedUpdate()
    {
        SimulateGravity();
        CheckPlayer();

        //SimulateFlight();
        //FollowTarget();
    }

    void SimulateGravity()
    {
        if (!isTriggeredOnce) return;


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Ground")))
        {
            float height = hitInfo.point.y + maxHeight;

            float oscillation = Mathf.Sin(Time.time * oscillationFrequency) * oscillationAmplitude;
            height += oscillation;


            float dist = (height - transform.position.y);
            float amountToLift;
            amountToLift = dist * hoverForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new(0, amountToLift, 0);
            rb.AddForce(liftForce, ForceMode.VelocityChange);



        }

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


    }

    void CheckPlayer()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToTarget, chaseRange, playerLayer))
        {
            FollowPlayer();
        }
        else
            ReturnStartPos();

        if (Physics.Raycast(transform.position, directionToTarget, attackRange, playerLayer))
        {
            shouldAttack = true;
        }
        else
            shouldAttack = false;
    }

    void ReturnStartPos()
    {
        //if (!shouldFollowPlayer)
        {
            if (!isTriggeredOnce)
            {
                return;
            }
            print("RETURN");
            if (Vector3.Distance(transform.position,defaultPos) >= 1.5f)
            {
                transform.LookAt(defaultPos);
            }
            Vector3 dir = (defaultPos- transform.position).normalized;
            
           // transform.LookAt(defaultPos);
            rb.velocity =new Vector3( dir.x * followSpeed,rb.velocity.y,dir.z*followSpeed);
        }
    }
    private void FollowPlayer()
    {
        isTriggeredOnce = true;
       // if (!shouldFollowPlayer) { return; }

        if (shouldAttack)
        {
            PerformAttack();
            return;
        }

        transform.LookAt(target);
        var vel = transform.forward * followSpeed;
        print("Follow");

        rb.velocity=vel;
    }

    void PerformAttack()
    {
        Vector3 temp = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = temp;
        transform.LookAt(target);
        if (attackTimer>=attackInterval)
        {
            print("Attack");
            attackTimer = 0;
            
        }
        
    
    }

    
    public void ShootProjectile()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        var a = Instantiate(projectilePrefab, projectileExitPoint.position, Quaternion.identity);
        a.GetComponent<Rigidbody>().velocity = directionToTarget * projectileSpeed;
        

    }

    private Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0, rb.velocity.y, 0);
    }

    void UpdateAnim()
    {
        anim.SetBool("shouldAttack", shouldAttack);
        anim.SetBool("triggered", isTriggeredOnce);
        wingAnim.SetBool("Flying", isTriggeredOnce);
    }
    void UpdateTimers()
    {
        attackTimer += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 100);


        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, directionToTarget * chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionToTarget * attackRange);
    }
}
