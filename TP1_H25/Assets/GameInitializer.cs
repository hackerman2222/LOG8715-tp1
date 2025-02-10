using UnityEngine;
using Components;
using System.Collections.Generic;

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

        float midX = (leftBound + rightBound) / 2;
        float midY = (bottomBound + topBound) / 2;

        Vector2[] startPositions = {
            new Vector2(leftBound, bottomBound),
            new Vector2(rightBound, bottomBound),
            new Vector2(leftBound, topBound),
            new Vector2(rightBound, topBound),
        };

        Vector2[] middlePositions = {
            new Vector2(midX - 8, midY),
            new Vector2(midX + 8, midY),
            new Vector2(midX, midY - 8),
            new Vector2(midX, midY + 8)
        };

        // Increased speed for corner circles
        Vector2[] velocities = {
            new Vector2(6, 3),
            new Vector2(-6, -3),
            new Vector2(0, 0),
            new Vector2(-3, 6)
        };

        // Lower speed for middle circles
        Vector2[] middleVelocities = {
            new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)),
            new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)),
            new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)),
            new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f))
        };

        int[] sizes = { 3, 6, 9, 4 }; // Random sizes between 1 and 10

        // Spawn corner circles
        for (int i = 0; i < 4; i++)
        {
            SpawnEntity(startPositions[i], velocities[i], sizes[i]);
        }

        // Spawn middle circles (lower speed)
        for (int i = 0; i < 4; i++)
        {
            SpawnEntity(middlePositions[i], middleVelocities[i], Random.Range(2, 6));
        }
    }

    void SpawnEntity(Vector2 position, Vector2 velocity, int size)
    {
        Entity entity = EntityManager.Instance.CreateEntity();

        entity.SetComponent(new PositionComponent(position));
        entity.SetComponent(new VelocityComponent(velocity));
        entity.SetComponent(new SizeComponent(size));
        entity.SetComponent(new CircleTypeComponent(velocity == Vector2.zero ? CircleType.Static : CircleType.Dynamic));
        entity.SetComponent(new CollisionCounterComponent(0));
        entity.SetComponent(new ProtectionComponent(false,0f,0f));

        testEntities.Add(entity);

        ECSController.Instance.CreateShape(entity.Id, size);
    }
}