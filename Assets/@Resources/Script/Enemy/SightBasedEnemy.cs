using UnityEngine;

public class SightBasedEnemy : MonoBehaviour
{
    [Header("�ɷ�ġ")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;

    [Header("�þ� ���� ����")]
    public float playerSightRange = 2.5f;  // �÷��̾ �߰��ϴ� ª�� ����
    public float ghostSightRange = 10f;    // ������ �߰��ϴ� ���� ����

    [Header("���� ����")]
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    [Header("Ž�� �ֱ�")]
    public float detectionInterval = 0.2f;

    // --- ���� ���� ���� ---
    private enum EnemyState { Idle, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Idle;

    private Transform currentTarget;
    private float lastAttackTime;
    private float detectionTimer;

    private Animator animator;
    // ���� isFacingRight ������ �� �̻� �ʿ� �����Ƿ� �����߽��ϴ�. ����
    // private bool isFacingRight = false;
    // SpriteRenderer�� ���� ��ȯ�� ������ ������, �ٸ� ���(��: ���� ����)�� ���� ���ܵ� �� �ֽ��ϴ�.
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

    // ���� ���� ��ȯ ���� ���� ����
    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // ������ ������ ���������� ����
        bool shouldFaceRight = destination.x > transform.position.x;

        // ���� �������� ���� �ִ��� Ȯ�� (localScale.x�� -1�̸� ������)
        bool isCurrentlyFacingRight = transform.localScale.x == -1;

        // ������ ����� ���� ������ �ٸ��� ������
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
        // localScale.x ���� �������� ���� ��ȯ
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