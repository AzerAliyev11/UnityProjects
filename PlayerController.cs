using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float rotationAngle;
    private Vector3 rotatedMove;
    private Player playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    protected bool Plant = false;
    private bool isFirst = true;

    private void Awake()
    {
        playerInput = new Player();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 MovementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(MovementInput.x, 0f, MovementInput.y);

        if (move != Vector3.zero)
        {
            if (isFirst)
            {
                rotationAngle = Vector3.Angle(Vector3.forward, transform.forward);
            }
            isFirst = false;
            rotatedMove = Quaternion.AngleAxis(rotationAngle, Vector3.up) * move;
            float angle = Mathf.Atan2(rotatedMove.x, rotatedMove.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            rotatedMove = Vector3.zero;
            isFirst = true;
        }


        controller.Move(rotatedMove * playerSpeed * Time.deltaTime);

        // Changes the height position of the player..
        if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
