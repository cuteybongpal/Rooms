using UnityEngine;

public class WanderingEnemy : MonoBehaviour
{
    [Header("�̵� �ɷ�ġ")]
    public float moveSpeed = 2f;
    public float patrolRadius = 5f;

    [Header("��� �ð� ����")]
    [Tooltip("��ǥ ������ ���� �� �ּ� ��� �ð�")]
    public float minIdleTime = 1f;
    [Tooltip("��ǥ ������ ���� �� �ִ� ��� �ð�")]
    public float maxIdleTime = 3f;

    // ���� ���� ���� ���� �߰� ����
    [Header("���� �ɷ�ġ")]
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    // --- ���� ���� ���� ---
    private enum PatrolState { Patrolling, Idle }
    private PatrolState currentState;

    private Vector2 startPosition;
    private Vector2 currentDestination;
    private float idleTimer;
    private float lastAttackTime; // ������ ���� �ð� ���

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
        // ���� ���� ���� �߰� ����
        // ���� ��ٿ��� �������� Ȯ��
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // ���� ���� ���� �ִ� �÷��̾ ����
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    // �÷��̾ ã���� ��� �����ϰ� ��ٿ� ����
                    Attack(hit.gameObject);
                    break; // �� ���� ����
                }
            }
        }

        // --- ���� ���� ���� ---
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

    // ���� ���� ��ȯ(Flip) ���� ���� ����
    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // �̵� ���� ����
        bool shouldFaceRight = destination.x > transform.position.x;

        // ���� �ٶ󺸴� ���� (localScale.x�� -1�̸� ������, 1�̸� ����)
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

    // ���� ���� �Լ� �߰� ����
    private void Attack(GameObject target)
    {
        lastAttackTime = Time.time;
        Debug.Log(gameObject.name + "��(��) " + target.name + "��(��) ����!");
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
        // ���� ���� (û�ϻ�)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : (Vector2)transform.position, patrolRadius);

        // ���� ���� (������)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}