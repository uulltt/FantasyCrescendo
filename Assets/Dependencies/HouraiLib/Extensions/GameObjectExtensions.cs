using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HouraiTeahouse {
    /// <summary>
    /// Set of extension methods for GameObjects
    /// </summary>
    public static class GameObjectExtensions {
        /// <summary>
        /// Gets a component of a certain type.
        /// If one doesn't exist, one will be added and returned.
        /// </summary>
        /// <typeparam name="T">the type of the component to retrieve</typeparam>
        /// <param name="gameObject">the GameObject to retrieve the Component</param>
        /// <exception cref="ArgumentNullException"><paramref name="gameObject"/> is null</exception>
        /// <returns>the retrieved Component</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            Check.NotNull("gameObject", gameObject);
            var attempt = gameObject.GetComponent<T>();
            return attempt ? attempt : gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Gets a component of a certain type on a GameObject.
        /// Works exactly like the normal GetComponent, but also logs an error in the console if one is not found.
        /// </summary>
        /// <typeparam name="T">the type of the component to retrieve</typeparam>
        /// <param name="gameObject">the GameObject to retrieve the Component</param>
        /// <exception cref="ArgumentNullException"><paramref name="gameObject"/> is null</exception>
        /// <returns>the retrieved Component</returns>
        public static T SafeGetComponent<T>(this GameObject gameObject) where T : class {
            Check.NotNull("gameObject", gameObject);
            var attempt = gameObject.GetComponent<T>();
            if (attempt != null)
                Log.Warning("Attempted to find a component of type {0}, but did not find one.", typeof(T));
            return attempt;
        }

        /// <summary>
        /// Gets all Components of a certain type that are attached to a set of GameObjects.
        /// </summary>
        /// <typeparam name="T">the type of component to retrieve, can be an interface</typeparam>
        /// <param name="gameObjects">the GameObjects to retrieve</param>
        /// <exception cref="ArgumentNullException"><paramref name="gameObjects"/> is null</exception>
        /// <returns>an enumeration of all components of the type attached to the GameObjects</returns>
        public static IEnumerable<T> GetComponents<T>(this IEnumerable<GameObject> gameObjects) where T : class {
            return Check.NotNull("gameObjects", gameObjects).IgnoreNulls().SelectMany(gameObject => gameObject.GetComponents<T>());
        }
    }
}
