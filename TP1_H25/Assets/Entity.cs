using UnityEngine;
using System;
using System.Collections.Generic;

namespace Components
{
    public class Entity
    {
        public uint Id { get; private set; }

        private Dictionary<Type, object> components = new Dictionary<Type, object>();

        public Entity(uint id)
        {
            Id = id;
        }

        // Vérifie si l'entité possède un composant de type T
        public bool HasComponent<T>()
        {
            return components.ContainsKey(typeof(T));
        }

        // Récupère le composant de type T
        public T GetComponent<T>()
        {
            if (components.TryGetValue(typeof(T), out object value))
                return (T)value;
            return default;
        }

        // Ajoute ou met à jour un composant
        public void SetComponent<T>(T component)
        {
            components[typeof(T)] = component;
        }

        // Supprime un composant
        public void RemoveComponent<T>()
        {
            if (components.ContainsKey(typeof(T)))
            {
                components.Remove(typeof(T));
            }
        }
    }
}