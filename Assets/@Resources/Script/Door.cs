using UnityEngine;
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Sprites")]
    public Sprite doorSprite1_Closed;     // 문이 닫혔을 때의 이미지 (door1)
    public Sprite doorSprite2_Intermediate; // 문이 중간 단계일 때의 이미지 (door2)
    public Sprite doorSprite3_Open;       // 문이 완전히 열렸을 때의 이미지 (door3)

    [Header("Animation Settings")]
    public float animationStepDelay = 0.25f; // 각 애니메이션 프레임 사이의 시간 간격 (초)

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D doorCollider;

    private bool isOpen = false;          // 문이 현재 완전히 열린 상태인지 여부
    private bool isAnimating = false;     // 현재 애니메이션이 재생 중인지 여부

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + ": Door 스크립트에 SpriteRenderer 컴포넌트가 없습니다!");
            enabled = false; return;
        }
        if (doorCollider == null)
        {
            Debug.LogError(gameObject.name + ": Door 스크립트에 BoxCollider2D 컴포넌트가 없습니다!");
            enabled = false; return;
        }
        if (doorSprite1_Closed == null || doorSprite2_Intermediate == null || doorSprite3_Open == null)
        {
            Debug.LogError(gameObject.name + ": Door 스크립트의 Inspector 창에서 하나 이상의 Door Sprite가 할당되지 않았습니다!");
            enabled = false; return;
        }

        // 초기 상태: 문은 닫혀 있음
        spriteRenderer.sprite = doorSprite1_Closed;
        doorCollider.isTrigger = false;
        isOpen = false;
    }

    public void Interact()
    {
        // 이미 애니메이션 중이라면 중복 실행 방지
        if (isAnimating)
        {
            Debug.Log(gameObject.name + ": 현재 문이 움직이는 중입니다.");
            return;
        }

        if (isOpen) // 문이 열려있다면 -> 닫는 애니메이션 실행
        {
            StartCoroutine(AnimateDoor(false)); // false는 닫는 것을 의미
        }
        else // 문이 닫혀있다면 -> 여는 애니메이션 실행
        {
            StartCoroutine(AnimateDoor(true)); // true는 여는 것을 의미
        }
    }

    // 문 애니메이션을 처리하는 코루틴
    // bool shouldOpen: true면 여는 애니메이션, false면 닫는 애니메이션
    IEnumerator AnimateDoor(bool shouldOpen)
    {
        isAnimating = true; // 애니메이션 시작

        if (shouldOpen) // 문 열기
        {
            Debug.Log(gameObject.name + " 문 열리기 시작...");
            // 문이 열리는 동안에는 통행 불가능 (isTrigger = false 상태 유지)
            doorCollider.isTrigger = false;

            // spriteRenderer.sprite = doorSprite1_Closed; // 이미 닫힌 상태일 것이므로 생략 가능
            // yield return new WaitForSeconds(animationStepDelay); // 닫힌 상태에서 바로 첫 프레임으로

            spriteRenderer.sprite = doorSprite2_Intermediate;
            yield return new WaitForSeconds(animationStepDelay);

            spriteRenderer.sprite = doorSprite3_Open;
            doorCollider.isTrigger = true; // 완전히 열린 후 통행 가능
            isOpen = true;
            Debug.Log(gameObject.name + " 문 활짝 열림 (isTrigger = true, 통과 가능)");
        }
        else // 문 닫기
        {
            Debug.Log(gameObject.name + " 문 닫히기 시작...");
            // 문이 닫히기 시작하면 바로 통행 불가능
            doorCollider.isTrigger = false;

            // spriteRenderer.sprite = doorSprite3_Open; // 이미 열린 상태일 것이므로 생략 가능
            // yield return new WaitForSeconds(animationStepDelay); // 열린 상태에서 바로 첫 프레임으로

            spriteRenderer.sprite = doorSprite2_Intermediate;
            yield return new WaitForSeconds(animationStepDelay);

            spriteRenderer.sprite = doorSprite1_Closed;
            isOpen = false;
            Debug.Log(gameObject.name + " 문 완전히 닫힘 (isTrigger = false, 통과 불가능)");
        }

        isAnimating = false; // 애니메이션 종료
    }
}