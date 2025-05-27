// RangedEnemy.cs (FirePoint ���� ������ ���� �߻� ����)
using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
    [Header("���� �� ����")]
    public float sightRange = 10f;
    public float prepareTime = 1f;
    public float fireCooldown = 3f;

    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab;
    // ���� firePoint ���� ���� ����
    // public Transform firePoint; 
    public float bulletSpeed = 10f;

    // --- ���� ���� ���� ---
    private enum EnemyState { Idle, Preparing, Firing, Cooldown }
    private EnemyState currentState = EnemyState.Idle;

    private Transform playerTarget;
    private float stateTimer;
    private Animator animator;
    public float turnSpeed = 5f; // ���� ȸ�� �ӵ� ���� �߰� ����
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        // ���� firePoint null üũ ���� ����
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab�� �������� �ʾҽ��ϴ�!", this);
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
                    // ���� ���� ����� transform���� ���� ����
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // -90f�� ��������Ʈ�� '����'�� ����

        // ��ǥ ȸ���� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // ���� Slerp�� ����Ͽ� �ε巴�� ȸ�� ����
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    void FireBullet()
    {
        // ���� firePoint ��� transform ��� ����
        if (bulletPrefab == null) return;

        if (animator != null)
        {
            animator.SetTrigger("attack");
            animator.SetBool("isAiming", false);
        }

        // �� �ڽ��� ��ġ�� ȸ������ �������� �Ѿ� ����
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