using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    public static CameraManager Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    CameraHandler_BirdEye CameraHandler_BirdEye = null;

    void Start() 
    {

    }
}