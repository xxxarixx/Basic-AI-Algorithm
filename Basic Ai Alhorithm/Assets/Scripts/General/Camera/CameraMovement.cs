using DG.Tweening;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General.InputHandler;
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 cameraBounds;
    [SerializeField] private Vector3 center;
    [SerializeField] private float dragSpeed = 1f;
    [SerializeField] private float zoomIntencity = 1f;
    [SerializeField] private float zoomSmoothness = 1f;
    private Vector3 dragOrigin;
    private Vector3 destinationPosition;
    private float newZoomValue = 0f;
    private bool isDragging = false;
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            newZoomValue = transform.position.y;
        }
        else
        {
            newZoomValue = 10f;
        }
    }
    private void OnEnable()
    {
        instance.onPrimaryBtnStartPressing += OnPrimaryStartPressing;
        instance.onPrimaryBtnPressed += OnPrimaryBtnPressed;
        instance.onPrimaryBtnRelease += OnPrimaryBtnRelease;
        instance.onMouseScroll += OnMouseScroll;
    }
    private void OnDisable()
    {
        instance.onPrimaryBtnStartPressing -= OnPrimaryStartPressing;
        instance.onPrimaryBtnPressed -= OnPrimaryBtnPressed;
        instance.onPrimaryBtnRelease -= OnPrimaryBtnRelease;
        instance.onMouseScroll -= OnMouseScroll;
    }
    private void OnMouseScroll(Vector2 scrollDelta)
    {
        ///perspective camera logick
        if (!cam.orthographic)
        {
            newZoomValue = transform.position.y + scrollDelta.y * zoomIntencity;
            newZoomValue = Mathf.Clamp(newZoomValue, center.y - cameraBounds.y, center.y + cameraBounds.y);
        }
        else
        {
            newZoomValue = cam.orthographicSize + scrollDelta.y * zoomIntencity;
            newZoomValue = Mathf.Clamp(newZoomValue, center.y - cameraBounds.y, center.y + cameraBounds.y);
        }

    }
    private void Update()
    {
        ScrollMovementSmoothess();
        ClampCameraToBounds();
    }
    private void ScrollMovementSmoothess()
    {
        ///perspective camera logick
        if(!cam.orthographic)
        {
            transform.DOMoveY(newZoomValue,duration:zoomSmoothness).SetEase(Ease.OutQuad);
        }
        else
        {
            cam.DOOrthoSize(newZoomValue, duration: zoomSmoothness).SetEase(Ease.OutQuad);
        }
    }
    private void OnPrimaryBtnPressed(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
    {
        if (!isDragging)
            return;
        Vector3 positionDelta = instance.mouseWorldPosition - dragOrigin;
        Vector3 move = -positionDelta * dragSpeed * Time.deltaTime;
        destinationPosition = new Vector3(move.x, 0f, move.z);
        transform.Translate(destinationPosition, Space.World);
        ClampCameraToBounds();
    }
    private void OnPrimaryStartPressing(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
    {
        transform.DOKill();
        ///start dragging
        dragOrigin = mouseWorldPos;
        ClampCameraToBounds();
        isDragging = true;
    }
    private void OnPrimaryBtnRelease(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
    {
        ///end dragging
        isDragging = false;
        transform.DOMove(transform.position + new Vector3(destinationPosition.x,0f, destinationPosition.z),duration:0.25f).SetEase(Ease.OutQuad);
    }
    private void ClampCameraToBounds()
    {
        transform.position = new Vector3(
            x: Mathf.Clamp(transform.position.x, center.x - cameraBounds.x, center.x + cameraBounds.x),
            y: Mathf.Clamp(transform.position.y, center.y - cameraBounds.y, center.y + cameraBounds.y),
            z: Mathf.Clamp(transform.position.z, center.z - cameraBounds.z, center.z + cameraBounds.z));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, cameraBounds);
    }
}
