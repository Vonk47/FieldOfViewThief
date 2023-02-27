using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PickableObjectValuable : PickableObject
    {
        [SerializeField] private int _value;
      
        public int Value => _value;

        protected override PickableObjectType ObjectType()
        {
            return PickableObjectType.Valuable;
        }

    }
}
