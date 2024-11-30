using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 15f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    private Rigidbody rb;
    private Vector3 targetVelocity;

    public KeyCode sprintKey;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float speed;
        if (IsSprinting())
        {
            speed = sprintSpeed;
        }
        else
            speed = moveSpeed;

        targetVelocity = (transform.right * moveX + transform.forward * moveZ) * speed;
    }

    bool IsSprinting() => Input.GetKeyDown(sprintKey);
    public void ApplyMovement()
    {
        
        

        Vector3 currentVelocity = Vector3.Lerp(rb.velocity, targetVelocity, (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }
}
