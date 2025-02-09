using UnityEngine;

namespace Components
{
    public struct VelocityComponent : IComponent
    {
        public Vector2 Velocity;

        public VelocityComponent(float x, float y)
        {
            Velocity = new Vector2(x, y);
        }

        public VelocityComponent(Vector2 velocity)
        {
            Velocity = velocity;
        }
    }
}