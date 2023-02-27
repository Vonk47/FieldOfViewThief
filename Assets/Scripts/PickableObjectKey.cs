using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PickableObjectKey : PickableObject
    {
        protected override PickableObjectType ObjectType()
        {
            return PickableObjectType.Key;
        }
    }
}
