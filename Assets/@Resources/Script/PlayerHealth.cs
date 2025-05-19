using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 플레이어의 체력을 관리하는 스크립트입니다.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // 플레이어의 최대 체력입니다.
    public int currentHealth;    // 플레이어의 현재 체력을 저장하는 변수입니다.
    private Animator animator;     // 플레이어의 Animator 컴포넌트를 저장할 변수입니다.
    private bool isDead = false;   // 플레이어가 이미 죽었는지 확인하는 플래그

    // 게임이 시작될 때 한번 호출되는 함수입니다.
    void Start()
    {
        // 현재 체력을 최대 체력으로 초기화합니다.
        currentHealth = maxHealth;
        // 현재 게임 오브젝트에 붙어있는 Animator 컴포넌트를 찾아서 animator 변수에 할당합니다.
        animator = GetComponent<Animator>();
        // Animator가 없는 경우를 대비한 null 체크 (오류 방지)
        if (animator == null)
        {
            Debug.LogError("Player GameObject에 Animator 컴포넌트가 없습니다!");
        }
        Debug.Log("Player health initialized to: " + currentHealth);
    }

    // 플레이어가 데미지를 입었을 때 호출되는 함수입니다.
    public void TakeDamage(int damageAmount)
    {
        // 이미 죽었다면 데미지를 받지 않음
        if (isDead) return;

        // 현재 체력에서 받은 데미지 양만큼 뺍니다.
        currentHealth -= damageAmount;

        // 체력이 0 미만으로 내려가지 않도록 합니다.
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // 콘솔에 데미지와 현재 체력을 로그로 출력합니다.
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        // 체력이 0 이하가 되면 Die 함수를 호출하여 사망 처리를 합니다.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 플레이어가 체력을 회복할 때 호출되는 함수입니다.
    public void Heal(int healAmount)
    {
        // 이미 죽었다면 회복하지 않음
        if (isDead) return;

        // 현재 체력에 회복량만큼 더합니다.
        currentHealth += healAmount;

        // 체력이 최대 체력을 초과하지 않도록 합니다.
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // 콘솔에 회복량과 현재 체력을 로그로 출력합니다.
        Debug.Log("Player healed " + healAmount + " health. Current health: " + currentHealth);
    }

    // 플레이어가 사망했을 때 호출되는 함수입니다.
    private void Die()
    {
        // 이미 죽음 처리가 되었다면 중복 실행 방지
        if (isDead) return;
        isDead = true; // 죽음 상태로 변경

        // 콘솔에 사망 메시지를 로그로 출력합니다.
        Debug.Log("Player Died!");

        // Animator 컴포넌트가 있고, "Die" 라는 이름의 Trigger 파라미터를 발동시킵니다.
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 플레이어 사망 시 추가 로직 (선택 사항 적용)

        // 1. 플레이어 이동 및 상호작용 스크립트 비활성화
        PlayerMovement movementScript = GetComponent<PlayerMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false; // PlayerMovement 스크립트를 비활성화하여 이동을 막습니다.
        }

        PlayerInteraction interactionScript = GetComponent<PlayerInteraction>();
        if (interactionScript != null)
        {
            interactionScript.enabled = false; // PlayerInteraction 스크립트 비활성화
        }

        // 2. 플레이어의 콜라이더 비활성화 (다른 오브젝트와 더 이상 충돌하지 않도록)
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // 3. Rigidbody의 물리 효과 중지 (선택적, 필요에 따라)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // 현재 속도 제거
            rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정 (또는 Gravity Scale = 0 등)
        }

        // 4. (선택) 몇 초 후 게임오버 화면을 띄우거나 씬을 다시 로드하는 로직을 여기에 추가할 수 있습니다.
        // 예: Invoke("ShowGameOverScreen", 2f); // 2초 후에 ShowGameOverScreen 함수 호출
    }

    // 예시: 게임오버 화면 처리 함수 (필요하다면 실제 구현 필요)
    /*
    void ShowGameOverScreen()
    {
        Debug.Log("Game Over!");
        // 예: UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }
    */
}