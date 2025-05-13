using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 상자 오브젝트의 상호작용 로직을 담당하는 스크립트입니다. IInteractable 인터페이스를 구현합니다.
public class Chest : MonoBehaviour, IInteractable
{
    // Inspector 창에서 연결할 스프라이트 변수들
    public Sprite closedSprite; // 닫혔을 때의 이미지 (여기에 "chest 0" 할당)
    public Sprite openSprite;   // 열렸을 때의 이미지 (여기에 "chest 2" 할당)

    private SpriteRenderer spriteRenderer; // 현재 오브젝트의 SpriteRenderer 컴포넌트
    private bool isOpen = false;         // 상자가 열렸는지 여부를 저장하는 변수입니다. 초기값은 false(닫힘)입니다.

    // 게임이 시작될 때 한번 호출되는 함수입니다.
    void Start()
    {
        // 현재 게임 오브젝트에 붙어있는 SpriteRenderer 컴포넌트를 찾아서 변수에 할당합니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작할 때 닫힌 이미지로 설정 (SpriteRenderer와 closedSprite가 모두 할당되어 있을 경우)
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
        else if (spriteRenderer == null)
        {
            Debug.LogError("Chest 오브젝트에 SpriteRenderer 컴포넌트가 없습니다!");
        }
        else if (closedSprite == null)
        {
            Debug.LogWarning("Chest 스크립트에 Closed Sprite가 할당되지 않았습니다. 기본 이미지가 유지됩니다.");
        }
    }

    // IInteractable 인터페이스의 Interact 함수를 구현합니다.
    // 플레이어가 상자와 상호작용했을 때 호출됩니다.
    public void Interact()
    {
        // 만약 상자가 아직 열리지 않았다면,
        if (!isOpen)
        {
            isOpen = true; // 상자를 열린 상태로 변경합니다.
            Debug.Log(gameObject.name + " 열림!"); // 콘솔에 상자 열림 메시지를 출력합니다.

            // (이미지 변경) SpriteRenderer와 openSprite가 할당되어 있다면 열린 이미지로 변경
            if (spriteRenderer != null && openSprite != null)
            {
                spriteRenderer.sprite = openSprite;
            }
            else if (openSprite == null)
            {
                Debug.LogWarning("Chest 스크립트에 Open Sprite가 할당되지 않았습니다. 이미지가 변경되지 않습니다.");
            }


            // 50% 확률 보상 로직은 그대로 유지
            float randomValue = Random.value;
            if (randomValue < 0.5f)
            {
                Debug.Log("엄청난 보상!");
            }
            else
            {
                Debug.Log("아무것도 없었다...");
            }
        }
        else // 만약 상자가 이미 열려있다면,
        {
            Debug.Log(gameObject.name + " 이미 열려있습니다.");
        }
    }
}