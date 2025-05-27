// SlowingBulletEnemy.cs
using UnityEngine;
using System.Collections;

public class SlowingBulletEnemy : MonoBehaviour
{
    [Header("감지 및 공격")]
    public float sightRange = 10f;
    public float prepareTime = 1f;
    public float fireCooldown = 3f;

    [Header("총알 설정")]
    public GameObject bulletPrefab;   // 둔화 총알 프리팹 (Is Trigger 체크된 콜라이더와 위 Bullet.cs 사용)
    public Transform firePoint;
    public float bulletSpeed = 10f;

    // ▼▼▼ 이 적이 발사하는 총알의 둔화 효과 설정 (Bullet.cs의 기본값을 덮어씀) ▼▼▼
    [Header("둔화 효과 설정")]
    public float bulletSlowFactor = 0.5f;
    public float bulletSlowDuration = 2f;

    private enum EnemyState { Idle, Preparing, Firing, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    private Transform playerTarget;
    private float stateTimer;
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (firePoint == null) firePoint = transform;
        if (bulletPrefab == null) Debug.LogError("Bullet Prefab이 설정되지 않았습니다!", this);
    }

    void Update()
    {
        // ... (RangedEnemy.cs와 동일한 Update 로직) ...
        switch (currentState)
        {
            case EnemyState.Idle: FindPlayer(); break;
            case EnemyState.Preparing:
                if (playerTarget != null) AimAtTarget(playerTarget.position);
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0) currentState = EnemyState.Firing;
                break;
            case EnemyState.Firing: FireEquippedBullet(); currentState = EnemyState.Cooldown; stateTimer = fireCooldown; break;
            case EnemyState.Cooldown: stateTimer -= Time.deltaTime; if (stateTimer <= 0) currentState = EnemyState.Idle; break;
        }
    }

    void FindPlayer()
    {
        // ... (RangedEnemy.cs와 동일한 FindPlayer 로직) ...
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
        // ... (RangedEnemy.cs와 동일한 AimAtTarget 로직) ...
        Vector2 direction = (targetPosition - firePoint.position).normalized; // firePoint 기준으로 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireEquippedBullet() // 함수 이름 변경 및 로직 추가
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (animator != null)
        {
            animator.SetTrigger("attack");
            animator.SetBool("isAiming", false);
        }

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // SlowingBulletEnemy.cs 의 FireEquippedBullet 함수 중 일부
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = this.bulletSpeed;
            bulletScript.canSlow = true; // 이 부분이 true로 설정되어야 합니다!
            bulletScript.slowFactor = this.bulletSlowFactor;
            bulletScript.slowDuration = this.bulletSlowDuration;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ... (RangedEnemy.cs와 동일한 OnDrawGizmosSelected 로직) ...
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}