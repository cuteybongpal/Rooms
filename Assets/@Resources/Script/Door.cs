using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// �Ϲ� ���� ��ȣ�ۿ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�. IInteractable �������̽��� �����մϴ�.
public class Door : MonoBehaviour, IInteractable
{
    private BoxCollider2D doorCollider; // ���� BoxCollider2D ������Ʈ�� ������ �����Դϴ�.
    private bool isOpen = false;        // ���� ���ȴ��� ���θ� �����ϴ� �����Դϴ�. �ʱⰪ�� false(����)�Դϴ�.

    // ������ ���۵� �� �ѹ� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        // ���� ���� ������Ʈ�� �پ��ִ� BoxCollider2D ������Ʈ�� ã�Ƽ� doorCollider ������ �Ҵ��մϴ�.
        doorCollider = GetComponent<BoxCollider2D>();
        // ���� �ʱ� ����(����)�� ���� �ݶ��̴��� isTrigger �Ӽ��� �����մϴ�.
        // isOpen�� false�̹Ƿ�, isTrigger�� false�� �Ǿ� ���� ������ ��ֹ� ������ �ϰ� �˴ϴ�.
        doorCollider.isTrigger = isOpen;
    }

    // IInteractable �������̽��� Interact �Լ��� �����մϴ�.
    // �÷��̾ ���� ��ȣ�ۿ����� �� ȣ��˴ϴ�.
    public void Interact()
    {
        // ���� ����/���� ���¸� ������ŵ�ϴ�. (���������� �ݰ�, ���������� ���ϴ�.)
        isOpen = !isOpen;
        // ����� ���¿� ���� �ݶ��̴��� isTrigger �Ӽ��� ������Ʈ�մϴ�.
        // isOpen�� true�̸� isTrigger�� true�� �Ǿ� ��� �����ϰ� �ǰ�, false�̸� ��� �Ұ����ϰ� �˴ϴ�.
        doorCollider.isTrigger = isOpen;

        // �� ���¿� ���� ������ �α� �޽����� �ֿܼ� ����մϴ�.
        if (isOpen)
        {
            Debug.Log(gameObject.name + " �� ���� (isTrigger = true, ��� ����)");
        }
        else
        {
            Debug.Log(gameObject.name + " �� ���� (isTrigger = false, ��� �Ұ���)");
        }
    }
}