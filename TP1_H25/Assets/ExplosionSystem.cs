using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems {
    public class ExplosionSystem : ISystem {
        public string Name => "ExplosionSystem";
        
        public void UpdateSystem() {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            List<Entity> toExplode = new List<Entity>();
            
            foreach (Entity e in entities) {
                if (e.HasComponent<SizeComponent>() && e.HasComponent<ExplosionFlagComponent>()) {
                    SizeComponent size = e.GetComponent<SizeComponent>();
                    if (size.size >= 4) {
                        toExplode.Add(e);
                    }
                }
            }
            
            foreach (Entity e in toExplode) {
                PositionComponent pos = e.GetComponent<PositionComponent>();
                SizeComponent size = e.GetComponent<SizeComponent>();
                int newSize = Mathf.Max(1, size.size / 4);
                Vector2 center = pos.Position;
                
                Vector2[] directions = {
                    new Vector2(1, 1).normalized,
                    new Vector2(-1, 1).normalized,
                    new Vector2(1, -1).normalized,
                    new Vector2(-1, -1).normalized
                };
                
                foreach (Vector2 dir in directions) {
                    Entity newEntity = EntityManager.Instance.CreateEntity();
                    newEntity.SetComponent(new PositionComponent(center.x, center.y));
                    newEntity.SetComponent(new VelocityComponent(dir.x * 2f, dir.y * 2f));
                    newEntity.SetComponent(new SizeComponent(newSize));
                    newEntity.SetComponent(new CircleTypeComponent(CircleType.Dynamic));
                    newEntity.SetComponent(new CollisionCounterComponent(0));
                    
                    ECSController.Instance.CreateShape(newEntity.Id, newSize);
                }
                
                ECSController.Instance.DestroyShape(e.Id);
                EntityManager.Instance.DestroyEntity(e.Id);
            }
        }
    }
}