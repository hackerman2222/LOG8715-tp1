using UnityEngine;

namespace Components
{
    public struct CollisionFlagComponent : IComponent
    {
        public bool JustCollided;
        
        public CollisionFlagComponent(bool flag)
        {
            JustCollided = flag;
        }
    }
}