using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems
{
    public class ProtectionSystem : ISystem
    {
        public string Name => "ProtectionSystem";

        public void UpdateSystem()
        {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            int count = entities.Count;

            float protectionCollisionCount = ECSController.Instance.Config.protectionCollisionCount;
            float protectionCooldown = ECSController.Instance.Config.protectionCooldown;
            float protectionDuration = ECSController.Instance.Config.protectionDuration;
            float protectionSize = ECSController.Instance.Config.protectionSize;

            for (int i = 0; i < count; i++)
            {
                Entity e1 = entities[i];
                CollisionCounterComponent collisionCount = e1.GetComponent<CollisionCounterComponent>();

                if (e1.GetComponent<CircleTypeComponent>().circleType == CircleType.Dynamic 
                    && collisionCount.CollisionCount >= protectionCollisionCount 
                    && e1.GetComponent<SizeComponent>().size <= protectionSize)
                {
                    ProtectionComponent protection = e1.GetComponent<ProtectionComponent>();
                    if (protection.CooldownTimeRemaining > 0f)
                    {
                        if (protection.ProtectionTimeRemaining > 0f)
                        {
                            protection.IsProtected = true;
                            protection.ProtectionTimeRemaining -= Time.deltaTime;
                            protection.CooldownTimeRemaining -= Time.deltaTime;
                            e1.SetComponent(protection);
                        } else
                        {
                            protection.IsProtected = false;
                            protection.ProtectionTimeRemaining = 0f;
                            protection.CooldownTimeRemaining -= Time.deltaTime;
                            e1.SetComponent(protection);
                        }
                        if (protection.CooldownTimeRemaining == 0f)
                        {
                            collisionCount.CollisionCount = 0;
                            e1.SetComponent(collisionCount);
                        }
                        continue;
                    }
                    protection.IsProtected = true;
                    protection.CooldownTimeRemaining = protectionCooldown;
                    protection.ProtectionTimeRemaining = protectionDuration;
                    e1.SetComponent(protection);
                }

            }
        }
    }
}
