using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldAI
{
    public static partial class Behavior
    {
        public static void PathFollowing(ref SteeringOutput output, Kinematic character, SteeringSpecs specs)
        {
            //if (specs.PathToFollow == null)
            //{
            //    Debug.Log("No path");
            //    return;
            //}

            //// verify that the player is on a node
            //Node currentPlayerPosition = Level.Instance.GetNodeAt(character.GetPosition());

            //int currentIndex = -1;

            //for (int i = specs.PathToFollow.Count - 1; i >= 0; i--)
            //{
            //    if (currentPlayerPosition == specs.PathToFollow[i])
            //    {
            //        currentIndex = i;
            //        break;
            //    }
            //}

            //if (currentIndex == -1)
            //{
            //    if (specs.LastNodeIndex != -1)
            //    {
            //        Seek(ref output, character, specs, Level.Instance.GetPositionAt(specs.PathToFollow[specs.LastNodeIndex].Coord));
            //    }
            //    return;
            //}

            //// find the last reachable node
            //// todo

            //// delegate seek on it
            //if (currentIndex == 0)
            //{
            //    Arrive(ref output, character, specs, Level.Instance.GetPositionAt(specs.PathToFollow[currentIndex].Coord));
            //}
            //else
            //{
            //    currentIndex--;
            //    Seek(ref output, character, specs, Level.Instance.GetPositionAt(specs.PathToFollow[currentIndex].Coord));
            //}

            //specs.LastNodeIndex = currentIndex;
        }
    }
}
