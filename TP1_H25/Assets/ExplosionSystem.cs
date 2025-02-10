using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems
{
    public class ExplosionSystem : ISystem
    {
        public string Name => "ExplosionSystem";

        public void UpdateSystem()
        {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            List<Entity> toExplode = new List<Entity>();
            List<Entity> newEntities = new List<Entity>();

            var config = ECSController.Instance?.Config;
            if (config == null)
            {
                Debug.LogError("ECSController.Instance or Config is missing!");
                return;
            }

            foreach (Entity e in entities)
            {
                if (e.HasComponent<SizeComponent>())
                {
                    SizeComponent size = e.GetComponent<SizeComponent>();
                    if (size.size >= config.explosionSize && e.GetComponent<CircleTypeComponent>().circleType == CircleType.Dynamic)
                    {
                        toExplode.Add(e);
                    }
                }
            }

            foreach (Entity e in toExplode)
            {
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

                foreach (Vector2 dir in directions)
                {
                    Entity newEntity = EntityManager.Instance.CreateEntity();
                    newEntity.SetComponent(new PositionComponent(center.x, center.y));
                    newEntity.SetComponent(new VelocityComponent(dir.x * 2f, dir.y * 2f));
                    newEntity.SetComponent(new SizeComponent(newSize));
                    newEntity.SetComponent(new CircleTypeComponent(CircleType.Dynamic));
                    newEntity.SetComponent(new ExplosionFlagComponent(true)); // Set explosion flag ONLY on newly created entities

                    ECSController.Instance.CreateShape(newEntity.Id, newSize);
                    newEntities.Add(newEntity); // Track newly created entities for flag removal
                }

                ECSController.Instance.DestroyShape(e.Id);
                EntityManager.Instance.DestroyEntity(e.Id);
            }

            // Remove explosion flags on newly created entities in the next frame
            if (newEntities.Count > 0)
            {
                ECSController.Instance.StartCoroutine(RemoveExplosionFlagsNextFrame(newEntities));
            }
        }

        private System.Collections.IEnumerator RemoveExplosionFlagsNextFrame(List<Entity> entities)
        {
            yield return null; // Wait for next frame
            foreach (Entity e in entities)
            {
                if (e.HasComponent<ExplosionFlagComponent>())
                {
                    e.RemoveComponent<ExplosionFlagComponent>();
                }
            }
        }
    }
}