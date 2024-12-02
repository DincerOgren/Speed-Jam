using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using static UnityEngine.GraphicsBuffer;

public class WraithController : MonoBehaviour
{
    [Header("Def Variables")]
    public float activateRange;
    public float moveTime = 2f;
    public float moveSpeed;
    public LayerMask playerLayer;
    bool startedWalking;
    bool walkDone;

    [Header("Throw Skil")]
    public GameObject projPrefab;
    public float projSpeed;
    public float timeBetweenProjectiles;
    public float maxProjAmount;
    float projTimer = Mathf.Infinity;
    public float projectileSkillRange;
    public float projLifetime;
    public float projDamage;
    public bool shouldStartProjectileThrow;
    public Transform projectileExitPoint;
    public float playerHeightCorrection = 1f;
    //add projectiles script for impact effects;

    [Header("Meteor Shower Skill")]
    public bool useMeteor;
    public Meteor meteorPrefab;

    public float meteorFallSpeed;
    //add meteors script for impact efect
    public float maxMeteorHeight;
    public float meteorAmount;
    //Randomize meteor drops karakterin etrafýna,
    public float meteorDamage;
    public float randomOffsetMultiplier;
    public float meteorAirWaitTime = 1f;
    public float timeBetweenMeteors = 1f;
    float meteorTimer = Mathf.Infinity;
    bool meteorStarted;
    bool meteorEnded;

    [Header("Meteor Ascend Settings")]
    public float ascendHeight;
    public float ascendForce;
    public float ascendDuration = 15f;
    float ascendTimer = Mathf.Infinity;
    bool isAscending;
    public Transform groundCheckPos;
    public float groundCheckDist;

    [Header("Shield and Spawn")]
    public float shieldDuration;
    public float[] shieldActivatePercentages = new float[2] { 60, 35 };
    public float enemySpawnAmount;
    public GameObject[] enemyTypes;
    float shieldTimer;
    //bool isEveryEnemyDead; ??


    [Header("Laser Beam")]
    public bool useLaser;

    public ParticleSystem laserParticle;



    public float laserDuration;
    float laserTimer;

    public float laserWarningDuration;
    float laserWarningTimer;
    public float laserDamage;
    public float laserRange;

    public float laserTurnSpeed;

    public LineRenderer laserBeam; //    public float warningDuration = 1f; // Time to telegraph the laser
                                   // Maximum distance of the laser
    public LayerMask hitLayers; // Layers the laser can hit

    private bool isFiring = false;






