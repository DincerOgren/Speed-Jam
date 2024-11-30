using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    public KeyCode attackKey;


    LightAttack lightAttack;
    Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
        lightAttack = GetComponent<LightAttack>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            lightAttack.PerformLightAttack(cam.forward);
        }
    }
}
