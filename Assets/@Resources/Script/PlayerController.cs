using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("패링 설정")]
    public float parryWindow = 0.3f;
    public KeyCode parryKey = KeyCode.P;

    private bool isParrying = false;
    private float parryTimer = 0f;

    [Header("치트키용 PlayerHealth 연결")]
    public PlayerHealth playerHealth; // 인스펙터에서 연결

    // 무적 치트키 내부 변수
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
            Debug.LogError("PlayerController에서 PlayerHealth 컴포넌트를 찾을 수 없습니다! 치트키가 작동하지 않을 수 있습니다.");
        }
    }

    void Update()
    {
        // 패링 로직
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
        // 무적 치트키 입력 감지 로직
        HandleCheatCodeInput();

        // 게임 종료 치트키 (ESC + L)
        if (Input.GetKey(KeyCode.Escape) && Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("치트: ESC + L 감지. 게임을 종료합니다.");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        // 전체 화면 <-> 창 모드 전환 치트키 (Shift + W)
        bool leftShiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool rightShiftHeld = Input.GetKey(KeyCode.RightShift);

        if ((leftShiftHeld || rightShiftHeld) && Input.GetKeyDown(KeyCode.W))
        {
            Screen.fullScreen = !Screen.fullScreen;
            if (Screen.fullScreen)
            {
                Debug.Log("전체 화면 모드로 전환되었습니다.");
            }
            else
            {
                Debug.Log("창 모드로 전환되었습니다.");
                // 창 모드일 때 특정 해상도로 설정하고 싶다면 아래 코드의 주석을 해제하고 값 조절
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
                Debug.Log("치트: Alt+Shift+F9 감지. K 입력 대기...");
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
                    playerHealth.ActivateInvincibility(3000f); // 30초 무적
                }
                f9PressedInCheatSequence = false;
                Debug.Log("치트: K 감지. 무적 상태 변경.");
            }
        }

        if (f9PressedInCheatSequence)
        {
            cheatKeyTimer -= Time.deltaTime;
            if (cheatKeyTimer <= 0)
            {
                f9PressedInCheatSequence = false;
                Debug.Log("치트: F9 후 K 입력 시간 초과. 시퀀스 초기화.");
            }
        }
    }
}