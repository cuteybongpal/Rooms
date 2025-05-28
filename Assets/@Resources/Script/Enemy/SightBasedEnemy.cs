using UnityEngine;

public class SightBasedEnemy : MonoBehaviour
{
    [Header("능력치")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;

    [Header("시야 범위 설정")]
    public float playerSightRange = 2.5f;  // 플레이어를 발견하는 짧은 범위
    public float ghostSightRange = 10f;    // 유령을 발견하는 넓은 범위

    [Header("공격 설정")]
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    [Header("탐색 주기")]
    public float detectionInterval = 0.2f;

    // --- 내부 상태 변수 ---
    private enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;

    private Transform currentTarget;
    private float lastAttackTime;
    private float detectionTimer;

    private Animator animator;
    // ▼▼▼ isFacingRight 변수는 더 이상 필요 없으므로 삭제했습니다. ▼▼▼
    // private bool isFacingRight = false;
    // SpriteRenderer는 방향 전환에 사용되지 않지만, 다른 기능(예: 색상 변경)을 위해 남겨둘 수 있습니다.
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", currentState == EnemyState.Chasing || currentState == EnemyState.Attacking);
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                detectionTimer += Time.deltaTime;
                if (detectionTimer >= detectionInterval)
                {
                    detectionTimer = 0f;
                    FindTarget();
                }
                break;
            case EnemyState.Chasing:
                HandleChasingState();
                break;
            case EnemyState.Attacking:
                HandleAttackingState();
                break;
        }
    }

    private void FindTarget()
    {
        Collider2D[] ghostHits = Physics2D.OverlapCircleAll(transform.position, ghostSightRange);
        foreach (var hit in ghostHits)
        {
            if (hit.CompareTag("Player") && hit.GetComponent<GhostMovement>() != null)
            {
                currentTarget = hit.transform;
                currentState = EnemyState.Chasing;
                return;
            }
        }

        Collider2D[] playerHits = Physics2D.OverlapCircleAll(transform.position, playerSightRange);
        foreach (var hit in playerHits)
        {
            if (hit.CompareTag("Player") && hit.GetComponent<PlayerMovement>() != null)
            {
                currentTarget = hit.transform;
                currentState = EnemyState.Chasing;
                return;
            }
        }
    }

    private void HandleChasingState()
    {
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.position) > ghostSightRange * 1.2f)
        {
            currentTarget = null;
            currentState = EnemyState.Idle;
            return;
        }

        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
            return;
        }

        MoveTowards(currentTarget.position);
    }

    private void HandleAttackingState()
    {
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.position) > attackRange)
        {
            currentState = EnemyState.Chasing;
            return;
        }
        MoveTowards(currentTarget.position);
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        if (animator != null) animator.SetTrigger("attack");

        PlayerHealth playerHealth = currentTarget.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        else
        {
            GameObject playerBody = GameObject.FindGameObjectWithTag("Player");
            if (playerBody != null)
            {
                playerHealth = playerBody.GetComponent<PlayerHealth>();
                if (playerHealth != null) playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    // ▼▼▼ 방향 전환 로직 수정 ▼▼▼
    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // 가야할 방향이 오른쪽인지 결정
        bool shouldFaceRight = destination.x > transform.position.x;

        // 현재 오른쪽을 보고 있는지 확인 (localScale.x가 -1이면 오른쪽)
        bool isCurrentlyFacingRight = transform.localScale.x == -1;

        // 가야할 방향과 현재 방향이 다르면 뒤집기
        if (shouldFaceRight && !isCurrentlyFacingRight)
        {
            Flip();
        }
        else if (!shouldFaceRight && isCurrentlyFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // localScale.x 값만 반전시켜 방향 전환
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerSightRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ghostSightRange);
    }
}