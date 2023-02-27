using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Mechanics.Levels
{
    public class VignetteKeyPoint : KeyPoint
    {
        protected override void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
