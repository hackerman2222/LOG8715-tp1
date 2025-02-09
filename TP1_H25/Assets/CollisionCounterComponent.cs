using UnityEngine;

namespace Components
{
 public struct CollisionCounterComponent  : IComponent
 {
    public int CollisionCount;

    public CollisionCounterComponent (int collisionCount)
    {
        CollisionCount = collisionCount;
    }
 }

}