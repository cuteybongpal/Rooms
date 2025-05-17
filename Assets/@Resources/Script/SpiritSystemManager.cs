using UnityEngine;

public class SpiritSystemManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Q;
    public GameObject playerGameObject;
    public GameObject ghostPrefab;
    public CameraFollow gameCameraFollowScript; // Main Camera의 CameraFollow 스크립트

    private PlayerMovement _playerMovementScript;
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
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleSpiritMode();
        }
    }

    void ToggleSpiritMode()
    {
        _isGhostMode = !_isGhostMode;

        if (_isGhostMode) // 유령 모드로 전환
        {
            // 1. 플레이어 조종 중지 (가장 먼저 실행)
            if (_playerMovementScript != null)
            {
                Debug.Log("SpiritSystemManager: 플레이어 조종 중지 호출");
                _playerMovementScript.SetControllable(false); // ★ 플레이어 조종 불가로 설정
            }

            // 2. 유령 생성 및 조종 활성화
            if (ghostPrefab != null && playerGameObject != null)
            {
                Vector3 spawnPosition = playerGameObject.transform.position;
                _activeGhostInstance = Instantiate(ghostPrefab, spawnPosition, playerGameObject.transform.rotation);
                _ghostMovementScript = _activeGhostInstance.GetComponent<GhostMovement>();

                if (_ghostMovementScript != null)
                {
                    _ghostMovementScript.SetControllable(true);
                    if (gameCameraFollowScript != null) // 카메라 타겟을 유령으로 변경
                    {
                        gameCameraFollowScript.target = _activeGhostInstance.transform;
                    }
                }
                else Debug.LogError("생성된 유령에서 GhostMovement 스크립트를 찾을 수 없습니다!");
                Debug.Log("유령 모드 활성화 완료");
            }
        }
        else // 플레이어 모드로 복귀
        {
            // 1. 유령 제거 및 조종 중지
            if (_activeGhostInstance != null)
            {
                if (_ghostMovementScript != null) _ghostMovementScript.SetControllable(false);
                Destroy(_activeGhostInstance);
                _activeGhostInstance = null;
                _ghostMovementScript = null;
                Debug.Log("플레이어 모드 복귀: 유령 제거 완료");
            }

            // 2. 플레이어 조종 재개
            if (_playerMovementScript != null)
            {
                Debug.Log("SpiritSystemManager: 플레이어 조종 재개 호출");
                _playerMovementScript.SetControllable(true); // ★ 플레이어 조종 가능으로 설정
                if (gameCameraFollowScript != null) // 카메라 타겟을 플레이어로 변경
                {
                    gameCameraFollowScript.target = playerGameObject.transform;
                }
            }
        }
    }
}