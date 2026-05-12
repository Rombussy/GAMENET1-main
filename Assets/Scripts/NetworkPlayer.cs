using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer: NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float groundedGravity = -2f;

    private CharacterController controller;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!IsOwner)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputDir = new Vector2(horizontalInput, verticalInput);
        
        if (IsServer)
        {
            MovePlayer(inputDir);
        }
        else
        {
            MovePlayerRPC(inputDir);
        }
    }

    [Rpc(SendTo.Server)]
    private void MovePlayerRPC(Vector2 movementInput)
    {
        MovePlayer(movementInput);
    }

    private void MovePlayer(Vector2 movementInput)
    {
        if (controller.isGrounded && verticalVelocity < 0f) {
            verticalVelocity = groundedGravity;
        } 
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        Vector3 horizontalMovement = moveDir * moveSpeed;
        Vector3 verticalMovement = Vector3.up * verticalVelocity;
        Vector3 finalMovement = horizontalMovement + verticalMovement;

        controller.Move(finalMovement * Time.deltaTime);
    }
}