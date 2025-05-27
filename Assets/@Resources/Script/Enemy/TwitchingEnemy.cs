// TwitchingEnemy.cs (플레이어 추적 및 ProximityAttack 연동 버전)
using UnityEngine;

public class TwitchingEnemy : MonoBehaviour
{
    [Header("추적 능력치")]
    public float moveSpeed = 3f;        // 적의 이동 속도
    public float sightRange = 7f;       // 플레이어를 감지하는 범위
    public float detectionInterval = 0.2f; // 플레이어 탐색 주기

    // --- 내부 상태 변수 ---
    private enum EnemyState { Idle, Chasing }
    private EnemyState currentState = EnemyState.Idle;

    private Transform currentTarget;
    private float detectionTimer;

    // Animator는 ProximityAttack 스크립트에서 공격 애니메이션을 직접 호출할 수 있으므로,
    // 여기서는 isMoving만 관리하거나, 필요 없다면 삭제해도 됩니다.
    private Animator animator;
    // private SpriteRenderer spriteRenderer; //localScale을 사용할 것이므로 SpriteRenderer 직접 참조는 불필요

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // spriteRenderer = GetComponentInChildren<SpriteRenderer>(); //localScale 사용 시 직접 참조 불필요
    }

    void Update()
    {
        // 상태에 따라 애니메이션 파라미터 업데이트
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
                    // 시야 범위를 벗어나면 다시 Idle 상태로 (선택적)
                    if (Vector2.Distance(transform.position, currentTarget.position) > sightRange * 1.2f)
                    {
                        currentTarget = null;
                        currentState = EnemyState.Idle;
                    }
                }
                else
                {
                    // 타겟이 사라졌으면 Idle 상태로
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
            // "Player" 태그를 가진 오브젝트를 찾으면 추적 시작
            // 유령과 플레이어를 구분하지 않고 "Player" 태그만 감지합니다.
            if (hit.CompareTag("Player"))
            {
                currentTarget = hit.transform;
                currentState = EnemyState.Chasing;
                return; // 첫 번째 찾은 플레이어를 타겟으로 설정
            }
        }
    }

    void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // 이동 방향에 따라 localScale.x를 사용하여 뒤집기
        // 원본 스프라이트가 왼쪽을 보고 있다고 가정 (scale.x = 1 이 왼쪽, -1 이 오른쪽)
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
        // 플레이어 감지 범위 (기존 움찔거림 관련 Gizmos는 삭제)
        Gizmos.color = Color.yellow; // 다른 적들과 구분하기 위해 노란색 사용
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}