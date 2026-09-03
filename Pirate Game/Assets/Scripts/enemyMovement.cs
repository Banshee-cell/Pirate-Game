using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Gravity")]
    public float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (player == null)
            return;

        // Direction toward the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        direction = direction.normalized;

        // Horizontal movement
        Vector3 movement = direction * moveSpeed;

        // Apply gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        // Move the enemy
        controller.Move(movement * Time.deltaTime);

        // Face the player
        if (direction != Vector3.zero)
        {
            transform.LookAt(
                new Vector3(
                    player.position.x,
                    transform.position.y,
                    player.position.z
                )
            );
        }
    }
}