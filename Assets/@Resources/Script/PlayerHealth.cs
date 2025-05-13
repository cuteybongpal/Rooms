using UnityEngine; // Unity 엔진의 기능을 사용하기 위해 필요합니다.

// 플레이어의 체력을 관리하는 스크립트입니다.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // 플레이어의 최대 체력입니다. Inspector 창에서 초기값을 설정할 수 있습니다.
    public int currentHealth;    // 플레이어의 현재 체력을 저장하는 변수입니다.

    // 게임이 시작될 때 한번 호출되는 함수입니다.
    void Start()
    {
        // 현재 체력을 최대 체력으로 초기화합니다.
        currentHealth = maxHealth;
        // 콘솔에 초기 체력 값을 로그로 출력합니다. (디버깅용)
        Debug.Log("Player health initialized to: " + currentHealth);
    }

    // 플레이어가 데미지를 입었을 때 호출되는 함수입니다.
    // damageAmount 만큼 체력이 감소합니다.
    public void TakeDamage(int damageAmount)
    {
        // 현재 체력에서 받은 데미지 양만큼 뺍니다.
        currentHealth -= damageAmount;

        // 체력이 0 미만으로 내려가지 않도록 합니다.
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // 콘솔에 데미지와 현재 체력을 로그로 출력합니다. (디버깅용)
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        // 체력이 0 이하가 되면 Die 함수를 호출하여 사망 처리를 합니다.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 플레이어가 체력을 회복할 때 호출되는 함수입니다.
    // healAmount 만큼 체력이 증가합니다.
    public void Heal(int healAmount)
    {
        // 현재 체력에 회복량만큼 더합니다.
        currentHealth += healAmount;

        // 체력이 최대 체력을 초과하지 않도록 합니다.
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // 콘솔에 회복량과 현재 체력을 로그로 출력합니다. (디버깅용)
        Debug.Log("Player healed " + healAmount + " health. Current health: " + currentHealth);
    }

    // 플레이어가 사망했을 때 호출되는 함수입니다.
    private void Die()
    {
        // 콘솔에 사망 메시지를 로그로 출력합니다. (디버깅용)
        Debug.Log("Player Died!");
        // 여기에 실제 사망 처리 로직을 추가합니다.
        // 예: 게임 오버 화면 표시, 플레이어 오브젝트 비활성화 또는 파괴 등
        // gameObject.SetActive(false); // 플레이어 오브젝트를 비활성화합니다.
    }
}