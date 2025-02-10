using UnityEngine;

namespace Components
{
    public struct ColorComponent : IComponent
    {
        public Color color;

        public ColorComponent(Color color)
        {
            this.color = color;
        }

    }

}
