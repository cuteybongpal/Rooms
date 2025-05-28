// Bullet.cs (최종 점검 버전: 튕겨내기, 둔화, 기본 기능 모두 포함)
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("기본 설정")]
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 3f;

    [Header("튕겨내기(Parry) 설정")]
    public bool isEnemyBullet = true;
    public bool isPlayerReflected = false;

    [Header("둔화(Slow) 설정")]
    public bool canSlow = false;
    public float slowFactor = 0.5f;
    public float slowDuration = 2f;

    public int maxBounces = 0;
    private int currentBounces = 0;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 이 스크립트는 Trigger 기반 콜라이더를 가정하고 Update에서 직접 이동합니다.
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 시 모든 관련 정보 로그로 출력
        string playerParryState = "PlayerController not found";
        PlayerController pcForLog = other.GetComponentInParent<PlayerController>();
        if (pcForLog != null)
        {
            playerParryState = pcForLog.IsParrying().ToString();
        }

        PlayerController playerController = other.GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
            PlayerHealth playerHealth = playerController.GetComponent<PlayerHealth>();
            PlayerMovement playerMovement = playerController.GetComponent<PlayerMovement>();

            if (playerController.IsParrying() && isEnemyBullet)
            {
                Debug.Log("[Bullet] Parry SUCCEEDED! Reflecting bullet.");
                isPlayerReflected = true;
                isEnemyBullet = false;
                transform.Rotate(0, 0, 180f);
                // (선택) 튕겨낸 총알 속도 증가 등
                // speed *= 1.5f; 
                return;
            }
            if (isEnemyBullet)
            {
                Debug.Log("[Bullet] Parry FAILED or not applicable for this bullet. Damaging/Slowing player.");
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                if (canSlow && playerMovement != null)
                {
                    playerMovement.ApplySlow(slowFactor, slowDuration);
                }
                Destroy(gameObject);
                return;
            }
        }
        else if (other.CompareTag("Obstacles"))
        {
            Debug.Log("[Bullet] Hit Obstacle. Destroying bullet.");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss") && isPlayerReflected)
        {
            Debug.Log("[Bullet] Reflected bullet hit Boss.");
            BossController bossController = other.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeReflectedHit();
            }
            Destroy(gameObject);
        }
    }
}