using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashForce = 20f;
    private Rigidbody rb;
    public float dashCooldown = 2f;

    float dashTimer = Mathf.Infinity;
    bool isDashing;
    public float dashResetTime = .2f;

    FPSCamera cam;

    void Start()
    {
        cam = Camera.main.GetComponent<FPSCamera>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        dashTimer += Time.deltaTime;

    }

    public void PerformDash()
    {
        if (dashTimer >= dashCooldown)
        {
            cam.TriggerDash(dashResetTime);
            dashTimer = 0;
            Vector3 dashDirection = transform.forward; // TODO: use input direction
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        }
        else
            return;
        Invoke(nameof(ResetDashState), dashResetTime); // Adjust timing as needed
    }

    private void ResetDashState()
    {
        isDashing = false;
        print("Dash resetted ");
    }
}