using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerStateMachine
{   
    public Dictionary<CustomerStateType,CustomerState> States = new();
    CustomerState _currentState;
    CustomerController _customerController;

    GoingToChairCustomerState _goingToChairCustomerState;
    IdleCustomerState _idleCustomerState;
    WalkOutOfStateCustomerState _walkOutOfStateCustomerState;

    public void InitializeStateMachine(CustomerStateMachineData data)
    {   
        _customerController = data.CustomerController;

        CreateStateDictionary(data);
        ChangeState(CustomerStateType.WalkingToChair);
    }

    void CreateStateDictionary(CustomerStateMachineData data)
    {
        _goingToChairCustomerState = new(data,this);
        States[CustomerStateType.WalkingToChair] =  _goingToChairCustomerState;

        _idleCustomerState = new(data,this);
        States[CustomerStateType.Idle] = _idleCustomerState;

        _walkOutOfStateCustomerState = new(data,this);
        States[CustomerStateType.WalkingOutOfPlace] = _walkOutOfStateCustomerState;
    }

    public void ChangeState(CustomerStateType stateType)
    {
        _currentState?.OnExit();
        _currentState = States[stateType];
        _customerController.StartCoroutine(_currentState.OnEnter());
    }
}

[Serializable]
public struct CustomerStateMachineData
{
    public GameObject CustomerObj;
    public NavMeshAgent CustomerAgent;
    public CustomerController CustomerController;
}