using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// ���� ������Ʈ�� ��ȣ�ۿ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�. IInteractable �������̽��� �����մϴ�.
public class Chest : MonoBehaviour, IInteractable
{
    // Inspector â���� ������ ��������Ʈ ������
    public Sprite closedSprite; // ������ ���� �̹��� (���⿡ "chest 0" �Ҵ�)
    public Sprite openSprite;   // ������ ���� �̹��� (���⿡ "chest 2" �Ҵ�)

    private SpriteRenderer spriteRenderer; // ���� ������Ʈ�� SpriteRenderer ������Ʈ
    private bool isOpen = false;         // ���ڰ� ���ȴ��� ���θ� �����ϴ� �����Դϴ�. �ʱⰪ�� false(����)�Դϴ�.

    // ������ ���۵� �� �ѹ� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        // ���� ���� ������Ʈ�� �پ��ִ� SpriteRenderer ������Ʈ�� ã�Ƽ� ������ �Ҵ��մϴ�.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ������ �� ���� �̹����� ���� (SpriteRenderer�� closedSprite�� ��� �Ҵ�Ǿ� ���� ���)
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else if (spriteRenderer == null)
        {
            Debug.LogError("Chest ������Ʈ�� SpriteRenderer ������Ʈ�� �����ϴ�!");
        }
        else if (closedSprite == null)
        {
            Debug.LogWarning("Chest ��ũ��Ʈ�� Closed Sprite�� �Ҵ���� �ʾҽ��ϴ�. �⺻ �̹����� �����˴ϴ�.");
        }
    }

    // IInteractable �������̽��� Interact �Լ��� �����մϴ�.
    // �÷��̾ ���ڿ� ��ȣ�ۿ����� �� ȣ��˴ϴ�.
    public void Interact()
    {
        // ���� ���ڰ� ���� ������ �ʾҴٸ�,
        if (!isOpen)
        {
            isOpen = true; // ���ڸ� ���� ���·� �����մϴ�.
            Debug.Log(gameObject.name + " ����!"); // �ֿܼ� ���� ���� �޽����� ����մϴ�.

            // (�̹��� ����) SpriteRenderer�� openSprite�� �Ҵ�Ǿ� �ִٸ� ���� �̹����� ����
            if (spriteRenderer != null && openSprite != null)
            {
                spriteRenderer.sprite = openSprite;
            }
            else if (openSprite == null)
            {
                Debug.LogWarning("Chest ��ũ��Ʈ�� Open Sprite�� �Ҵ���� �ʾҽ��ϴ�. �̹����� ������� �ʽ��ϴ�.");
            }


            // 50% Ȯ�� ���� ������ �״�� ����
            float randomValue = Random.value;
            if (randomValue < 0.5f)
            {
                Debug.Log("��û�� ����!");
            }
            else
            {
                Debug.Log("�ƹ��͵� ������...");
            }
        }
        else // ���� ���ڰ� �̹� �����ִٸ�,
        {
            Debug.Log(gameObject.name + " �̹� �����ֽ��ϴ�.");
        }
    }
}