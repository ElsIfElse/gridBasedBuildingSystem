using UnityEngine;
using UnityEngine.AI;

public abstract class Worker : MonoBehaviour
{
    public string WorkerName;
    public WorkerType WorkerType;
    public NavMeshAgent WorkerAgent;

}