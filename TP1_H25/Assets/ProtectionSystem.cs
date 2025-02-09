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

            for (int i = 0; i < count; i++)
            {
                Entity e1 = entities[i];

                if (e1.GetComponent<CircleTypeComponent>().circleType == CircleType.Dynamic && e1.GetComponent<CollisionCounterComponent>().CollisionCount >= protectionCollisionCount)
                {
                    ProtectionComponent protection = e1.GetComponent<ProtectionComponent>();
                    if (protection.CooldownTimeRemaining > 0)
                    {
                        if (protection.ProtectionTimeRemaining > 0)
                        {
                            protection.IsProtected = true;
                            protection.ProtectionTimeRemaining -= Time.deltaTime;
                            protection.CooldownTimeRemaining -= Time.deltaTime;
                            e1.SetComponent(protection);
                        } else
                        {
                            protection.IsProtected = false;
                            protection.ProtectionTimeRemaining = 0;
                            protection.CooldownTimeRemaining -= Time.deltaTime;
                            e1.SetComponent(protection);
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
