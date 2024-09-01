using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace General
{
    [DefaultExecutionOrder(-1)]
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance { get; private set; }
        [SerializeField]private Camera mainCam;
        private InputControlls inputControlls;
        public delegate void MouseEvents(Vector3 mouseScreenPos,Vector3 mouseWorldPos);
        public event MouseEvents onPrimaryBtnTap;
        public event MouseEvents onPrimaryBtnPressed;
        public event MouseEvents onPrimaryBtnRelease;
        public Vector3 mouseWorldPosition { get; private set; }
        public Vector3 mouseScreenPosition { get; private set; }
        private void Awake()
        {
            instance = this;
            inputControlls = new InputControlls();
            if(mainCam == null)
                mainCam = Camera.main;
        }
        private void OnEnable()
        {
            inputControlls.Enable();
            inputControlls.Mouse.PrimaryBtn.started += PrimaryBtn_started;
            inputControlls.Mouse.PrimaryBtn.performed += PrimaryBtn_performed;
            inputControlls.Mouse.PrimaryBtn.canceled += PrimaryBtn_canceled;
        }
        private void OnDisable()
        {
            inputControlls.Mouse.PrimaryBtn.started -= PrimaryBtn_started;
            inputControlls.Mouse.PrimaryBtn.performed -= PrimaryBtn_performed;
            inputControlls.Mouse.PrimaryBtn.canceled -= PrimaryBtn_canceled;
            inputControlls.Disable();
        }
        private void Update()
        {
            RecalculateMousePositions();
        }
        private void PrimaryBtn_performed(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnPressed?.Invoke(mouseScreenPosition, mouseWorldPosition);
        }
        private void PrimaryBtn_started(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnTap?.Invoke(mouseScreenPosition,mouseWorldPosition);
        }
        private void PrimaryBtn_canceled(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnRelease?.Invoke(mouseScreenPosition,mouseWorldPosition);
        }
        private void RecalculateMousePositions()
        {
            mouseScreenPosition = inputControlls.Mouse.MousePosition.ReadValue<Vector2>();
            mouseWorldPosition = ScreenPositionToWorldPosition(mouseScreenPosition);
        }
        private Vector3 ScreenPositionToWorldPosition(Vector3 screenPosition)
        {
            Ray ray = mainCam.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Raycast hit something
                Vector3 worldPoint = hit.point;
                return worldPoint;
            }
            return default;

        }
    }

}
