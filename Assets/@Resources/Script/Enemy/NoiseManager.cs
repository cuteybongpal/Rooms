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
        // ���� 1�ܰ� ����� �α� ����
        Debug.Log("1�ܰ�: ���� �߻�! ��ġ: " + position);
        OnNoiseMade?.Invoke(position);
    }
}