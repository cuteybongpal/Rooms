// ProximityAttack.cs
using UnityEngine;

public class ProximityAttack : MonoBehaviour
{
    [Header("���� ����")]
    public float attackRange = 2f;    // ���� ����
    public int attackDamage = 5;     // ���� ������
    public float attackCooldown = 1f; // ���� ���� (1�ʸ��� ����)

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
                    Debug.Log(gameObject.name + "�� ���� �������� " + hit.name + "���� ������ " + attackDamage);
                    playerHealth.TakeDamage(attackDamage);
                    // TODO: ���� ����Ʈ�� �ִϸ��̼��� �ִٸ� ���⼭ ó��
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // ���� ���� (������)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}