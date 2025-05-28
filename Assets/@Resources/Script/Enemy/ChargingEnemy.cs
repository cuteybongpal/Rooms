// ChargingEnemy.cs (공격 기능 추가 버전)
using UnityEngine;
using System.Collections;

public class ChargingEnemy : MonoBehaviour
{
    [Header("능력치")]
    public float sightRange = 8f;
    public float chargeSpeed = 10f;
    // ▼▼▼ 공격력 변수 추가 ▼▼▼
    public int attackDamage = 15; // 돌진 공격 데미지

    [Header("행동 타이밍")]
    public float prepareTime = 1f;
    public float cooldownTime = 3f;

    // --- 내부 상태 변수 ---
    private enum EnemyState { Idle, Preparing, Charging, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    private Transform playerTarget;
    private Vector2 chargeDestination;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator; // 공격 애니메이션을 위해 추가

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        if (currentState == EnemyState.Idle)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sightRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
                StartCoroutine(ChargeSequence());
                break;
            }
        }
    }

    private IEnumerator ChargeSequence()
    {
        currentState = EnemyState.Preparing;
        rb.linearVelocity = Vector2.zero;
        if (playerTarget != null) // 플레이어 타겟이 유효할 때만 위치 고정
        {
            chargeDestination = playerTarget.position;
        }
        else // 타겟이 사라졌다면 Idle 상태로 복귀
        {
            currentState = EnemyState.Idle;
            yield break;
        }


        if (spriteRenderer != null) spriteRenderer.color = Color.red;
        // TODO: 준비 애니메이션이 있다면 여기서 트리거
        // if (animator != null) animator.SetTrigger("prepareCharge"); 

        yield return new WaitForSeconds(prepareTime);

        if (spriteRenderer != null) spriteRenderer.color = Color.white;

        currentState = EnemyState.Charging;
        Vector2 chargeDirection = (chargeDestination - (Vector2)transform.position).normalized;
        rb.linearVelocity = chargeDirection * chargeSpeed;
        // TODO: 돌진 애니메이션이 있다면 여기서 isMoving 등을 true로
        // if (animator != null) animator.SetBool("isMoving", true); 
    }

    // ▼▼▼ OnCollisionEnter2D 함수 수정하여 공격 로직 추가 ▼▼▼
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Charging)
        {
            // 충돌한 대상이 플레이어인지 확인
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log(gameObject.name + "이(가) " + collision.gameObject.name + "에게 돌진 공격! 데미지: " + attackDamage);
                    playerHealth.TakeDamage(attackDamage);

                    // 공격 애니메이션 트리거
                    if (animator != null)
                    {
                        animator.SetTrigger("attack");
                    }
                }
            }

            // 벽이든 플레이어든 무언가에 부딪히면 돌진을 멈추고 쿨다운 시작
            StopAllCoroutines();
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        currentState = EnemyState.Cooldown;
        rb.linearVelocity = Vector2.zero;
        // if (animator != null) animator.SetBool("isMoving", false); // 이동 멈춤 애니메이션
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        currentState = EnemyState.Idle;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        // 공격 범위를 따로 표시할 필요는 없지만, 필요하다면 추가 가능
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}