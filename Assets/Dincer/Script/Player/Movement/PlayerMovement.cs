//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//public float moveSpeed = 10f;
//public float sprintSpeed = 15f;
//public float acceleration = 15f;
//public float deceleration = 20f;

//private Rigidbody rb;
//private Vector3 targetVelocity;

//public KeyCode sprintKey;

//public bool isPushbackActive;

//void Start()
//{
//    rb = GetComponent<Rigidbody>();
//}

//public void HandleMovementInput()
//{
//    float moveX = Input.GetAxis("Horizontal");
//    float moveZ = Input.GetAxis("Vertical");

//    // If there is input, calculate target velocity
//    if (Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveZ) > 0.01f)
//    {
//        float speed = IsSprinting() ? sprintSpeed : moveSpeed;
//        targetVelocity = (transform.right * moveX + transform.forward * moveZ) * speed;
//    }


//}

//bool IsSprinting() => Input.GetKeyDown(sprintKey);
//public void ApplyMovement()
//{


//    if (isPushbackActive)
//    {
//        return;
//    }

//    if (targetVelocity.magnitude > 0)
//    {
//        Vector3 currentVelocity = Vector3.Lerp(rb.velocity, targetVelocity, (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime);
//        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);

//    }
//}

//public void SetPushbackActive(bool active)
//{
//    isPushbackActive = active;

//    //if (active)
//    //{
//    //    rb.velocity = Vector3.zero; // Optionally reset velocity when enabling pushback
//    //}
//}
//}

// Old Buggy script



//Characther Controlleer
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float sprintSpeed = 15f;
//    public float acceleration = 15f;
//    public float deceleration = 20f;
//    public float gravity = 9.81f;

//    private CharacterController controller;
//    private Vector3 velocity;
//    private float currentSpeed;

//    public KeyCode sprintKey;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    public void HandleMovementInput()
//    {
//        float moveX = Input.GetAxis("Horizontal");
//        float moveZ = Input.GetAxis("Vertical");

//        // Determine the target speed (sprint or normal)
//        float targetSpeed = Input.GetKey(sprintKey) ? sprintSpeed : moveSpeed;

//        // Calculate desired velocity
//        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

//        if (moveDirection.magnitude > 0.01f)
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
//        }
//        else
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
//        }

//        velocity = moveDirection.normalized * currentSpeed;
//    }

//    public void ApplyMovement()
//    {
//        // Apply gravity
//        if (!controller.isGrounded)
//        {
//            velocity.y -= gravity * Time.deltaTime;
//        }

//        // Move the player
//        controller.Move(velocity * Time.deltaTime);
//    }
//}


//new cont

//using UnityEngine;


//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float walkSpeed = 10f; // Normal movement speed
//    public float sprintSpeed = 15f; // Sprint movement speed
//    public float acceleration = 20f; // Acceleration rate
//    public float deceleration = 25f; // Deceleration rate
//    public float gravity = 9.81f; // Gravity
//    public float jumpHeight = 2f; // Jump height

//    [Header("Pushback/Physics Interaction")]
//    public Rigidbody rb; // Reference to Rigidbody for external forces
//    public bool isPushbackActive = false; // Disable movement during pushback
//    public float pushForceDecay = 5f; // How quickly pushback forces decay

//    [Header("Ground Detection")]
//    public float groundCheckDistance = 0.3f; // Distance for ground detection
//    public LayerMask groundLayer; // Layers considered as ground

//    private CharacterController controller;
//    private Vector3 moveDirection;
//    private float currentSpeed;
//    private Vector3 externalForce; // Stores force from Rigidbody
//    private bool isGrounded;
//    private Vector3 velocity; // For gravity and jumping

//    public KeyCode sprintKey = KeyCode.LeftShift;
//    public KeyCode jumpKey = KeyCode.Space;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//        rb = GetComponent<Rigidbody>();
//        if (rb != null) rb.isKinematic = true; // Prevent Rigidbody from interfering with CharacterController
//    }

