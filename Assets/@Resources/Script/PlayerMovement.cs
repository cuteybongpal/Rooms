using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 플레이어의 이동을 담당하는 스크립트입니다.
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어의 이동 속도입니다. Inspector 창에서 조절할 수 있습니다.

    private Rigidbody2D rb;       // 플레이어의 Rigidbody2D 컴포넌트를 저장할 변수입니다. 물리 기반 이동에 사용됩니다.
    private Vector2 movement;     // 플레이어의 현재 이동 방향을 저장할 Vector2 변수입니다.

    // 게임이 시작될 때 한번 호출되는 함수입니다. 초기화에 주로 사용됩니다.
    void Start()
    {
        // 현재 게임 오브젝트에 붙어있는 Rigidbody2D 컴포넌트를 찾아서 rb 변수에 할당합니다.
        rb = GetComponent<Rigidbody2D>();
    }

    // 매 프레임마다 호출되는 함수입니다. 주로 입력을 받거나 비물리적인 로직 처리에 사용됩니다.
    void Update()
    {
        // 수평축(Horizontal: A, D, 좌우 화살표) 입력을 받습니다. GetAxisRaw는 -1, 0, 1 값만 반환합니다.
        movement.x = Input.GetAxisRaw("Horizontal");
        // 수직축(Vertical: W, S, 위아래 화살표) 입력을 받습니다.
        movement.y = Input.GetAxisRaw("Vertical");

        // movement 벡터를 정규화합니다. 대각선 이동 시 속도가 빨라지는 것을 방지하기 위함입니다.
        // (예: x=1, y=1 일 때 벡터 길이는 약 1.414, 정규화하면 길이가 1이 됨)
        movement.Normalize();
    }

    // 고정된 시간 간격으로 호출되는 함수입니다. 물리 계산 및 Rigidbody를 사용하는 이동은 여기서 처리하는 것이 좋습니다.
    void FixedUpdate()
    {
        // Rigidbody2D의 위치를 현재 위치(rb.position)에서 계산된 이동량만큼 옮깁니다.
        // movement 벡터에 moveSpeed와 Time.fixedDeltaTime을 곱하여 프레임 속도와 관계없이 일정한 속도로 이동하게 합니다.
        // Time.fixedDeltaTime은 FixedUpdate 호출 간의 시간 간격입니다.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}