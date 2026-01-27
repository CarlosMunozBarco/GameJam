using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerState currentState = PlayerState.Idle;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    public Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerData.currentHealth = playerData.maxHealth;
        UIManager.Instance.UpdateLifes(playerData.currentHealth, playerData.maxHealth);
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
            case PlayerState.Falling:
            case PlayerState.Jumping:
                Move();
                break;
            case PlayerState.Jump:
                Jump();
                break;
            

        }
    }

    private void Idle()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetFloat("SpeedX", 0);
        animator.SetFloat("SpeedY", rb.linearVelocity.y);
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
            currentState = PlayerState.Running;
        }
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        float movementDirection = input.normalized.x > 0 ? 1 : input.normalized.x < 0 ? -1 : 0;
        rb.linearVelocity = new Vector2(movementDirection * playerData.moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(movementDirection != 0 ? movementDirection : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        animator.SetFloat("SpeedX", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("SpeedY", rb.linearVelocity.y);

        Debug.Log("SpeedX: " + rb.linearVelocity.x + " SpeedY: " + rb.linearVelocity.y);
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckRadius);
    }

    public void TakeDamage(float damage)
    {
        playerData.currentHealth -= damage;
        if(playerData.currentHealth <= 0)
        {
            Debug.Log("Player Dead");
            Destroy(gameObject);
        }
        UIManager.Instance.UpdateLifes(playerData.currentHealth, playerData.maxHealth);
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
