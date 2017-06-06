using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI
{
    public struct WeightedSteeringOutput
    {
        public SteeringOutput Output;
        public float Weight;

        public WeightedSteeringOutput(SteeringOutput output, float weight)
        {
            Output = output;
            Weight = weight;
        }

        public void Scale(float totalWeight)
        {
            Output.Scale(Weight/totalWeight);
        }
    }
}
