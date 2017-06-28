using System;
using System.Collections.Generic;
using ATools;
using MapEditor;
using Pathfinding;
using Tools;
using UnityEngine;
using UnityEngine.Profiling;

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

        }

        //void Update()
        //{
        //    Debug.Break();

        //    Profiler.BeginSample("fullraycast");
        //    TestRaycastPerf();
        //    Profiler.EndSample();
        //}



        // tmp
        private List<Vector2i> coords = new List<Vector2i>();
        private List<Vector2> points = new List<Vector2>();

        private Vector2i[] coordsOpti = new Vector2i[100];

        void TestRaycastPerf()
        {
            ConstructTestLists();

            Profiler.BeginSample("unityraycast");
            Other();
            Profiler.EndSample();

            Profiler.BeginSample("myraycast");
            Mine();
            Profiler.EndSample();

            //Debug.Log("mine : " + executionTimeForMyRaycast);
            //Debug.Log("unity : " + executionTimeForUnityRaycast);
        }

        void Mine()
        {
            float executionTimeForMyRaycast = TimeExecution.MeasureFunction(MeasureMyRaycastWay);
        }

        void Other()
        {
            float executionTimeForUnityRaycast = TimeExecution.MeasureFunction(MeasureUnityRaycast);
        }

        void ConstructTestLists()
        {
            int testToDo = 10000;
            NavGrid navgrid = Map.instance.navGrid;

            for (int i = 0; i < testToDo; i++)
            {
                Vector2i startCoord = GetRandomPointOnNavgrid(navgrid);
                Vector2i endCoord = GetRandomPointOnNavgrid(navgrid);

                //Vector2i startCoord = new Vector2i(0, 0);
                //Vector2i endCoord = new Vector2i(navgrid.width-1, navgrid.height-1);

                coords.Add(startCoord);
                coords.Add(endCoord);

                Vector2 startPosition = navgrid.GetCasePosition(startCoord);
                Vector2 endPosition = navgrid.GetCasePosition(endCoord);

                points.Add(startPosition);
                points.Add(endPosition);
            }
        }

        void MeasureMyRaycastWay()
        {
            NavGrid navgrid = Map.instance.navGrid;

            for (int i = 0; i < coords.Count; i += 2)
            {
                navgrid.LineCast(coords[i], coords[i + 1], coordsOpti);
            }
        }

        void MeasureUnityRaycast()
        {
            for (int i = 0; i < points.Count; i += 2)
            {
                Physics.Linecast(points[i], points[i + 1]);
            }
        }

        Vector2i GetRandomPointOnNavgrid(NavGrid navgrid)
        {
            Vector2i point;

            point.x = RandomGenerator.instance.NextInt(0, navgrid.width);
            point.y = RandomGenerator.instance.NextInt(0, navgrid.height);

            return point;
        }
    }
}
