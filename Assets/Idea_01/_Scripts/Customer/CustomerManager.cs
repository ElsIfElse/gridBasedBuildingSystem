
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public CustomerFactoryData CustomerFactoryData;
    public CustomerSpawnerData CustomerSpawnerData;

    CustomerFactory _customerFactory;
    CustomerSpawner _customerSpawner;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        CreateCustomerFactory();
        CreateCustomerSpawner();
        InitializeHandlers();
    }

    void CreateCustomerFactory()
    {
        GameObject customerFactoryObject = new("CustomerFactory");
        _customerFactory = customerFactoryObject.AddComponent<CustomerFactory>();
        
    }
    void CreateCustomerSpawner()
    {
        _customerSpawner = new();
    }
    void InitializeHandlers()
    {
        CustomerSpawnerData.CustomerFactory = _customerFactory;
        _customerSpawner.Initialize(CustomerSpawnerData);
        _customerFactory.Initialize(CustomerFactoryData);
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 100, 30), "Spawn Customer"))
        {
            _customerSpawner.SpawnCustomer();
        }
    }
}