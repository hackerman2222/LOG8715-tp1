using UnityEngine;
using Components;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor.Experimental.GraphView;

namespace Systems {
    public class ColorSystem : ISystem
    {
        public string Name => "ColorSystem";

        public void UpdateSystem()
        {
            List<Entity> entities = EntityManager.Instance.GetAllEntities();

            foreach (Entity e in entities)
            {
                Color newColor = new();
                bool assigned = false;

                // Static circles  Red
                if (e.HasComponent<CircleTypeComponent>() && e.GetComponent<CircleTypeComponent>().circleType == CircleType.Static)
                {
                    newColor = Color.red;
                    assigned = true;
                }

                // Just collided  Green
                if (!assigned && e.HasComponent<CollisionFlagComponent>())
                {
                    newColor = Color.green;
                    assigned = true;
                }

                // The following protection-based colors are commented out for now
                // Protected  White
                if (!assigned && e.HasComponent<ProtectionComponent>())
                {
                    ProtectionComponent prot = e.GetComponent<ProtectionComponent>();
                    if (prot.IsProtected)
                    {
                        newColor = Color.white;
                        assigned = true;
                    }
                    else if (e.GetComponent<SizeComponent>().size <= ECSController.Instance.Config.protectionSize 
                        && e.GetComponent<CollisionCounterComponent>().CollisionCount == ECSController.Instance.Config.protectionCollisionCount - 1)
                    {
                        newColor = Color.blue;
                        assigned = true;
                    }
                    else if (prot.CooldownTimeRemaining > 0f)
                    {
                        newColor = Color.yellow;
                        assigned = true;
                    }
                }

                // Circles on the verge of explosion  Orange
                if (!assigned && e.HasComponent<SizeComponent>() && e.GetComponent<SizeComponent>().size + 1 == ECSController.Instance.Config.explosionSize)
                {
                    newColor = new Color(1f, 0.5f, 0f); // Orange
                    assigned = true;
                }

                // Just exploded  Pink
                if (!assigned && e.HasComponent<ExplosionFlagComponent>())
                {
                    newColor = Color.magenta;
                    assigned = true;
                }

                // Dynamic circles without collisions  Dark Blue
                if (!assigned && e.HasComponent<CircleTypeComponent>() && e.GetComponent<CircleTypeComponent>().circleType == CircleType.Dynamic)
                {
                    newColor = new Color(0f, 0f, 0.55f);
                    assigned = true;
                }

                ECSController.Instance.UpdateShapeColor(e.Id, newColor);
            }
        }
    }
}