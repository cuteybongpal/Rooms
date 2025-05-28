using UnityEngine;

public class SpiritSystemManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Q;
    public GameObject playerGameObject;
    public GameObject ghostPrefab;
    public CameraFollow gameCameraFollowScript;

    private PlayerMovement _playerMovementScript;
    private PlayerHealth _playerHealth; // �÷��̾� ü�� ��ũ��Ʈ�� ������ ����
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

        // �÷��̾� ������Ʈ���� PlayerHealth ������Ʈ ��������
        _playerHealth = playerGameObject.GetComponent<PlayerHealth>();
        if (_playerHealth == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject���� PlayerHealth ��ũ��Ʈ�� ã�� �� �����ϴ�!");
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
        // QŰ�� ������, �÷��̾ ������� ���� ���� ���� ��ȯ
        if (Input.GetKeyDown(toggleKey) && _playerHealth != null && _playerHealth.currentHealth > 0)
        {
            ToggleSpiritMode();
        }
    }

    public void ToggleSpiritMode()
    {
        _isGhostMode = !_isGhostMode;

        if (_isGhostMode) // ���� ���� ��ȯ
        {
            if (_playerMovementScript != null)
            {
                _playerMovementScript.SetControllable(false);
            }

            if (ghostPrefab != null && playerGameObject != null)
            {
                Vector3 spawnPosition = playerGameObject.transform.position;
                _activeGhostInstance = Instantiate(ghostPrefab, spawnPosition, playerGameObject.transform.rotation);
                _ghostMovementScript = _activeGhostInstance.GetComponent<GhostMovement>();

                if (_ghostMovementScript != null)
                {
                    _ghostMovementScript.SetControllable(true);
                    if (gameCameraFollowScript != null)
                    {
                        gameCameraFollowScript.target = _activeGhostInstance.transform;
                    }
                }
            }
        }
        else // �÷��̾� ���� ����
        {
            if (_activeGhostInstance != null)
            {
                Destroy(_activeGhostInstance);
                _activeGhostInstance = null;
                _ghostMovementScript = null;
            }

            // �÷��̾��� ���� ü���� 0���� Ŭ ���� �������� �����ݴϴ�.
            if (_playerMovementScript != null && _playerHealth.currentHealth > 0)
            {
                _playerMovementScript.SetControllable(true);

                if (gameCameraFollowScript != null)
                {
                    gameCameraFollowScript.target = playerGameObject.transform;
                }
            }
        }
    }

    public bool IsInGhostMode()
    {
        return _isGhostMode;
    }
}