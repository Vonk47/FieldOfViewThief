using Game.Core;
using Game.Core.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Room> _rooms = new List<Room>();
        [SerializeField] private List<PickableObject> _pickables = new List<PickableObject>();
        private List<PickableObject> _pickedObjects = new List<PickableObject>();
        public List<PickableObject> PickedObjects => _pickedObjects;
        public event Action<PickableObject> PickableRegistered;

        [SerializeField] private ChangeStateTriggerZone _changeStateTriggerZone;
        public event Action UserStateChanged;

        private void Start()
        {
            foreach (var pickable in _pickables)
            {
                pickable.OnObjectPicked += RegisterPickedObject;
            }
            foreach (var room in _rooms)
            {
                room.Setup(this);
            }
            _changeStateTriggerZone.ChangeStateTriggered += OnStateChanged;
        }

        private void OnDestroy()
        {
            foreach (var pickable in _pickables)
            {
                pickable.OnObjectPicked -= RegisterPickedObject;
            }
            foreach (var room in _rooms)
            {
                room.UnsubscribePickableEvent(this);
            }
            _changeStateTriggerZone.ChangeStateTriggered -= OnStateChanged;
        }

        private void RegisterPickedObject(PickableObject obj)
        {
            Debug.Log("registered object");
            _pickedObjects.Add(obj);
            PickableRegistered?.Invoke(obj);
        }

        private void OnStateChanged()
        {
            UserStateChanged?.Invoke();
        }

        public bool EverythingPicked => _pickables.Count == _pickedObjects.Count;
    }
}
