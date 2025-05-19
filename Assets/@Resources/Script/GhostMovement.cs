using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed = 6f; // 유령의 이동 속도
    private bool _canControl = false;
    private Animator _animator;
    private string _isMovingParamName = "isMoving"; // 애니메이션 파라미터 이름 (필요하다면)

    // --- [새로운 변수 추가] ---
    private SpriteRenderer spriteRenderer;
    private bool _isFacingRight = true; // 유령이 처음에 오른쪽을 보고 있다고 가정

    void Start()
    {
        _animator = GetComponent<Animator>();
        // 유령은 벽을 통과하게 하려면 콜라이더 설정을 변경할 수 있습니다.
        // 예: GetComponent<Collider2D>().isTrigger = true;

        // --- [SpriteRenderer 컴포넌트 가져오기 추가] ---
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("GhostMovement: SpriteRenderer 컴포넌트를 찾을 수 없습니다!");
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

        // --- [캐릭터 좌우 반전 로직 추가] ---
        if (moveX > 0 && !_isFacingRight) // 오른쪽으로 움직이는데 왼쪽을 보고 있다면
        {
            Flip();
        }
        else if (moveX < 0 && _isFacingRight) // 왼쪽으로 움직이는데 오른쪽을 보고 있다면
        {
            Flip();
        }
        // --- [여기까지 추가] ---

        // 이동 로직
        transform.Translate(moveInput.normalized * speed * Time.deltaTime, Space.World);

        // 애니메이션 로직 (필요하다면)
        if (_animator != null)
        {
            _animator.SetBool(_isMovingParamName, moveInput.sqrMagnitude > 0.01f);
        }
    }

    // --- [새로운 함수 추가] ---
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        if (spriteRenderer != null)
        {
            // 오른쪽을 보고 있으면 flipX는 false, 왼쪽을 보고 있으면 flipX는 true
            // 유령의 기본 스프라이트가 오른쪽을 향하고 있다고 가정합니다.
            spriteRenderer.flipX = !_isFacingRight;
        }
    }
}