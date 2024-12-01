using UnityEngine;

public class MothmanController : MonoBehaviour
{
    public float hoverForce = 5f; // Constant upward force
    public float moveSpeed = 3f;  // Movement speed
    public float oscillationAmplitude = 0.5f; // How much it moves up and down
    public float oscillationFrequency = 2f;   // Speed of the oscillation
    public Transform target; 

    private Rigidbody rb;
    private Vector3 hoverDirection;



    [Header("Gravity")]
    public float maxHeight = 50f;
    public float graivtyForce = 10f;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
    }

    void FixedUpdate()
    {
        SimulateGravity();
        //SimulateFlight();
        //FollowTarget();
    }

    void SimulateGravity()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo,1000, LayerMask.GetMask("Ground")))
        {
            float dist =(maxHeight-transform.position.y);
            print("Distance = " + dist);
            //if (dist < maxHeight)
            //{
            //    rb.AddForce(Vector3.up * hoverForce, ForceMode.VelocityChange);
            //}
            //else
            //    rb.AddForce(Vector3.down * hoverForce, ForceMode.VelocityChange);
            //if (dist>maxHeight)
            //{
            //    return;
            //}
            float amountToLift;
            amountToLift = dist * hoverForce - GetPlayerVerticalVelocity().y;
            print("AmountotLift = " + amountToLift);    
            Vector3 liftForce = new(0, amountToLift, 0);
            rb.AddForce(liftForce, ForceMode.VelocityChange);



        }



        
    }

    private Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0, rb.velocity.y, 0);
    }
    private void SimulateFlight()
    {
        // Apply constant upward force for hovering
        rb.AddForce(Vector3.up * hoverForce, ForceMode.Force);

        // Add oscillating movement (up and down)
        float oscillation = Mathf.Sin(Time.time * oscillationFrequency) * oscillationAmplitude;
        hoverDirection = new Vector3(0, oscillation, 0);

        // Apply hover oscillation
        rb.velocity += hoverDirection;
    }

    private void FollowTarget()
    {
        if (target == null) return;

        // Calculate direction to the target
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0; // Keep movement horizontal only

        // Apply forward force toward the target
        rb.AddForce(directionToTarget * moveSpeed, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 100);
    }
}
