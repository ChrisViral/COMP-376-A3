﻿using System;
using UnityEngine;

//ReSharper disable once CheckNamespace
namespace PlanetaryEscape
{
    /// <summary>
    /// MonoBehaviour extension methods
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        #region Extension methods
        /// <summary>
        /// Logs an object message
        /// </summary>
        /// <param name="o">MonoBehaviour object that is logging</param>
        /// <param name="message">Message to log</param>
        public static void Log<T>(this T o, object message) where T : MonoBehaviour => Debug.Log($"[{typeof(T).Name}]: {message}", o);

        /// <summary>
        /// Logs a given error message
        /// </summary>
        /// <param name="o">MonoBehaviour object that is logging</param>
        /// <param name="message">Message to log</param>
        public static void LogError<T>(this T o, object message) where T : MonoBehaviour => Debug.LogError($"[{typeof(T).Name}]: {message}", o);

        /// <summary>
        /// Logs an exception with the given message
        /// </summary>
        /// <param name="o">MonoBehaviour object that is logging</param>
        /// <param name="e">Exception to log</param>
        public static void LogException(this MonoBehaviour o, Exception e) => Debug.LogException(e, o);
        #endregion

    }
}
