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
    public GameObject gameClearPanel; // ���� 1. Game Clear �г� ���� ���� �߰� ����
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

    void Start() // Awake ��� Start�� �ű�ų�, Start���� �߰� (����)
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (gameClearPanel != null) gameClearPanel.SetActive(false); // ���� 2. ���� ���� �� Game Clear �г� ����� �߰� ����

        // ���� GameTimerManager ���� ���� (������ �߰��ߴ� ��) ����
        if (GameTimerManager.instance != null)
        {
            GameTimerManager.instance.StartTimer();
        }
        else
        {
            Debug.LogWarning("GameTimerManager �ν��Ͻ��� ã�� �� ���� Ÿ�̸Ӹ� ������ �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
            if (playerMovement != null) // playerMovement�� �Ҵ�Ǿ����� Ȯ��
            {
                playerMovement.SetControllable(!isActive);
            }
        }
    }

    public void ShowNotification(string message)
    {
        // ... (���� �˸� ������ ����) ...
        StopAllCoroutines(); // ���� �˸� �ڷ�ƾ ����
        StartCoroutine(NotificationCoroutine(message));
    }

    private IEnumerator NotificationCoroutine(string message)
    {
        // ... (���� �˸� �ڷ�ƾ�� ����) ...
        notificationPanel.SetActive(true);
        notificationText.text = message;
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
    }

    // ���� 3. "Game Clear" �޽����� ǥ���ϴ� �� �Լ� ��ü �߰� ����
    public void ShowGameClearPanel()
    {
        if (gameClearPanel != null)
        {
            field.gameObject.SetActive(true);
            button.gameObject.SetActive(true);
            // GameTimerManager���� Ÿ�̸� ���� �� �ð� ��������
            if (GameTimerManager.instance != null)
            {
                GameTimerManager.instance.StopTimer();
                string elapsedTimeString = GameTimerManager.instance.GetFormattedElapsedTime();


                // GameClearPanel ������ TextMeshProUGUI ������Ʈ�� ã�Ƽ� �ؽ�Ʈ ����
                TextMeshProUGUI clearTextMessage = gameClearPanel.GetComponent<TextMeshProUGUI>();
                if (clearTextMessage != null)
                {
                    clearTextMessage.text = "GAME CLEAR!\nTime: " + elapsedTimeString;
                }
                else
                {
                    Debug.LogError("GameClearPanel ���ο� TextMeshProUGUI ������Ʈ�� �����ϴ�!");
                }
            }
            else
            {
                Debug.LogError("GameTimerManager �ν��Ͻ��� ã�� �� �����ϴ�!");
                // Ÿ�̸� ���� �⺻ �޽����� ǥ���� ���� ����
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
            // Time.timeScale = 0f; // (������) ���� �ð� ����
        }
        else
        {
            Debug.LogError("UIManager�� GameClearPanel�� ������� �ʾҽ��ϴ�!");
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