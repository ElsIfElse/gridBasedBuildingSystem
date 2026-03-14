using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    #region Singleton
    public static TickManager Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    List<ITickable> _tickables = new();
    public void RegisterTickable(ITickable tickable)
    {
        Debug.Log("Registered tickable");
        _tickables.Add(tickable);
    }

    public void Tick()
    {
        // foreach(ITickable tickable in _tickables) tickable.Tick();
    }
}