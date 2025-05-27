using System;
using UnityEngine;

public class DataManager
{
    //랭킹값 저장 예정
    public void Save<T>(T obj) where T : class
    {

    }
    public T Load<T>() where T : class
    {
        return default(T);
    }
}
[Serializable]
public class RankingData
{
    public string name;
    public int score;
}