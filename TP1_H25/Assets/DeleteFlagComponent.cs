using UnityEngine;

namespace Components
{
    public struct DeleteFlagComponent : IComponent
    {
        public bool toBeDeleted;
        public int surviveTimer;

        public DeleteFlagComponent(bool flag)
        {
            toBeDeleted = flag;
            surviveTimer = 90;
        }
    }
}