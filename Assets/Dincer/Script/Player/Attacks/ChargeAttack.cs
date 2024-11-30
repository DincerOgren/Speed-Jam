using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{

    public GameObject blastPrefab;

    public float reverseForce;

    public GameObject chargeParticle;
    
    Vector3 reverseDir;

    bool isChargeAttacking;

    public float chargeDuration;
    // perfect Charge Duratijn?
    // DMG layer etc.
    float chargeTimer = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            print(true);
        }
    }
    void ChargeBlast()
    {

    }




}
