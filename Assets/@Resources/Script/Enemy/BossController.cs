// BossController.cs (CaptureSequence 삭제 및 ProcessBossCaptured 호출)
using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("공격 설정")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;
    public float attackCooldown = 4f;
    public int bulletsPerPatternShot = 1;
    public float burstInterval = 0.1f;

    [Header("보스 포획 설정")]
    public float captureRange = 3f;
    public KeyCode captureKey = KeyCode.T;
    public int requiredKeyPresses = 5;
    // public float captureAnimationDuration = 2f; // ProcessBossCaptured에서 직접 처리하므로 불필요 시 삭제 가능

    private int currentKeyPresses = 0;
    private bool isPlayerInRangeForCapture = false;
    private bool isCaptured = false;
    private Transform playerTransform; // OnTriggerEnter2D에서 설정됨
        public int reflectedHitsToDefeat = 3; // 보스를 쓰러뜨리기 위해 맞아야 하는 튕겨낸 총알 횟수
    private int currentReflectedHits = 0; // 현재까지 맞은 튕겨낸 총알 횟수
    private bool isDefeated = false; // 보스가 쓰러졌는지 여부

    private float currentAttackCooldown;
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab이 BossController에 설정되지 않았습니다!", this);
        }
    }

    void Start()
    {
        currentAttackCooldown = attackCooldown;
    }

    void Update()
    {
        if (isDefeated) return; // 보스가 쓰러졌으면 더 이상 공격 안 함

        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            ChooseAndExecuteAttackPattern();
            currentAttackCooldown = attackCooldown;
        }
        // 공격 패턴 로직
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            ChooseAndExecuteAttackPattern();
            currentAttackCooldown = attackCooldown;
        }

        // 포획 로직
        if (isPlayerInRangeForCapture && Input.GetKeyDown(captureKey))
        {
            currentKeyPresses++;
            Debug.Log($"포획 시도: {currentKeyPresses} / {requiredKeyPresses}");
            // 여기에 UI로 진행 상황 표시 가능

            if (currentKeyPresses >= requiredKeyPresses)
            {
                ProcessBossCaptured(); // CaptureSequence 대신 이 함수를 호출
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCaptured) return;

        if (other.CompareTag("Player"))
        {
            isPlayerInRangeForCapture = true;
            playerTransform = other.transform;
            // currentKeyPresses = 0; // 범위에 새로 들어올 때마다 초기화하려면 주석 해제
            Debug.Log("플레이어가 보스 포획 범위에 들어왔습니다. T키를 연타하세요!");
        }
    }
    public void TakeReflectedHit()
    {
        if (isDefeated) return;

        currentReflectedHits++;
        Debug.Log($"튕겨낸 총알에 맞음: {currentReflectedHits} / {reflectedHitsToDefeat}");
        // (선택) 보스 피격 시 시각적/청각적 피드백

        if (currentReflectedHits >= reflectedHitsToDefeat)
        {
            isDefeated = true;
            Debug.Log("보스 쓰러짐!");
            StopAllCoroutines(); // 모든 공격 중지
            if (animator != null) animator.SetTrigger("Defeated"); // (선택) 쓰러지는 애니메이션
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowGameClearPanel();
            }
            else
            {
                Debug.LogError("UIManager 인스턴스를 찾을 수 없습니다!");
            }
            Destroy(gameObject);
            // (선택) 보스 오브젝트 비활성화 또는 파괴
            // gameObject.SetActive(false);
            // Destroy(gameObject, 3f);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (isCaptured) return;

        if (other.CompareTag("Player"))
        {
            isPlayerInRangeForCapture = false;
            playerTransform = null;
            currentKeyPresses = 0; // 범위를 벗어나면 카운트 초기화
            //Debug.Log("플레이어가 보스 포획 범위를 벗어났습니다.");
        }
    }

    // BossController.cs의 ProcessBossCaptured 함수

    void ProcessBossCaptured()
    {
        isCaptured = true;
        //Debug.Log("보스 포획! 게임 클리어!");

        StopAllCoroutines(); // 진행 중인 공격 패턴 코루틴 중지

        if (animator != null)
        {
            animator.SetTrigger("Defeated"); // "Defeated" 또는 "Captured" 트리거
        }

        if (UIManager.instance != null)
        {
            // ▼▼▼ 여기를 ShowGameClearPanel()로 수정합니다 ▼▼▼
            UIManager.instance.ShowGameClearPanel();
        }
        else
        {
            Debug.LogError("UIManager 인스턴스를 찾을 수 없습니다!");
        }
        Destroy(gameObject);
        // 보스 오브젝트 자체를 바로 비활성화하거나 파괴할 수 있습니다.
        // gameObject.SetActive(false); 
        // 또는 몇 초 후 파괴: Destroy(gameObject, 2f);
    }

    void ChooseAndExecuteAttackPattern()
    {
        if (isCaptured) return; // 포획된 상태면 공격 실행 안 함

        int patternIndex = Random.Range(1, 4);
        // if (animator != null) animator.SetTrigger("AttackPattern" + patternIndex);
        //Debug.Log("보스 공격 패턴 실행: " + patternIndex);

        switch (patternIndex)
        {
            case 1: StartCoroutine(ExecutePattern1()); break;
            case 2: StartCoroutine(ExecutePattern2()); break;
            case 3: StartCoroutine(ExecutePattern3()); break;
        }
    }

    IEnumerator ExecutePattern1()
    {
        if (isCaptured) yield break;
        Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        for (int i = 0; i < bulletsPerPatternShot; i++) { foreach (Vector2 dir in directions) { FireBulletInDirection(dir); } if (bulletsPerPatternShot > 1 && i < bulletsPerPatternShot - 1) yield return new WaitForSeconds(burstInterval); }
    }

    IEnumerator ExecutePattern2()
    {
        if (isCaptured) yield break;
        Vector2[] directions = { new Vector2(1, 1).normalized, new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized, new Vector2(-1, 1).normalized };
        for (int i = 0; i < bulletsPerPatternShot; i++) { foreach (Vector2 dir in directions) { FireBulletInDirection(dir); } if (bulletsPerPatternShot > 1 && i < bulletsPerPatternShot - 1) yield return new WaitForSeconds(burstInterval); }
    }

    IEnumerator ExecutePattern3()
    {
        if (isCaptured) yield break;
        float[] angles = { 0f, 120f, 240f };
        for (int i = 0; i < bulletsPerPatternShot; i++) { foreach (float angleDeg in angles) { Vector2 direction = Quaternion.Euler(0, 0, angleDeg) * Vector2.right; FireBulletInDirection(direction.normalized); } if (bulletsPerPatternShot > 1 && i < bulletsPerPatternShot - 1) yield return new WaitForSeconds(burstInterval); }
    }

    void FireBulletInDirection(Vector2 direction)
    {
        if (bulletPrefab == null || isCaptured) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, rotation);
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null) { bulletScript.speed = this.bulletSpeed; }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, captureRange);
    }
}