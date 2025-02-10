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
            // Handle rewind key press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //EventManager.TriggerEvent("RewindRequested");
            }

            // Handle mouse click for interactions
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 clickPos = new Vector2(mousePos.x, mousePos.y);
                List<Entity> entities = EntityManager.Instance.GetAllEntities();

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
    }
}