using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    public KeyCode attackKey;


    LightAttack lightAttack;
    private void Start()
    {
        lightAttack = GetComponent<LightAttack>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            lightAttack.PerformLightAttack();
        }
    }
}
