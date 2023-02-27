using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Utility
{
    public class ChangeStateTriggerZone : MonoBehaviour
    {
        public event Action ChangeStateTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            ChangeStateTriggered?.Invoke();
        }
    }
}
