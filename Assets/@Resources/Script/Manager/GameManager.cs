using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float volume;
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
