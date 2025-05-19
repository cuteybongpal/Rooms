using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 가짜 상자의 상호작용 로직을 담당하는 스크립트입니다. IInteractable 인터페이스를 구현합니다.
public class FakeChest : MonoBehaviour, IInteractable
{
    public int damageAmount = 30; // 플레이어에게 입힐 데미지 양입니다. Inspector에서 조절 가능합니다.
    public Sprite closedSprite;   // (선택 사항) 겉보기에는 일반 상자처럼 보일 닫힌 이미지
    public Sprite sprungSprite;   // (선택 사항) 함정이 발동된 후의 이미지 (예: 빈 상자, 부서진 상자 등)

    private SpriteRenderer spriteRenderer; // 현재 오브젝트의 SpriteRenderer 컴포넌트
    private bool isTrapSprung = false;   // 함정이 이미 발동되었는지 여부를 저장하는 변수입니다.

    // 게임이 시작될 때 한번 호출되는 함수입니다.
    void Start()
    {
        // 현재 게임 오브젝트에 붙어있는 SpriteRenderer 컴포넌트를 찾아서 변수에 할당합니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작할 때 닫힌 이미지(closedSprite)가 할당되어 있다면, 그 이미지로 설정합니다.
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else if (spriteRenderer == null)
        {
            Debug.LogError("FakeChest 오브젝트에 SpriteRenderer 컴포넌트가 없습니다!");
        }
    }

    // IInteractable 인터페이스의 Interact 함수를 구현합니다.
    // 플레이어가 가짜 상자와 상호작용했을 때 호출됩니다.
    public void Interact()
    {
        // 만약 함정이 아직 발동되지 않았다면,
        if (!isTrapSprung)
        {
            isTrapSprung = true; // 함정 발동 상태로 변경합니다.
            Debug.Log("가짜 상자다! 함정 발동!"); // 콘솔에 함정 발동 메시지를 출력합니다.

            // (이미지 변경) SpriteRenderer와 sprungSprite가 할당되어 있다면 함정 발동 이미지로 변경
            if (spriteRenderer != null && sprungSprite != null)
            {
                spriteRenderer.sprite = sprungSprite;
            }
            else if (sprungSprite != null) // sprungSprite는 있는데 spriteRenderer가 없는 경우는 Start에서 이미 로그를 남겼을 것이므로, sprungSprite가 할당 안된 경우만 경고
            {
                Debug.LogWarning("FakeChest 스크립트에 Sprung Sprite가 할당되지 않았습니다. 이미지가 변경되지 않습니다.");
            }

            // "Player" 태그를 가진 게임 오브젝트를 찾습니다.
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
                else
                {
                    Debug.LogError("플레이어에서 PlayerHealth 스크립트를 찾을 수 없습니다!", playerObject);
                }
            }
            else
            {
                Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
            }
        }
        else // 만약 함정이 이미 발동되었다면,
        {
            Debug.Log("이미 함정이 발동된 가짜 상자입니다.");
        }
    }
}