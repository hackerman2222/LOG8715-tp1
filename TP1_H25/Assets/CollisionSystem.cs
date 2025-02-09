using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems {
    public class CollisionSystem : ISystem {
        public string Name => "CollisionSystem";
        
        public void UpdateSystem() {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            int count = entities.Count;
            
            for (int i = 0; i < count; i++) {
                for (int j = i + 1; j < count; j++) {
                    Entity e1 = entities[i];
                    Entity e2 = entities[j];
                    
                    if (!(e1.HasComponent<PositionComponent>() && e1.HasComponent<SizeComponent>() &&
                          e2.HasComponent<PositionComponent>() && e2.HasComponent<SizeComponent>())) {
                        continue;
                    }
                    
                    PositionComponent pos1 = e1.GetComponent<PositionComponent>();
                    SizeComponent size1 = e1.GetComponent<SizeComponent>();
                    PositionComponent pos2 = e2.GetComponent<PositionComponent>();
                    SizeComponent size2 = e2.GetComponent<SizeComponent>();
                    
                    float radius1 = size1.size / 2f;
                    float radius2 = size2.size / 2f;
                    float distance = Vector2.Distance(pos1.Position, pos2.Position);
                    float minDistance = radius1 + radius2;
                    
                    if (distance < minDistance) {
                        // Apply collision physics using CollisionUtility
                        if (e1.HasComponent<VelocityComponent>() && e2.HasComponent<VelocityComponent>())
                        {
                            CollisionResult result = CollisionUtility.CalculateCollision(
                                pos1.Position, e1.GetComponent<VelocityComponent>().Velocity, size1.size,
                                pos2.Position, e2.GetComponent<VelocityComponent>().Velocity, size2.size);

                            if (result != null)
                            {
                                pos1.Position = result.position1;
                                pos2.Position = result.position2;

                                e1.SetComponent(pos1);
                                e2.SetComponent(pos2);

                                VelocityComponent vel1 = e1.GetComponent<VelocityComponent>();
                                vel1.Velocity = result.velocity1;
                                e1.SetComponent(vel1);

                                VelocityComponent vel2 = e2.GetComponent<VelocityComponent>();
                                vel2.Velocity = result.velocity2;
                                e2.SetComponent(vel2);
                            }
                        }
                        
                        // Handle size changes
                        if (e1.GetComponent<CircleTypeComponent>().circleType == CircleType.Static || e2.GetComponent<CircleTypeComponent>().circleType == CircleType.Static)
                        {
                            continue;
                        }
                        if (size1.size > size2.size) {
                            size1.size += 1;
                            size2.size -= 1;
                        } else if (size2.size > size1.size) {
                            size2.size += 1;
                            size1.size -= 1;
                        }

                        // Destroy entity if size becomes 0
                        if (size1.size <= 0) {
                            EntityManager.Instance.DestroyEntity(e1.Id);
                            ECSController.Instance.DestroyShape(e1.Id);
                        }
                        if (size2.size <= 0) {
                            EntityManager.Instance.DestroyEntity(e2.Id);
                            ECSController.Instance.DestroyShape(e2.Id);
                        }
                        
                        e1.SetComponent(size1);
                        e2.SetComponent(size2);
                    }
                }
            }
        }
    }
}
