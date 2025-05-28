using UnityEngine;

public class Initializer : MonoBehaviour
{
    static bool isInitialized = false;
    private void Awake()
    {
        if (isInitialized)
            return;
        isInitialized = true;
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
