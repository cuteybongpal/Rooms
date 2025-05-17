using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ī�޶� ����ٴ� ��� (�÷��̾� �Ǵ� ����)
    public float smoothSpeed = 0.125f; // ī�޶� �̵��� �ε巯�� ���� (0�� �������� ������, 1�� �������� ������ ����)
                                       // ���� �����ϸ� ���ϴ� ������ ã������. ��� ���󰡰� �Ϸ��� �� ���� ũ�� �ϰų� �Ʒ� �ּ�ó���� �ڵ带 ����ϼ���.
    public Vector3 offset = new Vector3(0f, 0f, -10f); // ī�޶�� ��� ������ �Ÿ� (2D ���ӿ����� ���� Z�� ���� �߿�)

    void LateUpdate() // ��� Update ������ ���� �� ����Ǿ� ī�޶� ���� ����
    {
        if (target != null)
        {
            // ��ǥ ��ġ ���: Ÿ���� x, y ��ġ + �������� x, y ��ġ, �׸��� �������� z ��ġ ����
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                offset.z // ī�޶��� Z ��ġ�� ������ ������ ���� (��: -10)
            );

            // �ε巯�� �̵� (Lerp ���)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // ���� ��� ���󰡰� �ϰ� �ʹٸ� �� �� �� ��� �Ʒ� �ڵ带 ����ϼ���:
            // transform.position = desiredPosition;
        }
    }
}