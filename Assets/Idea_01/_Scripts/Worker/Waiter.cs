using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Waiter : Worker
{
    GameObject _targetTable;
    public GameObject PickupPoint;
    public void SetTargetTable(GameObject table) => _targetTable = table;
    public void SetPickupPoint(GameObject point) => PickupPoint = point;
    public GameObject GetTargetTable() => _targetTable;

    public IEnumerator GoToTable(GameObject table)
    {
        Debug.Log("Waiter Going to table");
        WorkerAgent.SetDestination(table.transform.position);
        yield return new WaitUntil(() => WorkerAgent.remainingDistance <= WorkerAgent.stoppingDistance);

        Debug.Log("Waiter Arrived to table");
        yield return new WaitForSeconds(2);

        Debug.Log("Waiter Served the table, going to pickup point");
        WorkerAgent.SetDestination(PickupPoint.transform.position);
        yield return new WaitUntil(() => WorkerAgent.remainingDistance <= WorkerAgent.stoppingDistance);
        yield return new WaitForSeconds(2);
        StartCoroutine(GoToTable(table));
    }
}   