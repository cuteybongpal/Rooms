using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// ��¥ ������ ��ȣ�ۿ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�. IInteractable �������̽��� �����մϴ�.
public class FakeChest : MonoBehaviour, IInteractable
{
    public int damageAmount = 30; // �÷��̾�� ���� ������ ���Դϴ�. Inspector���� ���� �����մϴ�.
    public Sprite closedSprite;   // (���� ����) �Ѻ��⿡�� �Ϲ� ����ó�� ���� ���� �̹���
    public Sprite sprungSprite;   // (���� ����) ������ �ߵ��� ���� �̹��� (��: �� ����, �μ��� ���� ��)

    private SpriteRenderer spriteRenderer; // ���� ������Ʈ�� SpriteRenderer ������Ʈ
    private bool isTrapSprung = false;   // ������ �̹� �ߵ��Ǿ����� ���θ� �����ϴ� �����Դϴ�.

    // ������ ���۵� �� �ѹ� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        // ���� ���� ������Ʈ�� �پ��ִ� SpriteRenderer ������Ʈ�� ã�Ƽ� ������ �Ҵ��մϴ�.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ������ �� ���� �̹���(closedSprite)�� �Ҵ�Ǿ� �ִٸ�, �� �̹����� �����մϴ�.
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else if (spriteRenderer == null)
        {
            Debug.LogError("FakeChest ������Ʈ�� SpriteRenderer ������Ʈ�� �����ϴ�!");
        }
    }

    // IInteractable �������̽��� Interact �Լ��� �����մϴ�.
    // �÷��̾ ��¥ ���ڿ� ��ȣ�ۿ����� �� ȣ��˴ϴ�.
    public void Interact()
    {
        // ���� ������ ���� �ߵ����� �ʾҴٸ�,
        if (!isTrapSprung)
        {
            isTrapSprung = true; // ���� �ߵ� ���·� �����մϴ�.
            Debug.Log("��¥ ���ڴ�! ���� �ߵ�!"); // �ֿܼ� ���� �ߵ� �޽����� ����մϴ�.

            // (�̹��� ����) SpriteRenderer�� sprungSprite�� �Ҵ�Ǿ� �ִٸ� ���� �ߵ� �̹����� ����
            if (spriteRenderer != null && sprungSprite != null)
            {
                spriteRenderer.sprite = sprungSprite;
            }
            else if (sprungSprite != null) // sprungSprite�� �ִµ� spriteRenderer�� ���� ���� Start���� �̹� �α׸� ������ ���̹Ƿ�, sprungSprite�� �Ҵ� �ȵ� ��츸 ���
            {
                Debug.LogWarning("FakeChest ��ũ��Ʈ�� Sprung Sprite�� �Ҵ���� �ʾҽ��ϴ�. �̹����� ������� �ʽ��ϴ�.");
            }

            // "Player" �±׸� ���� ���� ������Ʈ�� ã���ϴ�.
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
                else
                {
                    Debug.LogError("�÷��̾�� PlayerHealth ��ũ��Ʈ�� ã�� �� �����ϴ�!", playerObject);
                }
            }
            else
            {
                Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }
        else // ���� ������ �̹� �ߵ��Ǿ��ٸ�,
        {
            Debug.Log("�̹� ������ �ߵ��� ��¥ �����Դϴ�.");
        }
    }
}