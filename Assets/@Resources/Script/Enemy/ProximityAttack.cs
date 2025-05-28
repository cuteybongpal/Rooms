// ProximityAttack.cs
using UnityEngine;

public class ProximityAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public float attackRange = 2f;    // 공격 범위
    public int attackDamage = 5;     // 공격 데미지
    public float attackCooldown = 1f; // 공격 간격 (1초마다 공격)

    private float currentCooldown;

    void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            AttackNearbyPlayers();
            currentCooldown = attackCooldown;
        }
    }

    void AttackNearbyPlayers()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log(gameObject.name + "의 근접 공격으로 " + hit.name + "에게 데미지 " + attackDamage);
                    playerHealth.TakeDamage(attackDamage);
                    // TODO: 공격 이펙트나 애니메이션이 있다면 여기서 처리
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}