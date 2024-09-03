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
        #region Mouse
        public delegate void MouseEvents(Vector3 mouseScreenPos,Vector3 mouseWorldPos);
        public delegate void MouseScrollEvents(Vector2 scrollDelta);
        public event MouseEvents onPrimaryBtnTap;
        public event MouseEvents onPrimaryBtnStartPressing;
        public event MouseEvents onPrimaryBtnPressed;
        public event MouseEvents onPrimaryBtnRelease;
        #endregion
        #region Keyboard
        public delegate void KeyboardEvents();
        public event KeyboardEvents onFastForwardKeyTap;
        public event KeyboardEvents onAutoFocusKeyTap;
        #endregion
        public event MouseScrollEvents onMouseScroll;
        public Vector3 mouseWorldPosition { get; private set; }
        public Vector3 mouseScreenPosition { get; private set; }
        private bool isPerformingPrimary = false;
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
            inputControlls.Mouse.PrimaryBtnTap.performed += PrimaryBtnTap_performed;
            inputControlls.Mouse.PrimaryBtnPress.started += PrimaryBtn_started;
            inputControlls.Mouse.PrimaryBtnPress.canceled += PrimaryBtn_canceled;
            inputControlls.Mouse.MouseScroll.performed += MouseScroll_performed;
            inputControlls.Keyboard.FastForwardKeyTap.performed += FastForwardKeyTap_performed;
            inputControlls.Keyboard.AutoFocusKeyTap.performed += AutoFocusKeyTap_performed;
        }


        private void OnDisable()
        {
            inputControlls.Mouse.PrimaryBtnTap.performed -= PrimaryBtnTap_performed;
            inputControlls.Mouse.PrimaryBtnPress.started -= PrimaryBtn_started;
            inputControlls.Mouse.PrimaryBtnPress.canceled -= PrimaryBtn_canceled;
            inputControlls.Keyboard.FastForwardKeyTap.performed -= FastForwardKeyTap_performed;
            inputControlls.Keyboard.AutoFocusKeyTap.performed -= AutoFocusKeyTap_performed;
            inputControlls.Disable();
        }
        private void AutoFocusKeyTap_performed(InputAction.CallbackContext obj)
        {
            onAutoFocusKeyTap?.Invoke();
        }
        private void FastForwardKeyTap_performed(InputAction.CallbackContext obj)
        {
            onFastForwardKeyTap?.Invoke();
        }

        private void MouseScroll_performed(InputAction.CallbackContext obj)
        {
            onMouseScroll?.Invoke(inputControlls.Mouse.MouseScroll.ReadValue<Vector2>());
        }
        private void Update()
        {
            if(isPerformingPrimary)
            {
                PrimaryBtn_performed();
            }
            RecalculateMousePositions();
        }
        private void PrimaryBtn_performed()
        {
            RecalculateMousePositions();
            onPrimaryBtnPressed?.Invoke(mouseScreenPosition, mouseWorldPosition);
        }
        private void PrimaryBtn_started(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnStartPressing?.Invoke(mouseScreenPosition,mouseWorldPosition);
            isPerformingPrimary = true;
        }
        private void PrimaryBtnTap_performed(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnTap?.Invoke(mouseScreenPosition, mouseWorldPosition);
            isPerformingPrimary = true;
        }
        private void PrimaryBtn_canceled(InputAction.CallbackContext obj)
        {
            RecalculateMousePositions();
            onPrimaryBtnRelease?.Invoke(mouseScreenPosition,mouseWorldPosition);
            isPerformingPrimary = false;
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
