using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 카메라가 따라다닐 대상 (플레이어 또는 유령)
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움 정도 (0에 가까울수록 느리게, 1에 가까울수록 빠르게 따라감)
                                       // 값을 조정하며 원하는 느낌을 찾으세요. 즉시 따라가게 하려면 이 값을 크게 하거나 아래 주석처리된 코드를 사용하세요.
    public Vector3 offset = new Vector3(0f, 0f, -10f); // 카메라와 대상 사이의 거리 (2D 게임에서는 보통 Z축 값만 중요)

    void LateUpdate() // 모든 Update 로직이 끝난 후 실행되어 카메라 떨림 방지
    {
        if (target != null)
        {
            // 목표 위치 계산: 타겟의 x, y 위치 + 오프셋의 x, y 위치, 그리고 오프셋의 z 위치 고정
            Vector3 desiredPosition = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                offset.z // 카메라의 Z 위치는 오프셋 값으로 고정 (예: -10)
            );

            // 부드러운 이동 (Lerp 사용)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 만약 즉시 따라가게 하고 싶다면 위 두 줄 대신 아래 코드를 사용하세요:
            // transform.position = desiredPosition;
        }
    }
}