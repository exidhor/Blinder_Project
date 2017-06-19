using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Tools
{
    // use for random generator
    using Random = UnityEngine.Random;

    public class RandomGenerator : MonoSingleton<RandomGenerator>
    {
        public int Seed;
        public bool UseRandomSeed = true;

        void Start()
        {
            Restart();
        }

        public void Restart()
        {
            if (UseRandomSeed)
            {
                Seed = GenerateRandomSeed();
            }

            Random.InitState(Seed);
        }

        public int GenerateRandomSeed()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.Ticks.ToString().GetHashCode();
        }

        public int NextInt()
        {
            return NextInt(int.MinValue, int.MaxValue);
        }

        public int NextInt(int maxValue)
        {
            return NextInt(0, maxValue);
        }

        /// <summary>
        /// return a valuye between minValue (inclusive) and maxValue (exclusive)
        /// </summary>
        /// <returns></returns>
        public int NextInt(int minValue, int maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        /// <summary>
        /// return a valuye between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns></returns>
        public float NextFloat()
        {
            return Random.value;
        }

        public float NextFloat(float maxValue)
        {
            return NextFloat(0, maxValue);
        }

        public float NextFloat(float minValue, float maxValue)
        {
            return Random.Range(minValue, maxValue);
        }

        public float NextFloat(Vector2 interval)
        {
            return NextFloat(interval.x, interval.y);
        }

        public float NextBinomialFloat(float max)
        {
            return NextFloat(max) - NextFloat(max);
        }

        public bool NextBool()
        {
            int randomValue = NextInt(2);

            return randomValue == 0;
        }
    }
}
