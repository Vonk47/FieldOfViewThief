using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Levels
{
    [RequireComponent(typeof(Collider))]
    public class KeyPoint : MonoBehaviour
    {
        public event Action OnMainActionTriggered;

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnMainActionTriggered?.Invoke();
            Dispose();
        }

        protected virtual void Dispose()
        {
            
        }

    }
}
