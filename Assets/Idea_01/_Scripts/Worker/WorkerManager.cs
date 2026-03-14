using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    #region Singleton
    public static WorkerManager Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public List<Waiter> Waiters = new();
    public Transform SpawnPoint;
    public GameObject table;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Waiter waiter = WorkerFactory.Instance.CreateWaiter();
        waiter.SetTargetTable(table);
        waiter.SetPickupPoint(SpawnPoint.gameObject);
        StartCoroutine(waiter.GoToTable(table));
    }

    public void AddWaiter(Waiter waiter) => Waiters.Add(waiter);
    public void RemoveWaiter(Waiter waiter) => Waiters.Remove(waiter);
}