using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Player player;
    [Header("Stats")]
    [SerializeField] private int maxLives;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float speed;
    [SerializeField] private float followRange = 4;
    [SerializeField] private float followRangeY = 0.5f;
    public EnemyState state;
    void Awake()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if (!player)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
    }
    void Update()
    {
        UpdateState();
        ApplyState();
    }
    void FixedUpdate()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
    }

    private void UpdateState()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < followRange && Mathf.Abs(transform.position.y - playerTransform.position.y) < followRangeY)
        {
            state = EnemyState.Follow;
        }else
        {
            state = EnemyState.Idle;
        }
    }

    private void ApplyState()
    {
        if(state == EnemyState.Follow)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }
    
}
 public enum EnemyState
{
    Idle,
    Follow
}
