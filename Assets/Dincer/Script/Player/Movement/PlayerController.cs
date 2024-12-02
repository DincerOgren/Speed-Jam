using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody rb;
    private PlayerMovement movement;
    private Dash dash;
    public KeyCode dashKey;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        dash = GetComponent<Dash>();
    }

    void Update()
    {
        //movement.HandleMovementInput();
        if (Input.GetKeyDown(dashKey))
        {
           // dash.PerformDash();
        }
    }

    void FixedUpdate()
    {
        //movement.ApplyMovement();
    }
}
