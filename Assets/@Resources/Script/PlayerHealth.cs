// PlayerHealth.cs (무적 기능 추가)
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Animator animator;
    private bool isDead = false;
    private SpiritSystemManager spiritManager;

    // ▼▼▼ 무적 상태를 위한 변수 추가 ▼▼▼
    public bool isInvincible = false;
    private float invincibilityDuration = 0f; // 무적 지속 시간 (0 이하면 무한 또는 토글)
    private float invincibilityTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Player GameObject에 Animator 컴포넌트가 없습니다!");

        spiritManager = FindFirstObjectByType<SpiritSystemManager>(); // 최신 API 사용

        // ▼▼▼ 디버그용: 시작 시 무적 상태 로그 ▼▼▼
        Debug.Log("Player Health Initialized. Invincible: " + isInvincible);
    }

    // ▼▼▼ Update 함수에 무적 시간 타이머 추가 ▼▼▼
    void Update()
    {
        if (isInvincible && invincibilityDuration > 0) // 지속 시간이 설정된 경우에만 타이머 작동
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                DeactivateInvincibility();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // ▼▼▼ 무적 상태이면 데미지를 받지 않음 ▼▼▼
        if (isInvincible)
        {
            Debug.Log("Player is INVINCIBLE! No damage taken.");
            return;
        }

        if (isDead) return;
        currentHealth -= damageAmount;
        // ... (기존 데미지 처리 및 Die() 함수 호출 로직) ...
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0) Die();
    }

    // ... (Heal, Die 함수는 기존과 동일하거나 필요에 따라 수정) ...
    public void Heal(int healAmount)
    {
        if (isDead) return;
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("Player healed " + healAmount + " health. Current health: " + currentHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player Died!");
        if (animator != null) animator.SetTrigger("Die");

        PlayerMovement movementScript = GetComponent<PlayerMovement>();
        if (movementScript != null) movementScript.enabled = false;
        PlayerInteraction interactionScript = GetComponent<PlayerInteraction>();
        if (interactionScript != null) interactionScript.enabled = false;
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null) playerCollider.enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) { rb.linearVelocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Kinematic; }

        if (spiritManager != null && spiritManager.IsInGhostMode())
        {
            spiritManager.ToggleSpiritMode();
        }
        // ▼▼▼ 죽으면 무적 해제 (선택적) ▼▼▼
        // if (isInvincible) DeactivateInvincibility();

        UIManager.instance.ShowGameOver();
    }


    // ▼▼▼ 무적 활성화/비활성화 함수 추가 ▼▼▼
    public void ActivateInvincibility(float duration = 0f) // duration이 0이면 무한 토글용
    {
        isInvincible = true;
        invincibilityDuration = duration;
        if (duration > 0)
        {
            invincibilityTimer = duration;
            Debug.Log($"Player is INVINCIBLE for {duration} seconds!");
        }
        else
        {
            Debug.Log("Player is INVINCIBLE (Toggle On)!");
        }
        // (선택) 플레이어 외형 변경 (예: 깜빡임, 색상 변경)
    }

    public void DeactivateInvincibility()
    {
        isInvincible = false;
        invincibilityTimer = 0f;
        Debug.Log("Player is NO LONGER INVINCIBLE.");
        // (선택) 플레이어 외형 복구
    }
}