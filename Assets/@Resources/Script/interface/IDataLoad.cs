using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IDataLoad : IDependency, IPool
{
    public UniTask<List<RankingData>> LoadRanking();
}
public class DataLoad : IDataLoad
{
    DataManager dataManager;
    public async UniTask<List<RankingData>> LoadRanking()
    {
        return await dataManager.LoadRanking();
    }

    public void Pool()
    {
        dataManager = null;
    }

    public void Init()
    {
        dataManager = new DataManager();
    }

}