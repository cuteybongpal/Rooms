// RangedEnemy.cs (FirePoint 없이 몸에서 직접 발사 버전)
using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
    [Header("감지 및 공격")]
    public float sightRange = 10f;
    public float prepareTime = 1f;
    public float fireCooldown = 3f;

    [Header("총알 설정")]
    public GameObject bulletPrefab;
    // ▼▼▼ firePoint 변수 삭제 ▼▼▼
    // public Transform firePoint; 
    public float bulletSpeed = 10f;

    // --- 내부 상태 변수 ---
    private enum EnemyState { Idle, Preparing, Firing, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    private Transform playerTarget;
    private float stateTimer;
    private Animator animator;
    public float turnSpeed = 5f; // ▼▼▼ 회전 속도 변수 추가 ▼▼▼
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // ▼▼▼ firePoint null 체크 삭제 ▼▼▼
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab이 설정되지 않았습니다!", this);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                FindPlayer();
                break;
            case EnemyState.Preparing:
                if (playerTarget != null)
                {
                    // ▼▼▼ 조준 대상을 transform으로 변경 ▼▼▼
                    AimAtTarget(playerTarget.position);
                }
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    currentState = EnemyState.Firing;
                }
                break;
            case EnemyState.Firing:
                FireBullet();
                currentState = EnemyState.Cooldown;
                stateTimer = fireCooldown;
                break;
            case EnemyState.Cooldown:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
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
            if (hit.CompareTag("Player"))
            {
                playerTarget = hit.transform;
                currentState = EnemyState.Preparing;
                stateTimer = prepareTime;
                if (animator != null) animator.SetBool("isAiming", true);
                return;
            }
        }
    }

    void AimAtTarget(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // -90f는 스프라이트의 '위쪽'이 정면

        // 목표 회전값 생성
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // ▼▼▼ Slerp를 사용하여 부드럽게 회전 ▼▼▼
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    void FireBullet()
    {
        // ▼▼▼ firePoint 대신 transform 사용 ▼▼▼
        if (bulletPrefab == null) return;

        if (animator != null)
        {
            animator.SetTrigger("attack");
            animator.SetBool("isAiming", false);
        }

        // 적 자신의 위치와 회전값을 기준으로 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = this.bulletSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}