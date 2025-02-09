using UnityEngine;

namespace Components {
    public struct ExplosionFlagComponent : IComponent {
        public bool JustExploded;
        
        public ExplosionFlagComponent(bool flag) {
            JustExploded = flag;
        }
    }
}