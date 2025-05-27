// NoiseManager.cs
using UnityEngine;
using System;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager instance;
    public static event Action<Vector3> OnNoiseMade;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MakeNoise(Vector3 position)
    {
        // ▼▼▼ 1단계 디버그 로그 ▼▼▼
        Debug.Log("1단계: 소음 발생! 위치: " + position);
        OnNoiseMade?.Invoke(position);
    }
}