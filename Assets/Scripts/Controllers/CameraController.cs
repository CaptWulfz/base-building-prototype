using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    private CameraObject cameraObj;
    private Controls.CameraActions cameraControls;

    private void Awake()
    {
        this.cameraObj = this.GetComponent<CameraObject>();
        this.cameraControls = InputManager.Instance.Controls.Camera;
        this.cameraControls.Enable();      
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputManager.Instance.AllowInput)
            return;

        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        this.cameraObj.Move(this.cameraControls.Move.ReadValue<Vector2>());
    }

    private void ZoomCamera()
    {
        float zoom = this.cameraControls.Zoom.ReadValue<float>();
        // Limit Value to -0.1 and 0.1 as the default value is 120
        zoom = Mathf.Clamp(zoom, -0.1f, 0.1f);
        this.cameraObj.Zoom(zoom);
    }
}
