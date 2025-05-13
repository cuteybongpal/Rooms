using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// �÷��̾��� ü���� �����ϴ� ��ũ��Ʈ�Դϴ�.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // �÷��̾��� �ִ� ü���Դϴ�. Inspector â���� �ʱⰪ�� ������ �� �ֽ��ϴ�.
    public int currentHealth;    // �÷��̾��� ���� ü���� �����ϴ� �����Դϴ�.

    // ������ ���۵� �� �ѹ� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        // ���� ü���� �ִ� ü������ �ʱ�ȭ�մϴ�.
        currentHealth = maxHealth;
        // �ֿܼ� �ʱ� ü�� ���� �α׷� ����մϴ�. (������)
        Debug.Log("Player health initialized to: " + currentHealth);
    }

    // �÷��̾ �������� �Ծ��� �� ȣ��Ǵ� �Լ��Դϴ�.
    // damageAmount ��ŭ ü���� �����մϴ�.
    public void TakeDamage(int damageAmount)
    {
        // ���� ü�¿��� ���� ������ �縸ŭ ���ϴ�.
        currentHealth -= damageAmount;

        // ü���� 0 �̸����� �������� �ʵ��� �մϴ�.
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // �ֿܼ� �������� ���� ü���� �α׷� ����մϴ�. (������)
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        // ü���� 0 ���ϰ� �Ǹ� Die �Լ��� ȣ���Ͽ� ��� ó���� �մϴ�.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // �÷��̾ ü���� ȸ���� �� ȣ��Ǵ� �Լ��Դϴ�.
    // healAmount ��ŭ ü���� �����մϴ�.
    public void Heal(int healAmount)
    {
        // ���� ü�¿� ȸ������ŭ ���մϴ�.
        currentHealth += healAmount;

        // ü���� �ִ� ü���� �ʰ����� �ʵ��� �մϴ�.
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // �ֿܼ� ȸ������ ���� ü���� �α׷� ����մϴ�. (������)
        Debug.Log("Player healed " + healAmount + " health. Current health: " + currentHealth);
    }

    // �÷��̾ ������� �� ȣ��Ǵ� �Լ��Դϴ�.
    private void Die()
    {
        // �ֿܼ� ��� �޽����� �α׷� ����մϴ�. (������)
        Debug.Log("Player Died!");
        // ���⿡ ���� ��� ó�� ������ �߰��մϴ�.
        // ��: ���� ���� ȭ�� ǥ��, �÷��̾� ������Ʈ ��Ȱ��ȭ �Ǵ� �ı� ��
        // gameObject.SetActive(false); // �÷��̾� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
    }
}