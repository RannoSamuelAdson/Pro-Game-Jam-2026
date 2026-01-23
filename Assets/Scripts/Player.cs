using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 20f;
    public float maxSpeed = 10f;
    public float verticalSpeed = 10f;
    public float damping = 5f; // How quickly the player slows down
    public float naturalDrag = 0.1f;   // Light resistance (set to 0 for perfect space)
    public float gravityCompensation = 9.8f * 0.2f; // 80% counter to gravity
    public static bool playerPaused = false;

    [Header("References")]
    public Camera playerCamera;

    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Rigidbody rb;


    public GameObject playerSprite;
    private bool facingRight = true; // Keep track of facing direction


    void Awake()
    {
        playerPaused = false;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();

        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }


    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();
   
    public static void SetGamePaused(bool paused)
    {
        playerPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (playerPaused) {
            //rb.Sleep();
            return;
        };
        HandleMovement();
        //ApplyDrag();
        //rb.AddForce(Vector3.up * gravityCompensation, ForceMode.Acceleration);
        HandleSpriteFlip();


    }


void HandleMovement()
{
    // Build input vector
    Vector3 inputDirection = transform.right * moveInput.x + Vector3.up * moveInput.y;
    inputDirection = inputDirection.normalized;

    // Apply acceleration when input exists
    if (inputDirection.sqrMagnitude > 0.01f)
    {
        // More responsive start/turn
        rb.AddForce(inputDirection * acceleration, ForceMode.Acceleration);
    }
    else
    {
        // Apply gentle drag manually (more responsive than built-in drag)
        rb.linearVelocity *= 1f - (Time.deltaTime * damping);
    }

    // Limit max speed
    if (rb.linearVelocity.magnitude > maxSpeed)
        rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
}


    void ApplyDrag()
    {
        // Light drag over time
        rb.linearVelocity *= (1f - naturalDrag * Time.fixedDeltaTime);
    }
    void HandleSpriteFlip()
    {
        float xVel = rb.linearVelocity.x;

        // Small dead zone to prevent jitter near zero velocity
        const float flipThreshold = 0.1f;

        if (xVel > flipThreshold && !facingRight)
        {
            facingRight = true;
            playerSprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            //Debug.Log("facing right");
        }
        else if (xVel < -flipThreshold && facingRight)
        {
            facingRight = false;
            playerSprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            //Debug.Log("facing left");
        }
    }


}
