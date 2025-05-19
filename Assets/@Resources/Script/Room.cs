using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isCleared = false;
    void Start()
    {
        
    }

    public void StageClear()
    {
        isCleared = true;
    }

}
