using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public struct RewindComponent : IComponent
    {
        public List<Snapshot> history;
        public int cooldownFrameRemaining;

        public RewindComponent(int cooldown)
        {
            this.history = new List<Snapshot>();
            this.cooldownFrameRemaining = cooldown;
        }
    }
    public struct Snapshot
    {
        public Vector2 position;
        public Vector2 velocity;
        public CircleType circleType;
        public bool explosionFlag;
        public bool collisionFlag;
        public int size;
        public bool isProtected;
        public float protectionTimeRemaining;
        public float cooldownTimeRemaining;
        public float toBeDeleted;
        public int collisionCounter;
        public Color color;
        public bool toBeDeletedFlag;

    }
}
