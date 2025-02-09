using UnityEngine;

namespace Components
{
    public struct ProtectionComponent : IComponent
    {
        public bool IsProtected;
        public float ProtectionTimeRemaining;
        public float CooldownTimeRemaining;

        public ProtectionComponent(bool isProtected, float protectionTimeRemaining, float cooldownTimeRemaining)
        {
            IsProtected = isProtected;
            ProtectionTimeRemaining = protectionTimeRemaining;
            CooldownTimeRemaining = cooldownTimeRemaining;
        }
    }
}