using System;
using UnityEngine;

public class CustomerSpawner
{
    private Transform _customerSpawnPosition;
    private CustomerFactory _customerFactory;

    public void SpawnCustomer()
    {
        CustomerController customer = _customerFactory.GetNewCustomer();
        if(_customerFactory == null) Debug.LogError("Customer factory is null");
        if(_customerSpawnPosition == null) Debug.LogError("Customer spawn position is null");
        if( customer == null) Debug.LogError("Customer is null");

        customer.CustomerObj.transform.position = _customerSpawnPosition.position;
    }

    public void Initialize(CustomerSpawnerData data)
    {
        _customerSpawnPosition = data.CustomerSpawnPosition;
        _customerFactory = data.CustomerFactory;
    }


}
[Serializable]
public struct CustomerSpawnerData
{
    public Transform CustomerSpawnPosition;
    [HideInInspector] public CustomerFactory CustomerFactory;
}