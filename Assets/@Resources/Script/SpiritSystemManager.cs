using UnityEngine;

public class SpiritSystemManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Q;
    public GameObject playerGameObject;
    public GameObject ghostPrefab;
    public CameraFollow gameCameraFollowScript; // Main Camera�� CameraFollow ��ũ��Ʈ

    private PlayerMovement _playerMovementScript;
    private GameObject _activeGhostInstance;
    private GhostMovement _ghostMovementScript;
    private bool _isGhostMode = false;

    void Start()
    {
        // �÷��̾� ������Ʈ �� ��ũ��Ʈ Ȯ��
        if (playerGameObject == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false; return;
        }
        _playerMovementScript = playerGameObject.GetComponent<PlayerMovement>();
        if (_playerMovementScript == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject���� PlayerMovement ��ũ��Ʈ�� ã�� �� �����ϴ�!");
            enabled = false; return;
        }

        // ���� ������ Ȯ��
        if (ghostPrefab == null)
        {
            Debug.LogError("SpiritSystemManager: Ghost Prefab�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false; return;
        }

        // ī�޶� �ȷο� ��ũ��Ʈ Ȯ�� �� �ʱ� Ÿ�� ����
        if (gameCameraFollowScript == null)
        {
            if (Camera.main != null) gameCameraFollowScript = Camera.main.GetComponent<CameraFollow>();
            if (gameCameraFollowScript == null)
            {
                Debug.LogError("SpiritSystemManager: CameraFollow ��ũ��Ʈ�� ã�� �� ���ų� �Ҵ���� �ʾҽ��ϴ�!");
                enabled = false; return;
            }
        }
        if (playerGameObject != null) gameCameraFollowScript.target = playerGameObject.transform;

        // ���� �� �÷��̾� ���� ���� ���·� Ȯ���� ����
        _playerMovementScript.SetControllable(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleSpiritMode();
        }
    }

    void ToggleSpiritMode()
    {
        _isGhostMode = !_isGhostMode;

        if (_isGhostMode) // ���� ���� ��ȯ
        {
            // 1. �÷��̾� ���� ���� (���� ���� ����)
            if (_playerMovementScript != null)
            {
                Debug.Log("SpiritSystemManager: �÷��̾� ���� ���� ȣ��");
                _playerMovementScript.SetControllable(false); // �� �÷��̾� ���� �Ұ��� ����
            }

            // 2. ���� ���� �� ���� Ȱ��ȭ
            if (ghostPrefab != null && playerGameObject != null)
            {
                Vector3 spawnPosition = playerGameObject.transform.position;
                _activeGhostInstance = Instantiate(ghostPrefab, spawnPosition, playerGameObject.transform.rotation);
                _ghostMovementScript = _activeGhostInstance.GetComponent<GhostMovement>();

                if (_ghostMovementScript != null)
                {
                    _ghostMovementScript.SetControllable(true);
                    if (gameCameraFollowScript != null) // ī�޶� Ÿ���� �������� ����
                    {
                        gameCameraFollowScript.target = _activeGhostInstance.transform;
                    }
                }
                else Debug.LogError("������ ���ɿ��� GhostMovement ��ũ��Ʈ�� ã�� �� �����ϴ�!");
                Debug.Log("���� ��� Ȱ��ȭ �Ϸ�");
            }
        }
        else // �÷��̾� ���� ����
        {
            // 1. ���� ���� �� ���� ����
            if (_activeGhostInstance != null)
            {
                if (_ghostMovementScript != null) _ghostMovementScript.SetControllable(false);
                Destroy(_activeGhostInstance);
                _activeGhostInstance = null;
                _ghostMovementScript = null;
                Debug.Log("�÷��̾� ��� ����: ���� ���� �Ϸ�");
            }

            // 2. �÷��̾� ���� �簳
            if (_playerMovementScript != null)
            {
                Debug.Log("SpiritSystemManager: �÷��̾� ���� �簳 ȣ��");
                _playerMovementScript.SetControllable(true); // �� �÷��̾� ���� �������� ����
                if (gameCameraFollowScript != null) // ī�޶� Ÿ���� �÷��̾�� ����
                {
                    gameCameraFollowScript.target = playerGameObject.transform;
                }
            }
        }
    }
}