using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Levels
{
    public class Room : MonoBehaviour
    {
        List<KeyPoint> keys = new List<KeyPoint>();
        [SerializeField] PickableObject[] _availablePickables;
        [SerializeField] DoorTriggerZone[] _availableDoors;

        // todo add serialized dictionary
        private Dictionary<PickableObject, DoorTriggerZone> _availableDoorsWithKeys = new Dictionary<PickableObject, DoorTriggerZone>();

        private void Start()
        {
            if (_availablePickables.Length != _availableDoors.Length)
            {
                Debug.LogError("Pickables and doors seems wrong, please fix");
                return;
            }

            SubscribeEvents();
            for (int i = 0; i < _availableDoors.Length; i++)
            {
                _availableDoorsWithKeys.Add(_availablePickables[i], _availableDoors[i]);
            }
        }

        public void Setup(Level level)
        {
            level.PickableRegistered += OnPickableObjectRegistered;
        }

        private void OnPickableObjectRegistered(PickableObject obj)
        {
            if (obj.Type == PickableObjectType.Key)
            {
                _availableDoorsWithKeys[obj].SetEnabled();
            }
        }

        private void SubscribeEvents()
        {
            foreach (var key in keys)
            {
                switch (key)
                {
                    case VignetteKeyPoint:
                        key.OnMainActionTriggered += DoVignetteEvent;
                        break;
                }
            }
        }

        private void DoVignetteEvent()
        {

        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }


        private void UnsubscribeEvents()
        {
            foreach (var key in keys)
            {
                switch (key)
                {
                    case VignetteKeyPoint:
                        key.OnMainActionTriggered -= DoVignetteEvent;
                        break;
                }

            }
        }

        public void UnsubscribePickableEvent(Level level)
        {
            level.PickableRegistered -= OnPickableObjectRegistered;
        }
    }
}
