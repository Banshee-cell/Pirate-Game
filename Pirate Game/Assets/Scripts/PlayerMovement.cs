using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float jumpCooldown = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float nextJumpTime = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // -------- MOVEMENT --------
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            movement += transform.forward;

        if (Keyboard.current.sKey.isPressed)
            movement -= transform.forward;

        if (Keyboard.current.aKey.isPressed)
            movement -= transform.right;

        if (Keyboard.current.dKey.isPressed)
            movement += transform.right;

        movement = movement.normalized;

        float currentSpeed = Keyboard.current.leftShiftKey.isPressed
            ? sprintSpeed
            : walkSpeed;

        controller.Move(movement * currentSpeed * Time.deltaTime);

        // -------- JUMP WITH COOLDOWN --------
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            Time.time >= nextJumpTime)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Start the cooldown
            nextJumpTime = Time.time + jumpCooldown;
        }

        // -------- GRAVITY --------
        velocity.y += gravity * Time.deltaTime;

        // -------- VERTICAL MOVEMENT --------
        controller.Move(velocity * Time.deltaTime);
    }
}