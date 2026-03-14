using System.Collections;
using UnityEngine;

public class GoingToChairCustomerState : CustomerState
{
    public GoingToChairCustomerState(CustomerStateMachineData data, CustomerStateMachine customerStateMachine) : base(data, customerStateMachine)
    {
        _customerAgent = data.CustomerAgent;
        _customerObj = data.CustomerObj;
        _customerStateMachine = customerStateMachine;
    }

    public override IEnumerator OnEnter()
    {
        Chair closestChair = ItemTracker.Instance.GetClosestChair(_customerObj.transform.position);
        _customerAgent.SetDestination(closestChair.BuiltItemObj.transform.position);
        yield return new WaitUntil(() => _customerAgent.remainingDistance <= _customerAgent.stoppingDistance);
        yield return new WaitForSeconds(2f);
        _customerStateMachine.ChangeState(CustomerStateType.Idle);

    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {

    }
}