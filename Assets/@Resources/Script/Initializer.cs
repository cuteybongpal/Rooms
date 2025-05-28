using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        DIContainer.Bind<IDataLoad>(new System.Func<DataLoad>(() =>
        {
            return new DataLoad();
        }));
        DIContainer.Bind<IDataSave>(new System.Func<DataSave>(() =>
        {
            return new DataSave();
        }));
    }
}
