using System;
using UnityEngine;

public class DataManager
{
    //��ŷ�� ���� ����
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