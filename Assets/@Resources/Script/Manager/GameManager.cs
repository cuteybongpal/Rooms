using JetBrains.Annotations;
using System;

public class GameManager : Singleton<GameManager>
{

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