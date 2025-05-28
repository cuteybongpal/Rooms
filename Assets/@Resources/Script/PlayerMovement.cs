using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("플레이어 설정")]
    public float moveSpeed = 5f;        // 플레이어 이동 속도
    public KeyCode interactionKey = KeyCode.E; // 상호작용 키 (PlayerInteraction에서 사용하지만, 여기에 두어도 무방)

    [Header("소음 발생 설정")]
    public float noiseInterval = 0.5f; // 이 시간(초) 간격으로 이동 시 소음을 발생시킵니다.

    // --- 내부 변수 ---
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveX;
    private float moveY;
    private bool currentIsMovingState;
    private string animatorParameterName = "isMoving"; // 애니메이터의 '움직임' 파라미터 이름
    private bool _isFacingRight = true;   // 플레이어가 처음에 오른쪽을 보고 있다고 가정
    private bool _canControlPlayer = true; // 이 값이 false면 플레이어는 움직이지 않아야 합니다.
    private float noiseTimer;

    // ▼▼▼ 둔화 효과를 위한 변수 ▼▼▼
    private float originalMoveSpeed;
    private bool isSlowed = false;
    private float slowDurationTimer = 0f;

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

        // ▼▼▼ 원래 이동 속도 저장 ▼▼▼
        originalMoveSpeed = moveSpeed;
    }

    // 다른 스크립트(SpiritSystemManager, UIManager 등)가 이 함수를 호출하여 플레이어 조종 상태를 변경합니다.
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
            // Rigidbody2D가 있다면 속도를 0으로 만들 수도 있습니다.
            // 예: if (GetComponent<Rigidbody2D>() != null) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void Update()
    {
        // 조종 불가능 상태이면 아래 로직을 실행하지 않고 함수를 종료합니다.
        if (!_canControlPlayer)
        {
            return;
        }

        // ▼▼▼ 둔화 효과 처리 ▼▼▼
        if (isSlowed)
        {
            slowDurationTimer -= Time.deltaTime;
            if (slowDurationTimer <= 0f)
            {
                RestoreSpeed();
            }
        }

        // --- 아래는 _canControlPlayer가 true일 때만 실행됩니다 ---

        // 입력 감지
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        // 캐릭터 방향 전환 로직
        if (moveX > 0 && !_isFacingRight) // 오른쪽으로 움직이는데 왼쪽을 보고 있다면
        {
            FlipSprite();
        }
        else if (moveX < 0 && _isFacingRight) // 왼쪽으로 움직이는데 오른쪽을 보고 있다면
        {
            FlipSprite();
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

        // 이동 시 소음 발생 로직
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

        // 실제 이동 로직
        transform.Translate(moveInput.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private void FlipSprite() // 함수 이름 명확화 (기존 Flip에서)
    {
        _isFacingRight = !_isFacingRight;
        if (spriteRenderer != null)
        {
            // isFacingRight가 true면 오른쪽을 봄 (flipX = false)
            // isFacingRight가 false면 왼쪽을 봄 (flipX = true)
            // 원본 스프라이트가 오른쪽을 보고 있다고 가정
            spriteRenderer.flipX = !_isFacingRight;
        }
    }

    // ▼▼▼ 둔화 효과 적용 및 해제 함수 추가 ▼▼▼
    public void ApplySlow(float slowFactor, float duration)
    {
        // 이미 둔화된 상태에서 다시 둔화가 걸리면, 이전 둔화는 풀고 새 둔화 적용
        // 또는 더 강력한 둔화만 적용하거나, 지속시간을 갱신하는 등 정책을 정할 수 있음
        // 여기서는 간단히 새 둔화로 덮어쓰되, originalMoveSpeed는 최초 값 유지
        if (!isSlowed)
        {
            // originalMoveSpeed는 Start에서 이미 설정되었으므로 여기서는 변경하지 않음
        }

        moveSpeed = originalMoveSpeed * slowFactor; // 예: slowFactor 0.5면 속도 절반
        isSlowed = true;
        slowDurationTimer = duration;
        Debug.Log("플레이어 둔화 적용! 변경된 속도: " + moveSpeed + ", 지속시간: " + duration + "초");
    }

    private void RestoreSpeed()
    {
        moveSpeed = originalMoveSpeed;
        isSlowed = false;
        slowDurationTimer = 0f;
        Debug.Log("플레이어 속도 복구! 원래 속도: " + moveSpeed);
    }
}