using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed = 6f; // 유령의 이동 속도
    private bool _canControl = false;
    private Animator _animator;
    private string _isMovingParamName = "isMoving"; // 애니메이션 파라미터 이름 (필요하다면)
                                                    // 소음 발생 관련 변수
    public float noiseInterval = 0.5f; // 0.5초마다 소리를 발생시킵니다.
    private float noiseTimer;
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

    // GhostMovement.cs

    // GhostMovement.cs 파일의 Update 함수

    void Update()
    {
        if (!_canControl)
        {
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY);

        if (moveX > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && _isFacingRight)
        {
            Flip();
        }

        transform.Translate(moveInput.normalized * speed * Time.deltaTime, Space.World);

        // isMoving 변수를 여기서 직접 계산해서 사용합니다.
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        if (_animator != null)
        {
            _animator.SetBool(_isMovingParamName, isMoving);
        }

        // 수정된 소음 발생 로직
        if (isMoving)
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