//    void Update()
//    {
//        if (!isPushbackActive)
//        {
//            HandleMovementInput();
//        }

//        HandleGravity();
//    }

//    void FixedUpdate()
//    {
//        if (isPushbackActive)
//        {
//            ApplyPushbackForces();
//        }
//        else
//        {
//            ApplyMovement();
//        }
//    }

//    private void HandleMovementInput()
//    {
//        // Ground detection
//        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.2f, groundLayer);

//        // Get input
//        float moveX = Input.GetAxis("Horizontal");
//        float moveZ = Input.GetAxis("Vertical");

//        // Determine target speed
//        float targetSpeed = Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed;

//        // Calculate direction and apply acceleration
//        Vector3 inputDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
//        if (inputDirection.magnitude > 0.1f)
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
//        }
//        else
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
//        }

//        // Set move direction
//        moveDirection = inputDirection * currentSpeed;

//        // Handle jumping
//        if (isGrounded && Input.GetKeyDown(jumpKey))
//        {
//            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
//        }
//    }

//    private void HandleGravity()
//    {
//        if (!isGrounded)
//        {
//            velocity.y -= gravity * Time.deltaTime; // Apply gravity
//        }
//        else if (velocity.y < 0)
//        {
//            velocity.y = -2f; // Prevent jittering on the ground
//        }
//    }

//    private void ApplyMovement()
//    {
//        // Combine movement direction and vertical velocity
//        Vector3 finalMovement = moveDirection + Vector3.up * velocity.y;

//        // Apply movement via CharacterController
//        controller.Move(finalMovement * Time.deltaTime);
//    }

//    public void ApplyPushback(Vector3 force)
//    {
//        isPushbackActive = true;
//        externalForce = force;

//        // Temporarily disable CharacterController for pushback
//        controller.enabled = false;
//        rb.isKinematic = false;
//    }

//    private void ApplyPushbackForces()
//    {
//        if (externalForce.magnitude > 0.1f)
//        {
//            // Apply the pushback force
//            rb.velocity = externalForce;

//            // Decay the pushback force over time
//            externalForce = Vector3.Lerp(externalForce, Vector3.zero, pushForceDecay * Time.deltaTime);
//        }
//        else
//        {
//            // Stop pushback and re-enable CharacterController
//            externalForce = Vector3.zero;
//            rb.velocity = Vector3.zero;
//            rb.isKinematic = true;
//            controller.enabled = true;
//            isPushbackActive = false;
//        }
//    }
//} 

