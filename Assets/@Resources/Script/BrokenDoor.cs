using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// �μ��� ���� ��ȣ�ۿ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�. IInteractable �������̽��� �����մϴ�.
public class BrokenDoor : MonoBehaviour, IInteractable
{
    // IInteractable �������̽��� Interact �Լ��� �����մϴ�.
    // �÷��̾ �μ��� ���� ��ȣ�ۿ����� �� ȣ��˴ϴ�.
    public void Interact()
    {
        // ���� �μ����� �� �� ���ٴ� �޽����� �ֿܼ� ����մϴ�.
        Debug.Log("���� �μ����� �� �� ����...");
        // �� ���� �ƹ��� �߰� ���۵� ���� �ʽ��ϴ�.
    }
}