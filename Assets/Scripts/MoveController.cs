
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.Mechanics.Movement
{
    public class MoveController
    {
        private readonly PlayerInputAction _inputActions;

        public event Action OnPress;

        public MoveController()
        {
            _inputActions = new PlayerInputAction();
            _inputActions.Enable();
            _inputActions.Default.Pressing.performed += OnPressPerformed;
        }

        private void OnPressPerformed(InputAction.CallbackContext callbackContext)
        {
            OnPress?.Invoke();
        }

        public void Dispose()
        {
            _inputActions.Dispose();
        }

    }

   
}
