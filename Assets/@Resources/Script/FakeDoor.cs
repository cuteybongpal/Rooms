using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 가짜 문의 상호작용 로직을 담당하는 스크립트입니다. IInteractable 인터페이스를 구현합니다.
public class FakeDoor : MonoBehaviour, IInteractable
{
    public int damageAmount = 100; // 플레이어에게 입힐 데미지 양입니다. Inspector에서 조절 가능합니다.

    // IInteractable 인터페이스의 Interact 함수를 구현합니다.
    // 플레이어가 가짜 문과 상호작용했을 때 호출됩니다.
    public void Interact()
    {
        Debug.Log("가짜 문이다! 함정 발동!"); // 함정 발동 메시지를 콘솔에 출력합니다.

        // "Player" 태그를 가진 게임 오브젝트를 찾습니다. (플레이어 오브젝트에 "Player" 태그가 설정되어 있어야 합니다.)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        // 플레이어 오브젝트를 찾았다면,
        if (playerObject != null)
        {
            // 플레이어 오브젝트에서 PlayerHealth 스크립트 컴포넌트를 가져옵니다.
            PlayerHealth playerHealth = playerObject.GetComponent<PlayerHealth>();
            // PlayerHealth 스크립트를 성공적으로 가져왔다면,
            if (playerHealth != null)
            {
                // 플레이어에게 설정된 damageAmount 만큼 데미지를 입힙니다.
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}