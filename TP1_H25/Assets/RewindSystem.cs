using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems
{
    public class RewindSystem : ISystem
    {
        public string Name => "RewindSystem";

        private const int MaxFrames = 90;
        public void UpdateSystem()
        {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            int count = entities.Count;

            for (int i = 0; i < count; i++)
            {
                Entity e = entities[i];
                bool explosionFlag = false;
                bool collisionFlag = false;
                bool toBeDeletedFlag = false;

                RewindComponent rewind = e.GetComponent<RewindComponent>();
                Vector2 pos = e.GetComponent<PositionComponent>().Position;
                Vector2 vel = e.GetComponent<VelocityComponent>().Velocity;
                CircleType circleType = e.GetComponent<CircleTypeComponent>().circleType;
                if (e.HasComponent<ExplosionFlagComponent>())
                {
                    explosionFlag = e.GetComponent<ExplosionFlagComponent>().JustExploded;
                }
                if (e.HasComponent<CollisionFlagComponent>())
                {
                    collisionFlag = e.GetComponent<CollisionFlagComponent>().JustCollided;
                }
                if (e.HasComponent<DeleteFlagComponent>())
                {
                    DeleteFlagComponent deleteFlag = e.GetComponent<DeleteFlagComponent>();
                    toBeDeletedFlag = deleteFlag.toBeDeleted;
                    if (deleteFlag.surviveTimer > 0)
                    {
                        deleteFlag.surviveTimer--;
                    } else
                    {
                        e.RemoveComponent<DeleteFlagComponent>();
                    }
                }
                int size = e.GetComponent<SizeComponent>().size;
                bool isProtected = e.GetComponent<ProtectionComponent>().IsProtected;
                float protectionTimeRemaining = e.GetComponent<ProtectionComponent>().ProtectionTimeRemaining;
                float cooldownTimeRemaining = e.GetComponent<ProtectionComponent>().CooldownTimeRemaining;
                int collisionCounter = e.GetComponent<CollisionCounterComponent>().CollisionCount;
                Color color = e.GetComponent<ColorComponent>().color;

                rewind.history.Add(new Snapshot
                {
                    position = pos,
                    velocity = vel,
                    circleType = circleType,
                    explosionFlag = explosionFlag,
                    collisionFlag = collisionFlag,
                    size = size,
                    isProtected = isProtected,
                    protectionTimeRemaining = protectionTimeRemaining,
                    cooldownTimeRemaining = cooldownTimeRemaining,
                    collisionCounter = collisionCounter,
                    color = color,
                    toBeDeletedFlag = toBeDeletedFlag,
                });

                if (rewind.history.Count > MaxFrames)
                    rewind.history.RemoveAt(0);

                if (rewind.cooldownFrameRemaining > 0)
                    rewind.cooldownFrameRemaining--;

                e.SetComponent(rewind);
            }
        }
    }
}
