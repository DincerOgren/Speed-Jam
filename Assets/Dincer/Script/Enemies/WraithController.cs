using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WraithController : MonoBehaviour
{
    [Header("Def Variables")]
    public float activateRange;
    public float moveTime = 2f;
    public float moveSpeed;
    public LayerMask playerLayer;
    bool startedWalking;

    [Header("Throw Skil")]
    public GameObject projPrefab;
    public float projSpeed;
    public float timeBetweenProjectiles;
    public float maxProjAmount;
    float projTimer;
    public float projectileSkillRange;
    public float projLifetime;
    public float projDamage;
    //add projectiles script for impact effects;

    [Header("Meteor Shower Skill")]
    public GameObject meteorPrefab;
    //add meteors script for impact efect
    public float maxMeteorHeight;
    public float meteorAmount;
    //Randomize meteor drops karakterin etrafýna,
    public float meteorDamage;


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
        rb=GetComponent<Rigidbody>();
        anim=GetComponent<Animator>();
        player=GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        
    }


    private void Update()
    {

        UpdateAnimator();

        if (startedWalking)
        {
            transform.LookAt(player);

        }
        if (IsInActivateRange() && !startedWalking)
        {
            startedWalking = true;
            StartCoroutine(WalkTowardsPlayer());
        }

    }


    void UpdateAnimator()
    {
        anim.SetFloat("speed", rb.velocity.magnitude);
    }
    private IEnumerator WalkTowardsPlayer()
    {
        float movTime = 0;   

        while (moveTime>movTime)
        {
            print("In while " + movTime);
            Vector3 dir = (player.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            movTime += Time.deltaTime;
            yield return null;
        }
        print("Move Ended");
    }

    bool IsInActivateRange() => Physics.CheckSphere(transform.position, activateRange, playerLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, activateRange);
    }
}
