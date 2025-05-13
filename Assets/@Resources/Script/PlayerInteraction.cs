using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// �÷��̾ �ֺ� ȯ��� ��ȣ�ۿ��ϴ� ������ ����ϴ� ��ũ��Ʈ�Դϴ�.
public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 1.5f; // ��ȣ�ۿ��� ������ �ִ� �Ÿ��Դϴ�. Inspector���� ���� �����մϴ�.
    public KeyCode interactionKey = KeyCode.E; // ��ȣ�ۿ뿡 ����� Ű�Դϴ�. Inspector���� ���� �����մϴ�.

    // �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // ������ ��ȣ�ۿ� Ű(interactionKey)�� ���ȴ��� Ȯ���մϴ�. GetKeyDown�� Ű�� ������ ���� �ѹ��� true�� ��ȯ�մϴ�.
        if (Input.GetKeyDown(interactionKey))
        {
            // �÷��̾��� ���� ��ġ(transform.position)�� �߽����� interactionDistance �ݰ� ���� �ִ� ��� Collider2D�� ã���ϴ�.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);

            float closestDistanceSqr = Mathf.Infinity; // ���� ����� ��ü������ �Ÿ��� ������. �ʱⰪ�� ���Ѵ�� �����մϴ�.
            Collider2D closestCollider = null;         // ���� ����� ��ü�� Collider2D�� ������ �����Դϴ�.

            // ã�� ��� �ݶ��̴����� ��ȸ�մϴ�.
            foreach (Collider2D col in colliders)
            {
                // �ش� �ݶ��̴��� ���� ���� ������Ʈ�� �±װ� "Interactable"���� Ȯ���մϴ�.
                if (col.CompareTag("Interactable"))
                {
                    // �÷��̾�� ���� �˻� ���� �ݶ��̴�(col) ������ �Ÿ��� �������� ����մϴ�.
                    // ���� �Ÿ�(Magnitude)���� ���� �Ÿ�(sqrMagnitude)�� ����ϴ� ���� ���� ����� �����մϴ�.
                    float distanceSqr = (col.transform.position - transform.position).sqrMagnitude;

                    // ���� ���� �ݶ��̴��� ������ ã�� ���� ����� �ݶ��̴����� �� �����ٸ�,
                    if (distanceSqr < closestDistanceSqr)
                    {
                        // ���� ����� �Ÿ��� �ݶ��̴� ������ ������Ʈ�մϴ�.
                        closestDistanceSqr = distanceSqr;
                        closestCollider = col;
                    }
                }
            }

            // ���� ����� ��ȣ�ۿ� ������ ��ü(closestCollider)�� ã�Ҵٸ�,
            if (closestCollider != null)
            {
                // �ش� ��ü���� IInteractable �������̽��� ������ ������Ʈ�� �����ɴϴ�.
                IInteractable interactableObject = closestCollider.GetComponent<IInteractable>();

                // IInteractable ������Ʈ�� ���������� ���������ٸ� (null�� �ƴ϶��),
                if (interactableObject != null)
                {
                    // �ش� ��ü�� Interact() �Լ��� ȣ���Ͽ� ��ȣ�ۿ��� �����մϴ�.
                    interactableObject.Interact();
                }
            }
        }
    } // Update �Լ��� ��

    // Scene �信�� �� ��ũ��Ʈ�� ���� ���� ������Ʈ�� ���õǾ��� �� ȣ��Ǵ� �Լ��Դϴ�. (������ ����)
    // �����(Gizmos)�� �׷��� ��ȣ�ۿ� ������ �ð������� ǥ���մϴ�.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // ������� ������ ��������� �����մϴ�.
        // �÷��̾��� ��ġ�� �߽����� interactionDistance�� ���������� �ϴ� ���� �ܰ����� �׸��ϴ�.
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
} // PlayerInteraction Ŭ������ ��