//brand nww


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float acceleration = 20f; 
    public float deceleration = 25f; 
    public float gravity = 9.81f;
    public float maxGravity = 100f;
    public float jumpHeight = 2f; 

    [Header("Pushback/Physics Interaction")]
    public Rigidbody rb; 
    public bool isPushbackActive = false; 
    private Vector3 pushVelocity;
    public float horizontalDecay = 1.5f;
    bool wasGrounded;
    public float minVelocityThreshold = 0.1f;


    [Header("Ground Detection")]
    public float groundCheckDistance = 0.3f; 
    public LayerMask groundLayer; 

    private CharacterController controller;
    private Vector3 moveDirection;
    private float currentSpeed;
    private Vector3 externalForce; 
    private bool isGrounded;
    private Vector3 velocity; 

    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; 
    }

    void Update()
    {
        // Handle ground detection
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, 0.2f, groundLayer);

        if (!isPushbackActive)
        {
            HandleMovementInput();
            HandleJump(); 

        }

        if (isGrounded && !wasGrounded)
        {
            OnGrounded(); 
        }

        wasGrounded = isGrounded;


        HandleGravity();
        

        
    }

    void FixedUpdate()
    {
        if (isPushbackActive)
        {
            ApplyPushbackForces();
        }
        else
        {
            ApplyMovement();
        }
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float targetSpeed = Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed;

       
        Vector3 inputDirection = (transform.right * moveX + transform.forward * moveZ).normalized;
        if (inputDirection.magnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Set move direction
        moveDirection = inputDirection * currentSpeed;
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity); 
        }
    }

    private void HandleGravity()
    {
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            if (Mathf.Abs(velocity.y) >=maxGravity)
            {
                velocity.y = -maxGravity;
            }
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f; 
        }
    }

    private void ApplyMovement()
    {
        
        Vector3 finalMovement = moveDirection + Vector3.up * velocity.y;

        
        controller.Move(finalMovement * Time.deltaTime);
    }


    public void ApplyPushback(Vector3 initialForce)
    {
        pushVelocity = initialForce; 
        isPushbackActive = true;
    }
    //public void ApplyPushback(Vector3 force)
    //{
    //    isPushbackActive = true;
    //    externalForce = force;

    //    // Temporarily disable CharacterController for pushback
    //    controller.enabled = false;
    //    rb.isKinematic = false;
    //}

    //private void ApplyPushbackForces()
    //{


    //    if (pushVelocity.magnitude < minVelocityThreshold)
    //    {
    //        pushVelocity = Vector3.zero;
    //        ResetPushback();
    //        return;
    //    }



    //    pushVelocity.x = Mathf.Lerp(pushVelocity.x, 0, horizontalDecay * Time.deltaTime);
    //    pushVelocity.z = Mathf.Lerp(pushVelocity.z, 0, horizontalDecay * Time.deltaTime);

    //    if (!isGrounded)
    //    {
    //        pushVelocity.y -= gravity * Time.deltaTime; // Apply gravity
    //    }
    //    else
    //    {
    //        pushVelocity.y = 0; // Prevent downward motion while grounded
    //    }

    //    controller.Move(pushVelocity * Time.deltaTime);
    //}

    private void ApplyPushbackForces()
    {
        // Separate horizontal and vertical velocity components
        Vector3 horizontalVelocity = new Vector3(pushVelocity.x, 0, pushVelocity.z); // Only x and z
        float verticalVelocity = pushVelocity.y; // Only y

        print("HORIZONTAL MAG " + horizontalVelocity.magnitude);
        // Check if horizontal velocity is negligible and reset if true
        if (horizontalVelocity.magnitude < minVelocityThreshold && verticalVelocity <.5f && (isGrounded || verticalVelocity <= 0))
        {
            pushVelocity = Vector3.zero;
            ResetPushback();
            return;
        }

        // Apply horizontal decay (reduce x and z velocity gradually)
        pushVelocity.x = Mathf.Lerp(pushVelocity.x, 0, horizontalDecay * Time.deltaTime);
        pushVelocity.z = Mathf.Lerp(pushVelocity.z, 0, horizontalDecay * Time.deltaTime);

        // Apply gravity to vertical velocity if not grounded
        if (!isGrounded)
        {
            
            pushVelocity.y -= gravity * Time.deltaTime; // Apply gravity
        }
        else
        {
            pushVelocity.y = Mathf.Max(0, pushVelocity.y); // Prevent downward motion when grounded
        }

        // Move the character using the calculated velocity
        controller.Move(pushVelocity * Time.deltaTime);
    }

    void OnGrounded()
    {
        print("Pushback reset");
        ResetPushback();
    }
    private void ResetPushback()
    {
        isPushbackActive = false;
        pushVelocity = Vector3.zero;
    }

    //private void ApplyPushbackForces()
    //{
    //    if (externalForce.magnitude > 0.1f)
    //    {
    //        // Apply the pushback force
    //        rb.velocity = externalForce;

    //        // Decay the pushback force over time
    //        externalForce = Vector3.Lerp(externalForce, Vector3.zero, pushForceDecay * Time.deltaTime);
    //    }
    //    else
    //    {
    //        // Stop pushback and re-enable CharacterController
    //        externalForce = Vector3.zero;
    //        rb.velocity = Vector3.zero;
    //        rb.isKinematic = true;
    //        controller.enabled = true;
    //        isPushbackActive = false;
    //    }
    //}
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, 0.2f);
    }
}
