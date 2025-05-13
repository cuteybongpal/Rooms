using UnityEngine; // Unity ������ ����� ����ϱ� ���� �ʿ��մϴ�.

// �÷��̾��� �̵��� ����ϴ� ��ũ��Ʈ�Դϴ�.
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾��� �̵� �ӵ��Դϴ�. Inspector â���� ������ �� �ֽ��ϴ�.

    private Rigidbody2D rb;       // �÷��̾��� Rigidbody2D ������Ʈ�� ������ �����Դϴ�. ���� ��� �̵��� ���˴ϴ�.
    private Vector2 movement;     // �÷��̾��� ���� �̵� ������ ������ Vector2 �����Դϴ�.

    // ������ ���۵� �� �ѹ� ȣ��Ǵ� �Լ��Դϴ�. �ʱ�ȭ�� �ַ� ���˴ϴ�.
    void Start()
    {
        // ���� ���� ������Ʈ�� �پ��ִ� Rigidbody2D ������Ʈ�� ã�Ƽ� rb ������ �Ҵ��մϴ�.
        rb = GetComponent<Rigidbody2D>();
    }

    // �� �����Ӹ��� ȣ��Ǵ� �Լ��Դϴ�. �ַ� �Է��� �ްų� �񹰸����� ���� ó���� ���˴ϴ�.
    void Update()
    {
        // ������(Horizontal: A, D, �¿� ȭ��ǥ) �Է��� �޽��ϴ�. GetAxisRaw�� -1, 0, 1 ���� ��ȯ�մϴ�.
        movement.x = Input.GetAxisRaw("Horizontal");
        // ������(Vertical: W, S, ���Ʒ� ȭ��ǥ) �Է��� �޽��ϴ�.
        movement.y = Input.GetAxisRaw("Vertical");

        // movement ���͸� ����ȭ�մϴ�. �밢�� �̵� �� �ӵ��� �������� ���� �����ϱ� �����Դϴ�.
        // (��: x=1, y=1 �� �� ���� ���̴� �� 1.414, ����ȭ�ϸ� ���̰� 1�� ��)
        movement.Normalize();
    }

    // ������ �ð� �������� ȣ��Ǵ� �Լ��Դϴ�. ���� ��� �� Rigidbody�� ����ϴ� �̵��� ���⼭ ó���ϴ� ���� �����ϴ�.
    void FixedUpdate()
    {
        // Rigidbody2D�� ��ġ�� ���� ��ġ(rb.position)���� ���� �̵�����ŭ �ű�ϴ�.
        // movement ���Ϳ� moveSpeed�� Time.fixedDeltaTime�� ���Ͽ� ������ �ӵ��� ������� ������ �ӵ��� �̵��ϰ� �մϴ�.
        // Time.fixedDeltaTime�� FixedUpdate ȣ�� ���� �ð� �����Դϴ�.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}