using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Score;
    public List<Room> Rooms = new List<Room>();
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
    
    public void ChangeRoom()
    {

    }
}
