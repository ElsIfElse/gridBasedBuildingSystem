using System;
using UnityEngine;

public class CustomerFactory : MonoBehaviour
{
    public GameObject _customerPrefab;

    public CustomerController GetNewCustomer()
    {
        return Instantiate(_customerPrefab).GetComponent<CustomerController>();
    }

    public void Initialize(CustomerFactoryData data)
    {
        _customerPrefab = data.CustomerPrefab;
    }
}

[Serializable]
public struct CustomerFactoryData
{
    public GameObject CustomerPrefab;
}