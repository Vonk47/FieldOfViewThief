using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class DoorTriggerZone : MonoBehaviour
    {
        bool _isEnabled;
        [SerializeField] Door _door;

        public void SetEnabled()
        {
            Debug.Log("Door can be opened! " + _door.name);
            _isEnabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!_isEnabled)
            {
                return;
            }

            _door.OpenDoor();

        }

    }
}
