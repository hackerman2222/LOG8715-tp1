using UnityEngine;

namespace Components
{
    public enum CircleType
    {
        Static,
        Dynamic
    }
    
    public struct CircleTypeComponent : IComponent
    {
        public CircleType circleType;

        public CircleTypeComponent(CircleType circleType)
        {
            this.circleType = circleType;
        }
        
    }
    
}