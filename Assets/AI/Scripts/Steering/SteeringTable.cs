using System.Collections.Generic;
using MemoryManagement;
using Tools;

namespace AI
{
    public class SteeringTable : MonoSingleton<SteeringTable>
    {
        public List<SteeringPoolEntry> SteeringPoolEntries;

        private List<UnityPool> _pools = new List<UnityPool>();

        void Awake()
        {
            ConstructPools();
        }

        private void SortSteeringPoolEntries()
        {
            SteeringPoolEntries.Sort((x, y) => x.Type.CompareTo(y.Type));
        }

        private void ConstructPools()
        {
            SortSteeringPoolEntries();

            for (int i = 0; i < SteeringPoolEntries.Count; i++)
            {
                UnityPool newPool = UnityPoolTable.instance.AddPool(SteeringPoolEntries[i]);
                newPool.transform.parent = transform;

                _pools.Add(newPool);
            }
        }

        public Steering GetFreeSteering(ESteeringType type, StaticBody character, SteeringSpecs specs, Location target = null)
        {
            if (type == ESteeringType.None)
            {
                return null;
            }

            Steering steering = (Steering) _pools[(int) type].GetFreeResource();

            steering.Init(character, specs, target);

            return steering;
        }
    }
}
