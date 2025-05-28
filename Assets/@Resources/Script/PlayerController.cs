using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("�и� ����")]
    public float parryWindow = 0.3f;
    public KeyCode parryKey = KeyCode.P;

    private bool isParrying = false;
    private float parryTimer = 0f;

    [Header("ġƮŰ�� PlayerHealth ����")]
    public PlayerHealth playerHealth; // �ν����Ϳ��� ����

    // ���� ġƮŰ ���� ����
    private bool f9PressedInCheatSequence = false;
    private float cheatKeyInputWindow = 0.5f;
    private float cheatKeyTimer = 0f;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
        if (playerHealth == null)
        {
            Debug.LogError("PlayerController���� PlayerHealth ������Ʈ�� ã�� �� �����ϴ�! ġƮŰ�� �۵����� ���� �� �ֽ��ϴ�.");
        }
    }

    void Update()
    {
        // �и� ����
        if (Input.GetKeyDown(parryKey) && !isParrying)
        {
            StartParry();
        }
        if (isParrying)
        {
            parryTimer -= Time.deltaTime;
            if (parryTimer <= 0f) StopParry();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.instance.ShowDescription();
        }
        // ���� ġƮŰ �Է� ���� ����
        HandleCheatCodeInput();

        // ���� ���� ġƮŰ (ESC + L)
        if (Input.GetKey(KeyCode.Escape) && Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("ġƮ: ESC + L ����. ������ �����մϴ�.");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        // ��ü ȭ�� <-> â ��� ��ȯ ġƮŰ (Shift + W)
        bool leftShiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool rightShiftHeld = Input.GetKey(KeyCode.RightShift);

        if ((leftShiftHeld || rightShiftHeld) && Input.GetKeyDown(KeyCode.W))
        {
            Screen.fullScreen = !Screen.fullScreen;
            if (Screen.fullScreen)
            {
                Debug.Log("��ü ȭ�� ���� ��ȯ�Ǿ����ϴ�.");
            }
            else
            {
                Debug.Log("â ���� ��ȯ�Ǿ����ϴ�.");
                // â ����� �� Ư�� �ػ󵵷� �����ϰ� �ʹٸ� �Ʒ� �ڵ��� �ּ��� �����ϰ� �� ����
                // Screen.SetResolution(1280, 720, false); 
            }
        }
    }

    void StartParry()
    {
        isParrying = true;
        parryTimer = parryWindow;
    }

    void StopParry()
    {
        isParrying = false;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    void HandleCheatCodeInput()
    {
        if (playerHealth == null) return;

        bool altHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (altHeld && shiftHeld)
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                f9PressedInCheatSequence = true;
                cheatKeyTimer = cheatKeyInputWindow;
                Debug.Log("ġƮ: Alt+Shift+F9 ����. K �Է� ���...");
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(0);
            }
            if (f9PressedInCheatSequence && Input.GetKeyDown(KeyCode.K))
            {
                if (playerHealth.isInvincible)
                {
                    playerHealth.DeactivateInvincibility();
                }
                else
                {
                    playerHealth.ActivateInvincibility(3000f); // 30�� ����
                }
                f9PressedInCheatSequence = false;
                Debug.Log("ġƮ: K ����. ���� ���� ����.");
            }
        }

        if (f9PressedInCheatSequence)
        {
            cheatKeyTimer -= Time.deltaTime;
            if (cheatKeyTimer <= 0)
            {
                f9PressedInCheatSequence = false;
                Debug.Log("ġƮ: F9 �� K �Է� �ð� �ʰ�. ������ �ʱ�ȭ.");
            }
        }
    }
}