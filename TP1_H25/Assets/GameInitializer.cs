using UnityEngine;
using Components;
using System.Collections.Generic;
using static Config;

public class ECSTestSpawner : MonoBehaviour
{
    private List<Entity> testEntities = new List<Entity>();

    void Start()
    {
        SpawnTestEntities();
    }

    void SpawnTestEntities()
    {
        // Get camera bounds
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float leftBound = bottomLeft.x + 5f;  // Add margin
        float rightBound = topRight.x - 5f;
        float bottomBound = bottomLeft.y + 5f;
        float topBound = topRight.y - 5f;

        Vector2[] startPositions = {
            new Vector2(leftBound, bottomBound),
            new Vector2(rightBound, bottomBound),
            new Vector2(bottomLeft.x + 50f, topRight.y - 50f),
            new Vector2(rightBound, topBound)
        };

        // Increased speed to make circles move faster
        Vector2[] velocities = {
            new Vector2(6, 3),   // Faster diagonal movement
            new Vector2(-6, -3), // Opposite direction
            new Vector2(0, 0),
            new Vector2(-3, 6)
        };

        int[] sizes = { 3, 6, 9, 4 }; // Random sizes between 1 and 10

        for (int i = 0; i < 4; i++)
        {
            Entity entity = EntityManager.Instance.CreateEntity();

            if (velocities[i] != Vector2.zero)
            {
                entity.SetComponent(new CircleTypeComponent(CircleType.Dynamic));

            }
            else
            {
                entity.SetComponent(new CircleTypeComponent(CircleType.Static));
            }
            entity.SetComponent(new VelocityComponent(velocities[i]));
            entity.SetComponent(new PositionComponent(startPositions[i]));
            entity.SetComponent(new SizeComponent(sizes[i]));
            entity.SetComponent(new CollisionCounterComponent(0));

            testEntities.Add(entity);

            ECSController.Instance.CreateShape(entity.Id, sizes[i]); // Spawn the visual shape
        }
    }
}