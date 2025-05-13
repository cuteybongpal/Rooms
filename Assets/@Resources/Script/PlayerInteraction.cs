using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 플레이어가 주변 환경과 상호작용하는 로직을 담당하는 스크립트입니다.
public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 1.5f; // 상호작용이 가능한 최대 거리입니다. Inspector에서 조절 가능합니다.
    public KeyCode interactionKey = KeyCode.E; // 상호작용에 사용할 키입니다. Inspector에서 변경 가능합니다.

    // 매 프레임마다 호출됩니다.
    void Update()
    {
        // 지정된 상호작용 키(interactionKey)가 눌렸는지 확인합니다. GetKeyDown은 키가 눌리는 순간 한번만 true를 반환합니다.
        if (Input.GetKeyDown(interactionKey))
        {
            // 플레이어의 현재 위치(transform.position)를 중심으로 interactionDistance 반경 내에 있는 모든 Collider2D를 찾습니다.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);

            float closestDistanceSqr = Mathf.Infinity; // 가장 가까운 객체까지의 거리의 제곱값. 초기값은 무한대로 설정합니다.
            Collider2D closestCollider = null;         // 가장 가까운 객체의 Collider2D를 저장할 변수입니다.

            // 찾은 모든 콜라이더들을 순회합니다.
            foreach (Collider2D col in colliders)
            {
                // 해당 콜라이더를 가진 게임 오브젝트의 태그가 "Interactable"인지 확인합니다.
                if (col.CompareTag("Interactable"))
                {
                    // 플레이어와 현재 검사 중인 콜라이더(col) 사이의 거리의 제곱값을 계산합니다.
                    // 실제 거리(Magnitude)보다 제곱 거리(sqrMagnitude)를 사용하는 것이 연산 비용이 저렴합니다.
                    float distanceSqr = (col.transform.position - transform.position).sqrMagnitude;

                    // 만약 현재 콜라이더가 이전에 찾은 가장 가까운 콜라이더보다 더 가깝다면,
                    if (distanceSqr < closestDistanceSqr)
                    {
                        // 가장 가까운 거리와 콜라이더 정보를 업데이트합니다.
                        closestDistanceSqr = distanceSqr;
                        closestCollider = col;
                    }
                }
            }

            // 가장 가까운 상호작용 가능한 객체(closestCollider)를 찾았다면,
            if (closestCollider != null)
            {
                // 해당 객체에서 IInteractable 인터페이스를 구현한 컴포넌트를 가져옵니다.
                IInteractable interactableObject = closestCollider.GetComponent<IInteractable>();

                // IInteractable 컴포넌트가 성공적으로 가져와졌다면 (null이 아니라면),
                if (interactableObject != null)
                {
                    // 해당 객체의 Interact() 함수를 호출하여 상호작용을 실행합니다.
                    interactableObject.Interact();
                }
            }
        }
    } // Update 함수의 끝

    // Scene 뷰에서 이 스크립트가 붙은 게임 오브젝트가 선택되었을 때 호출되는 함수입니다. (에디터 전용)
    // 기즈모(Gizmos)를 그려서 상호작용 범위를 시각적으로 표시합니다.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // 기즈모의 색상을 노란색으로 설정합니다.
        // 플레이어의 위치를 중심으로 interactionDistance를 반지름으로 하는 원의 외곽선을 그립니다.
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
} // PlayerInteraction 클래스의 끝