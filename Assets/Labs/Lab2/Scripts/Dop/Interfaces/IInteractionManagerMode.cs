using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAB2DOP
{
    public interface IInteractionManagerMode
    {
        public void Activate();
        public void Deactivate();
        public void TouchInteraction(Touch[] touches);
    }

}
