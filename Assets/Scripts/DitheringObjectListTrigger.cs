using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Utility
{
    public class DitheringObjectListTrigger : MonoBehaviour
    {
        [SerializeField] private DitheringObject[] _ditheringObjects;
        [SerializeField] private Material _ditherMaterial;

        public void OnActionTriggered()
        {
            foreach(var dither in _ditheringObjects)
            {
                dither.OnDitherTrigger(_ditherMaterial);
            }
        }
    }
}
