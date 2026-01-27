using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerState currentState = PlayerState.Idle;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        ApplyState();
    }

    private void UpdateState()
    {
        if(currentState == PlayerState.Idle)
        {
            if(jumpAction.triggered)
            {
                currentState = PlayerState.Jump;
            }
            else if (moveAction.ReadValue<Vector2>().magnitude > 0.1f)
            {
                currentState = PlayerState.Running;
            }

        }
        else if(currentState == PlayerState.Running)
        {
            if (jumpAction.triggered)
            {
                currentState = PlayerState.Jump;
            }
            else if(moveAction.ReadValue<Vector2>().magnitude <= 0.1f)
            {
                currentState = PlayerState.Idle;
            }
        }
        else if(currentState == PlayerState.Jumping)
        {
            if(rb.linearVelocity.y < 0)
            {
                currentState = PlayerState.Falling;
            }
        }
        else if (currentState == PlayerState.Falling)
        {
            if (CheckGround())
            {
                currentState = PlayerState.Idle;
            }
        }
    }

    private void ApplyState()
    {
        switch(currentState)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Running:
                Move();
                break;
            case PlayerState.Jump:
                Jump();
                break;
            case PlayerState.Falling:
                Falling();
                break;

        }
    }

    private void Idle()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 movementDirection = input.normalized.x > 0 ? Vector3.right : input.normalized.x < 0 ? Vector3.left : Vector3.zero;
        rb.linearVelocity = movementDirection * playerData.moveSpeed; 
        transform.localScale = new Vector3(movementDirection.x != 0 ? movementDirection.x : transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void Jump()
    {
        if(CheckGround())
        {
            rb.AddForce(Vector2.up * playerData.jumpForce, ForceMode2D.Impulse);
            currentState = PlayerState.Jumping;
        }
        else
        {
            Debug.LogWarning("Tried to jump while not grounded.");
            currentState = PlayerState.Jumping;
        }
    }


    private void Falling()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        float movementDirection = input.normalized.x > 0 ? 1 : input.normalized.x < 0 ? -1 : 0;
        rb.linearVelocity = new Vector2(movementDirection * playerData.moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(movementDirection != 0 ? movementDirection : transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.1f);
    }
}

public enum PlayerState
{
    Idle,    
    Running,
    Jump,
    Jumping,
    Falling
}
