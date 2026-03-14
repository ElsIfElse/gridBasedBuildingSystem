using System.Collections;
using UnityEngine;

public class IdleCustomerState : CustomerState
{
    public IdleCustomerState(CustomerStateMachineData data, CustomerStateMachine customerStateMachine) : base(data, customerStateMachine)
    {
        _customerAgent = data.CustomerAgent;
        _customerObj = data.CustomerObj;
        _customerStateMachine = customerStateMachine;
    }

    public override IEnumerator OnEnter()
    {
        yield return new WaitForSeconds(4);
        _customerStateMachine.ChangeState(CustomerStateType.WalkingOutOfPlace);
        
    }

    public override void OnExit()
    {
    }

    public override void Tick()
    {
    }
}