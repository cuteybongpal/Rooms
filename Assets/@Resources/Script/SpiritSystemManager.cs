using UnityEngine;

public class SpiritSystemManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Q;
    public GameObject playerGameObject;
    public GameObject ghostPrefab;
    public CameraFollow gameCameraFollowScript;

    private PlayerMovement _playerMovementScript;
    private PlayerHealth _playerHealth; // 플레이어 체력 스크립트를 저장할 변수
    private GameObject _activeGhostInstance;
    private GhostMovement _ghostMovementScript;
    private bool _isGhostMode = false;

    void Start()
    {
        // 플레이어 오브젝트 및 스크립트 확인
        if (playerGameObject == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject가 할당되지 않았습니다!");
            enabled = false; return;
        }
        _playerMovementScript = playerGameObject.GetComponent<PlayerMovement>();
        if (_playerMovementScript == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject에서 PlayerMovement 스크립트를 찾을 수 없습니다!");
            enabled = false; return;
        }

        // 플레이어 오브젝트에서 PlayerHealth 컴포넌트 가져오기
        _playerHealth = playerGameObject.GetComponent<PlayerHealth>();
        if (_playerHealth == null)
        {
            Debug.LogError("SpiritSystemManager: Player GameObject에서 PlayerHealth 스크립트를 찾을 수 없습니다!");
            enabled = false; return;
        }

        // 유령 프리팹 확인
        if (ghostPrefab == null)
        {
            Debug.LogError("SpiritSystemManager: Ghost Prefab이 할당되지 않았습니다!");
            enabled = false; return;
        }

        // 카메라 팔로우 스크립트 확인 및 초기 타겟 설정
        if (gameCameraFollowScript == null)
        {
            if (Camera.main != null) gameCameraFollowScript = Camera.main.GetComponent<CameraFollow>();
            if (gameCameraFollowScript == null)
            {
                Debug.LogError("SpiritSystemManager: CameraFollow 스크립트를 찾을 수 없거나 할당되지 않았습니다!");
                enabled = false; return;
            }
        }
        if (playerGameObject != null) gameCameraFollowScript.target = playerGameObject.transform;

        // 시작 시 플레이어 조종 가능 상태로 확실히 설정
        _playerMovementScript.SetControllable(true);
    }

    void Update()
    {
        // Q키를 눌렀고, 플레이어가 살아있을 때만 유령 모드로 전환
        if (Input.GetKeyDown(toggleKey) && _playerHealth != null && _playerHealth.currentHealth > 0)
        {
            ToggleSpiritMode();
        }
    }

    public void ToggleSpiritMode()
    {
        _isGhostMode = !_isGhostMode;

        if (_isGhostMode) // 유령 모드로 전환
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
        else // 플레이어 모드로 복귀
        {
            if (_activeGhostInstance != null)
            {
                Destroy(_activeGhostInstance);
                _activeGhostInstance = null;
                _ghostMovementScript = null;
            }

            // 플레이어의 현재 체력이 0보다 클 때만 조종권을 돌려줍니다.
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