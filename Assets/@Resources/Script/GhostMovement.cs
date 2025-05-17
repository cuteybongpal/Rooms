using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed = 6f; // ������ �̵� �ӵ�
    private bool _canControl = false;
    private Animator _animator;
    private string _isMovingParamName = "isMoving"; // �ִϸ��̼� �Ķ���� �̸� (�ʿ��ϴٸ�)

    // --- [���ο� ���� �߰�] ---
    private SpriteRenderer spriteRenderer;
    private bool _isFacingRight = true; // ������ ó���� �������� ���� �ִٰ� ����

    void Start()
    {
        _animator = GetComponent<Animator>();
        // ������ ���� ����ϰ� �Ϸ��� �ݶ��̴� ������ ������ �� �ֽ��ϴ�.
        // ��: GetComponent<Collider2D>().isTrigger = true;

        // --- [SpriteRenderer ������Ʈ �������� �߰�] ---
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("GhostMovement: SpriteRenderer ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    public void SetControllable(bool controllable)
    {
        _canControl = controllable;
        if (!_canControl && _animator != null)
        {
            _animator.SetBool(_isMovingParamName, false);
        }
    }

    void Update()
    {
        if (!_canControl)
        {
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        // --- [ĳ���� �¿� ���� ���� �߰�] ---
        if (moveX > 0 && !_isFacingRight) // ���������� �����̴µ� ������ ���� �ִٸ�
        {
            Flip();
        }
        else if (moveX < 0 && _isFacingRight) // �������� �����̴µ� �������� ���� �ִٸ�
        {
            Flip();
        }
        // --- [������� �߰�] ---

        // �̵� ����
        transform.Translate(moveInput.normalized * speed * Time.deltaTime, Space.World);

        // �ִϸ��̼� ���� (�ʿ��ϴٸ�)
        if (_animator != null)
        {
            _animator.SetBool(_isMovingParamName, moveInput.sqrMagnitude > 0.01f);
        }
    }

    // --- [���ο� �Լ� �߰�] ---
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        if (spriteRenderer != null)
        {
            // �������� ���� ������ flipX�� false, ������ ���� ������ flipX�� true
            // ������ �⺻ ��������Ʈ�� �������� ���ϰ� �ִٰ� �����մϴ�.
            spriteRenderer.flipX = !_isFacingRight;
        }
    }
}