// RetreatingAttackerEnemy.cs (지속 공격 버전)
using UnityEngine;
using System.Collections;

public class RetreatingAttackerEnemy : MonoBehaviour
{
    [Header("감지 및 도주")]
    public float fearRange = 8f;
    public float retreatSpeed = 4f;
    public float retreatDuration = 2f;

    [Header("돌진 공격 설정")]
    public float attackPrepareTime = 1f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.5f;
    public int dashDamage = 20;
    public float attackCooldown = 3f;

    // sightRange는 이제 공격을 지속할지 판단하는 용도로도 사용됩니다.
    // fearRange보다 크거나 같게 설정하는 것이 일반적입니다.
    [Header("시야/재교전 범위")]
    public float sightRange = 8f;


    private enum EnemyState { Idle, Retreating, PreparingAttack, Dashing, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    private Transform playerTarget;
    private float stateTimer;
    private Vector2 lastKnownPlayerDirection;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                FindPlayer();
                if (animator != null) animator.SetBool("isMoving", false);
                break;
            case EnemyState.Retreating:
                HandleRetreating();
                if (animator != null) animator.SetBool("isMoving", true);
                break;
            case EnemyState.PreparingAttack:
                HandlePreparingAttack();
                if (animator != null) animator.SetBool("isMoving", false);
                break;
            case EnemyState.Dashing:
                HandleDashing();
                if (animator != null) animator.SetBool("isMoving", true);
                break;
            case EnemyState.Cooldown:
                HandleCooldown(); // 수정된 함수가 호출됩니다.
                if (animator != null) animator.SetBool("isMoving", false);
                break;
        }
    }

    void FindPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, fearRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
                currentState = EnemyState.Retreating;
                stateTimer = retreatDuration;
                return;
            }
        }
    }

    void HandleRetreating()
    {
        if (playerTarget == null || Vector2.Distance(transform.position, playerTarget.position) > fearRange * 1.2f) // 도주 중 플레이어를 너무 멀리 놓치면 Idle
        {
            playerTarget = null;
            currentState = EnemyState.Idle;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 retreatDirection = (transform.position - playerTarget.position).normalized;
        rb.linearVelocity = retreatDirection * retreatSpeed;
        FlipTowardsDirection(retreatDirection);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            currentState = EnemyState.PreparingAttack;
            stateTimer = attackPrepareTime;
            rb.linearVelocity = Vector2.zero;
            lastKnownPlayerDirection = (playerTarget.position - transform.position).normalized;
        }
    }

    void HandlePreparingAttack()
    {
        if (playerTarget != null) // 공격 준비 중에도 플레이어 방향으로 계속 조준
        {
            lastKnownPlayerDirection = (playerTarget.position - transform.position).normalized;
        }
        AimTowardsDirection(lastKnownPlayerDirection);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            currentState = EnemyState.Dashing;
            stateTimer = dashDuration;
            rb.linearVelocity = lastKnownPlayerDirection * dashSpeed;
            if (animator != null) animator.SetTrigger("attack");
        }
    }

    void HandleDashing()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            StopDashAndEnterCooldown();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Dashing)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(dashDamage);
                }
                StopDashAndEnterCooldown();
            }
            else if (collision.gameObject.CompareTag("Obstacles"))
            {
                StopDashAndEnterCooldown();
            }
        }
    }

    void StopDashAndEnterCooldown()
    {
        rb.linearVelocity = Vector2.zero;
        currentState = EnemyState.Cooldown;
        stateTimer = attackCooldown;
    }

    // ▼▼▼ 수정된 HandleCooldown 함수 ▼▼▼
    void HandleCooldown()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            // 쿨다운 종료. 플레이어가 여전히 감지 범위(sightRange) 내에 있고 유효한 타겟이라면 다시 공격 준비
            if (playerTarget != null && Vector2.Distance(transform.position, playerTarget.position) <= sightRange)
            {
                currentState = EnemyState.PreparingAttack;
                stateTimer = attackPrepareTime;
                // 현재 플레이어의 방향으로 다시 조준
                lastKnownPlayerDirection = (playerTarget.position - transform.position).normalized;
                Debug.Log("쿨다운 종료. 재공격 준비!");
            }
            else
            {
                // 플레이어를 놓쳤거나 범위 밖이면 Idle 상태로 전환
                playerTarget = null; // 타겟 정보 초기화
                currentState = EnemyState.Idle;
                Debug.Log("쿨다운 종료. 평시 상태로 복귀.");
            }
        }
    }

    void FlipTowardsDirection(Vector2 direction)
    {
        if (direction.x > 0.01f && transform.localScale.x > 0) FlipLocalScale();
        else if (direction.x < -0.01f && transform.localScale.x < 0) FlipLocalScale();
    }

    void AimTowardsDirection(Vector2 direction)
    {
        FlipTowardsDirection(direction);
    }

    void FlipLocalScale()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; // 공포 범위 (이 범위 안에 들어오면 도망)
        Gizmos.DrawWireSphere(transform.position, fearRange);

        Gizmos.color = Color.yellow; // 시야/재교전 범위 (쿨다운 후 이 범위 안에 있으면 다시 공격)
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}