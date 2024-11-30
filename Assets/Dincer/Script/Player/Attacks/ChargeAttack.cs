using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{

    [SerializeField]
    public Animator arms;
    public ParticleSystem blastPrefab;

    public float reverseForce;

    public GameObject chargeParticle;

    Vector3 reverseDir;


    public float chargeDuration;
    // perfect Charge Duratijn?
    // DMG layer etc.
    float chargeTimer = 0;

    float minHoldTime = .3f;

    float firstChargeTime = 2f;
    bool isStartedChargingAttack;
    bool firstCharge;
    bool chargeHold;
    float holdTimer = 0;
    float maxChargeTime = 3f;

    [Header("Force")]
    public float forceDuration = 2f;
    bool forceStarted = false;
    float forceTimer;
    PlayerMovement pmRef;



    Rigidbody rb;
    Transform lookDir;
    private void Awake()
    {
        pmRef=GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        lookDir = Camera.main.transform;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= minHoldTime)
            {

                StartCharging();

                if (chargeTimer >= maxChargeTime)
                {
                    chargeTimer = maxChargeTime;

                }
                else
                    chargeTimer += Time.deltaTime;

            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {

            chargeTimer = 0;
            holdTimer = 0;

            if (!isStartedChargingAttack)
            {
                return;
            }
            ReleaseBlast();
        }

        CheckForceTimer();
        
        
        UpdateAnim();

    }

    void UpdateAnim()
    {
        arms.SetBool("chargeHold", chargeHold);
        arms.SetBool("firstCharge", firstCharge);
    }

    void CheckForceTimer()
    {
        if (forceTimer >= forceDuration)
        {
            forceStarted = false;
            pmRef.SetPushbackActive(forceStarted);
            forceTimer = 0;
            return;
        }


        if (forceStarted)
        {
            forceTimer += Time.deltaTime;
        }
       // else
            //forceTimer = 0;

    }


    #region ChargeSection
    void StartCharging()
    {
        //if (chargeTimer >= firstChargeTime)
        //{
        //firstCharge = false;
        if (isStartedChargingAttack)
        {
            FirstCharge();
            return;
        }
        else
        {
            isStartedChargingAttack = true;
            firstCharge = true;
        }



    }

    void FirstCharge()
    {

        if (!firstCharge)
        {

            HoldCharge();



            print("Fþrsrt if")

                ;


            //if(pTimer>=pTime
            //perfectTime=.2f

            //PerfectRelease();
            //perfecttimer+=Time.deltatime;
            //Else
            //HoldCharge();
            //or make it like animEvents?

        }
        else
        {
            print("First Chargfe");
        }

    }

    public void EndFirstCharge()
    {
        firstCharge = false;
        print("END FIRST ");

    }
    void HoldCharge()
    {
        chargeHold = true;
        chargeParticle.SetActive(true);
        chargeDuration = chargeTimer;
    }

    void ReleaseBlast()
    {
        arms.SetTrigger("Release");
        print("Release Blast");
        firstCharge = false;
        chargeHold = false;
        isStartedChargingAttack = false;
        chargeParticle.SetActive(false);
        blastPrefab.Play();

        //Damage with chargetime*dmg
        AddReverseForce();
    }

    #endregion

    private void AddReverseForce()
    {

       

        forceStarted = true;
        pmRef.SetPushbackActive(forceStarted);
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody assigned for reverse force!");
            return;
        }


        reverseDir = -lookDir.forward;

        float forceStrength = reverseForce * chargeDuration;

        rb.AddForce(reverseDir * forceStrength, ForceMode.Impulse);

        

        // Calculate the reverse direction (opposite to the forward direction)
        //reverseDir = -lookDir.forward;
        //float forceStrength = reverseForce * chargeDuration;

        //print("Force Str = " + forceStrength);
        //Debug.Log("Reverse force applied: " + reverseDir);


        //reverseDir = reverseDir.normalized;

        //// Apply velocity directly along the reverse direction
        //rb.velocity = reverseDir * forceStrength;

        //Debug.Log("Reverse velocity applied: " + rb.velocity);

        //chargeDuration = 0;

    }


    public void ResetRelease()
    {
        arms.ResetTrigger("Release");

    }


}
