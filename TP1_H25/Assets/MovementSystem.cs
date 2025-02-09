using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems {
    public class MovementSystem : ISystem {
        public string Name => "MovementSystem";
        
        public void UpdateSystem() {
            Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
            Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));
            float leftBound = bottomLeft.x;
            float rightBound = topRight.x;
            float bottomBound = bottomLeft.y;
            float topBound = topRight.y;
            
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            foreach(Entity e in entities) {
                if(e.HasComponent<PositionComponent>() && e.HasComponent<VelocityComponent>()) {
                    PositionComponent pos = e.GetComponent<PositionComponent>();
                    VelocityComponent vel = e.GetComponent<VelocityComponent>();
                    
                    // Update position.
                    pos.Position += vel.Velocity * Time.deltaTime;
                    
                    // If available, use size to compute radius.
                    float radius = 0f;
                    if(e.HasComponent<SizeComponent>()) {
                        SizeComponent size = e.GetComponent<SizeComponent>();
                        radius = size.size / 2f;
                    }
                    
                    // Bounce off left/right edges.
                    if(pos.Position.x - radius < leftBound) {
                        pos.Position.x = leftBound + radius;
                        vel.Velocity.x = -vel.Velocity.x;
                    } else if(pos.Position.x + radius > rightBound) {
                        pos.Position.x = rightBound - radius;
                        vel.Velocity.x = -vel.Velocity.x;
                    }
                    
                    // Bounce off top/bottom edges.
                    if(pos.Position.y - radius < bottomBound) {
                        pos.Position.y = bottomBound + radius;
                        vel.Velocity.y = -vel.Velocity.y;
                    } else if(pos.Position.y + radius > topBound) {
                        pos.Position.y = topBound - radius;
                        vel.Velocity.y = -vel.Velocity.y;
                    }
                    
                    e.SetComponent(pos);
                    e.SetComponent(vel);
                    ECSController.Instance.UpdateShapePosition(e.Id, pos.Position);
                }
            }
        }
    }
}
