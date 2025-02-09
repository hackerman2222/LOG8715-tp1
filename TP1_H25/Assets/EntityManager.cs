using System.Collections.Generic;

namespace Components {
    public class EntityManager {
        private static EntityManager _instance;
        public static EntityManager Instance => _instance ??= new EntityManager();

        private Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();
        private uint nextId = 1;

        public Entity CreateEntity() {
            Entity entity = new Entity(nextId++);
            entities[entity.Id] = entity;
            return entity;
        }

        public void DestroyEntity(uint id) {
            if (entities.ContainsKey(id)) {
                entities.Remove(id);
            }
        }

        public List<Entity> GetAllEntities() {
            return new List<Entity>(entities.Values);
        }
    }
}