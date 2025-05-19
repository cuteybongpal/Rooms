using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 이동 및 애니메이션 관련 변수
    public float moveSpeed = 5f;        // 플레이어 이동 속도
    private Animator animator;
    private float moveX;
    private float moveY;
    private bool currentIsMovingState;
    private string animatorParameterName = "isMoving"; // 애니메이터의 '움직임' 파라미터 이름

    // 캐릭터 방향 관련 변수
    private SpriteRenderer spriteRenderer;
    private bool _isFacingRight = true;   // 플레이어가 처음에 오른쪽을 보고 있다고 가정

    // 제어 가능 상태 변수
    private bool _canControlPlayer = true; // 이 값이 false면 플레이어는 움직이지 않아야 합니다.

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerMovement: Animator 컴포넌트를 찾을 수 없습니다!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("PlayerMovement: SpriteRenderer 컴포넌트를 찾을 수 없습니다!");
        }
    }

    // SpiritSystemManager가 이 함수를 호출하여 플레이어 조종 상태를 변경합니다.
    public void SetControllable(bool controllable)
    {
        _canControlPlayer = controllable;
        if (!_canControlPlayer) // 조종 불가능 상태가 되면
        {
            // 움직임과 관련된 모든 상태를 즉시 '멈춤'으로 설정
            currentIsMovingState = false;
            if (animator != null)
            {
                animator.SetBool(animatorParameterName, false); // 애니메이션도 멈춤
            }
            //Rigidbody2D가 있다면 속도를 0으로 만들 수도 있습니다.
            // 예: if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Update()
    {
        // ★★★ 가장 중요한 부분 ★★★
        // _canControlPlayer가 false (조종 불가능)이면, 아래 로직을 실행하지 않고 함수를 종료합니다.
        if (!_canControlPlayer)
        {
            return;
        }
        // ★★★★★★★★★★★★★★★

        // --- 아래는 _canControlPlayer가 true일 때만 실행됩니다 ---

        // 입력 감지
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        // 캐릭터 방향 전환 로직
        if (moveX > 0 && !_isFacingRight) // 오른쪽으로 움직이는데 왼쪽을 보고 있다면
        {
            Flip();
        }
        else if (moveX < 0 && _isFacingRight) // 왼쪽으로 움직이는데 오른쪽을 보고 있다면
        {
            Flip();
        }

        // 애니메이션 상태 업데이트
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

        // 실제 이동 로직
        transform.Translate(moveInput.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !_isFacingRight; // 기본 스프라이트가 오른쪽을 향한다고 가정
        }
    }
}