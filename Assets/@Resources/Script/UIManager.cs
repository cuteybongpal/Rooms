// UIManager.cs
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Panels")]
    public GameObject inventoryPanel;
    public GameObject notificationPanel;
    public GameObject gameClearPanel; // ▼▼▼ 1. Game Clear 패널 변수 선언 추가 ▼▼▼
    public GameObject Description;
    public GameObject GameOver;

    [Header("UI Components")]
    public TextMeshProUGUI notificationText;
    public float notificationDuration = 2f;
    public InputField field;
    public Button button;

    [Header("Player")]
    public PlayerMovement playerMovement;

    public bool isShow = false;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start() // Awake 대신 Start로 옮기거나, Start에도 추가 (선택)
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (gameClearPanel != null) gameClearPanel.SetActive(false); // ▼▼▼ 2. 게임 시작 시 Game Clear 패널 숨기기 추가 ▼▼▼

        // ▼▼▼ GameTimerManager 시작 로직 (이전에 추가했던 것) ▼▼▼
        if (GameTimerManager.instance != null)
        {
            GameTimerManager.instance.StartTimer();
        }
        else
        {
            Debug.LogWarning("GameTimerManager 인스턴스를 찾을 수 없어 타이머를 시작할 수 없습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
            if (playerMovement != null) // playerMovement가 할당되었는지 확인
            {
                playerMovement.SetControllable(!isActive);
            }
        }
    }

    public void ShowNotification(string message)
    {
        // ... (기존 알림 로직은 동일) ...
        StopAllCoroutines(); // 기존 알림 코루틴 중지
        StartCoroutine(NotificationCoroutine(message));
    }

    private IEnumerator NotificationCoroutine(string message)
    {
        // ... (기존 알림 코루틴은 동일) ...
        notificationPanel.SetActive(true);
        notificationText.text = message;
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
    }

    // ▼▼▼ 3. "Game Clear" 메시지를 표시하는 새 함수 전체 추가 ▼▼▼
    public void ShowGameClearPanel()
    {
        if (gameClearPanel != null)
        {
            field.gameObject.SetActive(true);
            button.gameObject.SetActive(true);
            // GameTimerManager에서 타이머 중지 및 시간 가져오기
            if (GameTimerManager.instance != null)
            {
                GameTimerManager.instance.StopTimer();
                string elapsedTimeString = GameTimerManager.instance.GetFormattedElapsedTime();


                // GameClearPanel 내부의 TextMeshProUGUI 컴포넌트를 찾아서 텍스트 설정
                TextMeshProUGUI clearTextMessage = gameClearPanel.GetComponent<TextMeshProUGUI>();
                if (clearTextMessage != null)
                {
                    clearTextMessage.text = "GAME CLEAR!\nTime: " + elapsedTimeString;
                }
                else
                {
                    Debug.LogError("GameClearPanel 내부에 TextMeshProUGUI 컴포넌트가 없습니다!");
                }
            }
            else
            {
                Debug.LogError("GameTimerManager 인스턴스를 찾을 수 없습니다!");
                // 타이머 없이 기본 메시지만 표시할 수도 있음
                TextMeshProUGUI clearTextMessage = gameClearPanel.GetComponentInChildren<TextMeshProUGUI>();
                if (clearTextMessage != null)
                {
                    clearTextMessage.text = "GAME CLEAR!";
                }
            }

            gameClearPanel.SetActive(true);
            Debug.Log("GAME CLEAR!");

            if (playerMovement != null)
            {
                playerMovement.SetControllable(false);
            }
            // Time.timeScale = 0f; // (선택적) 게임 시간 정지
        }
        else
        {
            Debug.LogError("UIManager에 GameClearPanel이 연결되지 않았습니다!");
        }
        IDataSave save = DIContainer.GetInstance<IDataSave>() as IDataSave;
        button.onClick.AddListener(() =>
        {
            try
            {
                save.saveRanking(new RankingData(field.text, (int)GameTimerManager.instance.GetElapsedTime()));
            }
            catch (Exception e)
            {

            }
            SceneManager.LoadScene(0);
        });
    }
    public void ShowDescription()
    {
        isShow = !isShow;
        if (isShow)
        {
            Description.SetActive(true);
        }
        else
        {
            Description.SetActive(false);
        }
    }
    public void ShowGameOver()
    {
        GameOver.SetActive(true);
    }
}