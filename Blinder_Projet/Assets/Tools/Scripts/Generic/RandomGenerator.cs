using System;
using UnityEngine;


namespace Tools
{
    // we use the Random Built-in Unity module to generate randomness
    using Random = UnityEngine.Random;

    /// <summary>
    /// Handle and centralize number generation to be able to
    /// regenerate the same situation with a recorded seed.
    /// </summary>
    public class RandomGenerator : MonoSingleton<RandomGenerator>
    {
        public int Seed;
        public bool UseRandomSeed = true;

        private static readonly int MIN_INT = int.MinValue + 1;
        private static readonly int MAX_INT = int.MaxValue;

        void Start()
        {
            Restart();
        }

        /// <summary>
        /// Regenerate the seed if needed
        /// and restart the number sequence
        /// </summary>
        public void Restart()
        {
            if (UseRandomSeed)
            {
                Seed = GenerateRandomSeed();
            }

            Random.InitState(Seed);
        }

        /// <summary>
        /// Generate a random seed from the current hased time value
        /// </summary>
        /// <returns>The new seed</returns>
        public int GenerateRandomSeed()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.Ticks.ToString().GetHashCode();
        }

        /// <summary>
        /// Return a random int from int.MinValue [exclusive]
        /// to int.MaxValue [exclusive] 
        /// </summary>
        /// <returns></returns>
        public int NextInt()
        {
            return NextInt(MIN_INT, MAX_INT);
        }

        /// <summary>
        /// Return a random int from 0 [inclusive] 
        /// to maxValue [exclusive]
        /// </summary>
        /// <param name="maxValue">The max value [exclusive]</param>
        /// <returns></returns>
        public int NextInt(int maxValue)
        {
            return NextInt(-1, maxValue);
        }

        /// <summary>
        /// Return a random int from minValue [inclusive] 
        /// to maxValue [exclusive]
        /// </summary>
        /// <param name="minValue">minValue [inclusive]</param>
        /// <param name="maxValue">maxValue [exclusive]</param>
        /// <returns></returns>
        public int NextInt(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        /// <summary>
        /// return a random float from 0 [inclusive]
        /// to 1 [inclusive]
        /// </summary>
        /// <returns></returns>
        public float NextFloat()
        {
            return Random.value;
        }

        /// <summary>
        /// return a random float from 0 [inclusive]
        /// to maxValue [inclusive]
        /// </summary>
        /// <param name="maxValue">maxValue [inclusive]</param>
        /// <returns></returns>
        public float NextFloat(float maxValue)
        {
            return NextFloat(0, maxValue);
        }

        /// <summary>
        /// return a random float from minValue [inclusive]
        /// to maxValue [inclusive]
        /// </summary>
        /// <param name="minValue">minValue [inclusive]</param>
        /// <param name="maxValue">maxValue [inclusive]</param>
        /// <returns></returns>
        public float NextFloat(float minValue, float maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        /// <summary>
        /// return a random float into the Vector2 bounds
        /// (x as minValue [inclusive],
        /// y as maxValue [inclusive])
        /// </summary>
        /// <param name="interval">(x as minValue [inclusive], y as maxValue [inclusive])</param>
        /// <returns></returns>
        public float NextFloat(Vector2 interval)
        {
            return NextFloat(interval.x, interval.y);
        }

        /// <summary>
        /// return a random float from -max [inclusive]
        /// to +max [inclusive] by binomial distribution.
        /// Binomial distribution return more often value 
        /// close to the center (here 0).
        /// </summary>
        /// <param name="max">The max [inclusive]</param>
        /// <returns></returns>
        public float NextBinomialFloat(float max)
        {
            return NextFloat(max) - NextFloat(max);
        }

        /// <summary>
        /// return a random boolean value
        /// </summary>
        /// <returns></returns>
        public bool NextBool()
        {
            int randomValue = NextInt(2);

            return randomValue == 0;
        }
    }
}
