using UnityEngine;

namespace Components
{
    public struct SizeComponent : IComponent
    {
        public int size;

        public SizeComponent(int size)
        {
            this.size = size;
        }
        
    }
    
}
