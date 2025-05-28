// BossController.cs (CaptureSequence ���� �� ProcessBossCaptured ȣ��)
using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;
    public float attackCooldown = 4f;
    public int bulletsPerPatternShot = 1;
    public float burstInterval = 0.1f;

    [Header("���� ��ȹ ����")]
    public float captureRange = 3f;
    public KeyCode captureKey = KeyCode.T;
    public int requiredKeyPresses = 5;
    // public float captureAnimationDuration = 2f; // ProcessBossCaptured���� ���� ó���ϹǷ� ���ʿ� �� ���� ����

    private int currentKeyPresses = 0;
    private bool isPlayerInRangeForCapture = false;
    private bool isCaptured = false;
    private Transform playerTransform; // OnTriggerEnter2D���� ������
        public int reflectedHitsToDefeat = 3; // ������ �����߸��� ���� �¾ƾ� �ϴ� ƨ�ܳ� �Ѿ� Ƚ��
    private int currentReflectedHits = 0; // ������� ���� ƨ�ܳ� �Ѿ� Ƚ��
    private bool isDefeated = false; // ������ ���������� ����

    private float currentAttackCooldown;
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab�� BossController�� �������� �ʾҽ��ϴ�!", this);
        }
    }

    void Start()
    {
        currentAttackCooldown = attackCooldown;
    }

    void Update()
    {
        if (isDefeated) return; // ������ ���������� �� �̻� ���� �� ��

        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            ChooseAndExecuteAttackPattern();
            currentAttackCooldown = attackCooldown;
        }
        // ���� ���� ����
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown <= 0)
        {
            ChooseAndExecuteAttackPattern();
            currentAttackCooldown = attackCooldown;
        }

        // ��ȹ ����
        if (isPlayerInRangeForCapture && Input.GetKeyDown(captureKey))
        {
            currentKeyPresses++;
            Debug.Log($"��ȹ �õ�: {currentKeyPresses} / {requiredKeyPresses}");
            // ���⿡ UI�� ���� ��Ȳ ǥ�� ����

            if (currentKeyPresses >= requiredKeyPresses)
            {
                ProcessBossCaptured(); // CaptureSequence ��� �� �Լ��� ȣ��
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
            // currentKeyPresses = 0; // ������ ���� ���� ������ �ʱ�ȭ�Ϸ��� �ּ� ����
            Debug.Log("�÷��̾ ���� ��ȹ ������ ���Խ��ϴ�. TŰ�� ��Ÿ�ϼ���!");
        }
    }
    public void TakeReflectedHit()
    {
        if (isDefeated) return;

        currentReflectedHits++;
        Debug.Log($"ƨ�ܳ� �Ѿ˿� ����: {currentReflectedHits} / {reflectedHitsToDefeat}");
        // (����) ���� �ǰ� �� �ð���/û���� �ǵ��

        if (currentReflectedHits >= reflectedHitsToDefeat)
        {
            isDefeated = true;
            Debug.Log("���� ������!");
            StopAllCoroutines(); // ��� ���� ����
            if (animator != null) animator.SetTrigger("Defeated"); // (����) �������� �ִϸ��̼�
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowGameClearPanel();
            }
            else
            {
                Debug.LogError("UIManager �ν��Ͻ��� ã�� �� �����ϴ�!");
            }
            Destroy(gameObject);
            // (����) ���� ������Ʈ ��Ȱ��ȭ �Ǵ� �ı�
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
            currentKeyPresses = 0; // ������ ����� ī��Ʈ �ʱ�ȭ
            //Debug.Log("�÷��̾ ���� ��ȹ ������ ������ϴ�.");
        }
    }

    // BossController.cs�� ProcessBossCaptured �Լ�

    void ProcessBossCaptured()
    {
        isCaptured = true;
        //Debug.Log("���� ��ȹ! ���� Ŭ����!");

        StopAllCoroutines(); // ���� ���� ���� ���� �ڷ�ƾ ����

        if (animator != null)
        {
            animator.SetTrigger("Defeated"); // "Defeated" �Ǵ� "Captured" Ʈ����
        }

        if (UIManager.instance != null)
        {
            // ���� ���⸦ ShowGameClearPanel()�� �����մϴ� ����
            UIManager.instance.ShowGameClearPanel();
        }
        else
        {
            Debug.LogError("UIManager �ν��Ͻ��� ã�� �� �����ϴ�!");
        }
        Destroy(gameObject);
        // ���� ������Ʈ ��ü�� �ٷ� ��Ȱ��ȭ�ϰų� �ı��� �� �ֽ��ϴ�.
        // gameObject.SetActive(false); 
        // �Ǵ� �� �� �� �ı�: Destroy(gameObject, 2f);
    }

    void ChooseAndExecuteAttackPattern()
    {
        if (isCaptured) return; // ��ȹ�� ���¸� ���� ���� �� ��

        int patternIndex = Random.Range(1, 4);
        // if (animator != null) animator.SetTrigger("AttackPattern" + patternIndex);
        //Debug.Log("���� ���� ���� ����: " + patternIndex);

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