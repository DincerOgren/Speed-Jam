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










    Rigidbody rb;
    Animator anim;
    Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
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
        }

    }



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
        Vector3 playerPos = player.position;
        playerPos.y -= playerHeightCorrection;
        Vector3 dir = (playerPos - transform.position).normalized;
        print("Dir = " + dir);
        //dir.y -= playerHeightCorrection;
        print("Dir after correction = " + dir);

        a.GetComponent<Rigidbody>().velocity = dir * projSpeed;
        //a.lifetime = lifetime;

    }




    #endregion


    #region Meteor Section

    void StartMeteorSkill()
    {
        if (IsPlayerInRange(projectileSkillRange))
        {
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
        while (ascendTimer<ascendDuration)
        {

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100, LayerMask.GetMask("Ground")))
            {
                float height = hitInfo.point.y + ascendHeight;


                float dist = (height - transform.position.y);
                float amountToLift;
                amountToLift = dist * ascendForce - GetPlayerVerticalVelocity().y;
                Vector3 liftForce = new(0, amountToLift, 0);
                rb.AddForce(liftForce, ForceMode.VelocityChange);
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

        for (int i = 0; i < meteorAmount; i++)
        {
            

            // Spawn meteor
            SpawnMeteor();
            yield return null;
        
        }
    }

    private void SpawnMeteor()
    {
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * randomOffsetMultiplier;
        randomOffset.y = maxMeteorHeight;
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
        isAscending = !(Physics.Raycast(groundCheckPos.position, Vector3.down,groundCheckDist, LayerMask.GetMask("Ground")));
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
    }
}
