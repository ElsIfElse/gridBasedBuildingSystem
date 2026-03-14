using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    CustomerStateMachineData CustomerStateMachineData;
        
    public GameObject CustomerObj;
    public NavMeshAgent CustomerAgent;
    public CustomerStateMachine CustomerStateMachine;

    void OnEnable()
    {
        InitializeCustomer();
    }

    void InitializeCustomer()
    {
        CustomerObj = gameObject;
        CustomerAgent = CustomerObj.GetComponentInChildren<NavMeshAgent>();

        CustomerStateMachineData = new()
        {
            CustomerObj = CustomerObj,
            CustomerAgent = CustomerAgent,
            CustomerController = this
        };

        CustomerStateMachine = new();
        CustomerStateMachine.InitializeStateMachine(CustomerStateMachineData);
    }
}