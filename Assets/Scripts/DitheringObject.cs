using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Utility
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DitheringObject : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void OnDitherTrigger(Material target)
        {
            _renderer.material = target;
        }
    }
}
