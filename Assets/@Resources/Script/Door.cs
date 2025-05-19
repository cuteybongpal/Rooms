using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 일반 문의 상호작용 로직을 담당하는 스크립트입니다. IInteractable 인터페이스를 구현합니다.
public class Door : MonoBehaviour, IInteractable
{
    private BoxCollider2D doorCollider; // 문의 BoxCollider2D 컴포넌트를 저장할 변수입니다.
    private bool isOpen = false;        // 문이 열렸는지 여부를 저장하는 변수입니다. 초기값은 false(닫힘)입니다.

    // 게임이 시작될 때 한번 호출되는 함수입니다.
    void Start()
    {
        // 현재 게임 오브젝트에 붙어있는 BoxCollider2D 컴포넌트를 찾아서 doorCollider 변수에 할당합니다.
        doorCollider = GetComponent<BoxCollider2D>();
        // 문의 초기 상태(닫힘)에 따라 콜라이더의 isTrigger 속성을 설정합니다.
        // isOpen이 false이므로, isTrigger도 false가 되어 문이 물리적 장애물 역할을 하게 됩니다.
        doorCollider.isTrigger = isOpen;
    }

    // IInteractable 인터페이스의 Interact 함수를 구현합니다.
    // 플레이어가 문과 상호작용했을 때 호출됩니다.
    public void Interact()
    {
        // 문의 열림/닫힘 상태를 반전시킵니다. (열려있으면 닫고, 닫혀있으면 엽니다.)
        isOpen = !isOpen;
        // 변경된 상태에 따라 콜라이더의 isTrigger 속성을 업데이트합니다.
        // isOpen이 true이면 isTrigger가 true가 되어 통과 가능하게 되고, false이면 통과 불가능하게 됩니다.
        doorCollider.isTrigger = isOpen;

        // 문 상태에 따라 적절한 로그 메시지를 콘솔에 출력합니다.
        if (isOpen)
        {
            Debug.Log(gameObject.name + " 문 열림 (isTrigger = true, 통과 가능)");
        }
        else
        {
            Debug.Log(gameObject.name + " 문 닫힘 (isTrigger = false, 통과 불가능)");
        }
    }
}