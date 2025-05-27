using UnityEngine;

public class WanderingEnemy : MonoBehaviour
{
    [Header("이동 능력치")]
    public float moveSpeed = 2f;
    public float patrolRadius = 5f;

    [Header("대기 시간 설정")]
    [Tooltip("목표 지점에 도착 후 최소 대기 시간")]
    public float minIdleTime = 1f;
    [Tooltip("목표 지점에 도착 후 최대 대기 시간")]
    public float maxIdleTime = 3f;

    // ▼▼▼ 공격 관련 변수 추가 ▼▼▼
    [Header("공격 능력치")]
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    // --- 내부 상태 변수 ---
    private enum PatrolState { Patrolling, Idle }
    private PatrolState currentState;

    private Vector2 startPosition;
    private Vector2 currentDestination;
    private float idleTimer;
    private float lastAttackTime; // 마지막 공격 시간 기록

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        startPosition = transform.position;
        SetNewRandomDestination();
        currentState = PatrolState.Patrolling;
    }

    void Update()
    {
        // ▼▼▼ 공격 로직 추가 ▼▼▼
        // 공격 쿨다운이 지났는지 확인
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // 공격 범위 내에 있는 플레이어를 감지
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    // 플레이어를 찾으면 즉시 공격하고 쿨다운 시작
                    Attack(hit.gameObject);
                    break; // 한 번만 공격
                }
            }
        }

        // --- 기존 순찰 로직 ---
        if (animator != null)
        {
            animator.SetBool("isMoving", currentState == PatrolState.Patrolling);
        }

        switch (currentState)
        {
            case PatrolState.Patrolling:
                MoveTowards(currentDestination);
                if (Vector2.Distance(transform.position, currentDestination) < 0.1f)
                {
                    currentState = PatrolState.Idle;
                    idleTimer = Random.Range(minIdleTime, maxIdleTime);
                }
                break;

            case PatrolState.Idle:
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    SetNewRandomDestination();
                    currentState = PatrolState.Patrolling;
                }
                break;
        }
    }

    private void SetNewRandomDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        currentDestination = startPosition + randomPoint;
    }

    // ▼▼▼ 방향 전환(Flip) 로직 수정 ▼▼▼
    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // 이동 방향 결정
        bool shouldFaceRight = destination.x > transform.position.x;

        // 현재 바라보는 방향 (localScale.x가 -1이면 오른쪽, 1이면 왼쪽)
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

    // ▼▼▼ 공격 함수 추가 ▼▼▼
    private void Attack(GameObject target)
    {
        lastAttackTime = Time.time;
        Debug.Log(gameObject.name + "이(가) " + target.name + "을(를) 공격!");
        if (animator != null) animator.SetTrigger("attack");

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
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

    void OnDrawGizmosSelected()
    {
        // 순찰 범위 (청록색)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : (Vector2)transform.position, patrolRadius);

        // 공격 범위 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}