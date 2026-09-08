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

    [Header("Camera")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float nextJumpTime = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Automatically use the Main Camera if one isn't assigned
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // -------- MOVEMENT INPUT --------
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            movement.z += 1f;

        if (Keyboard.current.sKey.isPressed)
            movement.z -= 1f;

        if (Keyboard.current.aKey.isPressed)
            movement.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            movement.x += 1f;

        movement = movement.normalized;

        // -------- CAMERA-RELATIVE MOVEMENT --------
        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Prevent looking up/down from affecting movement
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            movement =
                cameraForward * movement.z +
                cameraRight * movement.x;
        }

        // -------- SPEED --------
        float currentSpeed = Keyboard.current.leftShiftKey.isPressed
            ? sprintSpeed
            : walkSpeed;

        controller.Move(movement * currentSpeed * Time.deltaTime);

        // -------- JUMP WITH COOLDOWN --------
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            Time.time >= nextJumpTime)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            nextJumpTime = Time.time + jumpCooldown;
        }

        // -------- GRAVITY --------
        velocity.y += gravity * Time.deltaTime;

        // -------- VERTICAL MOVEMENT --------
        controller.Move(velocity * Time.deltaTime);
    }
}