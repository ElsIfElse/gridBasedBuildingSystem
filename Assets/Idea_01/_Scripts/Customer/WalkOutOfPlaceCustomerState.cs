using System.Collections;
using UnityEngine;

public class WalkOutOfStateCustomerState : CustomerState
{
    public WalkOutOfStateCustomerState(CustomerStateMachineData data, CustomerStateMachine customerStateMachine) : base(data,customerStateMachine)
    {
        _customerAgent = data.CustomerAgent;
        _customerObj = data.CustomerObj;
        _customerStateMachine = customerStateMachine;
    }

    public override IEnumerator OnEnter()
    {
        _customerAgent.SetDestination(Vector3.zero);
        yield return new WaitUntil(() => _customerAgent.remainingDistance <= _customerAgent.stoppingDistance);
        yield return new WaitForSeconds(4f);
        _customerStateMachine.ChangeState(CustomerStateType.WalkingToChair);
    }

    public override void OnExit()
    {
    }

    public override void Tick()
    {
    }
}