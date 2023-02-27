using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Controllers
{
    [RequireComponent(typeof(Camera))]
    public class CameraObserver : MonoBehaviour
    {
        [SerializeField] private Transform _followingObject;
        [SerializeField] private Vector3 _offset= Vector3.zero;

        private void LateUpdate()
        {
            transform.position = _followingObject.position+_offset;
        }

    }
}
