using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class CameraHandler_BirdEye : MonoBehaviour
{
    #region Singleton
    public static CameraHandler_BirdEye Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public CinemachineCamera BirdEyeCamera; 
    public CinemachineCamera FpsCamera; 
    CinemachineCamera CurrentCamera;
    public GameObject BirdEyeCameraHolder;
    public float CameraMovementSpeed;

    public Transform BirdEyeCameraStartingPosition;
    public Transform FpsCameraStartingPosition; 

    InputAction MoveForwardAction;
    InputAction MoveBackwardAction;
    InputAction MoveLeftAction;
    InputAction MoveRightAction;

    InputAction CameraZoomInAction;
    InputAction CameraZoomOutAction;

    InputAction ChangeCameraAction;
    bool _isCameraFps = false;

    void Start()
    {
        InitializeActions();
        SetCameraStartingPoints();
    }
    void Update()
    {
        HandleCameraMovement();
        HandleCameraChangeButtonPress();
        HandleCameraZoom();
    }
    void InitializeActions()
    {
        // Movement
        MoveForwardAction = new InputAction("MoveForward", InputActionType.Button, "<Keyboard>/w");
        MoveBackwardAction = new InputAction("MoveBackward", InputActionType.Button, "<Keyboard>/s");
        MoveLeftAction = new InputAction("MoveLeft", InputActionType.Button, "<Keyboard>/a");
        MoveRightAction = new InputAction("MoveRight", InputActionType.Button, "<Keyboard>/d");

        MoveForwardAction.Enable();
        MoveBackwardAction.Enable();
        MoveLeftAction.Enable();
        MoveRightAction.Enable();

        // Camera Changer
        ChangeCameraAction = new("CameraChange",InputActionType.Button, "<Keyboard>/tab");
        ChangeCameraAction.Enable();

        // Zoom
        CameraZoomInAction = new("CameraZoomIn",InputActionType.Button, "<Mouse>/scroll/up");
        CameraZoomOutAction = new("CameraZoomOut",InputActionType.Button, "<Mouse>/scroll/down");
        CameraZoomInAction.Enable();
        CameraZoomOutAction.Enable();
    }
    void SetCameraStartingPoints()
    {
        BirdEyeCameraHolder.transform.position = BirdEyeCameraStartingPosition.position;
        if(FpsCameraStartingPosition != null) FpsCamera.transform.position = FpsCameraStartingPosition.position;
    }
    void HandleCameraMovement()
    {
        if(_isCameraFps) return;

        Vector3 movement = Vector3.zero;

        if(MoveForwardAction.IsPressed())
        {
            movement += CameraMovementSpeed * BirdEyeCameraHolder.transform.forward;
        }
        if(MoveBackwardAction.IsPressed())
        {
            movement += CameraMovementSpeed * -BirdEyeCameraHolder.transform.forward;
        }
        if(MoveLeftAction.IsPressed())
        {
            movement += CameraMovementSpeed * -BirdEyeCameraHolder.transform.right;
        }
        if(MoveRightAction.IsPressed())
        {
            movement += CameraMovementSpeed * BirdEyeCameraHolder.transform.right;
        }
 
        BirdEyeCameraHolder.transform.Translate(movement * Time.deltaTime, Space.World);
    }
    void HandleCameraZoom()
    {
        if(CameraZoomInAction.WasPressedThisFrame()) BirdEyeCamera.transform.Translate(BirdEyeCamera.transform.forward, Space.World);
        if(CameraZoomOutAction.WasPressedThisFrame()) BirdEyeCamera.transform.Translate(-BirdEyeCamera.transform.forward, Space.World);;
    }
    void HandleCameraChangeButtonPress()
    {
        if(ChangeCameraAction.WasPressedThisFrame()) _isCameraFps = !_isCameraFps;
        if (_isCameraFps)
        {
            FpsCamera.Priority = 3; 
            CurrentCamera = FpsCamera;
        }
        else
        {
            FpsCamera.Priority = 0; 
            CurrentCamera = BirdEyeCamera;
        }
    }
}