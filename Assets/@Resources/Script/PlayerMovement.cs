using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public float moveSpeed = 5f;        // �÷��̾� �̵� �ӵ�
    public KeyCode interactionKey = KeyCode.E; // ��ȣ�ۿ� Ű (PlayerInteraction���� ���������, ���⿡ �ξ ����)

    [Header("���� �߻� ����")]
    public float noiseInterval = 0.5f; // �� �ð�(��) �������� �̵� �� ������ �߻���ŵ�ϴ�.

    // --- ���� ���� ---
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveX;
    private float moveY;
    private bool currentIsMovingState;
    private string animatorParameterName = "isMoving"; // �ִϸ������� '������' �Ķ���� �̸�
    private bool _isFacingRight = true;   // �÷��̾ ó���� �������� ���� �ִٰ� ����
    private bool _canControlPlayer = true; // �� ���� false�� �÷��̾�� �������� �ʾƾ� �մϴ�.
    private float noiseTimer;

    // ���� ��ȭ ȿ���� ���� ���� ����
    private float originalMoveSpeed;
    private bool isSlowed = false;
    private float slowDurationTimer = 0f;

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

        // ���� ���� �̵� �ӵ� ���� ����
        originalMoveSpeed = moveSpeed;
    }

    // �ٸ� ��ũ��Ʈ(SpiritSystemManager, UIManager ��)�� �� �Լ��� ȣ���Ͽ� �÷��̾� ���� ���¸� �����մϴ�.
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
            // Rigidbody2D�� �ִٸ� �ӵ��� 0���� ���� ���� �ֽ��ϴ�.
            // ��: if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Update()
    {
        // ���� �Ұ��� �����̸� �Ʒ� ������ �������� �ʰ� �Լ��� �����մϴ�.
        if (!_canControlPlayer)
        {
            return;
        }

        // ���� ��ȭ ȿ�� ó�� ����
        if (isSlowed)
        {
            slowDurationTimer -= Time.deltaTime;
            if (slowDurationTimer <= 0f)
            {
                RestoreSpeed();
            }
        }

        // --- �Ʒ��� _canControlPlayer�� true�� ���� ����˴ϴ� ---

        // �Է� ����
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        // ĳ���� ���� ��ȯ ����
        if (moveX > 0 && !_isFacingRight) // ���������� �����̴µ� ������ ���� �ִٸ�
        {
            FlipSprite();
        }
        else if (moveX < 0 && _isFacingRight) // �������� �����̴µ� �������� ���� �ִٸ�
        {
            FlipSprite();
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

        // �̵� �� ���� �߻� ����
        if (currentIsMovingState)
        {
            noiseTimer += Time.deltaTime;
            if (noiseTimer >= noiseInterval)
            {
                noiseTimer = 0f;
                if (NoiseManager.instance != null)
                {
                    NoiseManager.instance.MakeNoise(transform.position);
                }
            }
        }

        // ���� �̵� ����
        transform.Translate(moveInput.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private void FlipSprite() // �Լ� �̸� ��Ȯȭ (���� Flip����)
    {
        _isFacingRight = !_isFacingRight;
        if (spriteRenderer != null)
        {
            // isFacingRight�� true�� �������� �� (flipX = false)
            // isFacingRight�� false�� ������ �� (flipX = true)
            // ���� ��������Ʈ�� �������� ���� �ִٰ� ����
            spriteRenderer.flipX = !_isFacingRight;
        }
    }

    // ���� ��ȭ ȿ�� ���� �� ���� �Լ� �߰� ����
    public void ApplySlow(float slowFactor, float duration)
    {
        // �̹� ��ȭ�� ���¿��� �ٽ� ��ȭ�� �ɸ���, ���� ��ȭ�� Ǯ�� �� ��ȭ ����
        // �Ǵ� �� ������ ��ȭ�� �����ϰų�, ���ӽð��� �����ϴ� �� ��å�� ���� �� ����
        // ���⼭�� ������ �� ��ȭ�� �����, originalMoveSpeed�� ���� �� ����
        if (!isSlowed)
        {
            // originalMoveSpeed�� Start���� �̹� �����Ǿ����Ƿ� ���⼭�� �������� ����
        }

        moveSpeed = originalMoveSpeed * slowFactor; // ��: slowFactor 0.5�� �ӵ� ����
        isSlowed = true;
        slowDurationTimer = duration;
        Debug.Log("�÷��̾� ��ȭ ����! ����� �ӵ�: " + moveSpeed + ", ���ӽð�: " + duration + "��");
    }

    private void RestoreSpeed()
    {
        moveSpeed = originalMoveSpeed;
        isSlowed = false;
        slowDurationTimer = 0f;
        Debug.Log("�÷��̾� �ӵ� ����! ���� �ӵ�: " + moveSpeed);
    }
}