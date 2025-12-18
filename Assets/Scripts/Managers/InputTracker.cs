using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Managers
{
    public class InputTracker : MonoBehaviour
    {
        public enum InputType
        {
            MouseKeyboard,
            Controller
        }
        
        [Header("Scene Configuration")]
        [Tooltip("Scenes where the cursor should always be visible.")]
        [SerializeField] private List<SceneAsset> cursorAlwaysVisibleScenes;
        
        public InputType LastInputType { get; private set; } = InputType.MouseKeyboard;
        public bool IsActive { get; private set; } = false;
        
        private Vector2 _lastMousePosition;
        private bool _allowCursorHide = true; // Controlled by scene
        
        private void Awake()
        {
            if (Mouse.current != null)
                _lastMousePosition = Mouse.current.position.ReadValue();

            ApplyCursorVisibility();
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Cursor can hide in all scenes EXCEPT the ones on the list
            _allowCursorHide = true;
            foreach (var sceneAsset in cursorAlwaysVisibleScenes)
            {
                if (sceneAsset != null && scene.name == sceneAsset.name)
                {
                    _allowCursorHide = false;
                    break;
                }
            }

            // Reset input type to Controller if hiding is allowed
            if (_allowCursorHide)
                LastInputType = InputType.Controller;

            ApplyCursorVisibility();
        }
        
        public void Activate()
        {
            IsActive = true;
            ApplyCursorVisibility();
        }
        
        public void Deactivate()
        {
            IsActive = false;

            // Hide cursor if the scene allows hiding
            if (_allowCursorHide)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None; // hidden and free
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // visible
            }
        }
        
        private void Update()
        {
            if (!IsActive)
                return;
            
            // Mouse movement
            if (Mouse.current != null)
            {
                Vector2 currentMousePos = Mouse.current.position.ReadValue();

                if (currentMousePos != _lastMousePosition)
                {
                    _lastMousePosition = currentMousePos;
                    SetMouseKeyboardInput();
                }
            }
            
            // Keyboard input
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                SetMouseKeyboardInput();
            }

            // Controller input
            if (Gamepad.current != null)
            {
                foreach (var control in Gamepad.current.allControls)
                {
                    if (control is ButtonControl button && button.wasPressedThisFrame)
                    {
                        SetControllerInput();
                        break;
                    }
                }
            }
        }
        
        // -------------------------
        // Public API (called by UI / gameplay)
        // -------------------------
        public void SetMouseKeyboardInput()
        {
            if (LastInputType == InputType.MouseKeyboard) return;

            LastInputType = InputType.MouseKeyboard;
            ApplyCursorVisibility();
        }

        public void SetControllerInput()
        {
            if (LastInputType == InputType.Controller) return;

            // Save last visible mouse position before hiding
            if (Mouse.current != null)
                _lastMousePosition = Mouse.current.position.ReadValue();

            LastInputType = InputType.Controller;
            ApplyCursorVisibility();
        }
        
        public void ApplyCursorVisibility()
        {
            if (!_allowCursorHide || LastInputType == InputType.MouseKeyboard)
            {
                // Cursor always visible in these scenes, or last input is mouse/keyboard
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; // show and free
            }
            else
            {
                // Cursor hidden in scenes that allow hiding, and last input was controller
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None; // hide and free
            }
        }
        
        // Registers all relevant callbacks on a VisualElement (Button, etc.)
        // Returns an Action that will unregister all of them
        public Action RegisterElementForInputTracking(VisualElement element)
        {
            EventCallback<PointerEnterEvent> pointerEnterHandler = _ =>
            {
                SetMouseKeyboardInput();
                element.Focus();
            };
            EventCallback<PointerDownEvent> pointerDownHandler = _ => SetMouseKeyboardInput();
            EventCallback<NavigationMoveEvent> navigationMoveHandler = _ => SetControllerInput();
            EventCallback<NavigationSubmitEvent> navigationSubmitHandler = _ => SetControllerInput();

            element.RegisterCallback(pointerEnterHandler);
            element.RegisterCallback(pointerDownHandler);
            element.RegisterCallback(navigationMoveHandler);
            element.RegisterCallback(navigationSubmitHandler);

            // Return an Action to unregister all callbacks
            return () =>
            {
                element.UnregisterCallback(pointerEnterHandler);
                element.UnregisterCallback(pointerDownHandler);
                element.UnregisterCallback(navigationMoveHandler);
                element.UnregisterCallback(navigationSubmitHandler);
            };
        }
    }
}
