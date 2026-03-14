using UnityEngine;

public class WorkerFactory : MonoBehaviour
{
    #region Singleton
    public static WorkerFactory Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public GameObject WaiterPrefab;

    public Waiter CreateWaiter()
    {
        Waiter waiter = Instantiate(WaiterPrefab).GetComponent<Waiter>();
        WorkerManager.Instance.AddWaiter(waiter);
        return waiter;
    }
}