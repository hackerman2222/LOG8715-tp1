using UnityEngine;
using Components;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UIElements;

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

            entity.SetComponent(new PositionComponent(shapeConfig.initialPosition));
            entity.SetComponent(new VelocityComponent(shapeConfig.initialVelocity));
            entity.SetComponent(new SizeComponent(shapeConfig.initialSize));
            entity.SetComponent(new CircleTypeComponent(shapeConfig.initialVelocity == Vector2.zero ? CircleType.Static : CircleType.Dynamic));
            entity.SetComponent(new CollisionCounterComponent(0));
            entity.SetComponent(new ProtectionComponent(false, 0f, 0f));
            entity.SetComponent(new RewindComponent(90));
            entity.SetComponent(new ColorComponent(new()));

            testEntities.Add(entity);

            ECSController.Instance?.CreateShape(entity.Id, shapeConfig.initialSize);
        }
    }
}
