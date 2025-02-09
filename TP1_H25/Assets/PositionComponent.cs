using UnityEngine;

namespace Components
{
    public struct PositionComponent : IComponent
    {
        public Vector2 Position;

        public PositionComponent(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public PositionComponent(Vector2 position)
        {
            Position = position;
        }
    }
}