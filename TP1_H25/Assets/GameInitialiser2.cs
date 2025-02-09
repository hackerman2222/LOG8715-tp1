using UnityEngine;
using Components;
using System.Collections.Generic;

public class GameInitialiser2 : MonoBehaviour
{
    private List<Entity> testEntities = new List<Entity>();

    void Start()
    {
        SpawnTestEntities();
    }

    void SpawnTestEntities()
    {
        var config = ECSController.Instance?.Config;

        if (config == null)
        {
            Debug.LogError("ECSController.Instance or Config is missing!");
            return;
        }

        if (config.circleInstancesToSpawn == null || config.circleInstancesToSpawn.Count == 0)
        {
            Debug.LogError("No circles to spawn in config!");
            return;
        }

        foreach (var shapeConfig in config.circleInstancesToSpawn)
        {
            Entity entity = EntityManager.Instance?.CreateEntity();
            if (entity == null)
            {
                Debug.LogError("EntityManager instance is null!");
                return;
            }

            entity.SetComponent(new PositionComponent(shapeConfig.initialPosition.x, shapeConfig.initialPosition.y));
            if (shapeConfig.initialVelocity != Vector2.zero)
            {
                entity.SetComponent(new VelocityComponent(shapeConfig.initialVelocity.x, shapeConfig.initialVelocity.y));
                entity.SetComponent(new CircleTypeComponent(CircleType.Dynamic));

            }
            else
            {
                entity.SetComponent(new CircleTypeComponent(CircleType.Static));
            }
            entity.SetComponent(new SizeComponent(shapeConfig.initialSize));
            entity.SetComponent(new CollisionCounterComponent(0));

            testEntities.Add(entity);

            ECSController.Instance?.CreateShape(entity.Id, shapeConfig.initialSize);
        }
    }
}
