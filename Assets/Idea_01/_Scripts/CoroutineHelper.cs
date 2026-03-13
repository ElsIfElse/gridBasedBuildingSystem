using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    #region Singleton
    public static CoroutineHelper Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public void RunRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void StopRoutine(IEnumerator routine)
    {
        StopCoroutine(routine);
    }
}