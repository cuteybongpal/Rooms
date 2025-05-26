// UIManager.cs
using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Panels")]
    public GameObject inventoryPanel; // ▼▼▼ 인벤토리 패널을 연결할 변수
    public GameObject notificationPanel;

    [Header("UI Components")]
    public TextMeshProUGUI notificationText;
    public float notificationDuration = 2f;

    [Header("Player")]
    public PlayerMovement playerMovement;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    // ▼▼▼ B키 입력을 받기 위한 Update 함수 추가/수정 ▼▼▼
    void Update()
    {
        // B 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.B))
        {
            // inventoryPanel의 현재 활성화 상태를 뒤집는다 (켜져있으면 끄고, 꺼져있으면 켠다)
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);

            // 인벤토리가 활성화되면 플레이어 조종을 막고, 비활성화되면 조종을 풀어준다.
            playerMovement.SetControllable(!isActive);
        }
    }

    public void ShowNotification(string message)
    {
        StopAllCoroutines();
        StartCoroutine(NotificationCoroutine(message));
    }

    private IEnumerator NotificationCoroutine(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
    }
}