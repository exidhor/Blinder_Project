using MemoryManagement;
using UnityEngine;

namespace AI
{
    public abstract class Steering : UnityPoolObject
    {
        [SerializeField] protected ESteeringType _type;
        [SerializeField] protected Body _character;
        [SerializeField] protected Location _target = new Location();

        [SerializeField] protected SteeringSpecs _specs;

        public void Init(Body character, SteeringSpecs specs, Location target)
        {
            _character = character;
            _specs = specs;

            SetTarget(target);
        }

        public void SetTarget(Location target)
        {
            if (target != null && target.isSet)
            {
                _target.Copy(target);
            }
            else
            {
                _target.Unset();
            }

            Recompute();
        }

        public abstract void Recompute();

        public abstract SteeringOutput GetOutput();

        public virtual bool NeedTarget()
        {
            return true;
        }
    }
}
