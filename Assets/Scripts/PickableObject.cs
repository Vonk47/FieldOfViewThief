using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PickableObject : MonoBehaviour
    {

        [SerializeField] private bool _leaveOnPlace;
        protected virtual bool LeaveObjectAfterPick()
        {
            return _leaveOnPlace;
        }

        protected virtual PickableObjectType ObjectType()
        {
            return _type;
        }

        private PickableObjectType _type => ObjectType();
        public PickableObjectType Type => _type;
        public event Action<PickableObject> OnObjectPicked;

        public virtual void PickObject()
        {
            OnObjectPicked?.Invoke(this);
            if (!_leaveOnPlace)
                SetObjectOff();
        }

        private void SetObjectOff()
        {
            gameObject.SetActive(false);
        }
    }


    public enum PickableObjectType
    {
        Key,
        Valuable
    }
}
