using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 0.1f;
    public float smoothSpeed = 10f;

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Mouse movement
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * sensitivity;
        pitch -= mouseDelta.y * sensitivity;

        // Limit vertical camera movement
        pitch = Mathf.Clamp(pitch, -40f, 60f);

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Camera position
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 targetPosition = player.position + offset;

        // Smoothly follow the player
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // Apply rotation
        transform.rotation = rotation;
    }
}