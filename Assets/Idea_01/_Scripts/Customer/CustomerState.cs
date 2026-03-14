using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class CustomerState
{
    public abstract IEnumerator OnEnter();
    public abstract void OnExit();
    public abstract void Tick();

    public GameObject _customerObj;
    public NavMeshAgent _customerAgent;
    public CustomerStateMachine _customerStateMachine;

    public CustomerState(CustomerStateMachineData data, CustomerStateMachine customerStateMachine)
    {
        _customerObj = data.CustomerObj;
        _customerAgent = data.CustomerAgent;
        _customerStateMachine = customerStateMachine;
    }
}