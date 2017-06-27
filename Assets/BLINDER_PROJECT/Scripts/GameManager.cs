using MapEditor;
using Tools;
using UnityEngine;

namespace BlinderProject
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameObject player
        {
            get { return _player; }
        }

        [SerializeField] private GameObject _player;

        void Awake()
        {
            Map.instance.CreateNavGrid();

            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < 10000; i++)
            {
                float x0 = RandomGenerator.instance.NextFloat();
                float y0 = RandomGenerator.instance.NextFloat();
                float x1 = RandomGenerator.instance.NextFloat();
                float y1 = RandomGenerator.instance.NextFloat();

                Vector2 v0 = new Vector2(x0, y0);
                Vector2 v1 = new Vector2(x1, y1);

                float value = MathHelper.Angle(v0, v1);

                if (value < min)
                {
                    min = value;
                }

                if (value < max)
                {
                    max = value;
                }
            }
        }

        void Start()
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int i = 0; i < 10000; i++)
            {
                float x0 = RandomGenerator.instance.NextFloat();
                float y0 = RandomGenerator.instance.NextFloat();
                float x1 = RandomGenerator.instance.NextFloat();
                float y1 = RandomGenerator.instance.NextFloat();

                Vector2 v0 = new Vector2(x0, y0);
                Vector2 v1 = new Vector2(x1, y1);

                float value = MathHelper.Angle(v0, v1);

                if (value < min)
                {
                    min = value;
                }

                if (value > max)
                {
                    max = value;
                }
            }

            Debug.Log("min : " + min);
            Debug.Log("max : " + max);
        }
    }
}
