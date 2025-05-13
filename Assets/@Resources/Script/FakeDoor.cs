using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// ��¥ ���� ��ȣ�ۿ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�. IInteractable �������̽��� �����մϴ�.
public class FakeDoor : MonoBehaviour, IInteractable
{
    public int damageAmount = 100; // �÷��̾�� ���� ������ ���Դϴ�. Inspector���� ���� �����մϴ�.

    // IInteractable �������̽��� Interact �Լ��� �����մϴ�.
    // �÷��̾ ��¥ ���� ��ȣ�ۿ����� �� ȣ��˴ϴ�.
    public void Interact()
    {
        Debug.Log("��¥ ���̴�! ���� �ߵ�!"); // ���� �ߵ� �޽����� �ֿܼ� ����մϴ�.

        // "Player" �±׸� ���� ���� ������Ʈ�� ã���ϴ�. (�÷��̾� ������Ʈ�� "Player" �±װ� �����Ǿ� �־�� �մϴ�.)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        // �÷��̾� ������Ʈ�� ã�Ҵٸ�,
        if (playerObject != null)
        {
            // �÷��̾� ������Ʈ���� PlayerHealth ��ũ��Ʈ ������Ʈ�� �����ɴϴ�.
            PlayerHealth playerHealth = playerObject.GetComponent<PlayerHealth>();
            // PlayerHealth ��ũ��Ʈ�� ���������� �����Դٸ�,
            if (playerHealth != null)
            {
                // �÷��̾�� ������ damageAmount ��ŭ �������� �����ϴ�.
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}