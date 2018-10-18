using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Asteroid generation wave controller
    /// </summary>
    public class AsteroidWaveController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField, Tooltip("Max amount of asteroids spawned at once"), Range(1, 4)]
        internal int max;
        [SerializeField]
        private Vector2 spawnTimeRange;
        #endregion

        #region Static methods
        /// <summary>
        /// Checks for collisions between created spawn locations
        /// </summary>
        /// <param name="spawns">Spawn locations created so far</param>
        /// <param name="pos">New spawn location to be added</param>
        /// <returns></returns>
        private static bool CheckCollisions(List<Vector3> spawns, Vector3 pos)
        {
            //Check all existing vectors
            foreach (Vector3 v in spawns)
            {
                //If their distance is less than 1, reject
                if (Mathf.Abs(v.x - pos.x) < 1f) { return false; }
            }

            //If valid, add the vector to the list
            spawns.Add(pos);
            return true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a random spawn vector within the specified range
        /// </summary>
        /// <returns>A random Vector3 in the spawn range</returns>
        private Vector3 RandomSpawn() => new Vector3(Random.Range(this.spawn.x, this.spawn.y), 0f, this.spawn.z);

        /// <summary>
        /// Asteroid wave generator
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //First wave delay
            yield return new WaitForSeconds(this.delay);

            //Spawn loop
            while (true)
            {
                //Decide randomly how many asteroids to spawn
                int count = Random.Range(1, this.max + 1);

                //Spawn only one
                if (count == 1) { Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], RandomSpawn(), Quaternion.identity); }
                else
                {
                    //Store spawn locations
                    List<Vector3> spawns = new List<Vector3>(count) { RandomSpawn() };
                    for (int i = 1; i < count; i++)
                    {
                        //Generate random vectors until they are distant enough
                        Vector3 v;
                        do { v = RandomSpawn(); }
                        while (!CheckCollisions(spawns, v));
                    }

                    //Spawn all asteroids on created spawn locations
                    foreach (Vector3 v in spawns) { Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], v, Quaternion.identity); }
                }

                //Wait a random amount of time before spawning next wave
                yield return new WaitForSeconds(Random.Range(this.spawnTimeRange.x, this.spawnTimeRange.y));
            }
        }
        #endregion
    }
}