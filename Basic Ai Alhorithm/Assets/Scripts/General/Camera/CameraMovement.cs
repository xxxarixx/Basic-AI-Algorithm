using AI.Farmer;
using DG.Tweening;
using General;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static General.InputHandler;
namespace General
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private FocusTarget cameraFocus = FocusTarget.Free;
        private Transform focusTarget;
        [SerializeField] private Vector3 cameraBounds;
        [SerializeField] private Vector3 center;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float zoomIntencity = 1f;
        [SerializeField] private float zoomSmoothness = 1f;
        private Vector3 dragOrigin;
        private Vector3 destinationPosition;
        private float newZoomValue = 0f;
        private bool isDragging = false;
        private Camera cam;
        private LayerMask farmerLayer = 1 << 6;
        private int currentFastFocusedIndex = 0;
        private List<AI_Farmer_Dependencies> farmers = new List<AI_Farmer_Dependencies>();
        private enum FocusTarget
        {
            Farmer,
            Free
        }
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
            UpdateListOfActorsToWatch();
        }
        public void UpdateListOfActorsToWatch()
        {
            farmers.Clear();
            farmers.AddRange(FindObjectsByType<AI_Farmer_Dependencies>(findObjectsInactive:FindObjectsInactive.Exclude, sortMode: FindObjectsSortMode.None));
        }
        private void OnEnable()
        {
            instance.onPrimaryBtnTap += OnPrimaryBtnTap;
            instance.onPrimaryBtnStartPressing += OnPrimaryStartPressing;
            instance.onPrimaryBtnPressed += OnPrimaryBtnPressed;
            instance.onPrimaryBtnRelease += OnPrimaryBtnRelease;
            instance.onMouseScroll += OnMouseScroll;
            instance.onAutoFocusKeyTap += OnAutoFocusKeyTap;
        }
        private void OnDisable()
        {
            instance.onPrimaryBtnTap -= OnPrimaryBtnTap;
            instance.onPrimaryBtnStartPressing -= OnPrimaryStartPressing;
            instance.onPrimaryBtnPressed -= OnPrimaryBtnPressed;
            instance.onPrimaryBtnRelease -= OnPrimaryBtnRelease;
            instance.onMouseScroll -= OnMouseScroll;
            instance.onAutoFocusKeyTap -= OnAutoFocusKeyTap;
        }
        private void Update()
        {
            ScrollMovementSmoothess();
            ClampCameraToBounds();
            CameraMovementToTargetFarmer();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, cameraBounds);
        }
        private void OnPrimaryBtnTap(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            Vector3 direction = (mouseWorldPos - cam.transform.position).normalized;
            Ray ray = new Ray(Camera.main.transform.position, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance:100f,farmerLayer))
            {
                if(hit.collider.transform.TryGetComponent(out AI_Farmer_Dependencies AI_Farmer))
                {
                    FocusOnFarmer(AI_Farmer);
                }
            }
            else if(focusTarget != null)
            {
                if (focusTarget.TryGetComponent(out AI_Farmer_Dependencies AI_Farmer))
                {
                    AI_Farmer.apperance.ResetToDefaultColor();
                    cameraFocus = FocusTarget.Free;
                }
                focusTarget = null;
            }
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
        private void OnPrimaryBtnPressed(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            if (!isDragging)
                return;
            Vector3 positionDelta = instance.mouseWorldPosition - dragOrigin;
            Vector3 move = -positionDelta * moveSpeed * Time.deltaTime;
            destinationPosition = new Vector3(move.x, 0f, move.z);
            transform.Translate(destinationPosition, Space.World);
            ClampCameraToBounds();
        }
        private void OnPrimaryStartPressing(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            if (cameraFocus != FocusTarget.Free)
                return;
            transform.DOKill();
            ///start dragging
            dragOrigin = mouseWorldPos;
            ClampCameraToBounds();
            isDragging = true;
        }
        private void OnPrimaryBtnRelease(Vector3 mouseScreenPos, Vector3 mouseWorldPos)
        {
            if (!isDragging)
                return;
            ///end dragging
            isDragging = false;
            var newPosition = transform.position + new Vector3(destinationPosition.x, 0f, destinationPosition.z);
            transform.DOMove(ClampPositionToBounds(newPosition),duration:0.25f).SetEase(Ease.OutQuad);
        }
        private void OnAutoFocusKeyTap()
        {
            if(currentFastFocusedIndex > 0)
            {
                farmers[currentFastFocusedIndex - 1].apperance.ResetToDefaultColor();
            }
            var foundedFarmer = farmers[currentFastFocusedIndex];
            currentFastFocusedIndex++;
            if (currentFastFocusedIndex >= farmers.Count)
                currentFastFocusedIndex = 0;
            if (foundedFarmer == null)
                return;
            FocusOnFarmer(foundedFarmer);
        }
        private void FocusOnFarmer(AI_Farmer_Dependencies AI_Farmer)
        {
            focusTarget = AI_Farmer.transform;
            AI_Farmer.apperance.SetTemporaryColor(AI_Farmer.apperance.whiteMat);
            cameraFocus = FocusTarget.Farmer;
        }
        private void CameraMovementToTargetFarmer()
        {
            if (cameraFocus != FocusTarget.Farmer)
                return;
            Vector3 correctionPosition = new Vector3(-1f, 0f, -3f);
            var newPosition = Vector3.Lerp(transform.position, focusTarget.position + correctionPosition, Time.deltaTime * moveSpeed);
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
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
        private void ClampCameraToBounds()
        {
            transform.position = new Vector3(
                x: Mathf.Clamp(transform.position.x, center.x - cameraBounds.x, center.x + cameraBounds.x),
                y: Mathf.Clamp(transform.position.y, center.y - cameraBounds.y, center.y + cameraBounds.y),
                z: Mathf.Clamp(transform.position.z, center.z - cameraBounds.z, center.z + cameraBounds.z));
        }
        private Vector3 ClampPositionToBounds(Vector3 position)
        {
            return new Vector3(
                x: Mathf.Clamp(position.x, center.x - cameraBounds.x, center.x + cameraBounds.x),
                y: Mathf.Clamp(position.y, center.y - cameraBounds.y, center.y + cameraBounds.y),
                z: Mathf.Clamp(position.z, center.z - cameraBounds.z, center.z + cameraBounds.z));
        }
    }
}
