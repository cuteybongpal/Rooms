// UIManager.cs
using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Panels")]
    public GameObject inventoryPanel; // ���� �κ��丮 �г��� ������ ����
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

    // ���� BŰ �Է��� �ޱ� ���� Update �Լ� �߰�/���� ����
    void Update()
    {
        // B Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.B))
        {
            // inventoryPanel�� ���� Ȱ��ȭ ���¸� �����´� (���������� ����, ���������� �Ҵ�)
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);

            // �κ��丮�� Ȱ��ȭ�Ǹ� �÷��̾� ������ ����, ��Ȱ��ȭ�Ǹ� ������ Ǯ���ش�.
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