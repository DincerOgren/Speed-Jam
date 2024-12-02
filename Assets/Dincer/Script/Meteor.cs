using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float meteorFallSpeed;
    public float indicatorYOffset;

    float meteorFallWait=2f;
    float fallTimer=0;

    Transform indicator;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        indicator=transform.GetChild(0);
        indicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (fallTimer < meteorFallWait)
        {
            CreateIndicator();
            fallTimer += Time.deltaTime;
            return;
        }
        else
            indicator.gameObject.SetActive(false);
        Descend();
    }

    private void CreateIndicator()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
        {

            indicator.position = hit.point + Vector3.up * indicatorYOffset;
            indicator.gameObject.SetActive(true);
        }
    }

    void Descend()
    {
        rb.velocity = Vector3.down * meteorFallSpeed;
    }

    public void SetWaitSpeed(float waitSpeed)
    {
        meteorFallWait = waitSpeed;
    }
    public void SetFallSpeed(float fallSpeed)
    {
        meteorFallSpeed = fallSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    
    }
}
