// SlowingBulletEnemy.cs
using UnityEngine;
using System.Collections;

public class SlowingBulletEnemy : MonoBehaviour
{
    [Header("���� �� ����")]
    public float sightRange = 10f;
    public float prepareTime = 1f;
    public float fireCooldown = 3f;

    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab;   // ��ȭ �Ѿ� ������ (Is Trigger üũ�� �ݶ��̴��� �� Bullet.cs ���)
    public Transform firePoint;
    public float bulletSpeed = 10f;

    // ���� �� ���� �߻��ϴ� �Ѿ��� ��ȭ ȿ�� ���� (Bullet.cs�� �⺻���� ���) ����
    [Header("��ȭ ȿ�� ����")]
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
        if (bulletPrefab == null) Debug.LogError("Bullet Prefab�� �������� �ʾҽ��ϴ�!", this);
    }

    void Update()
    {
        // ... (RangedEnemy.cs�� ������ Update ����) ...
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
        // ... (RangedEnemy.cs�� ������ FindPlayer ����) ...
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
        // ... (RangedEnemy.cs�� ������ AimAtTarget ����) ...
        Vector2 direction = (targetPosition - firePoint.position).normalized; // firePoint �������� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FireEquippedBullet() // �Լ� �̸� ���� �� ���� �߰�
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (animator != null)
        {
            animator.SetTrigger("attack");
            animator.SetBool("isAiming", false);
        }

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // SlowingBulletEnemy.cs �� FireEquippedBullet �Լ� �� �Ϻ�
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = this.bulletSpeed;
            bulletScript.canSlow = true; // �� �κ��� true�� �����Ǿ�� �մϴ�!
            bulletScript.slowFactor = this.bulletSlowFactor;
            bulletScript.slowDuration = this.bulletSlowDuration;
        }
    }

    void OnDrawGizmosSelected()
    {
        // ... (RangedEnemy.cs�� ������ OnDrawGizmosSelected ����) ...
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}