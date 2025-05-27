using Unity.VisualScripting;
using UnityEngine;

public interface IDataSave : IDependency, IPool
{
    public void saveRanking(RankingData rankingData);
}
public class DataSave : IDataSave
{
    DataManager dataManager;

    public void Init()
    {
        dataManager = new DataManager();
    }

    public void Pool()
    {
        dataManager = null;
    }

    public void saveRanking(RankingData rankingData)
    {
        Debug.Log("랭킹 세이브(데이터 매니저 간접 접근 단계)");
        dataManager.SaveRanking(rankingData);
    }

    
}