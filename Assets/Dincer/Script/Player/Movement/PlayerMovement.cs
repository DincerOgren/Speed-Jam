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

    public bool isPushbackActive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // If there is input, calculate target velocity
        if (Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveZ) > 0.01f)
        {
            float speed = IsSprinting() ? sprintSpeed : moveSpeed;
            targetVelocity = (transform.right * moveX + transform.forward * moveZ) * speed;
        }


    }

    bool IsSprinting() => Input.GetKeyDown(sprintKey);
    public void ApplyMovement()
    {


        if (isPushbackActive)
        {
            return;
        }

        if (targetVelocity.magnitude > 0)
        {
            Vector3 currentVelocity = Vector3.Lerp(rb.velocity, targetVelocity, (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);
            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

        }
    }

    public void SetPushbackActive(bool active)
    {
        isPushbackActive = active;

        //if (active)
        //{
        //    rb.velocity = Vector3.zero; // Optionally reset velocity when enabling pushback
        //}
    }
}
