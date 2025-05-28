using UnityEngine;

public class SoundSensitiveEnemy : MonoBehaviour
{
    [Header("능력치")]
    public float moveSpeed = 3f;
    public float hearingRange = 10f;
    public float attackRange = 1.5f;

    [Header("공격 설정")]
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    // --- 내부 상태 변수 ---
    private enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;

    private Vector3 lastKnownPosition;
    private Transform currentTarget;
    private float lastAttackTime;

    // ▼▼▼ 애니메이터 변수 추가 ▼▼▼
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = false;

    void Awake()
    {
        // ▼▼▼ 컴포넌트 찾아오기 (자식 포함) ▼▼▼
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable() { NoiseManager.OnNoiseMade += HearNoise; }
    void OnDisable() { NoiseManager.OnNoiseMade -= HearNoise; }

    void Update()
    {
        // ▼▼▼ isMoving 파라미터 업데이트 ▼▼▼
        if (animator != null)
        {
            animator.SetBool("isMoving", currentState == EnemyState.Chasing || currentState == EnemyState.Attacking);
        }

        switch (currentState)
        {
            case EnemyState.Chasing: HandleChasingState(); break;
            case EnemyState.Attacking: HandleAttackingState(); break;
        }
    }

    // ... (HandleChasingState, HandleAttackingState, HearNoise, FindTargetNearNoise, MoveTowards 함수는 이전과 동일) ...
    private void HandleChasingState()
    {
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) > hearingRange * 1.5f)
        {
            currentTarget = null;
            currentState = EnemyState.Idle;
            return;
        }

        if (currentTarget != null)
        {
            if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
            {
                currentState = EnemyState.Attacking;
                return;
            }
            MoveTowards(currentTarget.position);
        }
        else
        {
            MoveTowards(lastKnownPosition);
            if (Vector3.Distance(transform.position, lastKnownPosition) < 0.1f)
            {
                currentState = EnemyState.Idle;
            }
        }
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

    private void HearNoise(Vector3 noisePosition)
    {
        if (Vector3.Distance(transform.position, noisePosition) > hearingRange) return;

        lastKnownPosition = noisePosition;
        currentState = EnemyState.Chasing;
        FindTargetNearNoise(noisePosition);
    }

    private void FindTargetNearNoise(Vector3 noisePosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(noisePosition, 2f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                currentTarget = hit.transform;
                return;
            }
        }
    }

    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (destination.x > transform.position.x && !isFacingRight) Flip();
        else if (destination.x < transform.position.x && isFacingRight) Flip();
    }


    private void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log(gameObject.name + "이(가) " + currentTarget.name + "을(를) 공격!");

        // ▼▼▼ 애니메이션 트리거 호출 ▼▼▼
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }

        if (currentTarget != null)
        {
            PlayerHealth directHitTarget = currentTarget.GetComponent<PlayerHealth>();
            if (directHitTarget != null)
            {
                directHitTarget.TakeDamage(attackDamage);
            }
            else
            {
                GameObject playerBody = GameObject.FindGameObjectWithTag("Player");
                if (playerBody != null)
                {
                    PlayerHealth playerHealth = playerBody.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(attackDamage);
                    }
                }
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !isFacingRight;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}