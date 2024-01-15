using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    [SerializeField] float cameraSpeed;

    [Header("Camera Zoom Settings")]
    [SerializeField] float zoomMultiplier;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;
    [SerializeField] float velocity;
    [SerializeField] float smoothTime;

    private Camera cam;
    private Transform cameraTransform;

    private float zoom;

    private void Awake()
    {
        this.cam = this.GetComponent<Camera>();
        this.cameraTransform = this.GetComponent<Transform>();
        this.zoom = this.cam.orthographicSize;
    }

    public void Move(Vector3 move)
    {
        this.cameraTransform.position += move * this.cameraSpeed * Time.deltaTime; ;
    }

    public void Zoom(float scroll)
    {
        this.zoom -= scroll * this.zoomMultiplier;
        this.zoom = Mathf.Clamp(this.zoom, this.minZoom, this.maxZoom);
        this.cam.orthographicSize = Mathf.SmoothDamp(this.cam.orthographicSize, zoom, ref this.velocity, this.smoothTime);
    }
}
