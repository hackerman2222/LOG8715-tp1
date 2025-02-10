using UnityEngine;
using Components;
using System.Collections.Generic;

namespace Systems
{
    public class InputSystem : ISystem
    {
        public string Name => "InputSystem";

        public void UpdateSystem()
        {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();
            List<Entity> newFlags = new List<Entity>();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (Entity e in entities)
                {
                    RewindComponent rewind = e.GetComponent<RewindComponent>();

                    if (rewind.cooldownFrameRemaining > 0)
                    {
                        Debug.Log("Il reste " + rewind.cooldownFrameRemaining + " au Cooldown");
                        continue;
                    }

                    Snapshot rewindState = rewind.history[0];

                    if (rewindState.toBeDeletedFlag)
                    {
                        ECSController.Instance.DestroyShape(e.Id);
                        EntityManager.Instance.DestroyEntity(e.Id);
                        continue;
                    }

                    PositionComponent pos = e.GetComponent<PositionComponent>();
                    pos.Position = rewindState.position;
                    e.SetComponent(pos);

                    VelocityComponent vel = e.GetComponent<VelocityComponent>();
                    vel.Velocity = rewindState.velocity;
                    e.SetComponent(vel);

                    CircleTypeComponent circleType = e.GetComponent<CircleTypeComponent>();
                    circleType.circleType = rewindState.circleType;
                    e.SetComponent(circleType);

                    ExplosionFlagComponent explosionFlag = e.GetComponent<ExplosionFlagComponent>();
                    explosionFlag.JustExploded = rewindState.explosionFlag;
                    if (explosionFlag.JustExploded)
                    {
                        newFlags.Add(e);
                        e.SetComponent(explosionFlag);
                    }

                    CollisionFlagComponent collisionFlag = e.GetComponent<CollisionFlagComponent>();
                    collisionFlag.JustCollided = rewindState.collisionFlag;
                    if (collisionFlag.JustCollided)
                    {
                        e.SetComponent(collisionFlag);
                        if (!explosionFlag.JustExploded)
                        {
                            newFlags.Add(e);
                        }
                    }

                    SizeComponent size = e.GetComponent<SizeComponent>();
                    size.size = rewindState.size;
                    e.SetComponent(size);

                    ProtectionComponent protection = e.GetComponent<ProtectionComponent>();
                    protection.IsProtected = rewindState.isProtected;
                    protection.ProtectionTimeRemaining = rewindState.protectionTimeRemaining;
                    protection.CooldownTimeRemaining = rewindState.cooldownTimeRemaining;
                    e.SetComponent(protection);

                    CollisionCounterComponent collisionCounter = e.GetComponent<CollisionCounterComponent>();
                    collisionCounter.CollisionCount = rewindState.collisionCounter;
                    e.SetComponent(collisionCounter);

                    ColorComponent color = e.GetComponent<ColorComponent>();
                    color.color = rewindState.color;
                    e.SetComponent(color);

                    rewind.cooldownFrameRemaining = 90;
                    e.SetComponent(rewind);
                }
                if (newFlags.Count > 0)
                {
                    ECSController.Instance.StartCoroutine(RemoveFlags(newFlags));
                }
            }

            // Handle mouse click for interactions
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 clickPos = new Vector2(mousePos.x, mousePos.y);

                foreach (Entity e in entities)
                {
                    if (e.HasComponent<PositionComponent>() && e.HasComponent<SizeComponent>() && e.HasComponent<CircleTypeComponent>())
                    {
                        PositionComponent pos = e.GetComponent<PositionComponent>();
                        SizeComponent size = e.GetComponent<SizeComponent>();
                        CircleTypeComponent type = e.GetComponent<CircleTypeComponent>();
                        Vector2 center = pos.Position;

                        if (Vector2.Distance(clickPos, center) <= size.size / 2f)
                        {
                            if (type.circleType == CircleType.Dynamic)
                            {
                                if (size.size >= 4)
                                {
                                    Debug.Log("Explosion triggered by click on entity " + e.Id);
                                    size.size = ECSController.Instance.Config.explosionSize;
                                    e.SetComponent(size);
                                }
                                else
                                {
                                    Debug.Log("Destroying entity " + e.Id + " on click");
                                    ECSController.Instance.DestroyShape(e.Id);
                                    EntityManager.Instance.DestroyEntity(e.Id);
                                }
                            }
                        }
                    }
                }
            }
        }
        private System.Collections.IEnumerator RemoveFlags(List<Entity> entities)
        {
            yield return null; // Wait for next frame
            foreach (Entity e in entities)
            {
                if (e.HasComponent<ExplosionFlagComponent>())
                {
                    e.RemoveComponent<ExplosionFlagComponent>();
                }
                if (e.HasComponent<CollisionFlagComponent>())
                {
                    e.RemoveComponent<CollisionFlagComponent>();
                }
            }
        }
    }
}