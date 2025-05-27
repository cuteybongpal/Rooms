using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour, IPool
{
    public int damage;
    public string monsterName;
    public float speed;
    void Start()
    {
        Init();
    }

    public void Pool()
    {
        
    }

    public void Init()
    {
        
    }
}
