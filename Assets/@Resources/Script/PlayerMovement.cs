using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �̵� �� �ִϸ��̼� ���� ����
    public float moveSpeed = 5f;        // �÷��̾� �̵� �ӵ�
    private Animator animator;
    private float moveX;
    private float moveY;
    private bool currentIsMovingState;
    private string animatorParameterName = "isMoving"; // �ִϸ������� '������' �Ķ���� �̸�

    // ĳ���� ���� ���� ����
    private SpriteRenderer spriteRenderer;
    private bool _isFacingRight = true;   // �÷��̾ ó���� �������� ���� �ִٰ� ����

    // ���� ���� ���� ����
    private bool _canControlPlayer = true; // �� ���� false�� �÷��̾�� �������� �ʾƾ� �մϴ�.

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerMovement: Animator ������Ʈ�� ã�� �� �����ϴ�!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("PlayerMovement: SpriteRenderer ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    // SpiritSystemManager�� �� �Լ��� ȣ���Ͽ� �÷��̾� ���� ���¸� �����մϴ�.
    public void SetControllable(bool controllable)
    {
        _canControlPlayer = controllable;
        if (!_canControlPlayer) // ���� �Ұ��� ���°� �Ǹ�
        {
            // �����Ӱ� ���õ� ��� ���¸� ��� '����'���� ����
            currentIsMovingState = false;
            if (animator != null)
            {
                animator.SetBool(animatorParameterName, false); // �ִϸ��̼ǵ� ����
            }
            //Rigidbody2D�� �ִٸ� �ӵ��� 0���� ���� ���� �ֽ��ϴ�.
            // ��: if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Update()
    {
        // �ڡڡ� ���� �߿��� �κ� �ڡڡ�
        // _canControlPlayer�� false (���� �Ұ���)�̸�, �Ʒ� ������ �������� �ʰ� �Լ��� �����մϴ�.
        if (!_canControlPlayer)
        {
            return;
        }
        // �ڡڡڡڡڡڡڡڡڡڡڡڡڡڡ�

        // --- �Ʒ��� _canControlPlayer�� true�� ���� ����˴ϴ� ---

        // �Է� ����
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        // ĳ���� ���� ��ȯ ����
        if (moveX > 0 && !_isFacingRight) // ���������� �����̴µ� ������ ���� �ִٸ�
        {
            Flip();
        }
        else if (moveX < 0 && _isFacingRight) // �������� �����̴µ� �������� ���� �ִٸ�
        {
            Flip();
        }

        // �ִϸ��̼� ���� ������Ʈ
        if (moveInput.sqrMagnitude > 0.01f)
        {
            currentIsMovingState = true;
        }
        else
        {
            currentIsMovingState = false;
        }

        if (animator != null)
        {
            animator.SetBool(animatorParameterName, currentIsMovingState);
        }

        // ���� �̵� ����
        transform.Translate(moveInput.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !_isFacingRight; // �⺻ ��������Ʈ�� �������� ���Ѵٰ� ����
        }
    }
}