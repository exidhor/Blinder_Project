using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    [RequireComponent(typeof(Kinematic))]
    public class SteeringComponent : MonoBehaviour
    {
        public SteeringSpecs SteeringSpecs;

        public List<InternalBehavior> InternalBehaviors;

        private List<WeightedSteeringOutput> _weightedOutputs;

        private Kinematic _kinematic;

        private void Awake()
        {
            _weightedOutputs = new List<WeightedSteeringOutput>();

            _kinematic = GetComponent<Kinematic>();
        }

        public void ClearBehaviors()
        {
            InternalBehaviors.Clear();
            
            // clear buffers
            SteeringSpecs.LastNodeIndex = -1;
            SteeringSpecs.WanderOrientation = 0;
            //SteeringSpecs.PathToFollow = null;
        }

        public void AddBehavior(EBehavior behavior, Location target, float weight)
        {
            InternalBehaviors.Add(new InternalBehavior(behavior, target, weight));
        }

        public void ActualizeSteering()
        {
            SteeringOutput outputBuffer;

            float totalWeight = 0;
            _weightedOutputs.Clear();

            for (int i = 0; i < InternalBehaviors.Count; i++)
            {
                // fill the output buffer with the steering
                FillOutput(InternalBehaviors[i], _kinematic, out outputBuffer);

                // add the output to the result
                _weightedOutputs.Add(new WeightedSteeringOutput(outputBuffer, InternalBehaviors[i].Weight));

                // increase the total weight
                totalWeight += InternalBehaviors[i].Weight;
            }

            // Divide the accumulated output by the total weight
            if (totalWeight > 0)
            {
                for (int i = 0; i < _weightedOutputs.Count; i++)
                {
                    _weightedOutputs[i].Scale(totalWeight);
                }
            }
        }

        public void ApplySteeringOnKinematic(float deltaTime)
        {
            _kinematic.ResetVelocity();

            for (int i = 0; i < _weightedOutputs.Count; i++)
            {
                _kinematic.Actualize(_weightedOutputs[i].Output, deltaTime);
            }
        }

        private void FillOutput(InternalBehavior internalBehavior, Kinematic character, out SteeringOutput toFill)
        {
            toFill = new SteeringOutput();

            // Get the steering
            GiveSteering(internalBehavior.Behavior, character, internalBehavior.Target, ref toFill);
        }

        private void GiveSteering(EBehavior behavior, Kinematic character, Location target, ref SteeringOutput toFill)
        {
            Kinematic kinematicBuffer = null;

            switch (behavior)
            {
                case EBehavior.Seek:
                    Behavior.Seek(ref toFill, character, SteeringSpecs, target.GetPosition());
                    break;

                case EBehavior.Flee:
                    Behavior.Flee(ref toFill, character, SteeringSpecs, target.GetPosition());
                    break;

                case EBehavior.Arrive:
                    Behavior.Arrive(ref toFill, character, SteeringSpecs, target.GetPosition());
                    break;

                case EBehavior.Face:
                    Behavior.Face(ref toFill, character, target.GetPosition());
                    break;

                case EBehavior.Pursue:
                    kinematicBuffer = target.GetKinematic();

                    if (kinematicBuffer == null)
                    {
                        Behavior.Arrive(ref toFill, character, SteeringSpecs, target.GetPosition());
                    }
                    else
                    {
                        Behavior.Pursue(ref toFill, character, SteeringSpecs, kinematicBuffer);
                    }
                    break;

                case EBehavior.Evade:
                    kinematicBuffer = target.GetKinematic();

                    if (kinematicBuffer == null)
                    {
                        Behavior.Flee(ref toFill, character, SteeringSpecs, target.GetPosition());
                    }
                    else
                    {
                        Behavior.Evade(ref toFill, character, SteeringSpecs, kinematicBuffer);
                    }
                    break;

                case EBehavior.Wander:
                    Behavior.Wander(ref toFill, character, SteeringSpecs);
                    break;

                //case EBehavior.PathFollowing:
                //    Behavior.PathFollowing(ref toFill, character, SteeringSpecs);
                //    break;
            }
        }
    }
}