    Rigidbody rb;
    Animator anim;
    public Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        // player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {

    }


    private void Update()
    {

        UpdateAnimator();
        CheckGrounded();
        if (startedWalking)
        {
            transform.LookAt(player);

            print("eftyyuu");

        }
        if (IsInActivateRange() && !startedWalking)
        {
            startedWalking = true;
            StartCoroutine(WalkTowardsPlayer());

        }


        if (walkDone)
        {
            if (shouldStartProjectileThrow)
            {
                shouldStartProjectileThrow = false;
                StartProjectileThrow();
            }
            if (useMeteor)
            {
                useMeteor = false;
                StartMeteorSkill();
            }
            if (useLaser)
            {
                useLaser = false;
                StartLaserSkill();
            }

            if (isFiring)
            {
                RotateTowardsPlayer(laserTurnSpeed);
                UpdateLaserBeam();
            }

        }

    }


    #region Laser Section

    private void StartLaserSkill()
    {

        if (IsPlayerInRange(laserRange))
        {
            anim.SetTrigger("StartLaser");
            StartLaserBeam();
        }
        else
            MoveTowardsPlayer();


    }

    private void FireLaser()
    {
        throw new NotImplementedException();
    }


    void RotateTowardsPlayer(float speed)
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

        //Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //// Calculate target rotation
        //Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        //// Smoothly rotate toward the target rotation using RotateTowards
        //transform.rotation = Quaternion.RotateTowards(
        //    transform.rotation,
        //    targetRotation,
        //    laserTurnSpeed  // Control rotation speed
        //);
    }










    void StartLaserBeam()
    {
        StartCoroutine(LaserBeamSequence());
    }

    IEnumerator LaserBeamSequence()
    {
        // Telegraph the laser with a warning beam
        EnableLaserBeam(Color.red, 0.01f); // Thin red line as a warning
        laserWarningTimer = 0;
        transform.LookAt(player);
        while (laserWarningTimer < laserWarningDuration)
        {
            RotateTowardsPlayer(laserTurnSpeed * 2);
            UpdateLaserPos();
            print("LookAt");
            laserWarningTimer += Time.deltaTime;
            yield return null;
        }
        DisableLaserBeam();

        // Fire the actual laser
        //EnableLaserBeam(Color.yellow, 2f); // Thick yellow line for the main laser


        laserParticle.Play();
        isFiring = true;
        yield return new WaitForSeconds(laserDuration);

        // Disable the laser
        isFiring = false;
        laserParticle.Stop();
        anim.SetTrigger("LaserEnd");
    }

    void EnableLaserBeam(Color color, float width)
    {
        laserBeam.enabled = true;
        laserBeam.startColor = color;
        laserBeam.endColor = color;
        laserBeam.startWidth = width;
        laserBeam.endWidth = width;
    }

    void DisableLaserBeam()
    {
        laserBeam.enabled = false;
        //anim.SetTrigger("LaserEnd");
    }

    void UpdateLaserBeam()
    {
        // Update the laser beam's direction
        //UpdateLaserPos();

        // Detect collisions
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange, hitLayers))
        {
            // Adjust the laser's endpoint to the collision point
            // laserBeam.SetPosition(1, hit.point);

            // Handle collision (e.g., damage player)
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit by laser!");
                // Apply damage or effects here
            }
        }
    }

    private void UpdateLaserPos()
    {
        laserBeam.SetPosition(0, transform.position + Vector3.up * playerHeightCorrection); // Start at the boss's position
        Vector3 targetDirection = transform.forward * laserRange;
        laserBeam.SetPosition(1, transform.position + playerHeightCorrection * Vector3.up + targetDirection);
    }









    #endregion



    #region WalkSection
    private IEnumerator WalkTowardsPlayer()
    {
        float movTime = 0;

        while (moveTime > movTime)
        {
            print("In while " + movTime);
            Vector3 dir = (player.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            movTime += Time.deltaTime;
            yield return null;
        }
        walkDone = true;
        print("Move Ended");
    }

    bool IsInActivateRange() => Physics.CheckSphere(transform.position, activateRange, playerLayer);

    #endregion

    #region ProjectileSection

    void StartProjectileThrow()
    {
        if (IsPlayerInRange(projectileSkillRange))
        {
            anim.SetTrigger("Throw");
        }
        else
            MoveTowardsPlayer();
    }

    public void Throw()
    {
        StartCoroutine(ProjectileThrow());
    }

    IEnumerator ProjectileThrow()
    {
        float a = 0;

        while (a <= maxProjAmount)
        {
            if (projTimer > timeBetweenProjectiles)
            {
                projTimer = 0;
                a++;
                SpawnProjectile();
            }

            projTimer += Time.deltaTime;
            yield return null;
        }

        shouldStartProjectileThrow = false;
        anim.ResetTrigger("Throw");
    }

    void SpawnProjectile()
    {
        var a = Instantiate(projPrefab, projectileExitPoint.position, Quaternion.identity);

        Vector3 dir = (player.position - transform.position).normalized;


        a.GetComponent<Rigidbody>().velocity = dir * projSpeed;
        //a.lifetime = lifetime;

    }




    #endregion


    #region Meteor Section

    void StartMeteorSkill()
    {
        if (IsPlayerInRange(projectileSkillRange))
        {
            meteorEnded = false;
            //anim.SetTrigger("Meteor");

            StartCoroutine(AscendPlayer());
        }
        else
            MoveTowardsPlayer();
    }



    IEnumerator AscendPlayer()
    {
        rb.velocity = Vector3.zero;
        ascendTimer = 0;
        rb.useGravity = false;
        while (!meteorEnded)
        {





            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100, LayerMask.GetMask("Ground")))
            {
                float height = hitInfo.point.y + ascendHeight;



                float dist = (height - transform.position.y);
                float amountToLift;
                amountToLift = dist * ascendForce - GetPlayerVerticalVelocity().y;
                Vector3 liftForce = new(0, amountToLift, 0);
                if (!meteorStarted)
                {
                    rb.AddForce(liftForce, ForceMode.VelocityChange);

                }
                if (Mathf.Abs(transform.position.y - ascendHeight) <= .5f && !meteorStarted)
                {
                    meteorStarted = true;
                    anim.SetTrigger("MeteorStart");
                }
                ascendTimer += Time.deltaTime;

            }
            yield return null;
        }

        print("Ascend ended");
        
    }


    public void TriggerMeteorShower() { StartCoroutine(MeteorShower()); }
    IEnumerator MeteorShower()
    {
        Debug.Log("Meteor Shower Activated!");

        for (int i = 0; i < meteorAmount;)
        {
            if (meteorTimer > timeBetweenMeteors)
            {
                meteorTimer = 0;
                SpawnMeteor();
                i++;
            }
            // Spawn meteor

            meteorTimer += Time.deltaTime;
            yield return null;

        }
        meteorEnded = true;
        meteorStarted = false;
        anim.SetTrigger("MeteorEnd");
    }

    public void ResetGravity() => rb.useGravity = true;
    private void SpawnMeteor()
    {
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * randomOffsetMultiplier;
        print("Random offset " + randomOffset);
        randomOffset.y = maxMeteorHeight;
        print("Random offset y" + randomOffset);
        Vector3 spawnPosition = player.position + randomOffset;


        var a = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        a.SetWaitSpeed(meteorAirWaitTime);
        a.SetFallSpeed(meteorFallSpeed);

    }







    #endregion



    private Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0, rb.velocity.y, 0);
    }
    private void MoveTowardsPlayer()
    {
        throw new NotImplementedException();
    }
    bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(player.position, transform.position) < range;

    }

    void CheckGrounded()
    {
        isAscending = !(Physics.Raycast(groundCheckPos.position, Vector3.down, groundCheckDist, LayerMask.GetMask("Ground")));
    }
    void UpdateAnimator()
    {
        anim.SetFloat("speed", rb.velocity.magnitude);
        anim.SetBool("isAscending", isAscending);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, activateRange);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, projectileSkillRange);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(groundCheckPos.position, Vector3.down * groundCheckDist);

        Gizmos.color = Color.blue;
        Vector3 directionToTarget = (player.position - transform.position).normalized;


        Gizmos.DrawRay(transform.position, directionToTarget * laserRange);
    }
}
