using UnityEngine;
using System.Collections;
using System.Data;

public class FPSCamera : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float verticalRotation = 0f;

    [Header("FOV Settings")]
    public float normalFOV = 60f;
    public float sprintFOV = 80f;
    public float dashFOV = 90f; // Dash FOV
    public float fovTransitionSpeed = 5f;
    private Camera cam;

    [Header("Head Bobbing")]
    public bool enableHeadBobbing = true;
    public float bobFrequency = 2f;
    public float walkAmplitude = 0.05f;
    public float sprintAmplitudeMultiplier = 2f;
    private float currentAmplitude;
    private float bobTimer = 0f;
    private Vector3 defaultCameraPosition;

    [Header("Camera Shake")]
    public bool enableCameraShake = true;

    private bool isDashing = false; // Dash state

    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Get the Camera component
        cam = GetComponent<Camera>();
        cam.fieldOfView = normalFOV;

        // Store the default camera position
        defaultCameraPosition = transform.localPosition;
    }

    void Update()
    {
        HandleMouseLook();
        HandleFOV();
        HandleHeadBobbing();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player body (yaw)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate camera (pitch)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleFOV()
    {
        if (isDashing)
        {
            print("DASH FOVC");
            // Transition to dash FOV during dashing
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, dashFOV, fovTransitionSpeed*5 * Time.deltaTime);
            
        }
        else
        {
            // Check if sprinting
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            float transitionSpeed;
            if (isSprinting)
            {
                transitionSpeed = fovTransitionSpeed;
            }
            else
                transitionSpeed = fovTransitionSpeed / 5;

            // Smoothly transition to sprint or normal FOV
            float targetFOV = isSprinting ? sprintFOV : normalFOV;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, transitionSpeed * Time.deltaTime);
        }
    }

    private void HandleHeadBobbing()
    {
        if (!enableHeadBobbing)
            return;

        // Check if the player is moving
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0;

        // Determine amplitude based on sprinting
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float targetAmplitude = isSprinting ? walkAmplitude * sprintAmplitudeMultiplier : walkAmplitude;

        // Smoothly transition to the target amplitude
        currentAmplitude = Mathf.Lerp(currentAmplitude, targetAmplitude, Time.deltaTime * 10f);

        if (isMoving)
        {
            // Increment the bob timer based on movement
            bobTimer += Time.deltaTime * bobFrequency;

            // Calculate the bob offset
            float bobOffset = Mathf.Sin(bobTimer) * currentAmplitude;

            // Apply the offset to the camera's local position
            transform.localPosition = new Vector3(defaultCameraPosition.x, defaultCameraPosition.y + bobOffset, defaultCameraPosition.z);
        }
        else
        {
            // Reset bob timer and smoothly return to default position
            bobTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultCameraPosition, Time.deltaTime * 10f);
        }
    }

    public void TriggerCameraShake(float duration, float magnitude)
    {
        if (enableCameraShake)
        {
            StartCoroutine(Shake(duration, magnitude));
        }
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            // Generate random offsets
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply shake to the camera
            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restore the camera's original position
        transform.localPosition = originalPosition;
    }

    public void TriggerDash(float dashDuration)
    {
        StartCoroutine(DashFOVEffect(dashDuration));
    }

    private IEnumerator DashFOVEffect(float dashDuration)
    {
        isDashing = true;

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // End dash
        isDashing = false;
    }
}
