using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 부서진 문의 상호작용 로직을 담당하는 스크립트입니다. IInteractable 인터페이스를 구현합니다.
public class BrokenDoor : MonoBehaviour, IInteractable
{
    // IInteractable 인터페이스의 Interact 함수를 구현합니다.
    // 플레이어가 부서진 문과 상호작용했을 때 호출됩니다.
    public void Interact()
    {
        // 문이 부서져서 열 수 없다는 메시지를 콘솔에 출력합니다.
        Debug.Log("문이 부서져서 열 수 없다...");
        // 이 문은 아무런 추가 동작도 하지 않습니다.
    }
}