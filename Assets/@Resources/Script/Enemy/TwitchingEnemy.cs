// TwitchingEnemy.cs (�÷��̾� ���� �� ProximityAttack ���� ����)
using UnityEngine;

public class TwitchingEnemy : MonoBehaviour
{
    [Header("���� �ɷ�ġ")]
    public float moveSpeed = 3f;        // ���� �̵� �ӵ�
    public float sightRange = 7f;       // �÷��̾ �����ϴ� ����
    public float detectionInterval = 0.2f; // �÷��̾� Ž�� �ֱ�

    // --- ���� ���� ���� ---
    private enum EnemyState { Idle, Chasing }
    private EnemyState currentState = EnemyState.Idle;

    private Transform currentTarget;
    private float detectionTimer;

    // Animator�� ProximityAttack ��ũ��Ʈ���� ���� �ִϸ��̼��� ���� ȣ���� �� �����Ƿ�,
    // ���⼭�� isMoving�� �����ϰų�, �ʿ� ���ٸ� �����ص� �˴ϴ�.
    private Animator animator;
    // private SpriteRenderer spriteRenderer; //localScale�� ����� ���̹Ƿ� SpriteRenderer ���� ������ ���ʿ�

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //localScale ��� �� ���� ���� ���ʿ�
    }

    void Update()
    {
        // ���¿� ���� �ִϸ��̼� �Ķ���� ������Ʈ
        if (animator != null)
        {
            animator.SetBool("isMoving", currentState == EnemyState.Chasing);
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                detectionTimer += Time.deltaTime;
                if (detectionTimer >= detectionInterval)
                {
                    detectionTimer = 0f;
                    FindPlayer();
                }
                break;

            case EnemyState.Chasing:
                if (currentTarget != null)
                {
                    MoveTowards(currentTarget.position);
                    // �þ� ������ ����� �ٽ� Idle ���·� (������)
                    if (Vector2.Distance(transform.position, currentTarget.position) > sightRange * 1.2f)
                    {
                        currentTarget = null;
                        currentState = EnemyState.Idle;
                    }
                }
                else
                {
                    // Ÿ���� ��������� Idle ���·�
                    currentState = EnemyState.Idle;
                }
                break;
        }
    }

    void FindPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sightRange);
        foreach (var hit in hits)
        {
            // "Player" �±׸� ���� ������Ʈ�� ã���� ���� ����
            // ���ɰ� �÷��̾ �������� �ʰ� "Player" �±׸� �����մϴ�.
            if (hit.CompareTag("Player"))
            {
                currentTarget = hit.transform;
                currentState = EnemyState.Chasing;
                return; // ù ��° ã�� �÷��̾ Ÿ������ ����
            }
        }
    }

    void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // �̵� ���⿡ ���� localScale.x�� ����Ͽ� ������
        // ���� ��������Ʈ�� ������ ���� �ִٰ� ���� (scale.x = 1 �� ����, -1 �� ������)
        bool shouldFaceRight = destination.x > transform.position.x;
        bool isCurrentlyFacingRight = transform.localScale.x == -1;

        if (shouldFaceRight && !isCurrentlyFacingRight)
        {
            FlipLocalScale();
        }
        else if (!shouldFaceRight && isCurrentlyFacingRight)
        {
            FlipLocalScale();
        }
    }

    void FlipLocalScale()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    void OnDrawGizmosSelected()
    {
        // �÷��̾� ���� ���� (���� ����Ÿ� ���� Gizmos�� ����)
        Gizmos.color = Color.yellow; // �ٸ� ����� �����ϱ� ���� ����� ���
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}